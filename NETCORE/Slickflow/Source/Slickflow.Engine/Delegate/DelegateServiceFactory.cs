using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// 委托服务创建类
    /// </summary>
    public class DelegateServiceFactory
    {
        /// <summary>
        /// 创建委托服务
        /// </summary>
        /// <param name="scopeType">委托类型</param>
        /// <param name="session">数据会话</param>
        /// <param name="context">上下文</param>
        /// <returns>委托服务</returns>
        public static DelegateServiceBase CreateDelegateService(DelegateScopeTypeEnum scopeType, 
            IDbSession session, 
            DelegateContext context)
        {
            if (scopeType == DelegateScopeTypeEnum.Process)
            {
                var processDelegateService = new ProcessDelegateService(session, context);
                return processDelegateService;
            }
            else if (scopeType == DelegateScopeTypeEnum.Activity)
            {
                var activityDelegateService = new ActivityDelegateService(session, context);
                return activityDelegateService;
            }
            else
            {
                throw new ApplicationException("不正确的委托类型参数！");
            }
        }
    }
}
