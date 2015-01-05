using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 开始节点
    /// </summary>
    internal class StartNode : NodeBase
    {
        internal StartNode(ActivityEntity activity)
            : base(activity)
        {

        }
    }
}
