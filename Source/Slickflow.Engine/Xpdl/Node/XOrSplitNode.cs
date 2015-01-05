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
    /// 异或类型的节点
    /// </summary>
    internal class XOrSplitNode : NodeBase
    {
        internal XOrSplitNode(ActivityEntity activity)
            : base(activity)
        {
        }
        
    }
}
