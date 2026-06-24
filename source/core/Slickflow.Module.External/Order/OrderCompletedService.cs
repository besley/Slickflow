using System;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;

namespace Slickflow.Module.External.Order
{
    public class OrderCompletedService : ExternalServiceBase, IExternalService
    {
        public override void Execute()
        {
            //Implement your business logic
            if (EventService != null)
            {
                var id = EventService.GetProcessInstanceId();
                EventService.SaveVariable(ProcessVariableScopeEnum.Process, "price", "1000");
            }
            System.Diagnostics.Debug.WriteLine("order has been completed...");
        }
    }
}
