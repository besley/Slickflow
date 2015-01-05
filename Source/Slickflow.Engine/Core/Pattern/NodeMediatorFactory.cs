using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 节点执行器的工厂类
    /// </summary>
    internal class NodeMediatorFactory
    {
        /// <summary>
        /// 创建节点执行器的抽象类
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="activity"></param>
        /// <param name="activityResource"></param>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        internal static NodeMediator CreateNodeMediator(ActivityForwardContext forwardContext,
            IDbSession session)
        {
            if (forwardContext.Activity.ActivityType == ActivityTypeEnum.StartNode)
                return new NodeMediatorStart(forwardContext, session);
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.TaskNode)
                return new NodeMediatorTask(forwardContext, session);
            else if (forwardContext.Activity.ActivityType == ActivityTypeEnum.SubProcessNode)
                return new NodeMediatorSubProcess(forwardContext, session);
            else
                throw new ApplicationException(string.Format("不明确的节点类型: {0}", forwardContext.Activity.ActivityType.ToString()));
                
        }
    }
}
