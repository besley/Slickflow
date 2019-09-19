using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Graph
{
    /// <summary>
    /// 流程图形节点样式
    /// </summary>
    public class GraphStyle
    {
        /// <summary>
        /// 设置节点样式
        /// </summary>
        /// <param name="activityEntity"></param>
        /// <returns></returns>
        public static string SetNodeStyle(ActivityEntity activityEntity)
        {
            string style = "";
            if (activityEntity.ActivityType == ActivityTypeEnum.StartNode)
            {
                style = "symbol;image=scripts/mxGraph/src/editor/images/symbols/event.png";
            }
            else if (activityEntity.ActivityType == ActivityTypeEnum.EndNode)
            {
                style = "symbol;image=scripts/mxGraph/src/editor/images/symbols/event_end.png";
            }
            else if (activityEntity.ActivityType == ActivityTypeEnum.SubProcessNode)
            {
                style = "symbol;image=scripts/mxGraph/src/editor/images/symbols/subprocess.png";
            }
            else if (activityEntity.ActivityType == ActivityTypeEnum.MultipleInstanceNode)
            {
                style = "symbol;image=scripts/mxGraph/src/editor/images/symbols/multiple_instance_task.png";
            }
            else if (activityEntity.ActivityType == ActivityTypeEnum.GatewayNode)
            {
                if (activityEntity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Split)
                {
                    style = "symbol;image=scripts/mxGraph/src/editor/images/symbols/fork.png";
                }
                else
                {
                    style = "symbol;image=scripts/mxGraph/src/editor/images/symbols/merge.png";
                }
            }
            return style;
        }
    }
}
