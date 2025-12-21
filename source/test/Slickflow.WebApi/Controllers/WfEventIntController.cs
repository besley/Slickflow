using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.WebUtility;

namespace Slickflow.WebApi.Controllers
{
    //webapi: http://localhost/sfapi/api/wfeventint/
    //Database table: wf_process
    //Process record Id: 220
    //Process Name: Intermediate Event Testing Process
    //GUID:  5d6a7d6f-daa2-482d-8303-87b3b9f59a6a
    //startup process:
    //{"UserId":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceId":"100","ProcessId":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a"}

    //run process app:
    ////Order application node:
    ////The next step is the 'intermediate event' node
    //{"AppName":"SamplePrice","AppInstanceId":"100","ProcessId":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserId":"10","UserName":"Long","Conditions":{"amount":"10"},"NextActivityPerformers":{"c7486da0-61f7-45b9-f82a-4a19fb8f9ee7":[{"UserId":10,"UserName":"Long"}]}}


    /// <summary>
    /// Intermediate Event Controller
    /// </summary>
    public class WfEventIntController : Controller
    {
        [HttpPost]
        public ResponseResult Start([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            try
            {
                IWorkflowService wfService = new WorkflowService();
                //var wfResult = wfService.CreateRunner(runner)
                //            .Start();
                var wfResult = wfService.CreateRunner(runner.UserId, runner.UserName)
                         .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessId, runner.Version)
                         .Subscribe(EventFireTypeEnum.OnProcessStarted, (delegateContext, delegateService) => {
                             var processInstanceId = delegateContext.ProcessInstanceId;
                             delegateService.SaveVariable(ProcessVariableScopeEnum.Process, "name", "book");
                             delegateService.SaveVariable(ProcessVariableScopeEnum.Process, "amount", "30");
                             return true;  
                         })
                         .Start();

                result = ResponseResult.Success(wfResult.Message);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;     
        }

        [HttpPost]
        public ResponseResult Start2(WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                IWorkflowService wfService = new WorkflowService();
                //var wfResult = wfService.CreateRunner(runner)
                //            .Start(conn, trans);
                var wfResult = wfService.CreateRunner(runner.UserId, runner.UserName)
                         .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessId, runner.Version)
                         .Subscribe(EventFireTypeEnum.OnProcessStarted, (delegateContext, delegateService) => {
                             delegateService.SaveVariable(ProcessVariableScopeEnum.Process, "product", "toy");
                             delegateService.SaveVariable(ProcessVariableScopeEnum.Process, "country", "china");
                             var country = delegateService.GetVariable(ProcessVariableScopeEnum.Process, "country");

                             //var processInstance = delegateService.GetInstance<ProcessInstanceEntity>(processInstanceId);
                             //throw new ApplicationException("errror");
                             return true;
                         })
                         .Start(conn, trans);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                    trans.Commit();
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                    trans.Rollback();
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
                trans.Rollback();
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return result;
        }

        [HttpPost]
        public ResponseResult Run([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            try
            {
                string amount = string.Empty;
                IWorkflowService wfService = new WorkflowService();
                //var wfResult = wfService.CreateRunner(runner)
                //            .Run();
                var wfResult = wfService.CreateRunner(runner.UserId, runner.UserName)
                         .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessId, runner.Version)
                         .NextStep(runner.NextActivityPerformers)
                         .IfCondition(runner.Conditions)
                         .Subscribe(EventFireTypeEnum.OnActivityExecuting, (delegateContext, delegateService) => {
                             Debug.WriteLine(string.Format("Activity Executing...{0}", delegateContext.ActivityName));
                             if (delegateContext.ActivityCode == "OrderSubmit")
                             {
                                 delegateService.SaveVariable(ProcessVariableScopeEnum.Activity, "name", "book-task1");
                                 delegateService.SaveVariable(ProcessVariableScopeEnum.Activity, "amount", "50");
                                 var country = delegateService.GetVariable(ProcessVariableScopeEnum.Process, "country");
                                 delegateService.SaveVariable(ProcessVariableScopeEnum.Process, "date", System.DateTime.Today.ToShortDateString());
                                 amount = delegateService.GetVariable(ProcessVariableScopeEnum.Activity, "amount");
                             }
                             return true;
                         })
                         .Subscribe(EventFireTypeEnum.OnActivityExecuted, (delegateContext, delegateService) => {
                             Debug.WriteLine(string.Format("Activity Executed...{0}", delegateContext.ActivityName));
                             if (delegateContext.ActivityCode == "OrderSubmit")
                             {
                                 delegateService.SaveVariable(ProcessVariableScopeEnum.Activity, "name", "book-task1-0701"); ;
                             }
                             return true;
                         })
                         .Subscribe(EventFireTypeEnum.OnProcessCompleted, (delegateContext, delegateService) => {
                             Debug.WriteLine(string.Format("Process Completed...{0}", delegateContext.ProcessInstanceId.ToString()));

                             return true;
                         })
                         .Run();
                result = ResponseResult.Success(wfResult.Message);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }
    }
}

