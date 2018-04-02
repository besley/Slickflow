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
    //webapi: http://localhost/sfapi/api/wfreceivertype/
    //数据库表: WfProcess
    //接收者类型流程测试
    //流程记录ID：104
    //流程名称：ASK FOR LEAVE
    //GUID: b2a18777-43f1-4d4d-b9d5-f92aa655a93f
    //startup process:
    //{"UserID":"10","UserName":"LiJie","AppName":"Leave","AppInstanceID":"800","ProcessGUID":"b2a18777-43f1-4d4d-b9d5-f92aa655a93f"}


    //get next step role user tree
    //获取下一步办理人员节点树
    //{"AppName":"Leave","AppInstanceID":"800","ProcessGUID":"b2a18777-43f1-4d4d-b9d5-f92aa655a93f","UserID":"10","UserName":"LiJie","Conditions":{"days":"2"}}

    //run process app:
    //请假人员填写请假单并提交：
    //下一步是“部门经理”办理节点
    //{"AppName":"Leave","AppInstanceID":"800","ProcessGUID":"b2a18777-43f1-4d4d-b9d5-f92aa655a93f","UserID":"10","UserName":"LiJie","Conditions":{"days":"2"}, "NextActivityPerformers":{"6bd98004-cd04-4f3a-bf21-ca232dcd0533":[{"UserID":17,"UserName":"CuiHong"}]}}

    /// <summary>
    /// 接收者类型测试（分支节点之后定义接收者类型）
    /// </summary>
    public class WfReceiverTypeController : Controller
    {
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

                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                    //获取下一步办理步骤信息
                    IList<NodeView> nextStpes = wfService.GetNextActivityTree(starter);
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
        public ResponseResult RunProcessApp([FromBody] WfAppRunner runner)
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
        public ResponseResult WithdrawProcess([FromBody] WfAppRunner runner)
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
        public ResponseResult SendBackProcess([FromBody] WfAppRunner runner)
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
        public ResponseResult ReverseProcess([FromBody] WfAppRunner runner)
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
        public ResponseResult DiscardProcess([FromBody] WfAppRunner discarder)
        {
            IWorkflowService service = new WorkflowService();
            var result = service.DiscardProcess(discarder);

            return ResponseResult.Success();
        }

        /// <summary>
        /// 查询流程下一步信息的节点角色人员树
        /// </summary>
        /// <param name="runner">当前执行人</param>
        /// <returns>流程下一步信息</returns>
        [HttpPost]
        public ResponseResult<List<NodeView>> GetNextStepRoleUserTree(WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfservice = new WorkflowService();
                var nodeViewList = wfservice.GetNextActivityRoleUserTree(runner, runner.Conditions).ToList<NodeView>();
                result = ResponseResult<List<NodeView>>.Success(nodeViewList, "获取流程下一步信息成功!");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(string.Format(
                    " 请确认角色身份是否切换?! {0}",
                    ex.Message));
            }
            return result;
        }
        #endregion
    }
}
