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
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"eb833577-abb5-4239-875a-5f2e2fcb6d57":[{"UserID":10,"UserName":"Long"}]}}

    public class WfAsyncController : Controller
    {
        [HttpPost]
        public ResponseResult StartProcessAsync01([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();
            var task = wfService.StartProcessAsync(runner);
            string time = string.Format("webapi time:{0}", System.DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine(time);

            task.Wait();

            if (task.Result.Status == WfExecutedStatus.Success)
                return ResponseResult.Success(task.Result.Status);
            else
                return ResponseResult.Error(task.Result.Message);
        }

        [HttpPost]
        public async Task<ResponseResult> StartProcessAsync02([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();
            var task = wfService.StartProcessAsync(runner);
            string time = string.Format("webapi time:{0}", System.DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine(time);

            var result = await task;

            if (result.Status == WfExecutedStatus.Success)
                return ResponseResult.Success(task.Status);
            else
                return ResponseResult.Error(result.Message);
        }

        [HttpPost]
        public ResponseResult StartProcessAsync03([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();
            var task = wfService.StartProcessAsync(runner);
            string time = string.Format("webapi time:{0}", System.DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine(time);

            if (task.Result.Status == WfExecutedStatus.Success)
                return ResponseResult.Success(task.Result.Status);
            else
                return ResponseResult.Error(task.Result.Message);
        }

    }
}