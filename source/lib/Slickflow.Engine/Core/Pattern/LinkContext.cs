using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 节点连接线
    /// </summary>
    public class LinkContext
    {
        /// <summary>
        /// 起始节点定义
        /// </summary>
        public Activity FromActivity { get; set; }
        
        /// <summary>
        /// 起始节点实例
        /// </summary>
        public ActivityInstanceEntity FromActivityInstance { get; set; }

        /// <summary>
        /// 到达节点定义
        /// </summary>
        public Activity ToActivity { get; set; }

        /// <summary>
        /// 到达节点实例
        /// </summary>
        public ActivityInstanceEntity ToActivityInstance { get; set; } 
    }
}
