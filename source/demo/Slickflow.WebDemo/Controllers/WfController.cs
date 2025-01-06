using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using SlickOne.WebUtility;

namespace Slickflow.WebDemo.Controllers
{
    /// <summary>
    /// Process Service Controller
    /// Example code for developers' reference
    /// 流程服务控制器
    /// 示例代码，供开发人员参考
    /// </summary>
    public class WfController : Controller
    {
        [HttpGet]
        public string Test(string id)
        {
            return id;
        }

        /// <summary>
        /// Query process file by id
        /// 读取XML文件
        /// </summary>
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
                result = ResponseResult<ProcessFileEntity>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Get next step info
        /// 获取下一步步骤列表
        /// </summary>
        [HttpPost]
        public ResponseResult<NextStepInfo> GetNextStepInfo([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult<NextStepInfo>.Default();
            try
            {
                var wfService = new WorkflowService();
                var nextStepInfo = wfService.GetNextStepInfo(runner, runner.Conditions);
                var performers = nextStepInfo.NextActivityPerformers;
                if (performers == null)
                {
                    //read role data from activity definition
                }

                result = ResponseResult<NextStepInfo>.Success(nextStepInfo);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<NextStepInfo>.Error(ex.Message);
            }
            return result;
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
                var roleList = wfService.GetRoleByProcess(query.ProcessGUID, query.Version).ToList();
                var roleQuery = (from r in roleList
                                 select new Role
                                 {
                                     ID = r.ID,
                                     RoleName = string.Format("{0}({1})", r.RoleName, r.RoleCode)
                                 });
                var list = roleQuery.ToList();
                result = ResponseResult<List<Role>>.Success(list);
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
                var roleList = wfService.GetRoleUserListByProcess(query.ProcessGUID, query.Version).ToList();
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
        public ResponseResult<List<User>> GetUserByRole(string id)
        {
            var result = ResponseResult<List<User>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var userList = wfService.GetUserListByRole(id).ToList();
                result = ResponseResult<List<User>>.Success(userList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<User>>.Error(ex.Message);
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


        /// <summary>
        /// Run process
        /// </summary>
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
        /// Reject Process
        /// 驳回流程
        /// </summary>
        [HttpPost]
        public ResponseResult RejectProcess([FromBody] WfAppRunner runner)
        {
            var result = ResponseResult.Default();
            var wfService = new WorkflowService();

            try
            {
                var wfResult = wfService.RejectProcess(runner);
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
    }
}
