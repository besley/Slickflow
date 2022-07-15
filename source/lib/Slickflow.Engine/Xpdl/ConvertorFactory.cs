using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Convertor;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 转变器工厂类
    /// </summary>
    public class ConvertorFactory
    {
        /// <summary>
        /// 转变器构造工厂方法
        /// </summary>
        /// <param name="xmlNode">XML节点</param>
        /// <param name="xnpmgr">XML命名空间管理</param>
        /// <param name="activityType">节点类型</param>
        /// <returns>IConvert接口</returns>
        /// <exception cref="ApplicationException"></exception>
        public static IConvert CreateConvertor(XmlNode xmlNode, XmlNamespaceManager xnpmgr, out ActivityTypeEnum activityType)
        {
            IConvert convert = null;
            activityType = GetActivityTypeByNodeName(xmlNode);
            if (activityType == ActivityTypeEnum.Unknown)
            {
                throw new ApplicationException(String.Format("ConvertorFactory:Not suppported convertor as of node type:{0}",
                    activityType.ToString()));
            }
            else if (activityType == ActivityTypeEnum.GatewayNode)
            {
                convert = new GatewayConvertor(xmlNode, xnpmgr);
            }
            else if(activityType == ActivityTypeEnum.SubProcessNode)
            {
                convert = new SubProcessConvertor(xmlNode, xnpmgr);
            }
            else if(activityType == ActivityTypeEnum.MultiSignNode)
            {
                convert = new MultiSignConvertor(xmlNode, xnpmgr);
            }
            else if(activityType == ActivityTypeEnum.StartNode
                || activityType == ActivityTypeEnum.IntermediateNode
                || activityType == ActivityTypeEnum.EndNode)
            {
                convert = new EventConvertor(xmlNode, xnpmgr);
            }
            else
            {
                convert = new DefaultlConvertor(xmlNode, xnpmgr);
            }
            return convert;
        }

        /// <summary>
        /// 获取活动节点类型
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <returns>活动节点类型</returns>
        /// <exception cref="ApplicationException"></exception>
        private static ActivityTypeEnum GetActivityTypeByNodeName(XmlNode node)
        {
            ActivityTypeEnum activityType = ActivityTypeEnum.Unknown;
            if (node.Name == XPDLDefinition.BPMN2_ElementName_StartEvent)
            {
                activityType = ActivityTypeEnum.StartNode;
            }
            else if (node.Name == XPDLDefinition.BPMN2_ElementName_EndEvent)
            {
                activityType = ActivityTypeEnum.EndNode;
            }
            else if (node.Name == XPDLDefinition.BPMN2_ElementName_IntermediateEvent_Catch
                || node.Name == XPDLDefinition.BPMN2_ElementName_IntermediateEvent_Throw)
            {
                //中间事件节点
                activityType = ActivityTypeEnum.IntermediateNode;
            }
            else if (node.Name == XPDLDefinition.BPMN2_ElementName_SubProcess)
            {
                activityType = ActivityTypeEnum.SubProcessNode;
            }
            else if (node.Name == XPDLDefinition.BPMN2_ElementName_Task
                || node.Name == XPDLDefinition.BPMN2_ElementName_UserTask
                || node.Name == XPDLDefinition.BPMN2_ElementName_ManualTask)
            {
                //判断是否为多人会签节点
                var multipleDetailNode = node.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_MultiSignDetail, 
                    XPDLHelper.GetSlickflowXmlNamespaceManager(node.OwnerDocument));
                if (multipleDetailNode != null) activityType = ActivityTypeEnum.MultiSignNode;
                else activityType = ActivityTypeEnum.TaskNode;
            }
            else if(node.Name == XPDLDefinition.BPMN2_ElementName_ServiceTask)
            {
                activityType = ActivityTypeEnum.ServiceNode;
            }
            else if(node.Name == XPDLDefinition.BPMN2_ElementName_ScriptTask)
            {
                activityType = ActivityTypeEnum.ScriptNode;
            }
            else if (node.Name == XPDLDefinition.BPMN2_ElementName_ExclusiveGateway
                || node.Name == XPDLDefinition.BPMN2_ElementName_InclusiveGateway
                || node.Name == XPDLDefinition.BPMN2_ElementName_ParallelGateway)
            {
                //各类网关节点
                activityType = ActivityTypeEnum.GatewayNode;
            }
            else
            {
                throw new ApplicationException(string.Format("ConvertorFactory:Not supported node name:{0}", node.Name));
            }
            return activityType;
        }
    }
}
