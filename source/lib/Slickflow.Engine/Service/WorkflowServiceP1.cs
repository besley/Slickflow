﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Slickflow.Module.Form;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Workflow Service (Data Query)
    /// 工作流服务(数据查询)
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region Process Instance Query
        /// <summary>
        /// Get process instance by id
        /// 获取流程实例数据
        /// </summary>
        public ProcessInstanceEntity GetProcessInstance(int processInstanceID)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(processInstanceID);
            return instance;
        }

        /// <summary>
        /// Get process instance by id
        /// 获取流程实例数据
        /// </summary>
        public ProcessInstanceEntity GetProcessInstance(IDbConnection conn, 
            int processInstanceID, 
            IDbTransaction trans)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(conn, processInstanceID, trans);
            return instance;
        }

        /// <summary>
        /// Get process instance by app instance id
        /// 获取流程实例数据
        /// </summary>
        public IList<ProcessInstanceEntity> GetProcessInstance(string appInstanceID)
        {
            var pim = new ProcessInstanceManager();
            var list = pim.GetProcessInstance(appInstanceID).ToList();
            return list;
        }

        /// <summary>
        /// Get process instance by activity instance id
        /// 获取流程实例数据
        /// </summary>
        public ProcessInstanceEntity GetProcessInstanceByActivity(int activityInstanceID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                var pim = new ProcessInstanceManager();
                var instance = pim.GetByActivity(session.Connection, activityInstanceID, session.Transaction);
                return instance;
            }
        }

        /// <summary>
        /// Get process instance by runner
        /// 获取运行中的流程实例
        /// </summary>
        public ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            var entity = pim.GetRunningProcessInstance(runner.AppInstanceID, runner.ProcessGUID);
            return entity;
        }

        /// <summary>
        /// Get process instance count
        /// 判断流程实例是否存在
        /// </summary>
        public Int32 GetProcessInstanceCount(string processGUID, string version)
        {
            var pim = new ProcessInstanceManager();
            return pim.GetProcessInstanceCount(processGUID, version);
        }

        /// <summary>
        /// Get process initiator
        /// 获取流程发起人信息
        /// </summary>
        public User GetProcessInitiator(int processInstanceID)
        {
            User initiator = null;
            try
            {
                var pim = new ProcessInstanceManager();
                initiator = pim.GetProcessInitiator(processInstanceID);
            }
            catch
            {
                throw;
            }
            return initiator;
        }

        /// <summary>
        /// Get activity instance
        /// 获取活动实例数据
        /// </summary>
        public ActivityInstanceEntity GetActivityInstance(int activityInstanceID)
        {
            var aim = new ActivityInstanceManager();
            var instance = aim.GetById(activityInstanceID);
            return instance;
        }

        /// <summary>
        /// Get activity instance
        /// 获取一个流程实例下的所有活动实例
        /// </summary>
        public IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID)
        {
            var aim = new ActivityInstanceManager();
            var session = SessionFactory.CreateSession();
            try
            {
                return aim.GetActivityInstances(processInstanceID, session);
            }
            catch
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// Get target activity instance list
        /// 获取目标活动实例列表
        /// </summary>
        public IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceID)
        {
            var tim = new TransitionInstanceManager();
            return tim.GetTargetActivityInstanceList(fromActivityInstanceID);
        }

        /// <summary>
        /// Get task performers
        /// 获取当前等待办理节点的任务分配人列表
        /// </summary>
        public IList<Performer> GetTaskPerformers(WfAppRunner runner)
        {
            var tm = new TaskManager();
            var tasks = tm.GetReadyTaskOfApp(runner).ToList();

            Performer performer;
            IList<Performer> performerList = new List<Performer>();
            foreach (var task in tasks)
            {
                performer = new Performer(task.AssignedToUserID, task.AssignedToUserName);
                performerList.Add(performer);
            }
            return performerList;
        }

        /// <summary>
        /// Determine whether the current task is the last task
        /// (Suitable for scenarios with simple nodes or multiple instance nodes)
        /// 判断当前任务是否是最后一个任务
        /// (适应于简单节点或者多实例节点的场景)
        /// </summary>
        public Boolean IsLastTask(int taskID)
        {
            var tm = new TaskManager();
            return tm.IsLastTask(taskID);
        }

        /// <summary>
        /// Entrust task
        /// 创建新的委托任务
        /// </summary>
        public Boolean EntrustTask(TaskEntrustedEntity entrusted, bool cancalOriginalTask = true)
        {
            var tm = new TaskManager();
            return tm.Entrust(entrusted, cancalOriginalTask);
        }

        /// <summary>
        /// Replace task
        /// 取代任务
        /// </summary>
        public Boolean ReplaceTask(int taskID, List<TaskReplacedEntity> replaced, WfAppRunner runner)
        {
            var tm = new TaskManager();
            return tm.Replace(taskID, replaced, runner);
        }

        /// <summary>
        /// Set process overdue
        /// 设置流程实例的过期时间
        /// </summary>
        public Boolean SetProcessOverdue(int processInstanceID, DateTime overdueDateTime, WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.SetOverdue(processInstanceID, overdueDateTime, runner);
        }

        /// <summary>
        /// Set the scheduled job of the process instance to completion status
        /// (Used for HangFire backend polling task)
        /// 设置活动实例的定时作业为完成状态
        /// (用于HangFire后台轮询任务)
        /// </summary>
        public void SetProcessJobTimerCompleted(IDbConnection conn, int processInstanceID, IDbTransaction trans)
        {
            var pim = new ProcessInstanceManager();
            var processInstance = pim.GetById(conn, processInstanceID, trans);
            processInstance.JobTimerStatus = (short)JobTimerStatusEnum.Completed;
            processInstance.JobTimerTreatedDateTime = System.DateTime.Now;
            pim.Update(conn, processInstance, trans);
        }

        /// <summary>
        /// Set the scheduled job of the activity instance to completion status
        /// (Used for HangFire backend polling task)
        /// 设置活动实例的定时作业为完成状态
        /// (用于HangFire后台轮询任务)
        /// </summary>
        public void SetActivityJobTimerCompleted(IDbConnection conn, int activityInstanceID, IDbTransaction trans)
        {
            var aim = new ActivityInstanceManager();
            var activityInstance = aim.GetById(conn, activityInstanceID, trans);
            activityInstance.JobTimerStatus = (short)JobTimerStatusEnum.Completed;
            activityInstance.JobTimerTreatedDateTime = System.DateTime.Now;
            aim.Update(conn, activityInstance, trans);
        }

        /// <summary>
        /// Get running node
        /// 获取流程当前运行节点信息
        /// </summary>
        public ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            var aim = new ActivityInstanceManager();
            var entity = aim.GetRunningNode(runner);

            return entity;
        }

        /// <summary>
        /// Determine if it is my task
        /// 判断是否是我的任务
        /// </summary>
        public bool IsMineTask(ActivityInstanceEntity entity, string userID)
        {
            var aim = new ActivityInstanceManager();
            bool isMine = aim.IsMineTask(entity, userID);
            return isMine;
        }

        /// <summary>
        /// Get running activity instance
        /// 获取正在运行中的活动实例
        /// </summary>
        public IList<ActivityInstanceEntity> GetRunningActivityInstance(TaskQuery query)
        {
            var aim = new ActivityInstanceManager();
            var list = aim.GetRunningActivityInstanceList(query.AppInstanceID, query.ProcessGUID, query.Version).ToList();
            return list;
        }

        /// <summary>
        /// Get process variable
        /// 获取流程变量
        /// </summary>
        public ProcessVariableEntity GetProcessVariable(int variableID)
        {
            var pvm = new ProcessVariableManager();
            var entity = pvm.GetVariableEntity(variableID);

            return entity;
        }

        /// <summary>
        /// Get process variable
        /// 获取流程变量
        /// </summary>
        public ProcessVariableEntity GetProcessVariable(ProcessVariableQuery query)
        {
            var pvm = new ProcessVariableManager();
            var entity = pvm.GetVariableEntity(query);
            return entity;
        }

        /// <summary>
        /// Get process variable list
        /// 获取变量列表
        /// </summary>
        public IList<ProcessVariableEntity> GetProcessVariableList(ProcessVariableQuery query)
        {
            var pvm = new ProcessVariableManager();
            var list = pvm.GetVariableList(query.ProcessInstanceID);
            return list;
        }

        /// <summary>
        /// Delete process variable
        /// 删除流程变量
        /// </summary>
        public void DeleteProcessVariable(int variableID)
        {
            var pvm = new ProcessVariableManager();
            pvm.DeleteVariable(variableID);
        }

        /// <summary>
        /// Validate process variable
        /// 验证触发表达式是否满足
        /// </summary>
        public Boolean ValidateProcessVariable(int processInstanceID, string expression)
        {
            var isValidated = false;
            using (var conn = SessionFactory.CreateConnection())
            {
                var pvm = new ProcessVariableManager();
                isValidated = pvm.ValidateProcessVariable(conn, processInstanceID, expression, null);
            }
            return isValidated;
        }

        /// <summary>
        /// Update process instance
        /// 更新流程实例实体
        /// </summary>
        public void UpdateProcessInstance(ProcessInstanceEntity entity)
        {
            var pim = new ProcessInstanceManager();
            pim.Update(entity);
        }

        /// <summary>
        /// Save process variable
        /// 保存流程变量
        /// </summary>
        public int SaveProcessVariable(ProcessVariableEntity entity)
        {
            var pvm = new ProcessVariableManager();
            var entityID = pvm.SaveVariable(entity);
            return entityID;
        }
        #endregion

        #region Role Resource Data
        /// <summary>
        /// Get role all
        /// 获取所有角色数据
        /// </summary>
        public IList<Role> GetRoleAll()
        {
            return ResourceService.GetRoleAll();
        }

        /// <summary>
        /// Get role by process
        /// 获取流程定义文件中的角色信息
        /// </summary>
        public IList<Role> GetRoleByProcess(string processGUID, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processGUID, version);
            var roleList = processModel.GetRoles();

            return roleList;
        }

        /// <summary>
        /// Get role user list by process
        /// 获取流程文件中角色用户的列表数据
        /// </summary>
        public IList<Role> GetRoleUserListByProcess(string processGUID, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processGUID, version);
            var roleList = processModel.GetRoles();
            var idsin = roleList.Select(r => r.ID).ToList().ToArray();

            var newRoleList = ResourceService.FillUsersIntoRoles(idsin);

            return newRoleList;
        }

        /// <summary>
        /// Get user all
        /// 获取所有用户数据
        /// </summary>
        public IList<User> GetUserAll()
        {
            return ResourceService.GetUserAll();
        }

        /// <summary>
        /// Get user list by role
        /// 根据角色获取用户列表
        /// </summary>
        public IList<User> GetUserListByRole(string roleID)
        {
            return ResourceService.GetUserListByRole(roleID);
        }

        /// <summary>
        /// Get performer list
        /// 获取节点上的执行者列表
        /// </summary>
        public PerformerList GetPerformerList(NodeView nextNode)
        {
            var performerList = PerformerBuilder.CreatePerformerList(nextNode.Roles);
            return performerList;
        }
        #endregion
    }
}
