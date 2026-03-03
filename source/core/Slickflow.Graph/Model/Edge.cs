using System.Collections.Generic;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Transition Entity
    /// 连线实体
    /// </summary>
    public class Edge
    {
        #region Property and Constructor
        public string Id { get; set; }
        public Node Source { get; set; }
        public Node Target { get; set; }
        public string SourceRef { get; set; }
        public string TargetRef { get; set; }
        public string Description { get { return Transition.Description; } }
        //public ConditionEntity Condition { get; set; }
        //public Receiver Receiver { get; set; }
        internal Transition Transition { get; set; }
        public int BranchOrder { get; set; }

        public Edge(string description = null)
        {
            Transition = new Transition();
            Transition.Description = description;
        }
        #endregion
    }
}
