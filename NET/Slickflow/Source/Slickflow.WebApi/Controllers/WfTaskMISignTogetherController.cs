/*
* Slickflow 开源项目遵循LGPL协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
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
    //webapi: http://localhost/sfapi/api/WfTaskMISignTogether/
    //数据库表: WfProcess
    //普通顺序流程基本测试(顺序,返签,退回,撤销等测试)
    //流程记录ID：71
    //流程名称：报价会签流程
    //GUID: 9fb4bca4-5674-4181-a010-f0e730e166dd
    //会签流程测试 
    //price.task.multipleinstance.xml
    //流程调用JSON格式说明：

    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"9fb4bca4-5674-4181-a010-f0e730e166dd"}

    //run process app:
    ////运行到会签节点
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"9fb4bca4-5674-4181-a010-f0e730e166dd","UserID":"10","UserName":"Long","NextActivityPerformers":{"36cf2479-e8ec-4936-8bcd-b38101e4664a":[{"UserID":40, "UserName": "Susan"}, {"UserID":30,"UserName":"Jack"}, {"UserID":20,"UserName":"Meilinda"}]}}

    //run process app:
    //运行会签实例节点
    //运行串行的第一个任务
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"9fb4bca4-5674-4181-a010-f0e730e166dd","UserID":"40","UserName":"Susan","NextActivityPerformers":{"36cf2479-e8ec-4936-8bcd-b38101e4664a":[{"UserID":30,"UserName":"Jack"}]}}
    //运行串行的第二个任务
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"9fb4bca4-5674-4181-a010-f0e730e166dd","UserID":"30","UserName":"Jack","NextActivityPerformers":{"36cf2479-e8ec-4936-8bcd-b38101e4664a":[{"UserID":20,"UserName":"Meilinda"}]}}
    //运行串行的第三个任务
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"9fb4bca4-5674-4181-a010-f0e730e166dd","UserID":"20","UserName":"Meilinda","NextActivityPerformers":{"36cf2479-e8ec-4936-8bcd-b38101e4664a":[{"UserID":10,"UserName":"Long"}]}}

    //withdraw signtogether instance task
    //{"UserID":"40","UserName":"Susan","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"9fb4bca4-5674-4181-a010-f0e730e166dd"}

    /// <summary>
    /// 会签流程测试
    /// </summary>
    public class WfTaskMISignTogetherController : ApiController
    {
        #region Workflow Api访问操作
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

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    int newProcessInstanceID = result.ProcessInstanceIDStarted;
                    IList<NodeView> nextSteps = wfService.GetNextActivityTree(starter);
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
        #endregion
    }
}
