using System;
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
    /// XPDL Helper
    /// </summary>
    public class XPDLHelper
    {
        /// <summary>
        /// Is simple component node
        /// 是否简单组件节点
        /// </summary>
        public static Boolean IsSimpleComponentNode(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.TaskNode
                    || activityType == ActivityTypeEnum.AIServiceNode
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
        /// Is gateway component node
        /// 是否复合逻辑处理节点
        /// </summary>
        public static Boolean IsGatewayComponentNode(ActivityTypeEnum activityType)
        {
            return activityType == ActivityTypeEnum.GatewayNode;
        }

        /// <summary>
        /// Is cross over component node
        /// 是否中间事件或服务类型节点
        /// </summary>
        public static Boolean IsCrossOverComponentNode(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.IntermediateNode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsCrossOverComponentNodeContinueGoing(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.AIServiceNode
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
        /// Is intermediate timer event node
        /// 判断是否是中间Timer事件节点
        /// </summary>
        public static Boolean IsInterTimerEventComponentNode(Activity activity)
        {
            return activity.ActivityType == ActivityTypeEnum.IntermediateNode
                    && activity.TriggerDetail != null
                    && activity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer;
        }

        /// <summary>
        /// Is work item
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
        /// Get work item type by activity type
        /// 根据活动类型获取工作项类型
        /// </summary>
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
        /// Get message catch activity
        /// 获取跨流程XML的消息节点实体对象
        /// </summary>
        public static Activity GetMessageCatchActivity(ProcessInstanceEntity processInstance,
            Activity throwActivity,
            ActivityInstanceEntity throwActivityInstance,
            out ProcessEntity catchProcessEntity)
        {
            Activity catchActivity = null;
            var pm = new ProcessManager();
            var processEntity = pm.GetByVersion(processInstance.ProcessId, processInstance.Version);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(processEntity.XmlContent);
            var xnpmgr = XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc);

            var root = xmlDoc.DocumentElement;
            var xmlNodeCollaboration = root.SelectSingleNode(XPDLDefinition.BPMN_StrXmlPath_Collaboration,
                XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));

            var strFromPath = string.Format("{0}[@id='{1}']", XPDLDefinition.BPMN_StrXmlPath_MessageFlow, throwActivity.ActivityId);
            var xmlMessageFlow = xmlNodeCollaboration.SelectSingleNode(strFromPath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));

            var catchActivityId = XMLHelper.GetXmlAttribute(xmlMessageFlow, "id");
            var strXmlActivityPath = string.Format("//*[@id='{0}']", catchActivityId);

            var catchActivityNode = xmlNodeCollaboration.SelectSingleNode(strXmlActivityPath, XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc));
            var catchProcessNode = catchActivityNode.ParentNode;

            catchProcessEntity = new ProcessEntity();
            catchProcessEntity.ProcessName = XMLHelper.GetXmlAttribute(catchProcessNode, "name");
            catchProcessEntity.ProcessId = XMLHelper.GetXmlAttribute(catchProcessNode, "id");
            catchProcessEntity.Version = processInstance.Version;
            catchProcessEntity.ProcessCode = XMLHelper.GetXmlAttribute(catchProcessNode, "sf:code");

            catchActivity = ConvertHelper.ConvertXmlActivityNodeToActivityEntity(catchActivityNode, xnpmgr, catchProcessEntity.ProcessId);

            return catchActivity;
        }


        /// <summary>
        /// Get slickflow xml document namespace manager
        /// 添加XML文档的命名空间
        /// </summary>
        public static XmlNamespaceManager GetSlickflowXmlNamespaceManager(XmlDocument document, Boolean isBPMNDIContained = false)
        {
            var nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace(XPDLDefinition.BPMN_NameSpacePrefix, XPDLDefinition.BPMN_NameSpacePrefix_Value);
            nsmgr.AddNamespace(XPDLDefinition.Sf_NameSpacePrefix, XPDLDefinition.Sf_NameSpacePrefix_Value);

            if (isBPMNDIContained)
            {
                nsmgr.AddNamespace(XPDLDefinition.BPMNDI_NameSpacePrefix, XPDLDefinition.BPMNDI_NameSpacePrefix_Value);
            }
            return nsmgr;
        }

        /// <summary>
        /// Get xml path for process
        /// 获取流程节点的查找路径
        /// </summary>
        public static string GetXmlPathOfProcess(Boolean isSubProcess)
        {
            var strXmlPath = string.Empty;
            if (isSubProcess == false)
            {
                strXmlPath = XPDLDefinition.BPMN_StrXmlPath_Process;
            }
            else
            {
                strXmlPath = XPDLDefinition.BPMN_StrXmlPath_Process_Sub;
            }
            return strXmlPath;
        }

        /// <summary>
        /// Is or gateway
        /// 判断是否或网关
        /// </summary>
        public static bool IsOrGateway(XmlNode gatewayNode)
        {
            var isOr = gatewayNode.Name == XPDLDefinition.BPMN_ElementName_InclusiveGateway;
            return isOr;
        }

        /// <summary>
        /// Is xor gateway
        /// 判断是否或网关
        /// </summary>
        public static bool IsXOrGateway(XmlNode gatewayNode)
        {
            var isXOr = gatewayNode.Name == XPDLDefinition.BPMN_ElementName_ExclusiveGateway;
            return isXOr;
        }

        /// <summary>
        /// Is and gateway
        /// 判断是否或网关
        /// </summary>
        public static bool IsAndGateway(XmlNode gatewayNode)
        {
            var isAnd = gatewayNode.Name == XPDLDefinition.BPMN_ElementName_ParallelGateway;
            return isAnd;
        }
    }
}
