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
using Slickflow.Engine.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    public class WfTaskMISubProcessController : Controller
    {
        //会签流程测试 
        //price.subprocess.multipleinstance.xml
        //流程调用JSON格式说明：

        //startup process:
        //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

        //run process app:
        ////运行到会签节点
        //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"fc8c71c5-8786-450e-af27-9f6a9de8560f":[{"UserID":10,"UserName":"Long"}, {"UserID":20,"UserName":"Meilinda"}]}}

        //run process app:
        //运行会签实例节点
        //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":10,"UserName":"Long"}]}}
        //运行串行的第二个任务
        //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"20","UserName":"Meilinda","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":10,"UserName":"Long"}]}}

        //结束节点(业务员->结束)
        //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"b70e717a-08da-419f-b2eb-7a3d71f054de":[{"UserID":10,"UserName":"Long"}]}}

        #region Workflow Api访问操作
        [HttpPost]
        public ResponseResult StartProcess([FromBody] WfAppRunner starter)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                WfExecutedResult result = wfService.StartProcess(conn, starter, trans);
                trans.Commit();

                int newProcessInstanceID = result.ProcessInstanceIDStarted;
                IList<NodeView> nextSteps = wfService.GetNextActivityTree(starter);

                if (result.Status == WfExecutedStatus.Success)
                {
                    return ResponseResult.Success();
                }
                else
                {
                    return ResponseResult.Error(result.Message);
                }
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult RunProcessApp([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.RunProcessApp(conn, runner, trans);
                trans.Commit();

                if (result.Status == WfExecutedStatus.Success)
                    return ResponseResult.Success();
                else
                    return ResponseResult.Error(result.Message);
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult WithdrawProcess([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.WithdrawProcess(conn, runner, trans);
                trans.Commit();

                if (result.Status == WfExecutedStatus.Success)
                    return ResponseResult.Success();
                else
                    return ResponseResult.Error(result.Message);
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult SendBackProcess([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.SendBackProcess(conn, runner, trans);
                trans.Commit();

                if (result.Status == WfExecutedStatus.Success)
                    return ResponseResult.Success();
                else
                    return ResponseResult.Error(result.Message);
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult ReverseProcess([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.ReverseProcess(conn, runner, trans);
                trans.Commit();

                if (result.Status == WfExecutedStatus.Success)
                    return ResponseResult.Success();
                else
                    return ResponseResult.Error(result.Message);
            }
            catch (WorkflowException w)
            {
                trans.Rollback();
                return ResponseResult.Error(w.Message);
            }
            finally
            {
                trans.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        [HttpPost]
        public ResponseResult DiscardProcess([FromBody] WfAppRunner discarder)
        {
            IWorkflowService service = new WorkflowService();
            var result = service.DiscardProcess(discarder);

            return ResponseResult.Success();
        }
        #endregion
    }
}
