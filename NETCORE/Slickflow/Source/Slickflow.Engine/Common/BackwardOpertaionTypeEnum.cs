using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 撤销操作处理类型
    /// MIS: Multiple Instance Sequence 多实例串行
    /// MIP: Multiple Instance Parallel 多实例并行
    /// </summary>
    internal enum WithdrawOperationTypeEnum
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        Default = 0,

        /// <summary>
        /// 普通模式
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 第一个多实例子节点的场景
        /// </summary>
        MISFirstOneIsRunning = 2,

        /// <summary>
        /// 串行多实例节点的中间节点
        /// </summary>
        MISOneIsRunning = 3,

        /// <summary>
        /// 当前运行节点上一步是最后一个多实例子节点的场景
        /// </summary>
        MISPreviousIsLastOne = 4,

        /// <summary>
        /// 并行多实例刚发出(子节点全部处于待办状态)
        /// </summary>
        MIPAllIsInReadyState = 10,

        /// <summary>
        /// 并行多实例中间节点，有已经完成的
        /// </summary>
        MIPSeveralIsRunning = 11,

        /// <summary>
        /// 当前运行节点上一步是最后一个多实例子节点的场景
        /// </summary>
        MIPPreviousIsLastOne = 12,

        /// <summary>
        /// 前置网关引起的多个并行节点存在，不是会签多实例节点
        /// </summary>
        GatewayFollowedByParalleledNodes = 20,

        /// <summary>
        /// 其它复杂模式的多个并行节点存在
        /// </summary>
        ExistedComplexParalleledNodes = 24
    }

    /// <summary>
    /// 退回操作处理类型
    /// </summary>
    internal enum SendBackOperationTypeEnum
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        Default = -1,

        /// <summary>
        /// 普通模式
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 多实例节点模式
        /// </summary>
        MultipleInstance = 2,

        /// <summary>
        /// 第一个多实例子节点的场景
        /// </summary>
        MISFirstOneIsRunning = 3,

        /// <summary>
        /// 串行多实例节点的中间节点
        /// </summary>
        MISOneIsRunning = 4,

        /// <summary>
        /// 当前运行节点上一步是最后一个多实例子节点的场景
        /// </summary>
        MISPreviousIsLastOne = 5,

        /// <summary>
        /// 并行多实例刚发出(子节点全部处于待办状态)
        /// </summary>
        MIPAllIsInReadyState = 10,

        /// <summary>
        /// 并行多实例中间节点，只有一个是运行状态，其他的是待办状态
        /// </summary>
        MIPOneIsRunning = 11,

        /// <summary>
        /// 并行多实例中间节点，有已经完成的
        /// </summary>
        MIPSeveralIsRunning = 12,

        /// <summary>
        /// 当前运行节点上一步是最后一个多实例子节点的场景
        /// 此时并行多实例节点状态完成
        /// </summary>
        MIPAllIsInCompletedState = 13,

        /// <summary>
        /// 多个并行节点存在，但不是会签等多实例节点
        /// </summary>
        NormalParalleledRunningNodesQuiredByTaskID = 19,

        /// <summary>
        /// 多个并行节点存在，但不是会签多实例节点
        /// </summary>
        GatewayFollowedByParalleledNodes = 20,

        /// <summary>
        /// 其它复杂模式的多个并行节点存在
        /// </summary>
        ExistedComplexParalleledNodes = 24
    }
}
