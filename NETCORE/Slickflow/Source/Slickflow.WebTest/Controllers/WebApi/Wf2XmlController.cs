using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Parser;
using Slickflow.Engine.Utility;
using Slickflow.WebTest.Models;

namespace Slickflow.WebTest.Controllers.WebApi
{
    /// <summary>
    /// 流程控制器
    /// </summary>
    public class Wf2XmlController : Controller
    {
        #region 流程数据查询
        /// <summary>
        /// 获取流程记录列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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
        /// 获取待办任务
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>待办任务列表</returns>
        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> GetTaskList([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetReadyTasks(query);
                if (entity != null)
                    result = ResponseResult<List<TaskViewEntity>>.Success(entity.ToList());
                else
                    result = ResponseResult<List<TaskViewEntity>>.Success(null);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(
                    string.Format("获取任务信息失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 获取已办任务
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>已办任务列表</returns>
        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> GetDoneList([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetCompletedTasks(query);
                if (entity != null)
                    result = ResponseResult<List<TaskViewEntity>>.Success(entity.ToList());
                else
                    result = ResponseResult<List<TaskViewEntity>>.Success(null);
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

        #region 流程流转步骤获取
        /// <summary>
        /// 获取流程下一步
        /// </summary>
        /// <param name="runner">执行用户</param>
        /// <returns>下一步步骤列表</returns>
        [HttpPost]
        public ResponseResult<List<NodeView>> GextNextActivityUserTree([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {                              
                var wfService = new WorkflowService();
                var nextSteps = wfService.GetNextActivityRoleUserTree(runner, runner.Conditions).ToList();

                //追加模拟用户
                nextSteps = ProcessModelMimic.AppendMimicUser(nextSteps, runner).ToList();
                result = ResponseResult<List<NodeView>>.Success(nextSteps);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取下一步步骤列表
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>步骤列表</returns>
        [HttpPost]
        public ResponseResult<List<NodeView>> GetNextStepInfo([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var nextStepInfo = wfService.GetNextStepInfo(runner, runner.Conditions);

                //从下一步预选步骤人员中追加用户到步骤列表中去
                var nextStepTree = nextStepInfo.NextActivityRoleUserTree.ToList();
                var nextActivityPerformers = nextStepInfo.NextActivityPerformers;

                if (nextActivityPerformers != null)
                {
                    //加载预选用户
                    nextStepTree = ProcessModelMimic.AppendPremilinaryUser(nextStepTree, nextActivityPerformers, true);
                }
                else
                {
                    //追加模拟用户
                    nextStepTree = ProcessModelMimic.AppendMimicUser(nextStepTree, runner).ToList();
                }
                result = ResponseResult<List<NodeView>>.Success(nextStepTree);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取流程下一步
        /// </summary>
        /// <param name="runner">执行用户</param>
        /// <returns>下一步步骤列表</returns>
        [HttpPost]
        public ResponseResult<List<NodeView>> GetPrevActivityUserTree([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var prevSteps = wfService.GetPreviousActivityTree(runner).ToList();

                //追加模拟用户
                prevSteps = ProcessModelMimic.AppendMimicUser(prevSteps, runner).ToList();
                result = ResponseResult<List<NodeView>>.Success(prevSteps);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取下一步步骤列表
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>步骤列表</returns>
        [HttpPost]
        public ResponseResult<PreviousStepInfo> GetPreviousStepInfo([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<PreviousStepInfo>.Default();
            try
            {
                var wfService = new WorkflowService();
                var previousStepInfo = wfService.GetPreviousStepInfo(runner);

                result = ResponseResult<PreviousStepInfo>.Success(previousStepInfo);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<PreviousStepInfo>.Error(ex.Message);
            }
            return result;
        }
        #endregion

        #region 流程流转方法
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="runner">执行用户</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult StartProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();

            try
            {
                var wfResult = wfService.StartProcess(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 运行流程
        /// </summary>
        /// <param name="runner">执行用户</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult RunProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();

            try
            {
                var wfResult = wfService.RunProcessApp(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 退回流程
        /// </summary>
        /// <param name="runner">执行用户</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult SendBackProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();
            try
            {               
                var wfResult = wfService.SendBackProcess(runner);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch(System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 撤销流程
        /// </summary>
        /// <param name="runner">执行用户</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ResponseResult WithdrawProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();
            try
            {
                var wfResult = wfService.WithdrawProcess(runner);

                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    result = ResponseResult.Success(wfResult.Message);
                }
                else
                {
                    result = ResponseResult.Error(wfResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 删除流程实例
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>结果</returns>
        [HttpPost]
        public ResponseResult DeleteInstance([FromBody] ProcessQuery query)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();

            try
            {
                var isOK = wfService.DeleteInstanceInt(query.ProcessGUID, query.Version);

                if (isOK == true)
                {
                    result = ResponseResult.Success("已经成功删除！");
                }
                else
                {
                    result = ResponseResult.Error("删除失败，或者记录已经为空！");
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }
        #endregion

        #region 流程模型生成
        public ResponseResult CreateModel()
        {
            return null;
        }
        #endregion
    }
}
