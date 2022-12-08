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
    //webapi: http://localhost/sfapi/api/wfsequence/
    //数据库表: WfProcess
    //普通顺序流程基本测试(顺序,返签,退回,撤销等测试)
    //流程记录ID：3
    //流程名称：报价流程
    //GUID: 072af8c3-482a-4b1c-890b-685ce2fcc75d
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //run process app:
    ////业务员提交办理节点：
    ////下一步是“板房签字”办理节点
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"eb833577-abb5-4239-875a-5f2e2fcb6d57":[{"UserID":"10","UserName":"Long"}]}}

    //withdraw process:
    //撤销至上一步节点（由板房签字到上一步业务员提交）
    //每次修改一下已办任务的TaskID
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d", "TaskID": 30639}

    //runprocess app
    //板房签字办理节点
    //下一步是业务员确认
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"cab57060-f433-422a-a66f-4a5ecfafd54e":[{"UserID":"10","UserName":"Long"}]}}

    //流程结束
    //业务员确认办理节点
    //下一步流程结束
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","NextActivityPerformers":{"b53eb9ab-3af6-41ad-d722-bed946d19792":[{"UserID":"10","UserName":"Long"}]}}

    //run sub process
    //有子流程
    //启动子流程
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"5fa796f6-2d5d-4ed6-84e2-a7c4e4e6aabc":[{"UserID":"10","UserName":"Long"}]}}


    //reverse process:
    //返签
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //sendback process
    //退回
    //数据格式同返签(撤销,退回,返签Json数据格式相同.)

    //read task, and make activity running:
    //任务阅读：
    //{"UserID":"10","UserName":"Long","TaskID":"17"}}

    //获取下一步办理步骤：
    //1) 根据应用来获取
    //GetNextSteps
    //{"AppName":"SamplePrice","AppInstanceID":915,"UserID":"10","UserName":"Long","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":"10","UserName":"Long"}]},"Flowstatus":"启动"}

    //2) 根据任务ID来获取
    //GetTaskNextSteps

    //撤销流程: WithdrawProcess
    //退回流程：SendBackProcess
    //返签流程：ReverseProcess
    //取消运行流程：CancelProcess
    //废弃所有流程实例：DiscardProcess

    /// <summary>
    /// process test
    /// </summary>
    public class WfSequenceController : Controller
    {
       
        /// <summary>
        ///  启动流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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

                    //runner.NextActivityPerformers.Add(nextSteps[0].ActivityGUID, performerList);
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
        ///  运行流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        ///  运行流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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

        /// <summary>
        ///  运行流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult JumpEnd([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.JumpProcess(session.Connection, runner, session.Transaction, JumpOptionEnum.End);

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