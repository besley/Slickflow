using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Jump information described on node type
    /// 节点类型上描述的跳转信息
    /// </summary>
    public class SkipDetail
    {
        /// <summary>
        /// Whether to Skip
        /// 是否跳转
        /// </summary>
        public bool IsSkip { get; set; }

        /// <summary>
        /// Skip to which node
        /// 跳转到的节点
        /// </summary>
        public string Skipto { get; set; }
    }
}
