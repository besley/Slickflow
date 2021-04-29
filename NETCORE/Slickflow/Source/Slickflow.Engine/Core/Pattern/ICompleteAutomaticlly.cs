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
    /// 路由接口
    /// </summary>
    internal interface ICompleteAutomaticlly
    {
        NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityEntity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            ActivityEntity toActivity,
            WfAppRunner runner,
            IDbSession session);
    }

    /// <summary>
    /// 网关接口
    /// </summary>
    internal interface ICompleteGatewayAutomaticlly
    {
        NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityEntity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session);
    }
}
