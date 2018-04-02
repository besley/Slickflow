using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 常用的一些帮助方法
    /// </summary>
    internal class XPDLHelper
    {
        /// <summary>
        /// 是否简单组件节点
        /// </summary>
        /// <param name="acitivytType">活动类型</param>
        /// <returns>判断结果</returns>
        internal static Boolean IsSimpleComponentNode(ActivityTypeEnum acitivytType)
        {
            if (acitivytType == ActivityTypeEnum.TaskNode
                    || acitivytType == ActivityTypeEnum.MultipleInstanceNode
                    || acitivytType == ActivityTypeEnum.ScriptNode
                    || acitivytType == ActivityTypeEnum.PluginNode
                    || acitivytType == ActivityTypeEnum.StartNode
                    || acitivytType == ActivityTypeEnum.EndNode)
            {
                return true;
            }
            else
                return false;
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
    }
}
