using System;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Delegate.Event;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// Delegate Service Factory
    /// 委托服务创建类
    /// </summary>
    public class DelegateServiceFactory
    {
        /// <summary>
        /// Create Delegate Service
        /// 创建委托服务
        /// </summary>
        public static DelegateServiceBase CreateDelegateService(DelegateScopeTypeEnum scopeType, 
            IDbSession session, 
            DelegateContext context)
        {
            if (scopeType == DelegateScopeTypeEnum.Process)
            {
                var processDelegateService = new ProcessEventService(session, context);
                return processDelegateService;
            }
            else if (scopeType == DelegateScopeTypeEnum.Activity)
            {
                var activityDelegateService = new ActivityEventService(session, context);
                return activityDelegateService;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("delegateservicefactory.CreateDelegateService.error"));
            }
        }
    }
}
