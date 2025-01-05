
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Cross gateway object encapsulation
    /// 跨网关对象封装
    /// </summary>
    internal class CrossOverGatewayDetail
    {
        /// <summary>
        /// Wether to cross over gateway
        /// 是否跨越网关
        /// </summary>
        internal Boolean IsCrossOverGateway { get; set; }

        /// <summary>
        /// Parallelled choices ndoes
        /// 并行可选择节点
        /// </summary>
        internal IList<ActivityInstanceEntity> ParallelledChoicesNodes { get; set; }

        /// <summary>
        /// Previous actvity instance
        /// 前置活动实例
        /// </summary>
        internal ActivityInstanceEntity PreviousActivityInstance { get; set; }
    }
}
