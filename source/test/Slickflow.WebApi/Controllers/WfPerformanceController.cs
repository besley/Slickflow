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
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    //webapi: http://localhost/sfapi/api/wfperformance/
    /// <summary>
    /// process test
    /// </summary>
    public class WfPerformanceController : Controller
    {
       [HttpGet]
       public ResponseResult GetTaskView()
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();
            var processInstanceID = 5492;
            var activityInstanceID = 46593;

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var task = wfService.GetTaskView(processInstanceID, activityInstanceID);
            stopWatch.Stop();
            var time = stopWatch.Elapsed;
            result = ResponseResult.Success(time.ToString());

            return result;
        }

        /// <summary>
        /// 待办任务查询
        /// {"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"SEQ-C-1099","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult GetReadyTasks([FromBody]TaskQuery query)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var task = wfService.GetReadyTasks(query);
            stopWatch.Stop();
            var time = stopWatch.Elapsed;
            result = ResponseResult.Success(time.ToString());

            return result;
        }
    }
}