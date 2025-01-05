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
    /// Workflow Service Controller
    /// 工作流服务接口测试
    /// </summary>
    public class WfServiceController : Controller
    {
        #region Workflow Get Step Info
        /// <summary>
        /// Get Next Step Info
        /// 获取下一步的步骤列表
        /// </summary>
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

        #region Workflow Execute Process
        /// <summary>
        /// Start process
        ///  启动流程
        /// </summary>
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
        /// Run Process
        /// 运行流程测试
        /// </summary>
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
        /// Sign Forward Process
        /// 加签流程
        /// </summary>
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
        /// Withdraw process
        /// 撤销流程
        /// </summary>
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
        /// Sendback process
        /// 退回流程
        /// </summary>
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
        /// Reverse process
        ///  返签流程
        /// </summary>
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
        /// Reject process
        /// 跳转流程
        /// </summary>
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
        /// Jump process
        /// 跳转流程
        /// </summary>
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
        /// Close process
        /// 关闭流程
        /// </summary>
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
        /// Revise process
        /// 修订流程
        /// </summary>
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

        #region Workflow Process Definition
        /// <summary>
        /// Get process by version
        /// 获取流程定义实体
        /// </summary>
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
        /// Get Process List Simple
        /// 获取流程记录列表
        /// </summary>
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
        /// Get Task activity list
        /// 获取任务节点列表
        /// </summary>
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
        /// Get Task View
        /// 获取任务节点列表
        /// </summary>
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
        /// Import process
        /// 导入流程
        /// </summary>
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

        #region Workflow Task Data
        /// <summary>
        /// Get Ready Task list
        /// 获取任务列表
        /// </summary>
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
        /// Get Done Task List
        /// 获取任务列表
        /// </summary>
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
