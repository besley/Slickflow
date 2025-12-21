using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using static IronPython.Modules._ast;
using System.Threading.Tasks;
using RabbitMQ.Client.Impl;
using Slickflow.Engine.Business.Result;
using Slickflow.WebUtility;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// Workflow Service Unit Test
    /// 工作流服务单元测试
    /// </summary>
    public class WfUnitTestController : Controller
    {
        #region Workflow Api Test
        /// <summary>
        /// Start Process
        /// </summary>
        [HttpPost]
        public ResponseResult StartProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.StartProcess(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Start Process Parallel
        /// </summary>
        [HttpPost]
        public ResponseResult StartProcessParallel([FromBody] WfAppRunner runner)
        {
            var result = WfExecutedResult.Default();
            var pResult = Parallel.For(0, 2, i =>
            {
                using (var session = SessionFactory.CreateSession())
                {
                    var transaction = session.BeginTrans();
                    var wfService = new WorkflowService();

                    result = wfService.StartProcess(session.Connection, runner, session.Transaction);
                    if (result.Status == WfExecutedStatus.Success)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
            });
            return ResponseResult.Default(result.Message);
        }

        /// <summary>
        /// Run Process App
        /// </summary>
        [HttpPost]
        public ResponseResult RunProcessApp([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Run Process Continue
        /// </summary>
        [HttpPost]
        public ResponseResult RunProcessContinue([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();

            //运行当前任务
            //Run the current task
            var isContinued = false;
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    //确认是否继续执行
                    //Confirm whether to continue execution
                    isContinued = true;
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }

            //继续重复执行流程
            //Continue to repeat the process
            if (isContinued)
            {
                using (var session = SessionFactory.CreateSession())
                {
                    //检查待办任务，满足以下条件，继续默认执行流程
                    //1) 如果有当前用户的待办任务
                    //2) 流程只执行一次，不再循环调用
                    //Check the pending tasks, meet the following conditions, and continue with the default execution process
                    //1) If there are pending tasks for the current user
                    //2) The process is executed only once and will no longer be called in a loop
                    var transactionContinue = session.BeginTrans();
                    var myTask = wfService.GetTaskView(session.Connection, runner.AppInstanceId, runner.ProcessId, runner.UserId, session.Transaction);
                    if (myTask != null)
                    {
                        //构造执行用户信息
                        var runnerContinue = new WfAppRunner
                        {
                            AppInstanceId = runner.AppInstanceId,
                            AppName = runner.AppName,
                            ProcessId = runner.ProcessId,
                            Version = runner.Version,
                            TaskId = myTask.Id,
                            UserId = runner.UserId,
                            UserName = runner.UserName
                        };

                        //获取下一步的步骤信息
                        //Obtain information on the next steps
                        Dictionary<string, PerformerList> nextActivityPerformers = new Dictionary<string, PerformerList>();

                        var nextActivityRoleUserTree = wfService.GetNextActivityRoleUserTree(runner, runner.Conditions);
                        foreach (var nodeview in nextActivityRoleUserTree)
                        {
                            var performerList = new PerformerList();
                            foreach (var user in nodeview.Users)
                            {
                                var performer = new Performer(user.UserId, user.UserName);
                                performerList.Add(performer);
                            }
                            nextActivityPerformers.Add(nodeview.ActivityId, performerList);
                        }

                        runnerContinue.NextActivityPerformers = nextActivityPerformers;

                        //继续运行流程
                        //Continue running the process
                        var result = wfService.RunProcessApp(session.Connection, runnerContinue, session.Transaction);

                        if (result.Status == WfExecutedStatus.Success)
                        {
                            transactionContinue.Commit();
                            return ResponseResult.Success("The process has been successfully running continuously！");
                        }
                        else
                        {
                            transactionContinue.Rollback();
                            return ResponseResult.Error(result.Message);
                        }
                    }
                }
            }
            return ResponseResult.Default("The process only completes the execution of the current step!");
        }

        /// <summary>
        /// Sign Forward Process
        ///  加签流程测试
        /// </summary>
        [HttpPost]
        public ResponseResult SignForwardProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.SignForwardProcess(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Withdraw Process
        /// 撤销流程测试
        /// </summary>
        [HttpPost]
        public ResponseResult WithdrawProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.WithdrawProcess(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Sendback process
        /// 退回流程测试
        /// </summary>
        [HttpPost]
        public ResponseResult SendBackProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.SendBackProcess(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Reverse Process
        /// 返签流程测试
        /// </summary>
        [HttpPost]
        public ResponseResult ReverseProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.ReverseProcess(session.Connection, runner, 
                    session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Jump Process to Start Node
        /// 跳转流程测试
        /// </summary>
        [HttpPost]
        public ResponseResult JumpProcessStart([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.JumpProcess(session.Connection, runner,
                    session.Transaction, JumpOptionEnum.Startup);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Jump Process
        /// 跳转流程测试
        /// </summary>
        [HttpPost]
        public ResponseResult JumpProcess([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.JumpProcess(session.Connection, runner,
                    session.Transaction, JumpOptionEnum.Default);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }

        /// <summary>
        /// Jump Process to End Node
        /// 跳转流程测试
        /// </summary>
        [HttpPost]
        public ResponseResult JumpProcessEnd([FromBody] WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var wfService = new WorkflowService();
                var result = wfService.JumpProcess(session.Connection, runner, 
                    session.Transaction, JumpOptionEnum.End);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    return ResponseResult.Success();
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }
        }
        #endregion

        #region Get Process Data
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
                    string.Format("Failed to obtain process information！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Get Task Paged
        /// 获取流程记录列表
        /// </summary>
        /// <returns>流程列表</returns>
        [HttpGet]
        public ResponseResult<List<TaskViewEntity>> GetTaskPaged()
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var taskQuery = new TaskQuery
                {
                    UserId = "10"
                };
                var wfService = new WorkflowService();
                var entity = wfService.GetCompletedTasks(taskQuery).ToList();

                result = ResponseResult<List<TaskViewEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(
                    string.Format("Failed to obtain task information！{0}", ex.Message)
                );
            }
            return result;
        }
        #endregion

        #region Task Interface
        /// <summary>
        ///Task delegation method
        ///Test script:
        ///1) Start the process
        ///  http://localhost/sfapi2/api/wfunittest/startprocess
        /// {"UserId":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceId":"100","ProcessId":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}
        /// 
        ///2) Operation process
        ///  http://localhost/sfapi2/api/wfunittest/runprocessapp
        /// {"AppName":"SamplePrice","AppInstanceId":"100","ProcessId":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserId":"10","UserName":"Long","NextActivityPerformers":{"eb833577-abb5-4239-875a-5f2e2fcb6d57":[{"UserId":"20","UserName":"Jack"},{"UserId":"30","UserName":"Smith"},{"UserId":"40","UserName":"Tom"}]}}
        /// 
        ///3) Commissioned tasks
        ///  http://localhost/sfapi2/api/wfunittest/entrust
        /// {"TaskId":"210","RunnerId":"40","RunnerName":"Tom","EntrustToUserId":"30","EntrustToUserName":"smith"}
        /// 
        /// </summary>
        [HttpPost]
        public ResponseResult<WfDataManagedResult> Entrust([FromBody] TaskEntrustedEntity task)
        {
            var wfService = new WorkflowService();
            var entrustedResult = wfService.EntrustTask(task, true);

            if (entrustedResult.Status == WfDataManagedStatusEnum.Success)
            {
                return ResponseResult<WfDataManagedResult>.Success(entrustedResult);
            }
            else
            {
                return ResponseResult<WfDataManagedResult>.Error(entrustedResult.Message);
            }
        }
        #endregion
    }
}
