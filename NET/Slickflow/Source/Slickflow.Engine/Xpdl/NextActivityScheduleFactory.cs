using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// ActivitySchedule的工厂类
    /// </summary>
    internal class NextActivityScheduleFactory
    {
        /// <summary>
        /// 创建ActivitySchedule
        /// </summary>
        /// <param name="splitJoinType"></param>
        /// <returns></returns>
        internal static NextActivityScheduleBase CreateActivitySchedule(IProcessModel processModel,
            GatewaySplitJoinTypeEnum splitJoinType)
        {
            NextActivityScheduleBase activitySchedule = null;
            if (splitJoinType == GatewaySplitJoinTypeEnum.Split)
            {
                activitySchedule = new NextActivityScheduleSplit(processModel);
            }
            else if (splitJoinType == GatewaySplitJoinTypeEnum.Join)
            {
                activitySchedule = new NextActivityScheduleJoin(processModel);
            }
            else
            {
                throw new Exception("未知的splitJoinType!");
            }
            return activitySchedule;
        }
    }
}
