using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.Graph;
using Slickflow.Graph.Roslyn;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// Model Controller
    /// </summary>
    public class WfModelController : Controller
    {
        [HttpGet]
        public string Create()
        {
            try
            {
                var pmb = ProcessModelBuilder.CreateProcess("BookSellerProcess", "BookSellerProcessCode");
                var process = pmb.Start("Start")
                    .Task(VertexBuilder.CreateTask("Package Books", "003"))
                    .Task("Deliver Books", "005")
                    .End("End")
                    .Store();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Set()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "1");
                var process = pmb.Set("003", (a) => pmb.GetBuilder(a).SetUrl("slickflow.com").SetName("mer-sss-ryxmas"))
                    .Update();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Add()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "1");
                var process = pmb.Add("003", ActivityTypeEnum.TaskNode, "zzz", "zzz-code")
                    .Update();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Insert()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "1");
                var process = pmb.Insert("003", ActivityTypeEnum.TaskNode, "task004", "004")
                    .Insert("003", "005", ActivityTypeEnum.TaskNode, "task007", "007")
                    .Update();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Insert02()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "1");
                var process = pmb.Insert("003", "005", ActivityTypeEnum.TaskNode, "task004", "004")
                    .Insert("004", "005", ActivityTypeEnum.TaskNode, "task007", "007")
                    .Update();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //[HttpGet]
        public string Replace()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "1");
                var process = pmb.Replace("004", ActivityTypeEnum.TaskNode, "task222", "222")
                    .Update();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Exchange()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "1");
                var process = pmb.Exchange("003", "005")
                    .Update();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Remove()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "1");
                var process = pmb.Remove("004", true)
                    .Update();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string List1()
        {
            IList<Activity> list = new List<Activity>();
            list.Add(new Activity { ActivityCode = "001" });
            list.Add(new Activity { ActivityCode = "002" });
            list.Add(new Activity { ActivityCode = "003" });
            list.Add(new Activity { ActivityCode = "004" });

            Remove1(list.ToList());
            return list.Count.ToString();
        }

        private void Remove1(List<Activity> list)
        {
            list.RemoveAll(a => a.ActivityCode == "002");
        }

        [HttpGet]
        public string List()
        {
            IList<string> list = new List<string>();
            list.Add("001");
            list.Add("002");
            list.Add("003");
            list.Add("004");

            Remove(list);
            return list.Count.ToString();
        }

        private void Remove(IList<string> list)
        {
            list.ToList().RemoveAll(a => a == "002");
        }

        [HttpPost]
        public string Man([FromBody] ProcessGraph graph)
        {
            try
            {
                var body = graph.Body;

                Task<RoslynExecuteResult> result = RoslynHotSpot.Execute(body);
                if (result.IsCompleted)
                    return result.Status.ToString();
                else
                    return "exception";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Cover()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "3");
                pmb.Cover("003", "005",
                    VertexBuilder.CreateSplit(GatewayDirectionEnum.AndSplit, "AndSplit", "AndSplicCode"),
                    VertexBuilder.CreateJoin(GatewayDirectionEnum.AndJoin, "AndJoin", "AndJoinCode"),
                    VertexBuilder.CreateTask("branchTask001", "b001"),
                    VertexBuilder.CreateTask("branchTask002", "b002")
                    )
                    .Update();
                return "OK";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Uncover()
        {
            try
            {
                var pmb = ProcessModelBuilder.LoadProcess("BookSellerProcessCode", "3");
                pmb.Uncover("003", "005")
                   .Update();
                return "OK";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string Run()
        {
            try
            {
                var codeText = @"using Slickflow.Graph;
using Slickflow.Engine.Common;

//we will create a new process;
var pmb = ProcessModelBuilder.CreateProcess(""BookSellerProcess"", ""BookSellerProcessCode"", ""3"");
var process = pmb.Start(""Start"")
        .Task(""Package Books"", ""003"")
        .Task(""Deliver Books"", ""005"")
        .End(""End"")
        .Store(); ";
                codeText = codeText.Replace("\n", "");

                Task<RoslynExecuteResult> result = RoslynHotSpot.Execute(codeText);
                if (result.IsCompleted)
                    return result.Status.ToString();
                else
                    return "exception";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string WhileForward()
        {
            try
            {
                ////firstly, create a process model builder
                //var pmb = ProcessModelBuilder.CreateProcess("BookSellerProcess9286", "BookSellerProcessCode9286", "3");
                //var process = pmb.Start("Start")
                //    .Task("Package Books", "003")       //task name, code
                //    .Task("Deliver Books", "005")       //task name, code
                //    .End("End")
                //    .Store();

                //initialize a runner object
                WfAppRunner runner = new WfAppRunner();
                runner.ProcessGUID = "caa31df6-d4a4-49a1-8d39-7036db142363";
                runner.Version = "1";
                runner.UserID = "10";
                runner.UserName = "Jack";
                runner.AppName = "Test APP";
                runner.AppInstanceID = "tx001";
                runner.AppInstanceCode = "TX002";

                //startup a new process instance
                //the first task "Package Books" will be in ready status
                IWorkflowService wfService = new WorkflowService();
                var wfResultStart = wfService.CreateRunner(runner.UserID, runner.UserName)
                    .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
                    .UseProcess(runner.ProcessGUID, runner.Version)
                    .Start();

                var wfResultRun = Forward(runner, wfService);

                if (wfResultRun.Status == Engine.Core.Result.WfExecutedStatus.Success)
                {
                    var nextStep = wfService.GetRunningNode(runner);
                    while (nextStep != null)
                    {
                        var wfResultNew = Forward(runner, wfService);
                        nextStep = wfService.GetRunningNode(runner);
                    }
                }

            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

            return "OK";
        }

        private WfExecutedResult Forward(WfAppRunner runner, IWorkflowService wfService)
        {
            //continue to run the process instance
            //finished the first task "Package Books"
            //the second task "Deliver Books" will be in ready status
            var wfResultRun = wfService.CreateRunner(runner.UserID, runner.UserName)
                .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
                .UseProcess(runner.ProcessGUID, runner.Version)
                .NextStepInt("10", "Jack")      //this method is only for tutorial, OnTask() should be called normally before using it.
                .Run();
            return wfResultRun;

        }
    }
}