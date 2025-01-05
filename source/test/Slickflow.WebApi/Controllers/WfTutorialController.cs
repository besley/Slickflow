using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Graph;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl.Common;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// Tutorial Controller
    /// </summary>
    public class WfTutorialController : Controller
    {
        [HttpGet]
        public void Create()
        {
            var pmb = ProcessModelBuilder.CreateProcess("BookSellerProcess", "BookSellerProcessCode");
            var process = pmb.Start("Start")
                             .Task(VertexBuilder.CreateTask("Package Books", "003"))
                             .Task("Deliver Books", "004")
                             .End("End")
                             .Store();
        }

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
    /// AndSplitAndJoin Test
    /// </summary>
    public class WfAndSplitAndJoinController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void Create()
        {
            var pmb = ProcessModelBuilder.CreateProcess("LargeOrderProcess", "LargeOrderProcessCode");
            var process = pmb.Start("Start")
                             .Task("Large Order Received")
                             .AndSplit("AndSplit")
                             .Parallels(
                                   () => pmb.Branch(
                                       () => pmb.Task("Engineering Review")
                                   )
                                   ,() => pmb.Branch(
                                       () => pmb.Task("Design Review")
                                   )
                             )
                             .AndJoin("AndJoin")
                             .Task("Management Approve")
                             .End("End")
                             .Store();
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
        public void Create()
        {
            var pmb = ProcessModelBuilder.CreateProcess("LeaveRequest", "LeaveRequestCode");
            var process = pmb.Start("Start")
                             .Task("Fill Leave Days")
                             .OrSplit("OrSplit")
                             .Parallels(
                                   () => pmb.Branch(
                                       () => pmb.Task(
                                           VertexBuilder.CreateTask("CEO Evaluate"),
                                           LinkBuilder.CreateTransition("days>=3")
                                                      .AddCondition(ConditionTypeEnum.Expression, "days>=3")
                                           )
                                   )
                                   , () => pmb.Branch(
                                       () => pmb.Task(
                                           VertexBuilder.CreateTask("Manager Evaluate"),
                                           LinkBuilder.CreateTransition("days<3")
                                                      .AddCondition(ConditionTypeEnum.Expression, "days<3")
                                           )
                                    )
                             )
                             .OrJoin("OrJoin")
                             .Task("HR Notify")
                             .End("End")
                             .Store();
        }

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