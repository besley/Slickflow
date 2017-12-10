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
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Xml;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Core;
using Slickflow.Engine.Core.Result;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;
using Slickflow.Engine.Parser;
using Slickflow.WebApi.Utility;

namespace Slickflow.WebApi.Controllers
{
    public class Wf2GoController : ApiController
    {
        #region Workflow 数据访问基本操作
        [HttpGet]
        [AllowAnonymous]
        public string Hello()
        {
            return "Hello World!";
        }

        // GET: /Workflow/
        [HttpGet]
        [AllowAnonymous]
        public object Get()
        {
            ProcessManager pm = new ProcessManager();
            var process = pm.GetByVersion("072AF8C3-482A-4B1C-890B-685CE2FCC75D", "1");

            return process;
        }

        [HttpPost]
        [AllowAnonymous]
        public void InsertProcess(ProcessEntity obj)
        {
            ProcessManager pm = new ProcessManager();
            pm.Insert(obj);
        }

        [HttpPost]
        [AllowAnonymous]
        public void UpdateProcess(ProcessEntity obj)
        {
            ProcessManager pm = new ProcessManager();
            pm.Update(obj);
        }

        [HttpPost]
        [AllowAnonymous]
        public void RemoveProcess(ProcessEntity entity)
        {
            ProcessManager pm = new ProcessManager();
            pm.Delete(entity.ProcessGUID, entity.Version);
        }

        [HttpGet]
        [AllowAnonymous]
        public ProcessInstanceEntity GetProcessInstance()
        {
            IWorkflowService service = new WorkflowService();
            var instance = service.GetProcessInstance(14001);

            return instance;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActivityInstanceEntity GetActivityInstance()
        {
            IWorkflowService service = new WorkflowService();
            var instance = service.GetActivityInstance(100);

            return instance;
        }
        #endregion

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
                trans.Commit();

                int newProcessInstanceID = result.ProcessInstanceIDStarted;
                IList<NodeView> nextStpes = wfService.GetNextActivityTree(starter);

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
        public ResponseResult RunProcessApp(WfAppRunner runner)
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

        #region 任务数据读取操作
        [HttpPost]
        [AllowAnonymous]
        public ResponseResult GetRunningTasks(TaskQuery query)
        {
            IWorkflowService service = new WorkflowService();
            var result = service.GetRunningTasks(query);

            return ResponseResult.Success();
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseResult GetReadyTasks(TaskQuery query)
        {
            IWorkflowService service = new WorkflowService();
            var result = service.GetReadyTasks(query);

            return ResponseResult.Success();
        }
        #endregion

        #region 流程运行数据读取
        /// <summary>
        /// 获取下一步办理的节点
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseResult<IList<NodeView>> GetTaskNextSteps(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var nodeList = wfService.GetNextActivityTree(id);

            return ResponseResult<IList<NodeView>>.Success(nodeList);
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseResult<IList<NodeView>> GetNextSteps(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            var nodeList = wfService.GetNextActivityTree(runner);

            return ResponseResult<IList<NodeView>>.Success(nodeList);
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseResult<IList<NodeView>> GetNextStepsContainer(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            var nodeList = wfService.GetNextActivityTree(runner, null);

            return ResponseResult<IList<NodeView>>.Success(nodeList);
        }

        [HttpGet]
        [AllowAnonymous]
        public ResponseResult<IDictionary<Guid, PerformerList>> GetNextActivityPerformers()
        {
            var performers = new PerformerList();
            performers.Add(new Performer("10", "Long"));

            IDictionary<Guid, PerformerList> nexts = new Dictionary<Guid, PerformerList>();
            nexts[Guid.Parse("10f7481a-ad1a-40f6-aeaa-8d32ceb1fcab")] = performers;

            return ResponseResult<IDictionary<Guid, PerformerList>>.Success(nexts);
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseResult ReadTask(WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            bool isRead = wfService.SetTaskRead(runner);

            return ResponseResult.Success();
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseResult GetTaskPerformers(WfAppRunner runner)
        {
            IWorkflowService service = new WorkflowService();
            var performers = service.GetTaskPerformers(runner);

            return ResponseResult.Success(performers.Count.ToString());
        }


        /// <summary>
        /// 获取流程实例下所有活动节点的测试示例： 
        ///http://localhost/sfapi/api/Workflow/GetActivityInstances/16137
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseResult GetActivityInstances(int id)
        {
            //id -- processInstanceID
            IWorkflowService service = new WorkflowService();
            var instanceList = service.GetActivityInstances(id);

            return ResponseResult.Success(instanceList.Count.ToString());
        }

        /// <summary>
        /// 获取流程实例下所有活动节点的测试示例： 
        ///http://localhost/sfapi/api/Wf2Go/GetNextActivityInstanceList/16137
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponseResult<List<ActivityInstanceEntity>> GetNextActivityInstanceList(int id)
        {
            //id -- processInstanceID
            IWorkflowService service = new WorkflowService();
            var instanceList = service.GetNextActivityInstanceList(id).ToList();

            return ResponseResult<List<ActivityInstanceEntity>>.Success(instanceList);
        }

        /// <summary>
        /// http://localhost/sfapi/api/Workflow/EntrustTask
        /// { "TaskID":"18240","RunnerID":"10","RunnerName":"Long","EntrustToUserID":"20","EntrustToUserName":"Zhang" }
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ResponseResult EntrustTask(TaskEntrustedEntity entity)
        {
            IWorkflowService service = new WorkflowService();
            service.EntrustTask(entity);

            return ResponseResult.Success();
        }
        #endregion
    }
}
