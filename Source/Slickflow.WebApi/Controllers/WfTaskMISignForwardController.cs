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
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.Http;
using System.Web.Http.Controllers;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Core;
using Slickflow.Engine.Core.Result;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;
using Slickflow.WebApi.Utility;

namespace Slickflow.WebApi.Controllers
{
    //加签流程测试
    //price.task.multipleinstance.jiaqian.xml
    //流程调用JSON格式说明

    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"1019","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //run process app:
    ////运行到板房签字节点
    //{"AppName":"SamplePrice","AppInstanceID":"1019","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"fc8c71c5-8786-450e-af27-9f6a9de8560f":[{"UserID":10,"UserName":"Long"}, {"UserID":20,"UserName":"Meilinda"}]}}

    //signforward process
    //需要动态增加要加签的人
    //{"AppName":"SamplePrice","AppInstanceID":"1019","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","DynamicVariables":{"SignForwardType":"SignForwardBehind"}, "NextActivityPerformers":{"fc8c71c5-8786-450e-af27-9f6a9de8560f":[{"UserID":30,"UserName":"Alice"}]}}

    //run process app:
    //运行加签实例节点的第一个任务
    //{"AppName":"SamplePrice","AppInstanceID":"1019","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":40,"UserName":"FangFang"}]}}
    //运行串行加签的第二个任务
    //{"AppName":"SamplePrice","AppInstanceID":"1019","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"30","UserName":"Alice","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":40,"UserName":"FangFang"}]}}

    //加签之后的节点之后


    /// <summary>
    /// 加签流程测试
    /// </summary>
    public class WfTaskMISignForwardController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public ResponseResult StartProcess(WfAppRunner starter)
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
        [AllowAnonymous]
        public ResponseResult RunProcessApp(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.RunProcessApp(conn, runner, trans);

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    trans.Rollback();
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
        [AllowAnonymous]
        public ResponseResult SignForwardProcess(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.SignForwardProcess(conn, runner, trans);

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    trans.Rollback();
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
        [AllowAnonymous]
        public ResponseResult WithdrawProcess(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.WithdrawProcess(conn, runner, trans);

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    trans.Rollback();
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
        [AllowAnonymous]
        public ResponseResult SendBackProcess(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.SendBackProcess(conn, runner, trans);

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    trans.Rollback();
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
        [AllowAnonymous]
        public ResponseResult ReverseProcess(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            IDbConnection conn = SessionFactory.CreateConnection();

            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = wfService.ReverseProcess(conn, runner, trans);

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    trans.Rollback();
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
        [AllowAnonymous]
        public ResponseResult DiscardProcess(WfAppRunner discarder)
        {
            IWorkflowService service = new WorkflowService();
            var result = service.DiscardProcess(discarder);

            return ResponseResult.Success();
        }
    }
}
