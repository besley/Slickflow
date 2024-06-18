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

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// 事件节点转换器
    /// </summary>
    internal class EventConvertor : ConvertorBase, IConvert
    {
        public EventConvertor(XmlNode node, XmlNamespaceManager xnpmgr) : base(node, xnpmgr)
        {
        }

        public override Activity ConvertElementDetail(Activity entity)
        {
            var eventNode = base.XMLNode;
            TriggerTypeEnum triggerType = TriggerTypeEnum.None;
            var eventDefinitionNode = GetEventDefinitionNode(eventNode, out triggerType);
            if (eventDefinitionNode != null
                && triggerType != TriggerTypeEnum.None)
            {
                //trigger资料节点
                var triggerDetail = new TriggerDetail();
                triggerDetail.TriggerType = triggerType;
                triggerDetail.Expression = eventDefinitionNode.InnerText;

                entity.TriggerDetail = triggerDetail;
            }
            return entity;
        }

        private XmlNode GetEventDefinitionNode(XmlNode eventNode, out TriggerTypeEnum triggerType)
        {
            XmlNode definitionNode = null;
            triggerType = TriggerTypeEnum.None;
            foreach (XmlNode child in eventNode.ChildNodes)
            {
                if (child.Name == XPDLDefinition.BPMN2_ElementName_EventDefinition_Conditon)
                {
                    definitionNode = child;
                    triggerType = TriggerTypeEnum.Conditional;
                    break;
                }
                else if(child.Name == XPDLDefinition.BPMN2_ElementName_EventDefinition_Message)
                {
                    definitionNode = child;
                    triggerType = TriggerTypeEnum.Message;
                    break;
                }
                else if (child.Name == XPDLDefinition.BPMN2_ElementName_EventDefinition_Timer)
                {
                    definitionNode = child;
                    triggerType = TriggerTypeEnum.Timer;
                    break;
                }
            }
            return definitionNode;
        }

    }
}
