using System;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next Activity Schedule Factory
    /// </summary>
    internal class NextActivityScheduleFactory
    {
        /// <summary>
        /// Create ActivitySchedule
        /// </summary>
        internal static NextActivityScheduleBase CreateActivitySchedule(IProcessModel processModel,
            GatewaySplitJoinTypeEnum splitJoinType,
            Nullable<int> activityInstanceId = null)
        {
            NextActivityScheduleBase activitySchedule = null;
            if (splitJoinType == GatewaySplitJoinTypeEnum.Split)
            {
                activitySchedule = new NextActivityScheduleSplit(processModel, activityInstanceId);
            }
            else if (splitJoinType == GatewaySplitJoinTypeEnum.Join)
            {
                activitySchedule = new NextActivityScheduleJoin(processModel);
            }
            else
            {
                throw new Exception(LocalizeHelper.GetEngineMessage("nextactivityschedulefactory.unknownnodetype"));
            }
            return activitySchedule;
        }

        /// <summary>
        /// Create ActivitySchedule
        /// </summary>
        /// <param name="processModel"></param>
        /// <returns></returns>
        internal static NextActivityScheduleBase CreateActivityScheduleIntermediate(IProcessModel processModel)
        {
            NextActivityScheduleBase activitySchedule = new NextActivityScheduleIntermediate(processModel);
            return activitySchedule;
        }
    }
}
