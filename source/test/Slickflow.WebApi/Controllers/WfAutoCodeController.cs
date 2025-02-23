using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    //webapi:  http://localhost/sfapi/api/wfautocode/
    //Database table: WfProcess
    //Basic testing of ordinary sequential processes (testing of sequence, return, cancellation, etc.)
    //Process Name: Sequence Code Testing Process
    //GUID:  8b86e4fe-fef6-4e6f-9c8b-36bf0932f18b
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessID":" 8b86e4fe-fef6-4e6f-9c8b-36bf0932f18b"}

    //run process app:
    ////Order application node:
    ////The next step is the 'intermediate time' node
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserID":"10","UserName":"Long","Conditions":{"amount":"10"},"NextActivityPerformers":{"c7486da0-61f7-45b9-f82a-4a19fb8f9ee7":[{"UserID":10,"UserName":"Long"}]}}

    //withdraw process:
    //Revoke the previous node (signed by the board room and submitted by the previous salesperson)
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a"}

    //runprocess app
    //Node for signature processing of prefabricated houses
    //The next step is for the salesperson to confirm
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserID":"10","UserName":"Long","NextActivityPerformers":{"cab57060-f433-422a-a66f-4a5ecfafd54e":[{"UserID":10,"UserName":"Long"}]}}

    //Process completed
    //The salesperson confirms the processing node
    //The next step of the process ends
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","NextActivityPerformers":{"b53eb9ab-3af6-41ad-d722-bed946d19792":[{"UserID":10,"UserName":"Long"}]}}

    //run sub process
    //There are sub processes
    //Initiate sub process
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserID":"10","UserName":"Long","NextActivityPerformers":{"5fa796f6-2d5d-4ed6-84e2-a7c4e4e6aabc":[{"UserID":10,"UserName":"Long"}]}}


    //reverse process:
    //Return signature
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a"}

    //sendback process
    //Return
    //The data format is the same as the JSON data format for cancellation, return, and re signing

    //read task,  and make activity running:
    //Task Reading:
    //{"UserID":"10","UserName":"Long","TaskID":"17"}}

    //Obtain the next steps for processing:
    //1) Obtain based on the application
    //GetNextSteps
    //{"AppName": "SamplePrice", "AppInstanceID": 915, "UserID": "10", "UserName": "Long", "ProcessUID": "5d6a7d6f-da2-482d-8303b3b9f59a6a", "NextActivity Performers": {"39c71004-d822-4c15-9ff2-94ca1068d745": [{"UserID": "10", "UserName": "Long"}]}, "Flowstatus": "Start"}

    //2) Obtain based on task ID
    //GetTaskNextSteps


    /// <summary>
    /// Auto Code Test
    /// </summary>
    public class WfAutoCodeController : Controller
    {
        private static WfAppRunner _appRunner = null;

        [HttpPost]
        public ResponseResult Start()
        {
            var runner = _appRunner;

            var result = ResponseResult.Default();
            try
            {
                //Startup
                IWorkflowService wfService = new WorkflowService();
                //var wfResult = wfService.CreateRunner(runner)
                //            .Start();
                var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
                         .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessID, runner.Version)
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
        public ResponseResult Run()
        {
            var runner = _appRunner;

            var result = ResponseResult.Default();
            try
            {
                string amount = string.Empty;
                IWorkflowService wfService = new WorkflowService();
                //var wfResult = wfService.CreateRunner(runner)

                //            .Run();
                var wfResult = wfService.CreateRunner(runner.UserID, runner.UserName)
                         .UseApp(runner.AppInstanceID, runner.AppName, runner.AppInstanceCode)
                         .UseProcess(runner.ProcessID, runner.Version)
                         //.NextStep(runner.NextActivityPerformers)
                         //.NextStepInt(NextPerformerIntTypeEnum.Single)
                         .IfCondition(runner.Conditions)
                         .Subscribe(EventFireTypeEnum.OnActivityExecuting, (delegateContext, delegateService) => {
                             if (delegateContext.ActivityCode == "Task1")
                             {
                                 delegateService.SaveVariable(ProcessVariableTypeEnum.Process, "name", "book-task1");
                                 delegateService.SaveVariable(ProcessVariableTypeEnum.Process, "amount", "50");
                             }
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
