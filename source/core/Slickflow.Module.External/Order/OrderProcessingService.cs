using System;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;

namespace Slickflow.Module.External.Order
{
    public class OrderProcessingService : ExternalServiceBase, IExternalService
    {
        /// <summary>
        /// 业务逻辑前置调用方法
        /// </summary>
        public override void Execute()
        {
            //实现用户自己的业务逻辑
            if (EventService != null)
            {
                var id = EventService.GetProcessInstanceId();
                EventService.SaveVariable(ProcessVariableScopeEnum.Process, "date", System.DateTime.Now.ToShortDateString());
            }
            System.Diagnostics.Debug.WriteLine("order has been processing...");
        }
    }
}
