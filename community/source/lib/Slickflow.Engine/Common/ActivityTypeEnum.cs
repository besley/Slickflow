using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 活动类型
    /// </summary>
    public enum ActivityTypeEnum : int
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 开始事件节点
        /// </summary>
        StartNode = 1,

        /// <summary>
        /// 结束事件节点
        /// </summary>
        EndNode = 2,

        /// <summary>
        /// 中间事件节点
        /// </summary>
        IntermediateNode = 3,

        /// <summary>
        /// 人工任务节点
        /// </summary>
        TaskNode = 4,

        /// <summary>
        /// 子流程节点
        /// </summary>
        SubProcessNode = 5,

        /// <summary>
        /// 多实例节点
        /// </summary>
        MultiSignNode = 6,

        /// <summary>
        /// 网关节点
        /// </summary>
        GatewayNode = 8,

        /// <summary>
        /// 代码服务节点
        /// </summary>
        ServiceNode = 16,

        /// <summary>
        /// 脚本节点
        /// </summary>
        ScriptNode = 18
    }
}
