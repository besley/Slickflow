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
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    //Basic testing of branch processes
    //Process call JSON format description:
    //Process record ID: 205
    //Process Name: Parallel Process
    //GUID:  0eb141e3-cd17-4def-aa70-98ae654351a3
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"OfficeIn","AppInstanceID":"123","ProcessID":"0eb141e3-cd17-4def-aa70-98ae654351a3"}

    //Instantiation of 3 parallel branches
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"01a79ac7-af2f-49e3-fc23-bc8ad13483cc":[{"UserID":10,"UserName":"Long"}], "f32fd26e-626b-4613-9715-8a60964eddf9":[{"UserID":20,"UserName":"Jack"}], "f186d8c5-859d-4edb-a392-034dadfd2395":[{"UserID":30,"UserName":"Melinda"}]}}

    //The first parallel node executes
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"0ae0c551-0b8b-42f2-ad34-133543beed33":[{"UserID":40,"UserName":"Smith"}]}}

    //The second parallel node executes
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"20","UserName":"Jack", "NextActivityPerformers":{"0ae0c551-0b8b-42f2-ad34-133543beed33":[{"UserID":40,"UserName":"Smith"}]}}


    //Third parallel node execution


    //cross andjoin
    //Execution of nodes after merging (signed by the general manager)
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"0fdff3c0-be97-43d6-b4ff-90d52efb5d6f":[{"UserID":10,"UserName":"Long"}]}}

    //end node
    //End Node
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"76f7ef75-b538-40c8-b529-0849ca777b94":[{"UserID":10,"UserName":"Long"}]}}

    /// <summary>
    /// Process test
    /// 流程测试
    /// </summary>
    public class WfProcessTestController : Controller
    {
        /// <summary>
        ///  Start process
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
                    //performerList.Add(new Performer(runner.UserID, runner.UserID));

                    //runner.NextActivityPerformers.Add(nextSteps[0].ActivityID, performerList);
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
        /// Run process
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
                     .UseProcess("PriceProcessCode")
                     .OnTask(id)             //TaskID
                     .Withdraw();
            return wfResult.Status.ToString();
        }
    }
}