﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 常用的一些帮助方法
    /// </summary>
    public class XPDLHelper
    {
        /// <summary>
        /// 是否简单组件节点
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <returns>判断结果</returns>
        public static Boolean IsSimpleComponentNode(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.TaskNode
                    || activityType == ActivityTypeEnum.MultiSignNode
                    || activityType == ActivityTypeEnum.SubProcessNode
                    || activityType == ActivityTypeEnum.StartNode
                    || activityType == ActivityTypeEnum.EndNode)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 是否复合逻辑处理节点
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <returns>判断结果</returns>
        public static Boolean IsGatewayComponentNode(ActivityTypeEnum activityType)
        {
            return activityType == ActivityTypeEnum.GatewayNode;
        }

        /// <summary>
        /// 是否中间事件或服务类型节点
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <returns>判断结果</returns>
        public static Boolean IsCrossOverComponentNode(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.IntermediateNode 
                || activityType == ActivityTypeEnum.ServiceNode
                || activityType == ActivityTypeEnum.ScriptNode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否是中间Timer事件节点
        /// </summary>
        /// <param name="activity">活动节点</param>
        /// <returns>判断结果</returns>
        public static Boolean IsInterTimerEventComponentNode(Activity activity)
        {
            return activity.ActivityType == ActivityTypeEnum.IntermediateNode
                    && activity.TriggerDetail != null
                    && activity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer;
        }

        /// <summary>
        /// 是否是可办理的任务节点
        /// </summary>
        public static Boolean IsWorkItem(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.TaskNode
                || activityType == ActivityTypeEnum.MultiSignNode
                || activityType == ActivityTypeEnum.SubProcessNode)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 根据活动类型获取工作项类型
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <returns>工作项类型</returns>
        public static WorkItemTypeEnum GetWorkItemType(ActivityTypeEnum activityType)
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
        /// 获取跨流程XML的消息节点实体对象
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="throwActivity">消息抛出节点</param>
        /// <param name="throwActivityInstance">消息抛出节点活动实例</param>
        /// <param name="catchProcessEntity">消息捕获流程实体对象</param>
        /// <returns>消息接收节点实体</returns>
        public static Activity GetMessageCatchActivity(ProcessInstanceEntity processInstance,
            Activity throwActivity,
            ActivityInstanceEntity throwActivityInstance,
            out ProcessEntity catchProcessEntity)
        {
            Activity catchActivity = null;
            var pm = new ProcessManager();
            var processEntity = pm.GetByVersion(processInstance.ProcessGUID, processInstance.Version);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(processEntity.XmlContent);
            var xnpmgr = XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc);

            var root = xmlDoc.DocumentElement;
            var xmlNodeCollaboration = root.SelectSingleNode(XPDLDefinition.BPMN2_StrXmlPath_Collaboration,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));

            var strFromPath = string.Format("{0}[@sf:from='{1}']", XPDLDefinition.BPMN2_StrXmlPath_MessageFlow, throwActivity.ActivityGUID);
            var xmlMessageFlow = xmlNodeCollaboration.SelectSingleNode(strFromPath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));

            var catchActivityGUID = XMLHelper.GetXmlAttribute(xmlMessageFlow, "sf:to");
            var strXmlActivityPath = string.Format("//*[@sf:guid='{0}']", catchActivityGUID);

            var catchActivityNode = xmlNodeCollaboration.SelectSingleNode(strXmlActivityPath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            var catchProcessNode = catchActivityNode.ParentNode;

            catchProcessEntity = new ProcessEntity();
            catchProcessEntity.ProcessName = XMLHelper.GetXmlAttribute(catchProcessNode, "name");
            catchProcessEntity.ProcessGUID = XMLHelper.GetXmlAttribute(catchProcessNode, "sf:guid");
            catchProcessEntity.Version = processInstance.Version;
            catchProcessEntity.ProcessCode = XMLHelper.GetXmlAttribute(catchProcessNode, "sf:code");

            catchActivity = ConvertHelper.ConvertXmlActivityNodeToActivityEntity(catchActivityNode, xnpmgr, catchProcessEntity.ProcessGUID);

            return catchActivity;
        }


        /// <summary>
        /// 添加XML文档的命名空间
        /// </summary>
        /// <param name="document">XML文档</param>
        /// <param name="isBPMNDIContained">是否包含Shape节点</param>
        /// <returns>XML命名空间管理</returns>
        public static XmlNamespaceManager GetSlickflowXmlNamespaceManager(XmlDocument document, Boolean isBPMNDIContained = false)
        {
            var nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace(XPDLDefinition.BPMN2_NameSpacePrefix, XPDLDefinition.BPMN2_NameSpacePrefix_Value);
            nsmgr.AddNamespace(XPDLDefinition.Sf_NameSpacePrefix, XPDLDefinition.Sf_NameSpacePrefix_Value);

            if (isBPMNDIContained)
            {
                nsmgr.AddNamespace(XPDLDefinition.BPMNDI_NameSpacePrefix, XPDLDefinition.BPMNDI_NameSpacePrefix_Value);
            }
            return nsmgr;
        }

        /// <summary>
        /// 获取流程节点的查找路径
        /// </summary>
        /// <param name="isSubProcess">是否子流程</param>
        /// <returns>XML路径</returns>
        public static string GetXmlPathOfProcess(Boolean isSubProcess)
        {
            var strXmlPath = string.Empty;
            if (isSubProcess == false)
            {
                strXmlPath = XPDLDefinition.BPMN2_StrXmlPath_Process;
            }
            else
            {
                strXmlPath = XPDLDefinition.BPMN2_StrXmlPath_Process_Sub;
            }
            return strXmlPath;
        }

        /// <summary>
        /// 判断是否或网关
        /// </summary>
        /// <param name="gatewayNode">网关节点</param>
        /// <returns></returns>
        public static bool IsOrGateway(XmlNode gatewayNode)
        {
            var isOr = gatewayNode.Name == XPDLDefinition.BPMN2_ElementName_InclusiveGateway;
            return isOr;
        }

        /// <summary>
        /// 判断是否或网关
        /// </summary>
        /// <param name="gatewayNode">网关节点</param>
        public static bool IsXOrGateway(XmlNode gatewayNode)
        {
            var isXOr = gatewayNode.Name == XPDLDefinition.BPMN2_ElementName_ExclusiveGateway;
            return isXOr;
        }

        /// <summary>
        /// 判断是否或网关
        /// </summary>
        /// <param name="gatewayNode">网关节点</param>
        public static bool IsAndGateway(XmlNode gatewayNode)
        {
            var isAnd = gatewayNode.Name == XPDLDefinition.BPMN2_ElementName_ParallelGateway;
            return isAnd;
        }
    }
}
