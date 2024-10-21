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
using Slickflow.Engine.Service;
using static IronPython.Modules._ast;
using System.Threading.Tasks;
using RabbitMQ.Client.Impl;

namespace Slickflow.WebApi.Controllers
{
    /// <summary>
    /// 引擎接口单元测试
    /// </summary>
    public class WfUnitTestController : Controller
    {
        #region Workflow Api访问操作
        /// <summary>
        ///  启动流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        ///  启动流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        ///  运行流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        ///  运行流程(连续运行模式)
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult RunProcessContinue([FromBody] WfAppRunner runner)
        {
            var wfService = new WorkflowService();

            //运行当前任务
            var isContinued = false;
            using (var session = SessionFactory.CreateSession())
            {
                var transaction = session.BeginTrans();
                var result = wfService.RunProcessApp(session.Connection, runner, session.Transaction);

                if (result.Status == WfExecutedStatus.Success)
                {
                    transaction.Commit();
                    //确认是否继续执行
                    isContinued = true;
                }
                else
                {
                    transaction.Rollback();
                    return ResponseResult.Error(result.Message);
                }
            }

            //继续重复执行流程
            if (isContinued)
            {
                using (var session = SessionFactory.CreateSession())
                {
                    //检查待办任务，满足以下条件，继续默认执行流程
                    //1) 如果有当前用户的待办任务
                    //2) 流程只执行一次，不再循环调用
                    var transactionContinue = session.BeginTrans();
                    var myTask = wfService.GetTaskView(session.Connection, runner.AppInstanceID, runner.ProcessGUID, runner.UserID, session.Transaction);
                    if (myTask != null)
                    {
                        //构造执行用户信息
                        var runnerContinue = new WfAppRunner
                        {
                            AppInstanceID = runner.AppInstanceID,
                            AppName = runner.AppName,
                            ProcessGUID = runner.ProcessGUID,
                            Version = runner.Version,
                            TaskID = myTask.TaskID,
                            UserID = runner.UserID,
                            UserName = runner.UserName
                        };

                        //获取下一步的步骤信息
                        Dictionary<string, PerformerList> nextActivityPerformers = new Dictionary<string, PerformerList>();

                        var nextActivityRoleUserTree = wfService.GetNextActivityRoleUserTree(runner, runner.Conditions);
                        foreach (var nodeview in nextActivityRoleUserTree)
                        {
                            var performerList = new PerformerList();
                            foreach (var user in nodeview.Users)
                            {
                                var performer = new Performer(user.UserID, user.UserName);
                                performerList.Add(performer);
                            }
                            nextActivityPerformers.Add(nodeview.ActivityGUID, performerList);
                        }

                        runnerContinue.NextActivityPerformers = nextActivityPerformers;

                        //继续运行流程
                        var result = wfService.RunProcessApp(session.Connection, runnerContinue, session.Transaction);

                        if (result.Status == WfExecutedStatus.Success)
                        {
                            transactionContinue.Commit();
                            return ResponseResult.Success("根据下一步的待办人是当前在办人，流程可以继续向前运行，流程已经成功连续运行！");
                        }
                        else
                        {
                            transactionContinue.Rollback();
                            return ResponseResult.Error(result.Message);
                        }
                    }
                }
            }
            return ResponseResult.Default("流程仅完成当前步骤的运行!");
        }

        /// <summary>
        ///  加签流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        ///  撤销流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        ///  退回流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        ///  返签流程测试
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>执行结果</returns>
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
        /// 跳转流程测试
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
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
        /// 跳转流程测试
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
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
        /// 跳转流程测试
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
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

        #region 获取流程数据列表
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
                    string.Format("获取流程基本信息失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
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
                    UserID = "10"
                };
                var wfService = new WorkflowService();
                var entity = wfService.GetCompletedTasks(taskQuery).ToList();

                result = ResponseResult<List<TaskViewEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(
                    string.Format("获取任务信息失败！{0}", ex.Message)
                );
            }
            return result;
        }
        #endregion

        #region 任务处理接口
        /// <summary>
        /// 任务委托方法
        /// 测试脚本：
        /// 1) 启动流程
        /// http://localhost/sfapi2/api/wfunittest/startprocess
        /// {"UserID":"10","UserName":"Long","AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d"}
        /// 
        /// 2) 运行流程
        /// http://localhost/sfapi2/api/wfunittest/runprocessapp
        /// {"AppName":"SamplePrice","AppInstanceID":"100","ProcessGUID":"072af8c3-482a-4b1c-890b-685ce2fcc75d","UserID":"10","UserName":"Long","NextActivityPerformers":{"eb833577-abb5-4239-875a-5f2e2fcb6d57":[{"UserID":"20","UserName":"Jack"},{"UserID":"30","UserName":"Smith"},{"UserID":"40","UserName":"Tom"}]}}
        /// 
        /// 3) 委托任务
        /// http://localhost/sfapi2/api/wfunittest/entrust
        /// {"TaskID":"210","RunnerID":"40","RunnerName":"Tom","EntrustToUserID":"30","EntrustToUserName":"smith"}
        /// 
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult Entrust([FromBody] TaskEntrustedEntity task)
        {
            var wfService = new WorkflowService();
            var isEntrusted = wfService.EntrustTask(task, true);

            if (isEntrusted)
            {
                return ResponseResult.Success();
            }
            else
            {
                return ResponseResult.Error("Entrust task failture...");
            }
        }
        #endregion
    }
}
