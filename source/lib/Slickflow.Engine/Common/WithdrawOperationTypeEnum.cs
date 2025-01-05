using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Withdraw operation processing type
    /// MIS: Multiple Instance Sequence 多实例串行
    /// MIP: Multiple Instance Parallel 多实例并行
    /// 撤销操作处理类型
    /// </summary>
    internal enum WithdrawOperationTypeEnum
    {
        /// <summary>
        /// Default
        /// 缺省值
        /// </summary>
        Default = 0,

        /// <summary>
        /// Normal
        /// 普通模式
        /// </summary>
        Normal = 1,

        /// <summary>
        /// The scene of the first multi instance node
        /// 第一个多实例子节点的场景
        /// </summary>
        MISFirstOneIsRunning = 2,

        /// <summary>
        /// Intermediate nodes of serial multi instance nodes
        /// 串行多实例节点的中间节点
        /// </summary>
        MISOneIsRunning = 3,

        /// <summary>
        /// The previous step on the current running node is the scenario of the last multi instance node
        /// 当前运行节点上一步是最后一个多实例子节点的场景
        /// </summary>
        MISPreviousIsLastOne = 4,

        /// <summary>
        /// Parallel multiple instances just sent out (all child nodes are in ready state)
        /// 并行多实例刚发出(子节点全部处于待办状态)
        /// </summary>
        MIPAllIsInReadyState = 10,

        /// <summary>
        /// Parallel multi instance intermediate nodes with completed ones
        /// 并行多实例中间节点，有已经完成的
        /// </summary>
        MIPSeveralIsRunning = 11,

        /// <summary>
        /// The previous step on the current running node is the scenario of the last multi instance node
        /// 当前运行节点上一步是最后一个多实例子节点的场景
        /// </summary>
        MIPPreviousIsLastOne = 12,

        /// <summary>
        /// The presence of multiple parallel nodes caused by the front-end gateway is not a sign off of multiple instance nodes
        /// 前置网关引起的多个并行节点存在，不是会签多实例节点
        /// </summary>
        GatewayFollowedByParalleledNodes = 20,

        /// <summary>
        /// Multiple parallel nodes exist in other complex patterns
        /// 其它复杂模式的多个并行节点存在
        /// </summary>
        ExistedComplexParalleledNodes = 24
    }
}
