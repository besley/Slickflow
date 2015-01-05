using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 或分支节点
    /// </summary>
    internal class OrSplitNode : NodeBase
    {
        internal OrSplitNode(ActivityEntity activity) :
            base(activity)
        {
        }
        
    }
}
