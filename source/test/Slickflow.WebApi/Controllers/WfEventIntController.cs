using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    //webapi: http://localhost/sfapi/api/wfeventint/
    //Database table: WfProcess
    //Process record ID: 220
    //Process Name: Intermediate Event Testing Process
    //GUID:  5d6a7d6f-daa2-482d-8303-87b3b9f59a6a
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a"}

    //run process app:
    ////Order application node:
    ////The next step is the 'intermediate event' node
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserID":"10","UserName":"Long","Conditions":{"amount":"10"},"NextActivityPerformers":{"c7486da0-61f7-45b9-f82a-4a19fb8f9ee7":[{"UserID":10,"UserName":"Long"}]}}


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
                var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
                         .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessGUID, runner.Version)
                         .Subscribe(EventFireTypeEnum.OnProcessStarted, (delegateContext, delegateService) => {
                             var processInstanceID = delegateContext.ProcessInstanceID;
                             delegateService.SaveVariable(ProcessVariableTypeEnum.Process, "name", "book");
                             delegateService.SaveVariable(ProcessVariableTypeEnum.Process, "amount", "30");
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
                var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
                         .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessGUID, runner.Version)
                         .Subscribe(EventFireTypeEnum.OnProcessStarted, (delegateContext, delegateService) => {
                             delegateService.SaveVariable(ProcessVariableTypeEnum.Process, "product", "toy");
                             delegateService.SaveVariable(ProcessVariableTypeEnum.Process, "country", "china");
                             var country = delegateService.GetVariable(ProcessVariableTypeEnum.Process, "country");

                             //var processInstance = delegateService.GetInstance<ProcessInstanceEntity>(processInstanceID);
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
                var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
                         .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessGUID, runner.Version)
                         .NextStep(runner.NextActivityPerformers)
                         .IfCondition(runner.Conditions)
                         .Subscribe(EventFireTypeEnum.OnActivityExecuting, (delegateContext, delegateService) => {
                             Debug.WriteLine(string.Format("Activity Executing...{0}", delegateContext.ActivityName));
                             if (delegateContext.ActivityCode == "OrderSubmit")
                             {
                                 delegateService.SaveVariable(ProcessVariableTypeEnum.Activity, "name", "book-task1");
                                 delegateService.SaveVariable(ProcessVariableTypeEnum.Activity, "amount", "50");
                                 var country = delegateService.GetVariable(ProcessVariableTypeEnum.Process, "country");
                                 delegateService.SaveVariable(ProcessVariableTypeEnum.Process, "date", System.DateTime.Today.ToShortDateString());
                                 amount = delegateService.GetVariable(ProcessVariableTypeEnum.Activity, "amount");
                             }
                             return true;
                         })
                         .Subscribe(EventFireTypeEnum.OnActivityExecuted, (delegateContext, delegateService) => {
                             Debug.WriteLine(string.Format("Activity Executed...{0}", delegateContext.ActivityName));
                             if (delegateContext.ActivityCode == "OrderSubmit")
                             {
                                 delegateService.SaveVariable(ProcessVariableTypeEnum.Activity, "name", "book-task1-0701"); ;
                             }
                             return true;
                         })
                         .Subscribe(EventFireTypeEnum.OnProcessCompleted, (delegateContext, delegateService) => {
                             Debug.WriteLine(string.Format("Process Completed...{0}", delegateContext.ProcessInstanceID.ToString()));

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

