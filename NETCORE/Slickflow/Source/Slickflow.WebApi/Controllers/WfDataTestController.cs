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
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    public class WfDataTestController : Controller
    {
        #region 获取任务数据列表
        /// <summary>
        /// 获取任务记录列表
        /// </summary>
        /// <returns>流程列表</returns>
        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> GetTaskViewList([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetReadyTasks(query).ToList();

                result = ResponseResult<List<TaskViewEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(
                    string.Format("获取任务信息失败！{0}", ex.Message)
                );
            }
            return result;
        }
        #endregion
    }
}