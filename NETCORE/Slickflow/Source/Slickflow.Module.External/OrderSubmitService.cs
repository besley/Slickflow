using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;

namespace Slickflow.Module.External
{
    /// <summary>
    /// 订单提交服务类(对应订单流程中订单提交节点)
    /// </summary>
    public class OrderSubmitService : ExternalServiceBase, IExternalService
    {
        /// <summary>
        /// 业务逻辑前置调用方法
        /// </summary>
        public override void Execute()
        {
            //实现用户自己的业务逻辑

            //get variable from runner
            //var a = this.DynamicVariables["amount"];

            //get varaible from WfProcessVariable table
            var id = DelegateService.GetProcessInstanceID();
            var amount = DelegateService.GetVariable(ProcessVariableTypeEnum.Process,  "amount");

            DoSomethingElse(amount, 20);
        }

        /// <summary>
        /// 业务逻辑具体实现方法
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="newAmount"></param>
        private void DoSomethingElse(string amount, int newAmount)
        {
            var intAmount = 0;
            int.TryParse(amount, out intAmount);

            if (intAmount < newAmount)
            {
                DelegateService.SaveVariable(ProcessVariableTypeEnum.Process, "amount", newAmount.ToString());
            }

            //调用其它业务处理逻辑
            var session = DelegateService.GetSession();

            //实现其它数据库业务逻辑
            //.............................
        }
    }
}
