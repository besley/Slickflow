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
    /// 流程模型转换类
    /// </summary>
    public class ProcessModelConvertor
    {
        /// <summary>
        /// 流程实体转换
        /// </summary>
        /// <param name="xmlNodeProcess">流程XML节点</param>
        /// <returns>流程模型</returns>
        public static Process ConvertProcessModelFromXML(XmlNode xmlNodeProcess)
        {
            Process process = null;

            if (xmlNodeProcess != null)
            {
                //流程基本属性
                process = ConvertProcessAttribute(xmlNodeProcess);

                //流程子节点
                var childNodeList = xmlNodeProcess.ChildNodes;
                foreach (XmlNode child in childNodeList)
                {
                    if (child.Name != XPDLDefinition.BPMN2_ElementName_SequenceFlow)
                    {
                        ConvertXmlChildNode(child, process);
                    }
                }

                //流程连线
                foreach (XmlNode child in childNodeList)
                {
                    if (child.Name == XPDLDefinition.BPMN2_ElementName_SequenceFlow)
                    {
                        ConvertXmlChildNode(child, process);
                    }
                }
            }
            return process;
        }

        /// <summary>
        /// 转换流程属性
        /// </summary>
        /// <param name="xmlNode">XML节点</param>
        /// <returns>流程模型</returns>
        private static Process ConvertProcessAttribute(XmlNode xmlNode)
        {
            var process = new Process();
            process.ID = XMLHelper.GetXmlAttribute(xmlNode, "id");
            process.ProcessGUID = XMLHelper.GetXmlAttribute(xmlNode, "sf:guid");
            process.Name = XMLHelper.GetXmlAttribute(xmlNode, "name");
            process.Code = XMLHelper.GetXmlAttribute(xmlNode, "sf:code");
            return process;
        }

        /// <summary>
        /// 转换XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="process">流程</param>
        private static void ConvertXmlChildNode(XmlNode node, Process process)
        {
            if (node.Name == XPDLDefinition.BPMN2_ElementName_SubProcess)
            {
                ActivityTypeEnum subProcessActivityType = ActivityTypeEnum.Unknown;
                var subConvertor = ConvertorFactory.CreateConvertor(node, XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument), out subProcessActivityType);
                var subProcessActivity = subConvertor.Convert();
                subProcessActivity.ActivityType = subProcessActivityType;
                subProcessActivity.ProcessGUID = process.ProcessGUID;

                var subProcess = ConvertSubProcess(node);
                subProcessActivity.SubProcess = subProcess;

                //添加子流程节点到节点列表
                process.ActivityList.Add(subProcessActivity);
            }
            else if (node.Name == XPDLDefinition.BPMN2_ElementName_SequenceFlow)
            {
                var transition = ConvertTransition(node, process);
                process.TransitionList.Add(transition);
            }
            else if(node.Name == XPDLDefinition.BPMN2_StrXmlPath_Incoming
                || node.Name == XPDLDefinition.BPMN2_StrXmlPath_Outgoing)
            {
                ;
            }
            else
            {
                //不同节点类型的处理
                ActivityTypeEnum activityType = ActivityTypeEnum.Unknown;
                var convertor = ConvertorFactory.CreateConvertor(node, XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument), out activityType);
                var activity = convertor.Convert();
                activity.ActivityType = activityType;
                activity.WorkItemType = GetWorkItemType(activityType);
                activity.ProcessGUID = process.ProcessGUID;

                process.ActivityList.Add(activity);
            }
        }

        /// <summary>
        /// 根据活动类型获取工作项类型
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <returns>工作项类型</returns>
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

        private static Process ConvertSubProcess(XmlNode node)
        {
            //子流程
            var subProcess = ConvertProcessAttribute(node);

            //流程子节点
            var childNodeList = node.ChildNodes;
            foreach (XmlNode child in childNodeList)
            {
                if (child.Name != XPDLDefinition.BPMN2_ElementName_SequenceFlow)
                {
                    ConvertXmlChildNode(child, subProcess);
                }
            }

            //流程连线
            foreach (XmlNode child in childNodeList)
            {
                if (child.Name == XPDLDefinition.BPMN2_ElementName_SequenceFlow)
                {
                    ConvertXmlChildNode(child, subProcess);
                }
            }
            return subProcess;
        }

        /// <summary>
        /// 从XML节点转换为转移节点
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="process">流程对象</param>
        /// <returns></returns>
        public static Transition ConvertTransition(XmlNode node, Process process)
        {
            Transition transition = new Transition();
            transition.TransitionGUID = XMLHelper.GetXmlAttribute(node, "sf:guid");
            transition.FromActivityGUID = XMLHelper.GetXmlAttribute(node, "sf:from");
            transition.ToActivityGUID = XMLHelper.GetXmlAttribute(node, "sf:to");

            transition.FromActivity = ProcessModelHelper.GetActivity(process, transition.FromActivityGUID);
            transition.ToActivity = ProcessModelHelper.GetActivity(process, transition.ToActivityGUID);

            //构造转移的条件节点
            var conditionNode = node.SelectSingleNode(XPDLDefinition.BPMN2_ElementName_ConditionExpression,
                XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument));
            if (conditionNode != null)
            {
                var conditionDetail = new ConditionDetail();
                conditionDetail.ConditionType = ConditionTypeEnum.Expression;
                conditionDetail.ConditionText = conditionNode.InnerText;
                transition.Condition = conditionDetail;
            }

            //构造连线的群组行为节点
            var groupBehavioursNode = node.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_SequenceFlow_GroupBehaviours,
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
