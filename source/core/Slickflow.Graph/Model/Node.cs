using System;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Module.Resource;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Node Entity
    /// 节点实体
    /// </summary>
    public class Node
    {
        #region Property and Constructor
        public string Id { get; set; }
        public string Name { get { return Activity.ActivityName; } }
        public string Code { get { return Activity.ActivityCode; } }
        public ActivityTypeEnum NodeType { get { return Activity.ActivityType; } }
        internal Activity Activity { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Level { get; set; }
        public IList<Role> RoleList { get; set; }

        public Node(ActivityTypeEnum activityType, string name, string code = null, TriggerTypeEnum triggerType = TriggerTypeEnum.None)
        {
            Id = string.Format("{0}_{1}", activityType.ToString(), Utility.GetRandomInt().ToString());
            Activity = new Activity();
            Activity.ActivityName = name; 
            Activity.ActivityId = Guid.NewGuid().ToString();
            Activity.ActivityType = activityType;

            if (!string.IsNullOrEmpty(code))
            {
                Activity.ActivityCode = code;
            }

            //活动节点类型及触发类型
            //Activity node type and trigger type
            if (triggerType != TriggerTypeEnum.None)
            {
                var triggerDetail = new TriggerDetail();
                triggerDetail.TriggerType = triggerType;
                Activity.TriggerDetail = triggerDetail;
            }
        }
        #endregion
    }
}
