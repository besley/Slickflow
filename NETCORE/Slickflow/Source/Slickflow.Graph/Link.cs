using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Graph
{
    /// <summary>
    /// 连续实体
    /// </summary>
    public class Link
    {
        #region 属性及构造方法
        public Vertex Source { get; set; }
        public Vertex Target { get; set; }
        public string Description { get { return Transition.Description; } }
        //public ConditionEntity Condition { get; set; }
        //public Receiver Receiver { get; set; }
        internal TransitionEntity Transition { get; set; }

        public Link(string description = null)
        {
            Transition = new TransitionEntity();
            Transition.Description = description;
        }
        #endregion
    }
}
