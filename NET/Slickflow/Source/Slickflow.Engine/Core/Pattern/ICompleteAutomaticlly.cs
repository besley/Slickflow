using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 路由接口
    /// </summary>
    internal interface ICompleteAutomaticlly
    {
        GatewayExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityResource activityResource,
            IDbSession session);
    }
}
