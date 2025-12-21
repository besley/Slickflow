using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// Sub Process Node
    /// 子流程节点
    /// </summary>
    public class SubProcessNode : NodeBase
    {
        /// <summary>
        /// SubProcessId
        /// </summary>
        public int SubProcessDefId { get; set; }
        /// <summary>
        /// SubProcessId
        /// </summary>
        public string SubProcessId { get; set; }
        /// <summary>
        /// Dynamically bound variable name for subprocess Id
        /// 子流程ID的动态绑定变量名称
        /// </summary>
        public string SubVarName { get; set; }

        /// <summary>
        /// Embedded subprocess
        /// 内嵌子流程
        /// </summary>
        public Process SubProcessNested { get; set; }

        internal SubProcessNode(Activity activity) :
            base(activity)
        {

        }
    }
}
