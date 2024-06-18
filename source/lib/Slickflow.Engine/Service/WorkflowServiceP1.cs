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
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 工作流服务(数据查询)
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region 流程实例信息获取
        /// <summary>
        /// 获取流程实例数据
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>流程实例实体</returns>
        public ProcessInstanceEntity GetProcessInstance(int processInstanceID)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(processInstanceID);
            return instance;
        }

        /// <summary>
        /// 获取流程实例数据
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实例实体</returns>
        public ProcessInstanceEntity GetProcessInstance(IDbConnection conn, 
            int processInstanceID, 
            IDbTransaction trans)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(conn, processInstanceID, trans);
            return instance;
        }

        /// <summary>
        /// 获取流程实例数据
        /// </summary>
        /// <param name="appInstanceID">业务实例ID</param>
        /// <returns>流程实例实体</returns>
        public IList<ProcessInstanceEntity> GetProcessInstance(string appInstanceID)
        {
            var pim = new ProcessInstanceManager();
            var list = pim.GetProcessInstance(appInstanceID).ToList();
            return list;
        }

        /// <summary>
        /// 获取流程实例数据
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>流程实例实体</returns>
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
        /// 获取运行中的流程实例
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>流程实例实体</returns>
        public ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            var entity = pim.GetRunningProcessInstance(runner.AppInstanceID, runner.ProcessGUID);
            return entity;
        }

        /// <summary>
        /// 判断流程实例是否存在
        /// </summary>
        /// <param name="processGUID">流程定义ID</param>
        /// <param name="version">流程定义版本</param>
        /// <returns>流程实例记录数</returns>
        public Int32 GetProcessInstanceCount(string processGUID, string version)
        {
            var pim = new ProcessInstanceManager();
            return pim.GetProcessInstanceCount(processGUID, version);
        }

        /// <summary>
        /// 获取流程发起人信息
        /// </summary>
        /// <param name="processInstanceID">流程实例Id</param>
        /// <returns>执行者</returns>
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
        /// 获取活动实例数据
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <returns></returns>
        public ActivityInstanceEntity GetActivityInstance(int activityInstanceID)
        {
            var aim = new ActivityInstanceManager();
            var instance = aim.GetById(activityInstanceID);
            return instance;
        }

        /// <summary>
        /// 获取一个流程实例下的所有活动实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
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
        /// 获取当前节点的下一步已经发出的活动实例列表(transition实例表)
        /// 名称更改：GetNextActivityInstanceList --> GetTargetActivityInstanceList
        /// 更改用户：Besley
        /// 更改日期：20190326
        /// </summary>
        /// <param name="fromActivityInstanceID">活动实例ID</param>
        /// <returns></returns>
        public IList<ActivityInstanceEntity> GetTargetActivityInstanceList(int fromActivityInstanceID)
        {
            var tim = new TransitionInstanceManager();
            return tim.GetTargetActivityInstanceList(fromActivityInstanceID);
        }

        /// <summary>
        /// 获取当前等待办理节点的任务分配人列表
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <returns>执行者列表</returns>
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
        /// 判断当前任务是否是最后一个任务
        /// (适应于简单节点或者多实例节点的场景)
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns></returns>
        public Boolean IsLastTask(int taskID)
        {
            var tm = new TaskManager();
            return tm.IsLastTask(taskID);
        }

        /// <summary>
        /// 创建新的委托任务
        /// </summary>
        /// <param name="entrusted">被委托任务信息</param>
        /// <param name="cancalOriginalTask">是否取消原委托任务办理</param>
        /// <returns></returns>
        public Boolean EntrustTask(TaskEntrustedEntity entrusted, bool cancalOriginalTask = true)
        {
            var tm = new TaskManager();
            return tm.Entrust(entrusted, cancalOriginalTask);
        }

        public Boolean ReplaceTask(int taskID, List<TaskReplacedEntity> replaced, WfAppRunner runner)
        {
            var tm = new TaskManager();
            return tm.Replace(taskID, replaced, runner);
        }

        /// <summary>
        /// 设置流程实例的过期时间
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="overdueDateTime">过期时间</param>
        /// <param name="runner">当前运行用户</param>
        /// <returns></returns>
        public Boolean SetProcessOverdue(int processInstanceID, DateTime overdueDateTime, WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.SetOverdue(processInstanceID, overdueDateTime, runner);
        }

        /// <summary>
        /// 设置活动实例的定时作业为完成状态
        /// (用于HangFire后台轮询任务)
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="trans">事务</param>
        public void SetProcessJobTimerCompleted(IDbConnection conn, int processInstanceID, IDbTransaction trans)
        {
            var pim = new ProcessInstanceManager();
            var processInstance = pim.GetById(conn, processInstanceID, trans);
            processInstance.JobTimerStatus = (short)JobTimerStatusEnum.Completed;
            processInstance.JobTimerTreatedDateTime = System.DateTime.Now;
            pim.Update(conn, processInstance, trans);
        }

        /// <summary>
        /// 设置活动实例的定时作业为完成状态
        /// (用于HangFire后台轮询任务)
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="trans">事务</param>
        public void SetActivityJobTimerCompleted(IDbConnection conn, int activityInstanceID, IDbTransaction trans)
        {
            var aim = new ActivityInstanceManager();
            var activityInstance = aim.GetById(conn, activityInstanceID, trans);
            activityInstance.JobTimerStatus = (short)JobTimerStatusEnum.Completed;
            activityInstance.JobTimerTreatedDateTime = System.DateTime.Now;
            aim.Update(conn, activityInstance, trans);
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            var aim = new ActivityInstanceManager();
            var entity = aim.GetRunningNode(runner);

            return entity;
        }

        /// <summary>
        /// 判断是否是我的任务
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool IsMineTask(ActivityInstanceEntity entity, string userID)
        {
            var aim = new ActivityInstanceManager();
            bool isMine = aim.IsMineTask(entity, userID);
            return isMine;
        }

        /// <summary>
        /// 获取正在运行中的活动实例
        /// </summary>
        /// <param name="query">任务查询</param>
        /// <returns>活动实例列表</returns>
        public IList<ActivityInstanceEntity> GetRunningActivityInstance(TaskQuery query)
        {
            var aim = new ActivityInstanceManager();
            var list = aim.GetRunningActivityInstanceList(query.AppInstanceID, query.ProcessGUID, query.Version).ToList();
            return list;
        }

        /// <summary>
        /// 获取流程变量
        /// </summary>
        /// <param name="variableID">变量ID</param>
        /// <returns>变量实体</returns>
        public ProcessVariableEntity GetProcessVariable(int variableID)
        {
            var pvm = new ProcessVariableManager();
            var entity = pvm.GetVariableEntity(variableID);

            return entity;
        }

        /// <summary>
        /// 获取流程变量
        /// </summary>
        /// <param name="query">查询</param>
        /// <returns>变量实体</returns>
        public ProcessVariableEntity GetProcessVariable(ProcessVariableQuery query)
        {
            var pvm = new ProcessVariableManager();
            var entity = pvm.GetVariableEntity(query);
            return entity;
        }

        /// <summary>
        /// 获取变量列表
        /// </summary>
        /// <param name="query">变量查询</param>
        /// <returns>活动实例列表</returns>
        public IList<ProcessVariableEntity> GetProcessVariableList(ProcessVariableQuery query)
        {
            var pvm = new ProcessVariableManager();
            var list = pvm.GetVariableList(query.ProcessInstanceID);
            return list;
        }

        /// <summary>
        /// 删除流程变量
        /// </summary>
        /// <param name="variableID">变量ID</param>
        public void DeleteProcessVariable(int variableID)
        {
            var pvm = new ProcessVariableManager();
            pvm.DeleteVariable(variableID);
        }

        /// <summary>
        /// 验证触发表达式是否满足
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="expression">表达式</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
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
        #endregion

        #region 流程实例数据更新
        /// <summary>
        /// 更新流程实例实体
        /// </summary>
        /// <param name="entity">流程实例实体</param>
        public void UpdateProcessInstance(ProcessInstanceEntity entity)
        {
            var pim = new ProcessInstanceManager();
            pim.Update(entity);
        }

        /// <summary>
        /// 保存流程变量
        /// </summary>
        /// <param name="entity">流程实体</param>
        /// <returns>流程变量ID</returns>
        public int SaveProcessVariable(ProcessVariableEntity entity)
        {
            var pvm = new ProcessVariableManager();
            var entityID = pvm.SaveVariable(entity);
            return entityID;
        }
        #endregion

        #region 角色资源数据获取
        /// <summary>
        /// 获取所有角色数据
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetRoleAll()
        {
            return ResourceService.GetRoleAll();
        }

        /// <summary>
        /// 获取流程定义文件中的角色信息
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本</param>
        /// <returns>角色列表</returns>
        public IList<Role> GetRoleByProcess(string processGUID, string version)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            var roleList = processModel.GetRoles();

            return roleList;
        }

        /// <summary>
        /// 获取流程文件中角色用户的列表数据
        /// </summary>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="version">版本</param>
        /// <returns>角色列表</returns>
        public IList<Role> GetRoleUserListByProcess(string processGUID, string version)
        {
            var processModel = ProcessModelFactory.Create(processGUID, version);
            var roleList = processModel.GetRoles();
            var idsin = roleList.Select(r => r.ID).ToList().ToArray();

            var newRoleList = ResourceService.FillUsersIntoRoles(idsin);

            return newRoleList;
        }

        /// <summary>
        /// 根据角色获取用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>用户列表</returns>
        public IList<User> GetUserListByRole(string roleID)
        {
            return ResourceService.GetUserListByRole(roleID);
        }

        /// <summary>
        /// 获取节点上的执行者列表
        /// </summary>
        /// <param name="nextNode">节点</param>
        /// <returns>执行用户列表</returns>
        public PerformerList GetPerformerList(NodeView nextNode)
        {
            var performerList = PerformerBuilder.CreatePerformerList(nextNode.Roles);
            return performerList;
        }
        #endregion
    }
}
