using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 子流程节点
    /// </summary>
    public class SubProcessNode : NodeBase
    {
        /// <summary>
        /// 子流程类型
        /// </summary>
        public SubProcessTypeEnum SubProcessType { get; set; }
        /// <summary>
        /// 子流程节点GUID
        /// </summary>
        public string SubProcessGUID { get; set; }
        /// <summary>
        /// 子流程ID的动态绑定变量名称
        /// </summary>
        public string SubVarName { get; set; }

        internal SubProcessNode(Activity activity) :
            base(activity)
        {

        }
    }
}
