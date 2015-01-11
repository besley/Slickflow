using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 任务节点
    /// </summary>
    internal class TaskNode : NodeBase
    {
        internal TaskNode(ActivityEntity activity) :
            base(activity)
        {

        }
    }
}
