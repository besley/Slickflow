using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 子流程节点
    /// </summary>
    internal class SubProcessNode : NodeBase
    {
        public string SubProcessGUID { get; set; }

        internal SubProcessNode(ActivityEntity activity) :
            base(activity)
        {

        }
    }
}
