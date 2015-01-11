using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 互斥合并类型的节点
    /// </summary>
    internal class XOrJoinNode : NodeBase
    {
        internal XOrJoinNode(ActivityEntity activity)
            : base(activity)
        {

        }
    }
}
