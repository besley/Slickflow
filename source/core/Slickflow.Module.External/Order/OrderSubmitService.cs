using Slickflow.Engine.Common;
using Slickflow.Engine.External;

namespace Slickflow.Module.External.Order
{
    /// <summary>
    /// Order Submit Serivce
    /// 订单提交服务类(对应订单流程中订单提交节点)
    /// </summary>
    public class OrderSubmitService : ExternalServiceBase, IExternalService
    {
        /// <summary>
        /// Business Logic Process
        /// </summary>
        public override void Execute()
        {
            //Implement your business logic here
            //实现用户自己的业务逻辑
            //...

            //get variable from runner
            //var a = this.DynamicVariables["amount"];

            //get varaible from WfProcessVariable table
            if (EventService != null)
            {
                var id = EventService.GetProcessInstanceId();
                var amount = EventService.GetVariable(ProcessVariableScopeEnum.Process, "amount");

                DoSomethingElse(amount, 20);
            }
            System.Diagnostics.Debug.WriteLine("order has been submitted...");
        }

        /// <summary>
        /// Some other business logic process
        /// 业务逻辑具体实现方法
        /// </summary>
        /// <param name="amount">amount</param>
        /// <param name="newAmount">set new amount</param>
        private void DoSomethingElse(string amount, int newAmount)
        {
            var intAmount = 0;
            int.TryParse(amount, out intAmount);

            if (intAmount < newAmount)
            {
                EventService.SaveVariable(ProcessVariableScopeEnum.Process, "amount", newAmount.ToString());
            }

            //Do something else here
            //调用其它业务处理逻辑
            var session = EventService.GetSession();

            //You can still do something else...
            //实现其它数据库业务逻辑
            //.............................
        }
    }
}
