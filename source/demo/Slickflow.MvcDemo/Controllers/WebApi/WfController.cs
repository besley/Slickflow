using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.WebUtility;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;

namespace Slickflow.MvcDemo.Controllers.WebApi
{
    /// <summary>
    /// Process Service Controller
    /// Example code for developers' reference
    /// 流程服务控制器
    /// 示例代码，供开发人员参考
    /// </summary>
    public class WfController : Controller
    {
        /// <summary>
        /// Query process file
        /// 读取XML文件
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessFileEntity> QueryProcessFile([FromBody] ProcessFileQuery query)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessFile(query.ProcessId, query.Version);

                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Query next step
        /// 查询流程下一步信息
        /// </summary>
        [HttpPost]
        public ResponseResult<List<NodeView>> QueryNextStep([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfservice = new WorkflowService();
                var nodeViewList = wfservice.GetNextActivityTree(runner).ToList<NodeView>();
                result = ResponseResult<List<NodeView>>.Success(nodeViewList);
            }
            catch(System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get next step role user tree
        /// 查询流程下一步信息的节点角色人员树
        /// </summary>
        [HttpPost]
        public ResponseResult<List<NodeView>> GetNextStepRoleUserTree([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfservice = new WorkflowService();
                var nodeViewList = wfservice.GetNextActivityRoleUserTree(runner).ToList<NodeView>();
                result = ResponseResult<List<NodeView>>.Success(nodeViewList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get first step role user tree
        /// 查询流程下一步信息的节点角色人员树
        /// </summary>
        [HttpPost]
        public ResponseResult<List<NodeView>> GetFirstStepRoleUserTree([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<List<NodeView>>.Default();
            try
            {
                var wfservice = new WorkflowService();
                var nodeViewList = wfservice.GetFirstActivityRoleUserTree(runner, runner.Conditions).ToList<NodeView>();
                result = ResponseResult<List<NodeView>>.Success(nodeViewList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<NodeView>>.Error(ex.Message);
            }
            return result;
        }

        [HttpPost]
        public string Test([FromBody] WfAppRunner runner)
        {
            return "hello";
        }

        /// <summary>
        /// Query process roles
        /// 获取流程定义下的角色数据
        /// </summary>
        [HttpPost]
        public ResponseResult<List<Role>> QueryProcessRoles([FromBody] ProcessQuery query)
        {
            var result = ResponseResult<List<Role>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var roleList = wfService.GetRoleByProcess(query.ProcessId, query.Version).ToList();
                result = ResponseResult<List<Role>>.Success(roleList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Role>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Query process role user list
        /// 获取流程定义下的角色数据
        /// </summary>
        [HttpPost]
        public ResponseResult<List<Role>> QueryProcessRoleUserList([FromBody] ProcessQuery query)
        { 
            var result = ResponseResult<List<Role>>.Default();
                        try
            {
                var wfService = new WorkflowService();
                var roleList = wfService.GetRoleUserListByProcess(query.ProcessId, query.Version).ToList();
                result = ResponseResult<List<Role>>.Success(roleList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Role>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get user by role
        /// 获取角色下的用户数据
        /// </summary>
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
                    Performer performer = new Performer(item.UserId.ToString(), item.UserName);
                    performList.Add(performer);
                }
                result = ResponseResult<List<Performer>>.Success(performList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<Performer>>.Error(ex.Message);
            }
            return result;
        }


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
                result = ResponseResult<List<TaskViewEntity>>.Error(ex.Message);
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
                result = ResponseResult<List<ActivityInstanceEntity>>.Error(ex.Message);
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
                result = ResponseResult<List<TaskViewEntity>>.Error(ex.Message);
            }
            return result;
        }
    }
}
