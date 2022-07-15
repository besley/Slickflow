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
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;


namespace Slickflow.WebApi.Controllers
{
    public class WfCacheTestController : Controller
    {
        /// <summary>
        /// 重置流程缓存
        /// 测试示例：
        /// {"ProcessGUID":"5d6a7d6f-daa2-482d-8303-87b3b9f59a6a", "Version": "1"}
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ResetCache(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            wfService.ResetCache(runner.ProcessGUID, runner.Version);

            return ResponseResult.Success();
        }
    }
}
