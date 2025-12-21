using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Activity Type
    /// 活动类型
    /// </summary>
    public enum ActivityTypeEnum : int
    {
        /// <summary>
        /// Unknown
        /// 未知类型
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Start Node
        /// 开始事件节点
        /// </summary>
        StartNode = 1,

        /// <summary>
        /// End Node
        /// 结束事件节点
        /// </summary>
        EndNode = 2,

        /// <summary>
        /// Intermediated Event Node
        /// 中间事件节点
        /// </summary>
        IntermediateNode = 3,

        /// <summary>
        /// Task Node
        /// 人工任务节点
        /// </summary>
        TaskNode = 4,

        /// <summary>
        /// Sub Process Node
        /// 子流程节点
        /// </summary>
        SubProcessNode = 5,

        /// <summary>
        /// Multiple Instance Node
        /// 多实例节点
        /// </summary>
        MultiSignNode = 6,

        /// <summary>
        /// Gateway Node
        /// 网关节点
        /// </summary>
        GatewayNode = 8,

        /// <summary>
        /// Service Node
        /// 代码服务节点
        /// </summary>
        ServiceNode = 16,

        /// <summary>
        /// AI Service Node
        /// AI大模型节点
        /// </summary>
        AIServiceNode = 17,

        /// <summary>
        /// Script Node
        /// 脚本节点
        /// </summary>
        ScriptNode = 18
    }
}
