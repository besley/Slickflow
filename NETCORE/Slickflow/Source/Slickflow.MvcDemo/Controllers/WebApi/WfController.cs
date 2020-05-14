using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using SlickOne.WebUtility;

namespace Slickflow.MvcDemo.Controllers.WebApi
{
    /// <summary>
    /// 流程服务控制器
    /// 示例代码，请勿直接作为生产项目代码使用。
    /// </summary>
    public class WfController : Controller
    {
        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProcessFileEntity> QueryProcessFile([FromBody] ProcessFileQuery query)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessFile(query.ProcessGUID, query.Version);

                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(
                    string.Format("获取流程XML文件失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 查询流程下一步信息
        /// </summary>
        /// <param name="runner">当前执行人</param>
        /// <returns>流程下一步信息</returns>
        [HttpPost]
        public ResponseResult<List<NodeView>> QueryNextStep([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfservice = new WorkflowService();
                var nodeViewList = wfservice.GetNextActivityTree(runner).ToList<NodeView>();
                result = ResponseResult<List<NodeView>>.Success(nodeViewList, "获取流程下一步信息成功!");
            }
            catch(System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(string.Format(
                    " 请确认角色身份是否切换?! {0}", 
                    ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 查询流程下一步信息的节点角色人员树
        /// </summary>
        /// <param name="runner">当前执行人</param>
        /// <returns>流程下一步信息</returns>
        [HttpPost]
        public ResponseResult<List<NodeView>> GetNextStepRoleUserTree([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfservice = new WorkflowService();
                var nodeViewList = wfservice.GetNextActivityRoleUserTree(runner).ToList<NodeView>();
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

        /// <summary>
        /// 查询流程下一步信息的节点角色人员树
        /// </summary>
        /// <param name="runner">当前执行人</param>
        /// <returns>流程下一步信息</returns>
        [HttpPost]
        public ResponseResult<List<NodeView>> GetFirstStepRoleUserTree([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfservice = new WorkflowService();
                var nodeViewList = wfservice.GetFirstActivityRoleUserTree(runner, runner.Conditions).ToList<NodeView>();
                result = ResponseResult<List<NodeView>>.Success(nodeViewList, "获取流程第一个办理节点信息成功!");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(string.Format(
                    " 读取第一个办理节点发生异常： {0}",
                    ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 获取流程定义下的角色数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<Role>> QueryProcessRoles([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<List<Role>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var roleList = wfService.GetRoleByProcess(query.ProcessGUID, query.Version).ToList();
                result = ResponseResult<List<Role>>.Success(roleList, "成功获取流程定义的角色数据！");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Role>>.Error(string.Format(
                    "获取流程定义的角色数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 获取流程定义下的角色数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<Role>> QueryProcessRoleUserList([FromBody] ProcessQuery query)
        { 
            var result = ResponseResult<List<Role>>.Default();
                        try
            {
                var wfService = new WorkflowService();
                var roleList = wfService.GetRoleUserListByProcess(query.ProcessGUID, query.Version).ToList();
                result = ResponseResult<List<Role>>.Success(roleList, "成功获取流程定义的角色用户数据！");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Role>>.Error(string.Format(
                    "获取流程定义的角色用户数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 获取角色下的用户数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<Performer>> GetUserByRole(string id)
        {
            var result = ResponseResult<List<Performer>>.Default();
            try
            {
                var performList = new List<Performer>();
                var wfService = new WorkflowService();
                var itemList = wfService.GetUserListByRole(id).ToList();
                foreach (var item in itemList)
                {
                    Performer performer = new Performer(item.UserID.ToString(), item.UserName);
                    performList.Add(performer);
                }
                result = ResponseResult<List<Performer>>.Success(performList, "成功获取流角色用户数据！");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Performer>>.Error(string.Format(
                    "获取角色用户数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        ///// <summary>
        ///// 根据角色IDs获取用户列表
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ResponseResult<List<Role>> QueryRoleUserByRoleIDs([FromBody] RoleIDsQuery query)
        //{
        //    var result = ResponseResult<List<Role>>.Default();
        //    try
        //    {
        //        var wfService = new WorkflowService();
        //        var roleList = wfService.GetRoleUserByRoleIDs(query.RoleIDs).ToList();
        //        result = ResponseResult<List<Role>>.Success(roleList, "成功获取角色用户数据！");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        result = ResponseResult<List<Role>>.Error(string.Format(
        //            "获取角色用户数据失败, 异常信息:{0}",
        //            ex.Message));
        //    }
        //    return result;
        //}

        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> QueryReadyTasks([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var taskList = new List<TaskViewEntity>();
                var wfService = new WorkflowService();
                var itemList = wfService.GetReadyTasks(query);

                if (itemList != null)
                {
                    taskList = itemList.ToList();
                }
                result = ResponseResult<List<TaskViewEntity>>.Success(taskList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(string.Format(
                    "获取待办任务数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        [HttpPost]
        public ResponseResult<List<ActivityInstanceEntity>> QueryReadyActivityInstance([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<ActivityInstanceEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var itemList = wfService.GetRunningActivityInstance(query).ToList();

                
                result = ResponseResult<List<ActivityInstanceEntity>>.Success(itemList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ActivityInstanceEntity>>.Error(string.Format(
                    "获取待办任务数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        [HttpPost]
        public ResponseResult<List<TaskViewEntity>> QueryCompletedTasks([FromBody] TaskQuery query)
        {
            var result = ResponseResult<List<TaskViewEntity>>.Default();
            try
            {
                var taskList = new List<TaskViewEntity>();
                var wfService = new WorkflowService();
                var itemList = wfService.GetCompletedTasks(query);

                if (itemList != null)
                {
                    taskList = itemList.ToList();
                }
                result = ResponseResult<List<TaskViewEntity>>.Success(taskList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskViewEntity>>.Error(string.Format(
                    "获取已办任务数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }
    }
}
