using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// 串行序列流程测试流程
    /// </summary>
    public class WfTutorialController : Controller
    {

        [HttpGet]
        public string Start()
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .Start();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Run(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .OnTask(id)        //TaskID
                     .NextStepInt("20", "Alice")
                     .Run();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Withdraw(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "Jack")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .OnTask(id)             //TaskID
                     .Withdraw();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string SendBack(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("20", "Alice")
                     .UseApp("BS-100", "Delivery-Books", "BS-100-LX")
                     .UseProcess("BookSellerProcessCode")
                     .PrevStepInt()
                     .OnTask(id)             //TaskID
                     .SendBack();
            return wfResult.Status.ToString();
        }
    }

    /// <summary>
    /// 并行分支测试流程
    /// </summary>
    public class WfAndSplitAndJoinController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Start()
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("PS-100", "Large-Car-Order", "PS-100-LX")
                     .UseProcess("LargeOrderProcessCode")
             .Start();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Run(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("PS-100", "Large-Car-Order", "PS-100-LX")
                     .UseProcess("LargeOrderProcessCode")
                     .OnTask(id)
                     .NextStepInt("20", "Alice")
                     .Run();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Withdraw(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "Jack")
                     .UseApp("PS-100", "Large-Car-Order", "PS-100-LX")
                     .UseProcess("LargeOrderProcessCode")
                     .OnTask(id)             //TaskID
                     .Withdraw();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string SendBack(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("20", "Alice")
                     .UseApp("PS-100", "Large-Car-Order", "PS-100-LX")
                     .UseProcess("LargeOrderProcessCode")
                     .PrevStepInt()
                     .OnTask(id)             //TaskID
                     .SendBack();
            return wfResult.Status.ToString();
        }
    }

    /// <summary>
    /// 或分支测试流程
    /// </summary>
    public class WfOrSplitOrJoinController : Controller
    {

        [HttpGet]
        public string Start()
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("DS-100", "Leave-Request", "DS-100-LX")
                     .UseProcess("LeaveRequestCode")
                     .Start();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Run(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("DS-100", "Leave-Request", "DS-100-LX")
                     .UseProcess("LeaveRequestCode")
                     .OnTask(id)
                     .IfCondition("days", "3")
                     .NextStepInt("20", "Alice")
                     .Run();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Withdraw(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "Jack")
                     .UseApp("DS-100", "Leave-Request", "DS-100-LX")
                     .UseProcess("LeaveRequestCode")
                     .OnTask(id)             //TaskID
                     .Withdraw();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string SendBack(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("20", "Alice")
                     .UseApp("DS-100", "Leave-Request", "DS-100-LX")
                     .UseProcess("LeaveRequestCode")
                     .PrevStepInt()
                     .OnTask(id)             //TaskID
                     .SendBack();
            return wfResult.Status.ToString();
        }
    }
}