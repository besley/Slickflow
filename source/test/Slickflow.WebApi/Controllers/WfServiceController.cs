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
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Service;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// 引擎接口服务
    /// </summary>
    public class WfServiceController : Controller
    {
        #region Workflow 步骤获取操作
        /// <summary>
        /// 获取下一步的步骤列表
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>步骤列表</returns>
        [HttpPost]
        public ResponseResult<NextStepInfo> GetNextStepInfo([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<NextStepInfo>.Default();
            try
            {
                var wfService = new WorkflowService();

                var nextStepInfo = wfService.GetNextStepInfo(runner, runner.Conditions);
                result = ResponseResult<NextStepInfo>.Success(nextStepInfo);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<NextStepInfo>.Error(ex.Message);
            }
            return result;
        }
        #endregion

        #region Workflow 运行访问操作
        /// <summary>
        ///  启动流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> StartProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.StartProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        ///  运行流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> RunProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.RunProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        ///  加签流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> SignForwardProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.SignForwardProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        ///  撤销流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> WithdrawProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.WithdrawProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        ///  退回流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> SendBackProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.SendBackProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        ///  返签流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> ReverseProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.ReverseProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 跳转流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> RejectProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.RejectProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 跳转流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> JumpProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.JumpProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 关闭流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> CloseProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.CloseProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 修订流程
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult<WfExecutedResult> ReviseProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<WfExecutedResult>.Default();
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var wfResult = wfService.ReviseProcess(session.Connection, runner, session.Transaction);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    result = ResponseResult<WfExecutedResult>.Success(wfResult);
                }
                else
                {
                    transaction.Rollback();
                    result = ResponseResult<WfExecutedResult>.Error(wfResult.Message);
                }
            }
            return result;
        }
        #endregion

        #region Workflow 获取流程定义
        /// <summary>
        /// 获取流程定义实体
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>实体对象</returns>
        [HttpPost]
        public ResponseResult<ProcessEntity> GetProcessByVersion([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessByVersion(query.ProcessGUID, query.Version);

                result = ResponseResult<ProcessEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error(
                    string.Format("An error occurred when reading process by version!{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 获取流程记录列表
        /// </summary>
        /// <returns>流程列表</returns>
        [HttpGet]
        public ResponseResult<List<ProcessEntity>> GetProcessListSimple()
        {
            var result = ResponseResult<List<ProcessEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessListSimple().ToList();

                result = ResponseResult<List<ProcessEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ProcessEntity>>.Error(
                    string.Format("An error occurred when reading process list!{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 获取任务节点列表
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <returns>节点列表</returns>
        [HttpGet]
        public ResponseResult<List<Activity>> GetTaskActivityList(int id)
        {
            var result = ResponseResult<List<Activity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetTaskActivityList(id).ToList();

                result = ResponseResult<List<Activity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Activity>>.Error(
                    string.Format("An error occurred when reading taskview list!{0}", ex.Message)
                );
            }
            return result;
        }

        [HttpPost]
        public ResponseResult<List<Activity>> GetAllTaskActivityList([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<List<Activity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetAllTaskActivityList(query.ProcessGUID, query.Version).ToList();

                result = ResponseResult<List<Activity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Activity>>.Error(
                    string.Format("An error occurred when reading taskview list!{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 获取任务节点列表
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <returns>节点列表</returns>
        [HttpGet]
        public ResponseResult<TaskViewEntity> GetTaskView(int id)
        {
            var result = ResponseResult<TaskViewEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetTaskView(id);

                result = ResponseResult<TaskViewEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<TaskViewEntity>.Error(
                    string.Format("An error occurred when reading taskview entity!{0}", ex.Message)
                );
            }
            return result;
        }


        /// <summary>
        /// 导入流程
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult ImportProcess([FromBody] ProcessEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var wfService = new WorkflowService();
                wfService.ImportProcess(entity.XmlContent);
                result = ResponseResult.Success("Importing process diagram succussed");
            }
            catch(System.Exception ex)
            {
                result = ResponseResult.Error(
                   string.Format("An error occurred when importing process xml!{0}", ex.Message)
               );
            }
            return result;
        }
        #endregion

        #region Workflow 任务数据获取
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="query">任务查询</param>
        /// <returns>任务列表</returns>
        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> GetReadyTaskList([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var tasks = wfService.GetReadyTasks(query);
                if (tasks != null)
                    result = ResponseResult<List<TaskViewEntity>>.Success(tasks.ToList());
                else
                    result = ResponseResult<List<TaskViewEntity>>.Success(null);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="query">任务查询</param>
        /// <returns>任务列表</returns>
        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> GetDoneTaskList([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var tasks = wfService.GetCompletedTasks(query);
                if (tasks != null)
                    result = ResponseResult<List<TaskViewEntity>>.Success(tasks.ToList());
                else
                    result = ResponseResult<List<TaskViewEntity>>.Success(null);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(ex.Message);
            }
            return result;
        }
        #endregion
    }
}
