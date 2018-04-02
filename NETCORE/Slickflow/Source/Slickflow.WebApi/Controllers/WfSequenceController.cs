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
    //webapi: http://localhost/sfapi/api/wfsequence/
    //数据库表: WfProcess
    //普通顺序流程基本测试(顺序,返签,退回,撤销等测试)
    //流程记录ID：3
    //流程名称：报价流程
    //GUID: 072af8c3-482a-4b1c-890b-685ce2fcc75d
    //startup process:
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //run process app:
    ////业务员提交办理节点：
    ////下一步是“板房签字”办理节点
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"eb833577-abb5-4239-875a-5f2e2fcb6d57":[{"UserID":10,"UserName":"Long"}]}}

    //withdraw process:
    //撤销至上一步节点（由板房签字到上一步业务员提交）
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //runprocess app
    //板房签字办理节点
    //下一步是业务员确认
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"cab57060-f433-422a-a66f-4a5ecfafd54e":[{"UserID":10,"UserName":"Long"}]}}

    //流程结束
    //业务员确认办理节点
    //下一步流程结束
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","NextActivityPerformers":{"b53eb9ab-3af6-41ad-d722-bed946d19792":[{"UserID":10,"UserName":"Long"}]}}

    //run sub process
    //有子流程
    //启动子流程
    //{"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"5fa796f6-2d5d-4ed6-84e2-a7c4e4e6aabc":[{"UserID":10,"UserName":"Long"}]}}


    //reverse process:
    //返签
    //{"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}

    //sendback process
    //退回
    //数据格式同返签(撤销,退回,返签Json数据格式相同.)

    //read task, and make activity running:
    //任务阅读：
    //{"UserID":"10","UserName":"Long","TaskID":"17"}}

    //获取下一步办理步骤：
    //1) 根据应用来获取
    //GetNextSteps
    //{"AppName":"SamplePrice","AppInstanceID":915,"UserID":"10","UserName":"Long","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","NextActivityPerformers":{"39c71004-d822-4c15-9ff2-94ca1068d745":[{"UserID":"10","UserName":"Long"}]},"Flowstatus":"启动"}

    //2) 根据任务ID来获取
    //GetTaskNextSteps

    //撤销流程: WithdrawProcess
    //退回流程：SendBackProcess
    //返签流程：ReverseProcess
    //取消运行流程：CancelProcess
    //废弃所有流程实例：DiscardProcess


    /// <summary>
    /// 序列控制流
    /// </summary>
    public class WfSequenceController  : Controller
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
            var process = pm.GetByVersion("072AF8C3-482A-4B1C-890B-685CE2FCC75D", "1");      //版本默认为1

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
            var service = new WorkflowService();
            service.DeleteProcess(entity.ProcessGUID, entity.Version);
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
        #endregion

        #region 任务数据读取操作
        [HttpPost]
        public ResponseResult GetRunningTasks([FromBody] TaskQuery query)
        {
            IWorkflowService service = new WorkflowService();
            var result = service.GetRunningTasks(query);

            return ResponseResult.Success();
        }

        [HttpPost]
        public ResponseResult GetReadyTasks([FromBody] TaskQuery query)
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
        public ResponseResult<IList<NodeView>> GetTaskNextSteps(int id)
        {
            IWorkflowService wfService = new WorkflowService();
            var nodeList = wfService.GetNextActivityTree(id);

            return ResponseResult<IList<NodeView>>.Success(nodeList);
        }

        [HttpPost]
        public ResponseResult<IList<NodeView>> GetNextSteps([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            var nodeList = wfService.GetNextActivityTree(runner);

            return ResponseResult<IList<NodeView>>.Success(nodeList);
        }

        [HttpPost]
        public ResponseResult<IList<NodeView>> GetNextStepsContainer([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            var nodeList = wfService.GetNextActivityTree(runner, null);

            return ResponseResult<IList<NodeView>>.Success(nodeList);
        }

        [HttpGet]
        public ResponseResult<IDictionary<Guid, PerformerList>> GetNextActivityPerformers()
        {
            var performers = new PerformerList();
            performers.Add(new Performer("10", "Long"));

            IDictionary<Guid, PerformerList> nexts = new Dictionary<Guid, PerformerList>();
            nexts[Guid.Parse("10f7481a-ad1a-40f6-aeaa-8d32ceb1fcab")] = performers;

            return ResponseResult<IDictionary<Guid, PerformerList>>.Success(nexts);
        }

        [HttpPost]
        public ResponseResult ReadTask([FromBody] WfAppRunner runner)
        {
            IWorkflowService wfService = new WorkflowService();
            bool isRead = wfService.SetTaskRead(runner);

            return ResponseResult.Success();
        }

        [HttpPost]
        public ResponseResult GetTaskPerformers([FromBody] WfAppRunner runner)
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
        public ResponseResult GetActivityInstances(int id)
        {
            //id -- processInstanceID
            IWorkflowService service = new WorkflowService();
            var instanceList = service.GetActivityInstances(id);

            return ResponseResult.Success(instanceList.Count.ToString());
        }

        /// <summary>
        /// http://localhost/sfapi/api/Workflow/EntrustTask
        /// { "TaskID":"18240","RunnerID":"10","RunnerName":"Long","EntrustToUserID":"20","EntrustToUserName":"Zhang" }
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult EntrustTask([FromBody] TaskEntrustedEntity entity)
        {
            IWorkflowService service = new WorkflowService();
            service.EntrustTask(entity);

            return ResponseResult.Success();
        }
        #endregion
    }
}

