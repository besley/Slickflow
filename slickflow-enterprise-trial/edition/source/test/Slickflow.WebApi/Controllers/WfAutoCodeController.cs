/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/


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
    //webapi: http://localhost/sfapi/api/wfautocode/
    //数据库表: WfProcess
    //普通顺序流程基本测试(顺序,返签,退回,撤销等测试)
    //流程名称：序列代码测试流程
    //GUID: 8b86e4fe-fef6-4e6f-9c8b-36bf0932f18b
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":" 8b86e4fe-fef6-4e6f-9c8b-36bf0932f18b"}

    //run process app:
    ////订单申请节点：
    ////下一步是“中间时间”节点
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserID":"10","UserName":"Long","Conditions":{"amount":"10"},"NextActivityPerformers":{"c7486da0-61f7-45b9-f82a-4a19fb8f9ee7":[{"UserID":10,"UserName":"Long"}]}}

    //withdraw process:
    //撤销至上一步节点（由板房签字到上一步业务员提交）
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a"}

    //runprocess app
    //板房签字办理节点
    //下一步是业务员确认
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserID":"10","UserName":"Long","NextActivityPerformers":{"cab57060-f433-422a-a66f-4a5ecfafd54e":[{"UserID":10,"UserName":"Long"}]}}

    //流程结束
    //业务员确认办理节点
    //下一步流程结束
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","NextActivityPerformers":{"b53eb9ab-3af6-41ad-d722-bed946d19792":[{"UserID":10,"UserName":"Long"}]}}

    //run sub process
    //有子流程
    //启动子流程
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","UserID":"10","UserName":"Long","NextActivityPerformers":{"5fa796f6-2d5d-4ed6-84e2-a7c4e4e6aabc":[{"UserID":10,"UserName":"Long"}]}}


    //reverse process:
    //返签
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a"}

    //sendback process
    //退回
    //数据格式同返签(撤销,退回,返签Json数据格式相同.)

    //read task, and make activity running:
    //任务阅读：
    //{"UserID":"10","UserName":"Long","TaskID":"17"}}

    //获取下一步办理步骤：
    //1) 根据应用来获取
    //GetNextSteps
    //{"AppName":"SamplePrice","AppInstanceID":915,"UserID":"10","UserName":"Long","ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":"10","UserName":"Long"}]},"Flowstatus":"启动"}

    //2) 根据任务ID来获取
    //GetTaskNextSteps

    //撤销流程: WithdrawProcess
    //退回流程：SendBackProcess
    //返签流程：ReverseProcess
    //取消运行流程：CancelProcess
    //废弃所有流程实例：DiscardProcess


    /// <summary>
    /// 自定义编码流程，用于测试
    /// </summary>
    public class WfAutoCodeController : Controller
    {
        #region Workflow Api访问操作
        private static WfAppRunner _appRunner = null;

        [HttpPost]
        public ResponseResult Start()
        {
            var runner = _appRunner;

            var result = ResponseResult.Default();
            try
            {
                //流程启动
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
                         .UseProcess(runner.ProcessGUID, runner.Version)
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
        #endregion
    }
}
