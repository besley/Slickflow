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

namespace Slickflow.WebApi.Controllers
{
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
        public void Create()
        {
            var pmb = ProcessModelBuilder.CreateProcess("process-split-name", "process-split-code");
            var process = pmb.Start("Start")
                             .Task("Task-001")
                             .AndSplit("Split")
                             .Parallels(
                                   () => pmb.Branch(
                                       () => pmb.Task("Task-100")
                                   )
                                   , () => pmb.Branch(
                                        () => pmb.Task("Task-200")
                                    )
                             )
                             .AndJoin("Join")
                             .Task("Task-500")
                             .End("End")
                             .Store();
        }

        [HttpGet]
        public string Start()
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("DS-100", "Book-Order", "DS-100-LX")
                     .UseProcess("process-split-code")
             .Start();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Run(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("DS-100", "Book-Order", "DS-100-LX")
                     .UseProcess("process-split-code")
                     .OnTask(id)
                     .NextStepInt("20", "Alice")
                     .Run();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string SendBack(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("20", "Alice")
                     .UseApp("DS-100", "Book-Order", "DS-100-LX")
                     .UseProcess("process-split-code")
                     .PrevStepInt()
                     .OnTask(id)             //TaskID
                     .SendBack();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Withdraw(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "Jack")
                     .UseApp("DS-100", "Book-Order", "DS-100-LX")
                     .UseProcess("process-split-code")
                     .OnTask(id)             //TaskID
                     .Withdraw();
            return wfResult.Status.ToString();
        }
    }

    /// <summary>
    /// 或分支测试流程
    /// </summary>
    public class WfOrSplitOrJoinController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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
    }
}