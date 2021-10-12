using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Utility;

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
        internal static Boolean IsSimpleComponentNode(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.TaskNode
                    || activityType == ActivityTypeEnum.MultipleInstanceNode
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
        internal static Boolean IsGatewayComponentNode(ActivityTypeEnum activityType)
        {
            return activityType == ActivityTypeEnum.GatewayNode;
        }

        /// <summary>
        /// 是否中间事件或服务类型节点
        /// </summary>
        /// <param name="activityType">活动类型</param>
        /// <returns>判断结果</returns>
        internal static Boolean IsCrossOverComponentNode(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.IntermediateNode 
                || activityType == ActivityTypeEnum.ServiceNode)
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
        internal static Boolean IsInterTimerEventComponentNode(ActivityEntity activity)
        {
            return activity.ActivityType == ActivityTypeEnum.IntermediateNode
                    && activity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Timer;
        }

        /// <summary>
        /// 是否是可办理的任务节点
        /// </summary>
        internal static Boolean IsWorkItem(ActivityTypeEnum activityType)
        {
            if (activityType == ActivityTypeEnum.TaskNode
                || activityType == ActivityTypeEnum.MultipleInstanceNode
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
        internal static WorkItemTypeEnum GetWorkItemType(ActivityTypeEnum activityType)
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.NonWorkItem;

            if (activityType == ActivityTypeEnum.TaskNode
                || activityType == ActivityTypeEnum.MultipleInstanceNode
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
        internal static ActivityEntity GetMessageCatchActivity(ProcessInstanceEntity processInstance,
            ActivityEntity throwActivity,
            ActivityInstanceEntity throwActivityInstance,
            out ProcessEntity catchProcessEntity)
        {
            ActivityEntity catchActivity = null;
            var pm = new ProcessManager();
            var processEntity = pm.GetByVersion(processInstance.ProcessGUID, processInstance.Version);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(processEntity.XmlContent);

            var packageNode = xmlDoc.DocumentElement;
            var messagesNode = packageNode.SelectSingleNode("Layout/Messages");
            var messageNode = messagesNode.SelectSingleNode(string.Format("Message[@from='{0}']", throwActivity.ActivityGUID));
            var catchActivityGUID = XMLHelper.GetXmlAttribute(messageNode, "to");

            var catchActivityNode = packageNode.SelectSingleNode(string.Format("WorkflowProcesses/Process/Activities/Activity[@id='{0}']", catchActivityGUID));
            var catchProcessNode = catchActivityNode.ParentNode.ParentNode;

            catchProcessEntity = new ProcessEntity();
            catchProcessEntity.ProcessGUID = XMLHelper.GetXmlAttribute(catchProcessNode, "id");
            catchProcessEntity.Version = XMLHelper.GetXmlAttribute(catchProcessNode, "version");
            catchProcessEntity.ProcessName = XMLHelper.GetXmlAttribute(catchProcessNode, "name");
            catchProcessEntity.ProcessCode = XMLHelper.GetXmlAttribute(catchProcessNode, "code");

            catchActivity = ConvertHelper.ConvertXmlActivityNodeToActivityEntity(catchActivityNode, catchProcessEntity.ProcessGUID);

            return catchActivity;
        }
    }
}
