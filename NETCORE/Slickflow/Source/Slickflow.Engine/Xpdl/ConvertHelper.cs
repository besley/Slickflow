using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Convert XmlNode to Activity Entity Helper Class
    /// </summary>
    public class ConvertHelper
    {
        #region 活动节点获取
        /// <summary>
        /// 获取XML的节点信息
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="activityGUID">节点GUID</param>
        /// <returns>Xml节点</returns>
        internal static XmlNode GetXmlActivityNodeFromXmlFile(XmlDocument xmlDoc, string activityGUID)
        {
            XmlNode xmlNode = XMLHelper.GetXmlNodeByXpath(xmlDoc,
                    string.Format("{0}[@id='" + activityGUID + "']", XPDLDefinition.StrXmlActivityPath));
            return xmlNode;
        }

        /// <summary>
        /// 获取活动节点
        /// </summary>
        /// <param name="xmlDoc">XML文档</param>
        /// <param name="activityGUID">活动节点GUID</param>
        /// <param name="processGUID">流程节点GUID</param>
        /// <returns>活动实体</returns>
        internal static ActivityEntity GetActivity(XmlDocument xmlDoc, string activityGUID, string processGUID)
        {
            XmlNode activityNode = GetXmlActivityNodeFromXmlFile(xmlDoc, activityGUID);
            ActivityEntity entity = ConvertHelper.ConvertXmlActivityNodeToActivityEntity(activityNode, processGUID);
            return entity;
        }
        #endregion

        #region 活动节点 XML 转换信息
        /// <summary>
        /// Convert XmlNode to Activity Entity
        /// </summary>
        /// <param name="node">Xml Node</param>
        /// <param name="processGUID">ProcessGUID</param>
        /// <returns>Activity Entity</returns>
        public static ActivityEntity ConvertXmlActivityNodeToActivityEntity(XmlNode node, string processGUID)
        {
            ActivityEntity entity = new ActivityEntity();
            entity.ActivityGUID = XMLHelper.GetXmlAttribute(node, "id");
            entity.ActivityName = XMLHelper.GetXmlAttribute(node, "name");
            entity.ActivityCode = XMLHelper.GetXmlAttribute(node, "code");
            entity.ActivityUrl = XMLHelper.GetXmlAttribute(node, "url");
            entity.ProcessGUID = processGUID;

            //描述信息
            XmlNode descNode = node.SelectSingleNode("Description");
            entity.Description = (descNode != null) ? descNode.InnerText : string.Empty;

            //节点类型信息
            XmlNode typeNode = node.SelectSingleNode("ActivityType");
            entity.ActivityType = (ActivityTypeEnum)Enum.Parse(typeof(ActivityTypeEnum), XMLHelper.GetXmlAttribute(typeNode, "type"));
            entity.ActivityTypeDetail = ConvertXmlNodeToActivityTypeDetail(typeNode);
            entity.WorkItemType = XPDLHelper.GetWorkItemType(entity.ActivityType);

            if (entity.ActivityType == ActivityTypeEnum.SubProcessNode)             //sub process node
            {
                //子流程节点
                var subProcessNode = new SubProcessNode(entity);
                subProcessNode.SubProcessGUID = XMLHelper.GetXmlAttribute(typeNode, "subId");
                entity.Node = subProcessNode;
            }
            else if (entity.ActivityType == ActivityTypeEnum.MultipleInstanceNode)      //multiple instance node
            {
                var multipleInstanceNode = new MultipleInstanceNode(entity);
                entity.Node = multipleInstanceNode;
            }

            //获取节点的操作列表
            XmlNode actionsNode = node.SelectSingleNode("Actions");
            if (actionsNode != null)
            {
                XmlNodeList xmlActionList = actionsNode.ChildNodes;
                List<ActionEntity> actionList = new List<ActionEntity>();
                foreach (XmlNode element in xmlActionList)
                {
                    actionList.Add(ConvertXmlActionNodeToActionEntity(element));
                }
                entity.ActionList = actionList;
            }

            //获取节点边界列表
            XmlNode boundariesNode = node.SelectSingleNode("Boundaries");
            if (boundariesNode != null)
            {
                XmlNodeList xmlBoundaryList = boundariesNode.ChildNodes;
                List<BoundaryEntity> boundaryList = new List<BoundaryEntity>();
                foreach (XmlNode element in xmlBoundaryList)
                {
                    boundaryList.Add(ConvertXmlBoundaryNodeToBoundaryEntity(element));
                }
                entity.BoundaryList = boundaryList;
            }

            //获取节点自定义章节信息
            XmlNode sectionsNode = node.SelectSingleNode("Sections");
            if (sectionsNode != null)
            {
                XmlNodeList xmlSectionList = sectionsNode.ChildNodes;
                List<SectionEntity> sectionList = new List<SectionEntity>();
                foreach (XmlNode element in xmlSectionList)
                {
                    sectionList.Add(ConvertXmlSectionNodeToSectionEntity(element));
                }
                entity.SectionList = sectionList;
            }


            //节点的Split Join 类型
            string gatewaySplitJoinType = XMLHelper.GetXmlAttribute(typeNode, "gatewaySplitJoinType");
            if (!string.IsNullOrEmpty(gatewaySplitJoinType))
            {
                entity.GatewaySplitJoinType = (GatewaySplitJoinTypeEnum)Enum.Parse(typeof(GatewaySplitJoinTypeEnum), gatewaySplitJoinType);
            }

            string gatewayDirection = XMLHelper.GetXmlAttribute(typeNode, "gatewayDirection");
            //节点的路由信息
            if (!string.IsNullOrEmpty(gatewayDirection))
            {
                entity.GatewayDirectionType = (GatewayDirectionEnum)Enum.Parse(typeof(GatewayDirectionEnum), gatewayDirection);
            }

            string gatewayJoinPass = XMLHelper.GetXmlAttribute(typeNode, "gatewayJoinPass");
            if (!string.IsNullOrEmpty(gatewayJoinPass))
            {
                entity.GatewayJoinPassType = EnumHelper.TryParseEnum<GatewayJoinPassEnum>(gatewayJoinPass);
            }

            return entity;
        }

        /// <summary>
        /// 把Xml节点转换为ActivityTypeDetail 类（用于会签等复杂类型）
        /// </summary>
        /// <param name="typeNode">类型节点</param>
        /// <returns>活动类型详细</returns>
        private static ActivityTypeDetail ConvertXmlNodeToActivityTypeDetail(XmlNode typeNode)
        {
            ActivityTypeDetail entity = new ActivityTypeDetail();
            entity.ActivityType = (ActivityTypeEnum)Enum.Parse(typeof(ActivityTypeEnum), XMLHelper.GetXmlAttribute(typeNode, "type"));

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "trigger")))
            {
                TriggerTypeEnum triggerType = TriggerTypeEnum.None;
                Enum.TryParse<TriggerTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "trigger"), out triggerType);
                entity.TriggerType = triggerType;

                //获取时间表达式
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "expression")))
                {
                    entity.Expression = XMLHelper.GetXmlAttribute(typeNode, "expression");
                }
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "complexType")))
            {
                entity.ComplexType = EnumHelper.ParseEnum<ComplexTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "complexType"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "mergeType")))
            {
                entity.MergeType = EnumHelper.ParseEnum<MergeTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "mergeType"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "compareType")))
            {
                entity.CompareType = EnumHelper.ParseEnum<CompareTypeEnum>(XMLHelper.GetXmlAttribute(typeNode, "compareType"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "completeOrder")))
            {
                entity.CompleteOrder = float.Parse(XMLHelper.GetXmlAttribute(typeNode, "completeOrder"));
            }

            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(typeNode, "skip")))
            {
                var skip = Boolean.Parse(XMLHelper.GetXmlAttribute(typeNode, "skip"));
                var skipto = XMLHelper.GetXmlAttribute(typeNode, "to");

                if (skip)
                {
                    entity.SkipInfo = new SkipInfo { IsSkip = skip, Skipto = skipto };
                }
            }

            return entity;
        }

        /// <summary>
        /// 将Action的XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <returns>操作实体</returns>
        private static ActionEntity ConvertXmlActionNodeToActionEntity(XmlNode node)
        {
            if (node == null) return null;

            ActionEntity action = new ActionEntity();
            var actionType = XMLHelper.GetXmlAttribute(node, "type");
            action.ActionType = EnumHelper.TryParseEnum<ActionTypeEnum>(actionType);

            var actionMethod = XMLHelper.GetXmlAttribute(node, "method");
            action.ActionMethod = EnumHelper.TryParseEnum<ActionMethodEnum>(actionMethod);

            var subMethod = XMLHelper.GetXmlAttribute(node, "subMethod");
            action.SubMethod = EnumHelper.TryParseEnum<SubMethodEnum>(subMethod);

            var fireType = XMLHelper.GetXmlAttribute(node, "fire");
            action.FireType = EnumHelper.TryParseEnum<FireTypeEnum>(fireType);

            if (action.ActionType == ActionTypeEnum.Event)
            {
                action.Arguments = XMLHelper.GetXmlAttribute(node, "arguments");
                action.Expression = XMLHelper.GetXmlAttribute(node, "expression");

                if (action.ActionMethod == ActionMethodEnum.CSharpLibrary)
                {
                    var methodInfoNode = node.SelectSingleNode("MethodInfo");
                    if (methodInfoNode != null)
                    {
                        var methodInfo = action.MethodInfo = new MethodInfo();
                        methodInfo.AssemblyFullName = XMLHelper.GetXmlAttribute(methodInfoNode, "assemblyFullName");
                        methodInfo.TypeFullName = XMLHelper.GetXmlAttribute(methodInfoNode, "typeFullName");
                        //method.ConstructorParameters = XMLHelper.GetXmlAttribute(methodNode, "constructorParameters");
                        methodInfo.MethodName = XMLHelper.GetXmlAttribute(methodInfoNode, "methodName");
                        //method.MethodParameters = XMLHelper.GetXmlAttribute(methodNode, "methodParameters");
                    }
                }
                else if(action.ActionMethod == ActionMethodEnum.SQL
                    || action.ActionMethod == ActionMethodEnum.Python)
                {
                    var codeInfoNode = node.SelectSingleNode("CodeInfo");
                    if (codeInfoNode != null)
                    {
                        var codeInfo = action.CodeInfo = new CodeInfo();
                        codeInfo.CodeText = codeInfoNode.InnerText;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 转换Boundary节点
        /// </summary>
        /// <param name="node">xml节点</param>
        /// <returns>实体对象</returns>
        private static BoundaryEntity ConvertXmlBoundaryNodeToBoundaryEntity(XmlNode node)
        {
            BoundaryEntity boundary = new BoundaryEntity();
            var eventType = XMLHelper.GetXmlAttribute(node, "event");
            boundary.EventTriggerType = EnumHelper.TryParseEnum<EventTriggerEnum>(eventType);

            if (boundary.EventTriggerType == EventTriggerEnum.Timer)
            {
                boundary.Expression = XMLHelper.GetXmlAttribute(node, "expression");
            }
            return boundary;
        }

        /// <summary>
        /// 转换Section节点
        /// </summary>
        /// <param name="node">xml节点</param>
        /// <returns>实体对象</returns>
        private static SectionEntity ConvertXmlSectionNodeToSectionEntity(XmlNode node)
        {
            SectionEntity section = new SectionEntity();
            section.Name = XMLHelper.GetXmlAttribute(node, "name");
            section.Value = node.InnerText;

            return section;
        }
        #endregion

        #region Xml节点转换信息
        /// <summary>
        /// 把XML节点转换为ActivityEntity实体对象
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="node">节点</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>转移对象</returns>
        public static TransitionEntity ConvertXmlTransitionNodeToTransitionEntity(XmlDocument xmlDoc, XmlNode node, string processGUID)
        {
            //构造转移的基本属性
            TransitionEntity transition = new TransitionEntity();
            transition.TransitionGUID = XMLHelper.GetXmlAttribute(node, "id");
            transition.FromActivityGUID = XMLHelper.GetXmlAttribute(node, "from");
            transition.ToActivityGUID = XMLHelper.GetXmlAttribute(node, "to");
            if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(node, "direction")))
            {
                transition.DirectionType = (TransitionDirectionTypeEnum)Enum.Parse(typeof(TransitionDirectionTypeEnum),
                    XMLHelper.GetXmlAttribute(node, "direction"));
            }

            //转移描述
            XmlNode descriptionNode = node.SelectSingleNode("Description");
            if (descriptionNode != null)
            {
                transition.Description = descriptionNode.InnerText;
            }

            //构造活动节点的实体对象
            transition.FromActivity = GetActivity(xmlDoc, transition.FromActivityGUID, processGUID);
            transition.ToActivity = GetActivity(xmlDoc, transition.ToActivityGUID, processGUID);

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
            XmlNode conditionNode = node.SelectSingleNode("Condition");
            if (conditionNode != null)
            {
                transition.Condition = new ConditionEntity();
                if (!string.IsNullOrEmpty(XMLHelper.GetXmlAttribute(conditionNode, "type")))
                {
                    ConditionTypeEnum conditionTypeEnum;
                    Enum.TryParse<ConditionTypeEnum>(XMLHelper.GetXmlAttribute(conditionNode, "type"), out conditionTypeEnum);
                    transition.Condition.ConditionType = conditionTypeEnum;
                }

                if ((conditionNode.SelectSingleNode("ConditionText") != null)
                    && !string.IsNullOrEmpty(XMLHelper.GetXmlNodeText(conditionNode, "ConditionText")))
                {
                    transition.Condition.ConditionText = XMLHelper.GetXmlNodeText(conditionNode, "ConditionText");
                }
            }

            //构造转移的行为节点
            XmlNode groupBehavioursNode = node.SelectSingleNode("GroupBehaviours");
            if (groupBehavioursNode != null)
            {
                transition.GroupBehaviours = new GroupBehaviourEntity();
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
            }
            return transition;
        }
        #endregion
    }
}
