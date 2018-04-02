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
    #region 测试数据
    //带有子流程的报销流程基本测试
    //文件名: baoxiao.subprocess.xml
    //startup process:
    //{"UserID":"30","UserName":"Jack","AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2"}

    //run process app:
    ////填写申请节点：
    ////下一步是“主管审批”办理节点
    //{"AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2","UserID":"30","UserName":"Jack","NextActivityPerformers":{"f9a8b5be-0026-4ac7-bc2f-bf99878d7242":[{"UserID":50,"UserName":"Hansam"}]}}

    //withdraw process:
    //撤销至上一步节点（由主管审批到上一步填写申请）
    //{"UserID":"30","UserName":"Jack","AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2"}

    //runprocess app
    //主管审批
    //启动子流程
    //{"AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2","UserID":"50","UserName":"Hansam","NextActivityPerformers":{"6b7152b8-ee18-4419-b62b-54b91f28aa40":[{"UserID":10,"UserName":"Long"}]}}

    //runprocess app
    //子流程结束，子流程节点继续前进执行
    //子流程节点办理执行：
    //{"UserID":"10","UserName":"Long","AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2","NextActivityPerformers":{"a43b51a9-4b68-42ee-aa31-ba821ae49885":[{"UserID":40,"UserName":"Susan"}]}}

    //runprocess app
    //财务审批节点：
    //{"UserID":"40","UserName":"Susan","AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2","NextActivityPerformers":{"a43b51a9-4b68-42ee-aa31-ba821ae49885":[{"UserID":20,"UserName":"Meilinda"}]}}

    //runprocess app
    //备案节点：
    //下一步为结束节点
    //{"UserID":"20","UserName":"Meilinda","AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2"}

    //reverse process:
    //返签
    //{"UserID":"20","UserName":"Meilinda","AppName":"ExpenseAccount","AppInstanceID":"300","ProcessGUID":"b5d12fe5-8942-4ec7-9f5f-832daec31ec2"}

    //sendback process
    //退回
    //数据格式同返签(撤销,退回,返签Json数据格式相同.)

    //撤销流程: WithdrawProcess
    //退回流程：SendBackProcess
    //返签流程：ReverseProcess
    //取消运行流程：CancelProcess
    //废弃所有流程实例：DiscardProcess
    #endregion

    /// <summary>
    /// 子流程测试控制器
    /// </summary>
    public class WfSubProcessController : Controller
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
                trans.Commit();

                int newProcessInstanceID = result.ProcessInstanceIDStarted;
                IList<NodeView> nextStpes = wfService.GetNextActivityTree(starter);

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
        #endregion
    }
}
