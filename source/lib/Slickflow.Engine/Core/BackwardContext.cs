using System;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;


namespace Slickflow.Engine.Core
{
    /// <summary>
    /// Context object during process backward processing
    /// 流程回退处理时的上下文对象
    /// </summary>
    internal class BackwardContext
    {
        internal Activity BackwardToTaskActivity { get; set; }
        internal ActivityInstanceEntity BackwardToTaskActivityInstance { get; set; }
        internal Activity BackwardFromActivity { get; set; }
        internal ActivityInstanceEntity BackwardFromActivityInstance { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        internal String BackwardToTargetTransitionGUID { get; set; }
        internal WfBackwardTaskReceiver BackwardTaskReceiver { get; set; }
        internal ActivityResource ActivityResource { get; set; }
        internal ActivityInstanceEntity PreviousActivityInstance { get; set; }
        internal CrossOverGatewayDetail CrossOverGatewayDetail { get; set; }
        internal WithdrawOperationTypeEnum WithdrawOperationType { get; set; }
        internal SendBackOperationTypeEnum SendbackOperationType { get; set; }
    }
}
