using System;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.SendBack
{
    /// <summary>
    /// 退回操作类型
    /// </summary>
    internal class SendBackOperation
    {
        internal SendBackOperationTypeEnum CurrentNodeOperationType { get; set; }
        internal SendBackOperationTypeEnum CurrentMultipleInstanceDetailType { get; set; }
        internal SendBackOperationTypeEnum PreviousNodeOperationType { get; set; }
        internal BackwardTypeEnum BackwardType { get; set; }
        internal Boolean HasGatewayPassed { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        internal ActivityInstanceEntity BackwardFromActivityInstance { get; set; }
        internal Activity BackwardToTaskActivity { get; set; }
        internal Performer BackwardToTaskPerformer { get; set; }
        internal ActivityResource ActivityResource { get; set; }
        internal IProcessModel ProcessModel { get; set; }
        internal Boolean IsCancellingBrothersNode { get; set; }
    }
}
