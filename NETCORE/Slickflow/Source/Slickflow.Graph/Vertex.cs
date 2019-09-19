using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Module.Resource;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Graph
{
    /// <summary>
    /// 节点实体
    /// </summary>
    public class Vertex
    {
        #region 属性及构造方法
        public string Name { get { return Activity.ActivityName; } }
        public string Code { get { return Activity.ActivityCode; } }
        public string ActivityGUID { get { return Activity.ActivityGUID; } }
        public ActivityTypeEnum VertexType { get { return Activity.ActivityType; } }
        internal ActivityEntity Activity { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Level { get; set; }
        public IList<Role> RoleList { get; set; }

        /// <summary>
        /// 节点构造方法
        /// </summary>
        /// <param name="activityType">节点类型</param>
        /// <param name="name">名称</param>
        /// <param name="code">代码</param>
        public Vertex(ActivityTypeEnum activityType, string name, string code = null)
        {
            Activity = new ActivityEntity();
            Activity.ActivityName = name;
            
            if (!string.IsNullOrEmpty(code))
            {
                Activity.ActivityCode = code;
            }

            Activity.ActivityType = activityType;
        }
        #endregion
    }
}
