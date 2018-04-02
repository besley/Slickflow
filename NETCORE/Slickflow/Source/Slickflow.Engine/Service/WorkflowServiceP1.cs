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
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// 工作流服务（数据查询）
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region 流程实例信息获取
        /// <summary>
        /// 获取流程实例数据
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        public ProcessInstanceEntity GetProcessInstance(int processInstanceID)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetById(processInstanceID);
            return instance;
        }

        public ProcessInstanceEntity GetProcessInstanceByActivity(int activityInstanceID)
        {
            var pim = new ProcessInstanceManager();
            var instance = pim.GetByActivity(activityInstanceID);
            return instance;
        }

        /// <summary>
        /// 获取运行中的流程实例
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public ProcessInstanceEntity GetRunningProcessInstance(WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var pim = new ProcessInstanceManager();
                return pim.GetRunningProcessInstance(runner.AppInstanceID, runner.ProcessGUID, session);
            }
        }

        /// <summary>
        /// 判断流程实例是否存在
        /// </summary>
        /// <param name="prcessGUID">流程定义ID</param>
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
        /// 获取活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>活动实例</returns>
        public ActivityInstanceEntity GetActivityInstance(int activityInstanceID)
        {
            var aim = new ActivityInstanceManager();
            var instance = aim.GetById(activityInstanceID);
            return instance;
        }

        /// <summary>
        /// 获取活动实例列表
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>活动实例列表</returns>
        public IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID)
        {
            var aim = new ActivityInstanceManager();
            return aim.GetActivityInstances(processInstanceID);
        }

        /// <summary>
        /// 获取当前节点的下一步已经发出的活动实例列表(transition实例表)
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>活动实例列表</returns>
        public IList<ActivityInstanceEntity> GetNextActivityInstanceList(int fromActivityInstanceID)
        {
            var tim = new TransitionInstanceManager();
            return tim.GetNextActivityInstanceList(fromActivityInstanceID);
        }

        /// <summary>
        /// 获取当前等待办理节点的任务分配人列表
        /// </summary>
        /// <param name="runner">运行者</param>
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
        /// <returns>标识</returns>
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
        public Boolean EntrustTask(TaskEntrustedEntity entrusted, bool cancalOriginalTask = true)
        {
            var tm = new TaskManager();
            return tm.Entrust(entrusted, cancalOriginalTask);
        }

        /// <summary>
        /// 设置流程实例的过期时间
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="overdueDateTime">过期时间</param>
        /// <param name="runner">当前运行用户</param>
        /// <returns>标识</returns>
        public Boolean SetProcessOverdue(int processInstanceID, DateTime overdueDateTime, WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.SetOverdue(processInstanceID, overdueDateTime, runner);
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>活动实例</returns>
        public ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            var aim = new ActivityInstanceManager();
            var entity = aim.GetRunningNode(runner);

            return entity;
        }

        /// <summary>
        /// 判断是否是我的任务
        /// </summary>
        /// <param name="entity">活动实例</param>
        /// <param name="userID">用户ID</param>
        /// <returns>标识</returns>
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
            var list = aim.GetRunningActivityInstanceList(query.AppInstanceID, query.ProcessGUID).ToList();
            return list;
        }
        #endregion

        #region 角色资源数据获取
        /// <summary>
        /// 获取所有角色数据
        /// </summary>
        /// <returns>角色列表</returns>
        public IList<Role> GetRoleAll()
        {
            return ResourceService.GetRoleAll();
        }

        /// <summary>
        /// 获取流程定义文件中的角色信息
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
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
        /// <param name="processGUId">流程GUID</param>
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
        /// <returns>执行者列表</returns>
        public PerformerList GetPerformerList(NodeView nextNode)
        {
            var performerList = PerformerBuilder.CreatePerformerList(nextNode.Roles);
            return performerList;
        }
        #endregion
    }
}
