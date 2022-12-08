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
    //分支流程基本测试
    //流程调用JSON格式说明：
    //流程记录ID：205
    //流程名称：并行流程
    //GUID: 0eb141e3-cd17-4def-aa70-98ae654351a3
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"0eb141e3-cd17-4def-aa70-98ae654351a3"}

    //3个并行分支实例化
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"01a79ac7-af2f-49e3-fc23-bc8ad13483cc":[{"UserID":10,"UserName":"Long"}], "f32fd26e-626b-4613-9715-8a60964eddf9":[{"UserID":20,"UserName":"Jack"}], "f186d8c5-859d-4edb-a392-034dadfd2395":[{"UserID":30,"UserName":"Melinda"}]}}

    //第一个并行节点执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"0ae0c551-0b8b-42f2-ad34-133543beed33":[{"UserID":40,"UserName":"Smith"}]}}

    //第二个并行节点执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"20","UserName":"Jack", "NextActivityPerformers":{"0ae0c551-0b8b-42f2-ad34-133543beed33":[{"UserID":40,"UserName":"Smith"}]}}


    //第三个并行节点执行


    //cross andjoin
    //合并节点后的节点（总经理签字）执行
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"0fdff3c0-be97-43d6-b4ff-90d52efb5d6f":[{"UserID":10,"UserName":"Long"}]}}

    //end node
    //结束节点
    //{"AppName":"OfficeIn","AppInstanceID":"123","ProcessGUID":"0eb141e3-cd17-4def-aa70-98ae654351a3","UserID":"10","UserName":"Long", "NextActivityPerformers":{"76f7ef75-b538-40c8-b529-0849ca777b94":[{"UserID":10,"UserName":"Long"}]}}

    /// <summary>
    /// process test
    /// </summary>
    public class WfProcessTestController : Controller
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