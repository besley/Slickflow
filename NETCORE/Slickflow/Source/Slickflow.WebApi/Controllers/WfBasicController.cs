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
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{

    //webapi: http://localhost/sfapi/api/wfevent/
    //数据库表: WfProcess
    //流程记录ID：219
    //流程名称：事件测试交互流程
    //GUID: 4be58a96-926c-4aff-a383-fe71185572e5
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"4be58a96-926c-4aff-a383-fe71185572e5"}

    //run process app:
    ////订单处理节点：
    ////下一步是结束节点
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"4be58a96-926c-4aff-a383-fe71185572e5","UserID":"10","UserName":"Long","NextActivityPerformers":{"de50335a-034c-4c58-db72-ddd00c1aebfe":[{"UserID":10,"UserName":"Long"}]}}

    /// <summary>
    /// 事件控制器
    /// </summary>
    public class WfBasicController : Controller
    {
        #region Workflow 数据访问基本操作
        [HttpGet]
        public string Hello()
        {
            return "Hello World!";
        }

        // GET: /Workflow/
        [HttpGet]
        public object Get()
        {
            ProcessManager pm = new ProcessManager();
            var process = pm.GetByVersion("5d6a7d6f-daa2-482d-8303-87b3b9f59a6a", "1");      //版本默认为1

            return process;
        }

        [HttpPost]
        public void InsertProcess([FromBody] ProcessEntity obj)
        {
            ProcessManager pm = new ProcessManager();
            pm.Insert(obj);
        }

        [HttpPost]
        public void UpdateProcess([FromBody] ProcessEntity obj)
        {
            ProcessManager pm = new ProcessManager();
            pm.Update(obj);
        }

        [HttpPost]
        public void RemoveProcess([FromBody] ProcessEntity entity)
        {
            ProcessManager pm = new ProcessManager();
            pm.Delete(entity.ProcessGUID, entity.Version);
        }

        [HttpGet]
        public ProcessInstanceEntity GetProcessInstance()
        {
            IWorkflowService service = new WorkflowService();
            var instance = service.GetProcessInstance(14001);

            return instance;
        }

        [HttpGet]
        public ActivityInstanceEntity GetActivityInstance()
        {
            IWorkflowService service = new WorkflowService();
            var instance = service.GetActivityInstance(100);

            return instance;
        }
        #endregion
    }
}

