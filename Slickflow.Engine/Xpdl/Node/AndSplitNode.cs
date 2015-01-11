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
    /// 与分支节点
    /// </summary>
    internal class AndSplitNode : NodeBase
    {
        internal AndSplitNode(ActivityEntity activity)
            : base(activity)
        {
        }
    }
}
