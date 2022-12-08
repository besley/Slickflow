using System;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// ActivitySchedule的工厂类
    /// </summary>
    internal class NextActivityScheduleFactory
    {
        /// <summary>
        /// 创建ActivitySchedule
        /// </summary>
        /// <param name="processModel">流程模型</param>
        /// <param name="splitJoinType">分支合并类型</param>
        /// <param name="taskID">任务ID</param>
        /// <returns>下一步调度类</returns>
        internal static NextActivityScheduleBase CreateActivitySchedule(IProcessModel processModel,
            GatewaySplitJoinTypeEnum splitJoinType,
            Nullable<int> taskID = null)
        {
            NextActivityScheduleBase activitySchedule = null;
            if (splitJoinType == GatewaySplitJoinTypeEnum.Split)
            {
                activitySchedule = new NextActivityScheduleSplit(processModel, taskID);
            }
            else if (splitJoinType == GatewaySplitJoinTypeEnum.Join)
            {
                activitySchedule = new NextActivityScheduleJoin(processModel);
            }
            else
            {
                //未知的splitJoinType
                throw new Exception(LocalizeHelper.GetEngineMessage("nextactivityschedulefactory.unknownnodetype"));
            }
            return activitySchedule;
        }

        /// <summary>
        /// 创建ActivitySchedule
        /// </summary>
        /// <param name="processModel">流程模型</param>
        /// <returns>下一步调度类</returns>
        internal static NextActivityScheduleBase CreateActivityScheduleIntermediate(IProcessModel processModel)
        {
            NextActivityScheduleBase activitySchedule = new NextActivityScheduleIntermediate(processModel);
            return activitySchedule;
        }
    }
}
