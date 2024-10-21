﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
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
        /// 子流程ID
        /// </summary>
        public int SubProcessID { get; set; }
        /// <summary>
        /// 子流程节点GUID
        /// </summary>
        public string SubProcessGUID { get; set; }
        /// <summary>
        /// 子流程ID的动态绑定变量名称
        /// </summary>
        public string SubVarName { get; set; }

        /// <summary>
        /// 内嵌子流程
        /// </summary>
        public Process SubProcessNested { get; set; }

        internal SubProcessNode(Activity activity) :
            base(activity)
        {

        }
    }
}
