using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 节点连接线
    /// </summary>
    public class Linker
    {
        /// <summary>
        /// 起始节点定义
        /// </summary>
        public ActivityEntity FromActivity { get; set; }
        
        /// <summary>
        /// 起始节点实例
        /// </summary>
        public ActivityInstanceEntity FromActivityInstance { get; set; }

        /// <summary>
        /// 到达节点定义
        /// </summary>
        public ActivityEntity ToActivity { get; set; }

        /// <summary>
        /// 到达节点实例
        /// </summary>
        public ActivityInstanceEntity ToActivityInstance { get; set; } 
    }
}
