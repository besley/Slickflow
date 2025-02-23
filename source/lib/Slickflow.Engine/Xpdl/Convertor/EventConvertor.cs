using Slickflow.Engine.Xpdl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using IronPython.Compiler.Ast;
using System.Xml.Linq;

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// Event Convertor
    /// </summary>
    internal class EventConvertor : ConvertorBase, IConvert
    {
        public EventConvertor(XmlNode node, XmlNamespaceManager xnpmgr) : base(node, xnpmgr)
        {
        }

        /// <summary>
        /// Convert Element Detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Activity ConvertElementDetail(Activity entity)
        {
            var eventNode = base.XMLNode;
            TriggerTypeEnum triggerType = TriggerTypeEnum.None;
            var eventDefinitionNode = GetEventDefinitionNode(eventNode, out triggerType);
            if (eventDefinitionNode != null
                && triggerType != TriggerTypeEnum.None)
            {
                var triggerDetail = new TriggerDetail();
                triggerDetail.TriggerType = triggerType;
                if (triggerType == TriggerTypeEnum.Conditional)
                {
                    triggerDetail.Expression = eventDefinitionNode.InnerText;
                }
                else if (triggerType == TriggerTypeEnum.Message)
                {
                    //消息触发类型Message Trigger Type
                    //Message Trigger Type
                    var messageRef = XMLHelper.GetXmlAttribute(eventDefinitionNode, "messageRef");
                    if (!string.IsNullOrEmpty(messageRef)) 
                    {
                        triggerDetail.MessageDirection = GetMessageDirectionFromEventNode(eventNode);
                        var xmlDoc = eventDefinitionNode.OwnerDocument;
                        var root = xmlDoc.DocumentElement;
                        var strMessagePath = string.Format("{0}[@id='{1}']", XPDLDefinition.BPMN_StrXmlPath_Message, messageRef);
                        var xmlMessageNode = root.SelectSingleNode(strMessagePath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                        if (xmlMessageNode != null) 
                        {
                            var strMessageName = XMLHelper.GetXmlAttribute(xmlMessageNode, "name");
                            triggerDetail.Expression = strMessageName; 
                        }
                    }
                }
                else if (triggerType == TriggerTypeEnum.Signal)
                {
                    //信号触发类型Signal Trigger Type
                    //Signal Trigger Type
                    var signalRef = XMLHelper.GetXmlAttribute(eventDefinitionNode, "signalRef");
                    if (!string.IsNullOrEmpty(signalRef))
                    {
                        triggerDetail.MessageDirection = GetMessageDirectionFromEventNode(eventNode);
                        var xmlDoc = eventDefinitionNode.OwnerDocument;
                        var root = xmlDoc.DocumentElement;
                        var strSignalPath = string.Format("{0}[@id='{1}']", XPDLDefinition.BPMN_StrXmlPath_Signal, signalRef);
                        var xmlSignalNode = root.SelectSingleNode(strSignalPath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
                        if (xmlSignalNode != null)
                        {
                            var strSignalName = XMLHelper.GetXmlAttribute(xmlSignalNode, "name");
                            triggerDetail.Expression = strSignalName;
                        }
                    }
                }
                entity.TriggerDetail = triggerDetail;
            }
            return entity;
        }

        /// <summary>
        /// Get Event Definition Node
        /// 获得事件定义节点
        /// </summary>
        /// <param name="eventNode"></param>
        /// <param name="triggerType"></param>
        /// <returns></returns>
        private XmlNode GetEventDefinitionNode(XmlNode eventNode, out TriggerTypeEnum triggerType)
        {
            XmlNode definitionNode = null;
            triggerType = TriggerTypeEnum.None;
            foreach (XmlNode child in eventNode.ChildNodes)
            {
                if (child.Name == XPDLDefinition.BPMN_ElementName_EventDefinition_Conditon)
                {
                    definitionNode = child;
                    triggerType = TriggerTypeEnum.Conditional;
                    break;
                }
                else if(child.Name == XPDLDefinition.BPMN_ElementName_EventDefinition_Message)
                {
                    definitionNode = child;
                    triggerType = TriggerTypeEnum.Message;
                    break;
                }
                else if (child.Name == XPDLDefinition.BPMN_ElementName_EventDefinition_Timer)
                {
                    definitionNode = child;
                    triggerType = TriggerTypeEnum.Timer;
                    break;
                }
                else if (child.Name == XPDLDefinition.BPMN_ElementName_EventDefinition_Signal)
                {
                    definitionNode = child;
                    triggerType = TriggerTypeEnum.Signal;
                    break;
                }
            }
            return definitionNode;
        }

        /// <summary>
        /// Get Message Direction from Event Node
        /// 获取消息Throw/Catch 类型
        /// </summary>
        /// <param name="eventNode"></param>
        /// <returns></returns>
        private MessageDirectionEnum GetMessageDirectionFromEventNode(XmlNode eventNode)
        {
            var messageDirection = MessageDirectionEnum.None;
            var nodeName = eventNode.Name;
            if (nodeName == XPDLDefinition.BPMN_ElementName_IntermediateEvent_Throw)
            {
                messageDirection = MessageDirectionEnum.Throw;
            }
            else if (nodeName == XPDLDefinition.BPMN_ElementName_IntermediateEvent_Catch)
            {
                messageDirection = MessageDirectionEnum.Catch;
            }
            else if (nodeName == XPDLDefinition.BPMN_ElementName_StartEvent)
            {
                messageDirection = MessageDirectionEnum.Catch;
            }
            else if (nodeName == XPDLDefinition.BPMN_ElementName_EndEvent)
            {
                messageDirection = MessageDirectionEnum.Throw;
            }
            return messageDirection;
        }

    }
}
