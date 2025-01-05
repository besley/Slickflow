using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// WorkItem type
    /// 工作项类型
    /// </summary>
    public enum WorkItemTypeEnum
    {
        /// <summary>
        /// Non-WorkItem
        /// StartNode, EndNode, GatewayNode, IntermediateNode
        /// 非工作项
        /// </summary>
        NonWorkItem = 0,

        /// <summary>
        /// WorkItem
        /// TaskNode, MultipleInstanceNode
        /// 工作项
        /// </summary>
        IsWorkItem = 1
    }
}
