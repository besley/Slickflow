using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.WebUtility;

namespace Slickflow.WebApi.Controllers
{
    //webapi: http://localhost/sfapi/api/wfsequence/
    //Database table: wf_process
    //Conditional start process testing
    //Process record Id: 870
    //Process Name: Quotation Process
    //GUID: 444db0c2-4dcb-4f86-c75d-43719b28cbb4
    //startup process:
    //{"UserId":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceId":"100","ProcessId":"52023958-996d-48ac-b9a7-ba51e48d4821","Version":"1","ControlParameterSheet":{"ConditionalVariables":{"days":"10"}}}


    /// <summary>
    /// Process Start with Condition TEst
    /// </summary>
    public class WfStartConditionalController : Controller
    {
        /// <summary>
        /// Start Process
        /// </summary>
        [HttpPost]
        public ResponseResult StartProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.StartProcess(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    //var nextSteps = wfService.GetNextActivityTree(session.Connection, runner, null, session.Transaction).ToList();
                    //runner.NextActivityPerformers = new Dictionary<string, PerformerList>();

                    //var performerList = new PerformerList();
                    //performerList.Add(new Performer(runner.UserId, runner.UserId));

                    //runner.NextActivityPerformers.Add(nextSteps[0].ActivityId, performerList);
                    //var result2 = wfService.RunProcess(session.Connection, runner, session.Transaction);

                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Run Process
        /// </summary>
        [HttpPost]
        public ResponseResult RunProcessApp([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Run Process
        /// </summary>
        [HttpPost]
        public ResponseResult RunProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        [HttpGet]
        public string Start()
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("DS-100", "Book-Order", "DS-100-LX")
                     .UseProcess("PriceProcessCode")
                     .Start();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Run()
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "jack")
                     .UseApp("DS-100", "Book-Order", "DS-100-LX")
                     .UseProcess("PriceProcessCode")
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
                     .UseProcess("PriceProcessCode")
                     .PrevStepInt()
                     .OnTask(id)             //TaskId
                     .SendBack();
            return wfResult.Status.ToString();
        }

        [HttpGet]
        public string Withdraw(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var wfResult = wfService.CreateRunner("10", "Jack")
                     .UseApp("DS-100", "Book-Order", "DS-100-LX")
                     .UseProcess("PriceProcessCode")
                     .OnTask(id)             //TaskId
                     .Withdraw();
            return wfResult.Status.ToString();
        }
    }
}