using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 并行合并节点
    /// </summary>
    internal class AndJoinNode : NodeBase
    {
        internal AndJoinNode(ActivityEntity activity)
            : base(activity)
        {

        }
    }
}
