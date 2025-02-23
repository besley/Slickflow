using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Process Model Convertor
    /// </summary>
    public class ProcessModelConvertor
    {
        /// <summary>
        /// Convert Process Model from xml
        /// </summary>
        public static Process ConvertProcessModelFromXML(XmlNode xmlNodeProcess)
        {
            Process process = null;

            if (xmlNodeProcess != null)
            {
                //Process attribute
                process = ConvertProcessAttribute(xmlNodeProcess);

                //Process child ndoes
                var childNodeList = xmlNodeProcess.ChildNodes;
                foreach (XmlNode child in childNodeList)
                {
                    if (child.Name != XPDLDefinition.BPMN_ElementName_SequenceFlow)
                    {
                        ConvertXmlChildNode(child, process);
                    }
                }

                //process transition
                foreach (XmlNode child in childNodeList)
                {
                    if (child.Name == XPDLDefinition.BPMN_ElementName_SequenceFlow)
                    {
                        ConvertXmlChildNode(child, process);
                    }
                }
            }
            return process;
        }

        /// <summary>
        /// Process attribute
        /// </summary>
        private static Process ConvertProcessAttribute(XmlNode xmlNode)
        {
            var process = new Process();
            process.ProcessID = XMLHelper.GetXmlAttribute(xmlNode, "id");
            process.Name = XMLHelper.GetXmlAttribute(xmlNode, "name");
            process.Code = XMLHelper.GetXmlAttribute(xmlNode, "sf:code");
            return process;
        }

        /// <summary>
        /// Convert xml child node
        /// </summary>
        private static void ConvertXmlChildNode(XmlNode node, Process process)
        {
            if (node.Name == XPDLDefinition.BPMN_ElementName_ExtensionElements)
            {
                var formsXmlElement = node.SelectSingleNode(XPDLDefinition.Sf_ElementName_Forms, XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument));
                if (formsXmlElement != null)
                {
                    var childNodes = formsXmlElement.ChildNodes;
                    foreach (XmlNode child in childNodes)
                    {
                        var form = ConvertHelper.ComnvertXmlFormNodeToFormEntity(child,
                            XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument), process.ProcessID);
                        process.FormList.Add(form);
                    }
                }
                else
                {
                    throw new NotImplementedException("NOT supported extension xml element");
                }
            }
            else if (node.Name == XPDLDefinition.BPMN_ElementName_SubProcess)
            {
                ActivityTypeEnum subProcessActivityType = ActivityTypeEnum.Unknown;
                var subConvertor = ConvertorFactory.CreateConvertor(node, XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument), out subProcessActivityType);
                var subProcessActivity = subConvertor.Convert();
                subProcessActivity.ActivityType = subProcessActivityType;
                subProcessActivity.ProcessID = process.ProcessID;

                //add subprocess into activitylist
                process.ActivityList.Add(subProcessActivity);
            }
            else if (node.Name == XPDLDefinition.BPMN_ElementName_SequenceFlow)
            {
                var transition = ConvertTransition(node, process);
                process.TransitionList.Add(transition);
            }
            else if(node.Name == XPDLDefinition.BPMN_StrXmlPath_Incoming
                || node.Name == XPDLDefinition.BPMN_StrXmlPath_Outgoing
                || node.Name == XPDLDefinition.BPMN_StrXmlPath_LaneSet)
            {
                ;
            }
            else
            {
                //不同节点类型的处理
                //Processing of different node types
                ActivityTypeEnum activityType = ActivityTypeEnum.Unknown;
                var convertor = ConvertorFactory.CreateConvertor(node, XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument), out activityType);
                var activity = convertor.Convert();
                activity.ActivityType = activityType;
                activity.WorkItemType = GetWorkItemType(activityType);
                activity.ProcessID = process.ProcessID;

                process.ActivityList.Add(activity);
            }
        }

        /// <summary>
        /// Obtain work item types based on activity types
        /// 根据活动类型获取工作项类型
        /// </summary>
        private static WorkItemTypeEnum GetWorkItemType(ActivityTypeEnum activityType)
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.NonWorkItem;

            if (activityType == ActivityTypeEnum.TaskNode
                || activityType == ActivityTypeEnum.MultiSignNode
                || activityType == ActivityTypeEnum.SubProcessNode)
            {
                workItemType = WorkItemTypeEnum.IsWorkItem;
            }
            return workItemType;
        }

        /// <summary>
        /// Convert sub process
        /// </summary>
        public static Process ConvertSubProcess(XmlNode node)
        {
            //subprocess
            var subProcess = ConvertProcessAttribute(node);

            //child nodes
            var childNodeList = node.ChildNodes;
            foreach (XmlNode child in childNodeList)
            {
                if (child.Name != XPDLDefinition.BPMN_ElementName_SequenceFlow)
                {
                    ConvertXmlChildNode(child, subProcess);
                }
            }

            //transitions
            foreach (XmlNode child in childNodeList)
            {
                if (child.Name == XPDLDefinition.BPMN_ElementName_SequenceFlow)
                {
                    ConvertXmlChildNode(child, subProcess);
                }
            }
            return subProcess;
        }

        /// <summary>
        /// Convert to transition
        /// </summary>
        public static Transition ConvertTransition(XmlNode node, Process process)
        {
            Transition transition = new Transition();
            transition.TransitionID = XMLHelper.GetXmlAttribute(node, "id");
            transition.FromActivityID = XMLHelper.GetXmlAttribute(node, "sourceRef");
            transition.ToActivityID = XMLHelper.GetXmlAttribute(node, "targetRef");

            transition.FromActivity = ProcessModelHelper.GetActivity(process, transition.FromActivityID);
            transition.ToActivity = ProcessModelHelper.GetActivity(process, transition.ToActivityID);

            //Condition node
            var conditionNode = node.SelectSingleNode(XPDLDefinition.BPMN_ElementName_ConditionExpression,
                XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument));
            if (conditionNode != null)
            {
                var conditionDetail = new ConditionDetail();
                conditionDetail.ConditionType = ConditionTypeEnum.Expression;
                conditionDetail.ConditionText = conditionNode.InnerText;
                transition.Condition = conditionDetail;
            }

            //Group behavious
            var groupBehavioursNode = node.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_SequenceFlow_GroupBehaviours,
                XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument));
            if (groupBehavioursNode != null)
            {
                transition.GroupBehaviours = new GroupBehaviour();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehavioursNode, "defaultBranch")))
                {
                    bool defaultBranch = false;
                    bool isDefaultParsed = Boolean.TryParse(XMLHelper.GetXmlAttribute(groupBehavioursNode, "defaultBranch"), out defaultBranch);
                    if (isDefaultParsed) transition.GroupBehaviours.DefaultBranch = defaultBranch;
                }

                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehavioursNode, "priority")))
                {
                    short priority = 0;
                    bool isPriorityParsed = short.TryParse(XMLHelper.GetXmlAttribute(groupBehavioursNode, "priority"), out priority);
                    if (isPriorityParsed) transition.GroupBehaviours.Priority = priority;
                }

                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehavioursNode, "forcedMerge")))
                {
                    bool isForced = false;
                    bool canBeParsed = Boolean.TryParse(XMLHelper.GetXmlAttribute(groupBehavioursNode, "forcedMerge"), out isForced);
                    if (canBeParsed) transition.GroupBehaviours.Forced = isForced;
                }

                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehavioursNode, "approval")))
                {
                    var approval = XMLHelper.GetXmlAttribute(groupBehavioursNode, "approval");
                    transition.GroupBehaviours.Approval = EnumHelper.TryParseEnum<ApprovalStatusEnum>(approval);
                }
            }
            return transition;
        }
    }
}
