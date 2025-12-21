using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Complete Automatically
    /// </summary>
    internal interface ICompletedAutomaticlly
    {
        NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionId,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            ActivityInstanceEntity currentActivityInstance,
            WfAppRunner runner,
            IDbSession session);
    }

    /// <summary>
    /// Complete Automatically
    /// </summary>
    internal interface ICompleteGatewayAutomaticlly
    {
        NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionId,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session);
    }
}
