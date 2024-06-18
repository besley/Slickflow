using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// XML节点转为为活动实体对象
    /// </summary>
    public class ConvertHelper
    {
        #region 活动节点获取
        /// <summary>
        /// 获取活动节点
        /// </summary>
        /// <param name="xmlDoc">XML文档</param>
        /// <param name="xnpmgr">XML命名空间管理</param>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <param name="processGUID">流程节点GUID</param>
        /// <param name="isSubProcess">是否子流程</param>
        /// <returns>活动实体</returns>
        internal static Activity GetActivity(XmlDocument xmlDoc, 
            XmlNamespaceManager xnpmgr, 
            string activityGUID, 
            string processGUID,
            Boolean isSubProcess)
        {
            XmlNode activityNode = GetXmlActivityNodeFromXmlFile(xmlDoc, xnpmgr, activityGUID, isSubProcess);
            Activity entity = ConvertHelper.ConvertXmlActivityNodeToActivityEntity(activityNode, xnpmgr, processGUID);
            return entity;
        }

        /// <summary>
        /// 获取XML的节点信息
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="activityGUID">节点GUID</param>
        /// <param name="xnpmgr">xml命名空间</param>
        /// <param name="isSubProcess">是否子流程</param>
        /// <returns>Xml节点</returns>
        internal static XmlNode GetXmlActivityNodeFromXmlFile(XmlDocument xmlDoc,
            XmlNamespaceManager xnpmgr,
            string activityGUID,
            Boolean isSubProcess)
        {
            XmlNode activityNode = null;
            var processNode = XMLHelper.GetXmlNodeByXpath(xmlDoc, XPDLHelper.GetXmlPathOfProcess(isSubProcess), xnpmgr);
            XmlNodeList xmlNodeList = processNode.ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                if (XMLHelper.GetXmlAttribute(node, "sf:guid") == activityGUID)
                {
                    activityNode = node;
                    break;
                }
            }
            return activityNode;
        }
        #endregion

        #region 活动节点 XML 转换信息
        /// <summary>
        /// Convert XmlNode to Activity Entity
        /// </summary>
        /// <param name="xmlNode">Xml Node</param>
        /// <param name="xnpmgr">XML命名空间管理</param>
        /// <param name="processGUID">ProcessGUID</param>
        /// <returns>Activity Entity</returns>
        public static Activity ConvertXmlActivityNodeToActivityEntity(XmlNode xmlNode, 
            XmlNamespaceManager xnpmgr, 
            string processGUID)
        {
            if (xmlNode == null)
            {
                throw new WfXpdlException("The xml node can't be null, please check the xml file");
            }

            ActivityTypeEnum activityType = ActivityTypeEnum.Unknown;
            var convert = ConvertorFactory.CreateConvertor(xmlNode, xnpmgr, out activityType);
            Activity entity = convert.Convert();
            entity.ProcessGUID = processGUID;
            entity.ActivityType = activityType;

            return entity;
        }
        #endregion

        #region Xml节点连线信息
        /// <summary>
        /// 把XML节点转换为ActivityEntity实体对象
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="xnmgr">XML命名空间管理</param>
        /// <param name="node">节点</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="isSubProcess">是否子流程</param>
        /// <returns>转移对象</returns>
        public static Transition ConvertXmlTransitionNodeToTransitionEntity(XmlDocument xmlDoc,
            XmlNamespaceManager xnmgr,
            XmlNode node,
            string processGUID,
            bool isSubProcess)
        {
            //构造转移的基本属性
            Transition transition = new Transition();
            transition.TransitionGUID = XMLHelper.GetXmlAttribute(node, "sf:guid");
            transition.FromActivityGUID = XMLHelper.GetXmlAttribute(node, "sf:from");
            transition.ToActivityGUID = XMLHelper.GetXmlAttribute(node, "sf:to");

            //构造活动节点的实体对象
            transition.FromActivity = GetActivity(xmlDoc, xnmgr, transition.FromActivityGUID, processGUID, isSubProcess);
            transition.ToActivity = GetActivity(xmlDoc, xnmgr, transition.ToActivityGUID, processGUID, isSubProcess);

            //构造转移的接收者类型
            XmlNode receiverNode = node.SelectSingleNode("Receiver");
            if (receiverNode != null)
            {
                transition.Receiver = new Receiver();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(receiverNode, "type")))
                {
                    transition.Receiver.ReceiverType = (ReceiverTypeEnum)Enum.Parse(typeof(ReceiverTypeEnum),
                        XMLHelper.GetXmlAttribute(receiverNode, "type"));
                    int candidates = 0;
                    if (int.TryParse(XMLHelper.GetXmlAttribute(receiverNode, "candidates"), out candidates) == true)
                    {
                        transition.Receiver.Candidates = candidates;
                    }
                }
            }

            //构造转移的条件节点
            var conditionNode = node.SelectSingleNode(XPDLDefinition.BPMN2_ElementName_ConditionExpression, xnmgr);
            if (conditionNode != null)
            {
                var conditionDetail = new ConditionDetail();
                conditionDetail.ConditionType = ConditionTypeEnum.Expression;
                conditionDetail.ConditionText = conditionNode.InnerText;
                transition.Condition = conditionDetail;
            }
       
            //XmlNode conditionNode = node.SelectSingleNode("Condition");
            //if (conditionNode != null)
            //{
            //    transition.Condition = new ConditionEntity();
            //    if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(conditionNode, "type")))
            //    {
            //        ConditionTypeEnum conditionTypeEnum;
            //        Enum.TryParse<ConditionTypeEnum>(XMLHelper.GetXmlAttribute(conditionNode, "type"), out conditionTypeEnum);
            //        transition.Condition.ConditionType = conditionTypeEnum;
            //    }

            //    if ((conditionNode.SelectSingleNode("ConditionText") != null)
            //        && !string.IsNullOrEmpty(XMLHelper.GetXmlNodeText(conditionNode, "ConditionText")))
            //    {
            //        transition.Condition.ConditionText = XMLHelper.GetXmlNodeText(conditionNode, "ConditionText");
            //    }
            //}

            //构造转移的行为节点
            var groupBehavioursNode = node.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_SequenceFlow_GroupBehaviours, xnmgr);
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

                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(groupBehavioursNode, "forced")))
                {
                    bool isForced = false;
                    bool canBeParsed = Boolean.TryParse(XMLHelper.GetXmlAttribute(groupBehavioursNode, "forced"), out isForced);
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
        #endregion
    }
}
