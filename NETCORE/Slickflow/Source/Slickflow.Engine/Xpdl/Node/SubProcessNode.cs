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
        public SubProcessTypeEnum SubProcessType { get; set; }
        public string SubProcessGUID { get; set; }
        public string SubVarName { get; set; }

        internal SubProcessNode(ActivityEntity activity) :
            base(activity)
        {

        }
    }
}
