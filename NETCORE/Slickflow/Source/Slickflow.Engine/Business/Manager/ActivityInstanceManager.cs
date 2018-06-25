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
using System.Text;
using System.Data;
using System.Diagnostics;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 活动实例管理类
    /// </summary>
    internal class ActivityInstanceManager
    {
        #region ActivityInstanceManager get data
        /// <summary>
        /// 根据ID获取活动实例
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetById(int activityInstanceID)
        {
            try
            {
                using (var session = DbFactory.CreateSession())
                {
                    var entity = session.GetRepository<ActivityInstanceEntity>()
                        .GetByID(activityInstanceID);
                    return entity;
                }
            }
            catch (System.Exception e)
            {
                throw new ApplicationException(string.Format("数据读取方法GetById发生错误，请查看内部异常: {0}",
                    e.Message), e);
            }
        }

        /// <summary>
        /// 根据ID获取活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetById(int activityInstanceID, IDbSession session)
        {
            var entity = session.GetRepository<ActivityInstanceEntity>()
                .GetByID(activityInstanceID);
            return entity;
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            var appInstanceID = runner.AppInstanceID;
            var processGUID = runner.ProcessGUID;
            var taskID = runner.TaskID;

            //如果流程在运行状态，则返回运行时信息
            TaskViewEntity task = null;
            var aim = new ActivityInstanceManager();
            var entity = GetRunningNode(runner, out task);

            return entity;
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="taskView">任务视图</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetRunningNode(WfAppRunner runner, 
            out TaskViewEntity taskView)
        {
            var appInstanceID = runner.AppInstanceID;
            var processGUID = runner.ProcessGUID;
            var taskID = runner.TaskID;

            taskView = null;
            ActivityInstanceEntity activityInstance = null;

            //如果流程在运行状态，则返回运行时信息
            var tm = new TaskManager();

            var aim = new ActivityInstanceManager();
            var activityInstanceList = aim.GetRunningActivityInstanceList(runner.AppInstanceID, runner.ProcessGUID).ToList();

            if ((activityInstanceList != null) && (activityInstanceList.Count == 1))
            {
                activityInstance = activityInstanceList[0];
                taskView = tm.GetTaskOfMine(activityInstance.ID, runner.UserID);
            }
            else if (activityInstanceList.Count > 0)
            {
                if (runner.TaskID != null && runner.TaskID.Value != 0)
                {
                    taskView = tm.GetTaskView(taskID.Value);

                    foreach (var ai in activityInstanceList)
                    {
                        if (ai.ID == taskView.ActivityInstanceID)
                        {
                            activityInstance = ai;
                            break;
                        }
                    }

                    //判断是否有并行节点存在，如果有并行节点存在，取并行节点的主节点（并行会签）
                    if (activityInstance == null && activityInstanceList[0].MIHostActivityInstanceID != null)
                    {
                        var mainActivityInstanceID = activityInstanceList[0].MIHostActivityInstanceID;
                        activityInstance = aim.GetById(mainActivityInstanceID.Value);
                    }
                }
                else
                {
                    //并行模式处理
                    //根据当前执行者身份取出(他或她)要办理的活动实例（并行模式下有多个处于待办或运行状态的节点）
                    foreach (var ai in activityInstanceList)
                    {
                        if (ai.AssignedToUserIDs == runner.UserID)
                        {
                            activityInstance = ai;
                            break;
                        }
                    }

                    if (activityInstance != null)
                    {
                        //获取taskview
                        taskView = tm.GetTaskOfMine(activityInstance.ID, runner.UserID);
                    }
                    else
                    {
                        //当前用户的待办任务不唯一，抛出异常，需要TaskID唯一界定
                        var e = new WorkflowException("当前流程有多个运行节点，但没有TaskID传入，状态异常！");
                        LogManager.RecordLog("获取当前运行节点信息异常", LogEventType.Exception, LogPriority.Normal, null, e);
                        throw e;
                    }
                }
            }
            else
            {
                //当前没有运行状态的节点存在，流程不存在，或者已经结束或取消
                var e = new WorkflowException("当前流程没有运行节点，状态异常！");
                LogManager.RecordLog("获取当前运行节点信息异常", LogEventType.Exception, LogPriority.Normal, null, e);
                throw e;
            }
            return activityInstance;
        }

        /// <summary>
        /// 判断任务所属
        /// </summary>
        /// <param name="entity">活动实例</param>
        /// <param name="userID">用户ID</param>
        /// <returns>标识</returns>
        internal bool IsMineTask(ActivityInstanceEntity entity,
            string userID)
        {
            bool isMine = entity.AssignedToUserIDs.Contains(userID.ToString());
            return isMine;
        }

        /// <summary>
        /// 获取活动实例列表
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>活动实例列表</returns>
        internal IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID)
        {
            //var sql = @"SELECT * FROM WfActivityInstance 
            //            WHERE ProcessInstanceID = @processInstanceID 
            //                ORDER BY ID";
            using (var session = DbFactory.CreateSession())
            {
                var instanceList = session.GetRepository<ActivityInstanceEntity>()
                    .Query(e => e.ProcessInstanceID == processInstanceID)
                    .OrderBy(e => e.ID)
                    .ToList();

                return instanceList;
            }
        }

        /// <summary>
        /// 活动实例列表
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动实例列表</returns>
        internal IList<ActivityInstanceEntity> GetActivityInstance(string appInstanceID,
            string processGUID,
            string activityGUID)
        {
            //var sql = @"SELECT * FROM WfActivityInstance 
            //            WHERE AppInstanceID = @appInstanceID 
            //                AND ProcessGUID = @processGUID
            //                AND ActivityGUID = @activityGUID
            //                ORDER BY ID DESC";
            using (var session = DbFactory.CreateSession())
            {
                var instanceList = session.GetRepository<ActivityInstanceEntity>()
                    .Query(e => e.AppInstanceID == appInstanceID
                        && e.ProcessGUID == processGUID
                        && e.ActivityGUID == activityGUID)
                    .OrderByDescending(e => e.ID)
                    .ToList();

                return instanceList;
            }
        }

        /// <summary>
        /// 获取运行状态的活动实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetActivityRunning(int processInstanceID,
            string activityGUID,
            IDbSession session)
        {
            return GetActivityByState(processInstanceID, 
                activityGUID, 
                ActivityStateEnum.Running, 
                session);
        }

        /// <summary>
        /// 根据状态获取活动实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="activityState">活动状态</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetActivityByState(int processInstanceID,
            string activityGUID,
            ActivityStateEnum activityState,
            IDbSession session)
        {
            //activityState: 4-completed（完成）
            //var sql = @"SELECT * FROM WfActivityInstance 
            //            WHERE ProcessInstanceID = @processInstanceID 
            //                AND ActivityGUID = @activityGUID 
            //                AND ActivityState = @state
            //            ORDER BY ID DESC";
            ActivityInstanceEntity entity = null;
            var repository = session.GetRepository<ActivityInstanceEntity>();
            var instanceList = repository.Query(
                e => e.ProcessInstanceID == processInstanceID
                    && e.ActivityGUID == activityGUID
                    && e.ActivityState == (short)activityState)
                .OrderByDescending(e => e.ID)
                .ToList();
            if (instanceList != null && instanceList.Count == 1) entity = instanceList[0];

            return entity;
        }

        /// <summary>
        /// 获取完成状态的活动实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity GetActivityCompleted(int processInstanceID,
            string activityGUID)
        {
            using (var session = DbFactory.CreateSession())
            {
                return GetActivityByState(processInstanceID, activityGUID, ActivityStateEnum.Completed, session);
            }
        }

        /// <summary>
        /// 由任务ID获取活动实例信息
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity GetByTask(int taskID,
            IDbSession session)
        {
            //var sql = @"SELECT 
            //                AI.* 
            //            FROM WfActivityInstance AI
            //            INNER JOIN WfTasks T ON AI.ID = T.ActivityInstanceID
            //            WHERE T.ID = @taskID";
            var repositoryAI = session.GetRepository<ActivityInstanceEntity>();
            var repositoryTask = session.GetRepository<TaskEntity>();
            var list = (from ai in repositoryAI.GetDbSet()
                        join t in repositoryTask.GetDbSet() on ai.ID equals t.ActivityInstanceID
                        where t.ID == taskID
                        select ai)
            .ToList();
            if (list != null && list.Count() == 1)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// 获取流程实例中运行的活动节点
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>活动实例列表</returns>
        internal IEnumerable<ActivityInstanceEntity> GetRunningActivityInstanceList(string appInstanceID, 
            string processGUID)
        {
            //var sql = @"SELECT 
            //                    AI.* 
            //                FROM WfActivityInstance AI
            //                INNER JOIN WfProcessInstance PI 
            //                    ON AI.ProcessInstanceID = PI.ID
            //                WHERE (AI.ActivityState=1 OR AI.ActivityState=2)
            //                    AND PI.ProcessState = 2 
            //                    AND AI.AppInstanceID = @appInstanceID 
            //                    AND AI.ProcessGUID = @processGUID";
            using (var session = DbFactory.CreateSession())
            {
                var repositoryAI = session.GetRepository<ActivityInstanceEntity>();
                var repositoryPI = session.GetRepository<ProcessInstanceEntity>();
                var list = (from ai in repositoryAI.GetDbSet()
                            join pi in repositoryPI.GetDbSet() on ai.ProcessInstanceID equals pi.ID
                            where pi.ProcessState == 2
                                && ai.AppInstanceID == appInstanceID
                                && ai.ProcessGUID == processGUID
                                && (ai.ActivityState == 1 || ai.ActivityState == 2)
                            select ai)
                            .ToList();

                return list;
            }
        }

        /// <summary>
        /// 获取已经运行完成的节点
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns></returns>
        internal List<ActivityInstanceEntity> GetCompletedActivityInstanceList(string appInstanceID,
            string processGUID)
        {
            //activityState: 4-completed（完成）
            //var sql = @"SELECT 
            //                    AI.* 
            //                FROM WfActivityInstance AI
            //                INNER JOIN WfProcessInstance PI 
            //                    ON AI.ProcessInstanceID = PI.ID
            //                WHERE PI.ProcessState = 2 
            //                    AND AI.AppInstanceID = @appInstanceID 
            //                    AND AI.ProcessGUID = @processGUID
            //                    AND AI.ActivityState = 4";
            using (var session = DbFactory.CreateSession())
            {
                var repositoryAI = session.GetRepository<ActivityInstanceEntity>();
                var repositoryPI = session.GetRepository<ProcessInstanceEntity>();
                var list = (from ai in repositoryAI.GetDbSet()
                            join pi in repositoryPI.GetDbSet() on ai.ProcessInstanceID equals pi.ID
                            where pi.ProcessState == 2
                                && ai.AppInstanceID == appInstanceID
                                && ai.ProcessGUID == processGUID
                                && ai.ActivityState == 4
                            select ai)
                            .ToList();

                return list;
            }
        }

        /// <summary>
        /// 获取主节点下已经完成的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID">主活动实例ID</param>
        /// <returns></returns>
        internal List<ActivityInstanceEntity> GetCompletedMultipleInstanceList(int mainActivityInstanceID)
        {
            //activityState: 4-completed（完成）
            //var sql = @"SELECT 
            //                    AI.* 
            //                FROM WfActivityInstance AI
            //                INNER JOIN WfProcessInstance PI 
            //                    ON AI.ProcessInstanceID = PI.ID
            //                WHERE PI.ProcessState = 2 
            //                    AND MIHostActivityInstanceID = @mainActivityInstanceID
            //                    AND AI.ActivityState = 4";
            using (var session = DbFactory.CreateSession())
            {
                var repositoryAI = session.GetRepository<ActivityInstanceEntity>();
                var repositoryPI = session.GetRepository<ProcessInstanceEntity>();
                var list = (from ai in repositoryAI.GetDbSet()
                     join pi in repositoryPI.GetDbSet() on ai.ProcessInstanceID equals pi.ID
                     where pi.ProcessState == 2
                         && ai.MIHostActivityInstanceID == mainActivityInstanceID
                         && ai.ActivityState == 4
                     select ai)
                    .ToList();

                return list;
            }
        }

        /// <summary>
        /// 判断用户是否是分配下来任务的用户
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userID">用户ID</param>
        /// <returns>标识</returns>
        private bool IsAssignedUserInActivityInstance(ActivityInstanceEntity entity,
            int userID)
        {
            var assignedToUsers = entity.AssignedToUserIDs;
            var userList = assignedToUsers.Split(',');
            var single = userList.FirstOrDefault<string>(a => a == userID.ToString());
            if (!string.IsNullOrEmpty(single))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取会签节点的多实例节点
        /// </summary>
        /// <param name="mainActivityInstanceID">会签节点</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityState">活动状态</param>
        /// <returns>活动实例列表</returns>
        internal List<ActivityInstanceEntity> GetActivityMulitipleInstanceWithState(int mainActivityInstanceID,
            int processInstanceID,
            short activityState)
        {
            using (var session = DbFactory.CreateSession())
            {
                var childActivityInstance = GetActivityMulitipleInstanceWithState(mainActivityInstanceID,
                    processInstanceID,
                    activityState,
                    session);

                return childActivityInstance;
            }
        }

        /// <summary>
        /// 获取会签节点的多实例节点
        /// </summary>
        /// <param name="mainActivityInstanceID">会签节点</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityState">活动状态</param>
        /// <param name="session">数据上下文</param>
        /// <returns>活动实例列表</returns>
        internal List<ActivityInstanceEntity> GetActivityMulitipleInstanceWithState(int mainActivityInstanceID,
            int processInstanceID,
            short activityState,
            IDbSession session)
        {
            //activityState: 1-ready（准备）, 2-running（）运行；
            //var sql = @"SELECT 
            //                    * 
            //                FROM WfActivityInstance 
            //                WHERE MIHostActivityInstanceID = @activityInstanceID 
            //                    AND processInstanceID = @processInstanceID
            //                    AND ActivityState = @activityState 
            //                ORDER BY CompleteOrder";
            var instanceList = session.GetRepository<ActivityInstanceEntity>().Query(e => e.MIHostActivityInstanceID == mainActivityInstanceID
                && e.ProcessInstanceID == processInstanceID
                && e.ActivityState == activityState)
                .OrderBy(e => e.CompleteOrder)
                .ToList();
            return instanceList;
        }

        /// <summary>
        /// 获取会签节点的多实例节点
        /// </summary>
        /// <param name="mainActivityInstanceID">会签节点</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>活动实例列表</returns>
        internal List<ActivityInstanceEntity> GetActivityMultipleInstance(int mainActivityInstanceID,
            int processInstanceID)
        {
            //var sql = @"SELECT 
            //                    * 
            //                FROM WfActivityInstance 
            //                WHERE MIHostActivityInstanceID = @activityInstanceID 
            //                    AND processInstanceID = @processInstanceID
            //                ORDER BY CompleteOrder";
            using (var session = DbFactory.CreateSession())
            {
                return GetActivityMultipleInstance(mainActivityInstanceID, processInstanceID, session);
            }
        }

        /// <summary>
        /// 获取会签节点的多实例节点
        /// </summary>
        /// <param name="mainActivityInstanceID">会签节点</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="session">数据上下文</param>
        /// <returns>活动实例列表</returns>
        internal List<ActivityInstanceEntity> GetActivityMultipleInstance(int mainActivityInstanceID,
            int processInstanceID,
            IDbSession session)
        {
            //var sql = @"SELECT 
            //                    * 
            //                FROM WfActivityInstance 
            //                WHERE MIHostActivityInstanceID = @activityInstanceID 
            //                    AND processInstanceID = @processInstanceID
            //                ORDER BY CompleteOrder";

            var instanceList = session.GetRepository<ActivityInstanceEntity>()
                .Query(e => e.MIHostActivityInstanceID == mainActivityInstanceID
                    && e.ProcessInstanceID == processInstanceID)
                .OrderBy(e => e.CompleteOrder)
                .ToList();
            return instanceList;
        }

        /// <summary>
        /// 获取多实例主节点下的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点活动实例ID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityState">活动状态</param>
        /// <returns>活动实例列表</returns>
        internal List<ActivityInstanceEntity> GetActivityMultipleInstance(int mainActivityInstanceID, 
            int processInstanceID,
            short activityState)
        {
            using (var session = DbFactory.CreateSession())
            {
                return GetActivityMulitipleInstanceWithState(mainActivityInstanceID,
                    processInstanceID,
                    activityState,
                    session);
            }
        }

        /// <summary>
        /// 查询实例节点的前置节点
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="completeOrder"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetPreviousOfMultipleInstanceNode(int mainActivityInstanceID,
            int activityInstanceID,
            double completeOrder)
        {
            //var whereSql = @"SELECT * FROM WfActivityInstance
            //                WHERE MIHostActivityInstanceID = @mainActivityInstanceID
            //                    AND CompleteOrder = @completeOrder-1
            //                    AND ActivityState=@activityState
            //                ORDER BY ID DESC
            //                ";
            ActivityInstanceEntity entity = null;
            using (var session =DbFactory.CreateSession())
            {
                var instanceList = session.GetRepository<ActivityInstanceEntity>().Query(e => e.MIHostActivityInstanceID == mainActivityInstanceID
                        && e.CompleteOrder == completeOrder - 1
                        && e.ActivityState == (short)ActivityStateEnum.Completed)
                        .OrderByDescending(e => e.ID)
                        .ToList();

                if (instanceList != null && instanceList.Count() > 0)
                {
                    entity = instanceList[0];
                }
                return entity;
            }
        }
		/// <summary>
        /// 查询分支实例的个数
        /// </summary>
        /// <param name="splitActivityGUID">节点GUID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>网关实例数目</returns>
        internal int GetInstanceGatewayCount(string splitActivityGUID,
            int processInstanceID)
        {
            //var whereSql = @"SELECT 
            //                    * 
            //                FROM wftransitioninstance
            //                WHERE processinstanceid=@processinstanceId 
            //                    AND fromactivityguid=@fromActivityGUID";
            using (var session = DbFactory.CreateSession())
            {
                var instanceList = session.GetRepository<TransitionInstanceEntity>().Query(e => e.ProcessInstanceID == processInstanceID
                    && e.FromActivityGUID == splitActivityGUID);
                return instanceList.Count();
            }
        }
        #endregion

        #region 创建活动实例
        /// <summary>
        /// 创建活动实例的对象
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activity">活动</param>
        /// <param name="runner">运行者</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity CreateActivityInstanceObject(string appName,
            string appInstanceID,
            int processInstanceID,
            ActivityEntity activity,
            WfAppRunner runner)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityGUID = activity.ActivityGUID;
            instance.ActivityName = activity.ActivityName;
            instance.ActivityType = (short)activity.ActivityType;
            instance.WorkItemType = (short)activity.WorkItemType;
            instance.GatewayDirectionTypeID = (short)activity.GatewayDirectionType;
            instance.ProcessGUID = activity.ProcessGUID;
            instance.AppName = appName;
            instance.AppInstanceID = appInstanceID;
            instance.ProcessInstanceID = processInstanceID;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedByUserID = runner.UserID;
            instance.CreatedByUserName = runner.UserName;
            instance.CreatedDateTime = System.DateTime.Now;
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// 根据主节点复制子节点
        /// </summary>
        /// <param name="main">活动实例</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity CreateActivityInstanceObject(ActivityInstanceEntity main)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityGUID = main.ActivityGUID;
            instance.ActivityName = main.ActivityName;
            instance.ActivityType = main.ActivityType;
            instance.WorkItemType = main.WorkItemType;
            instance.GatewayDirectionTypeID = main.GatewayDirectionTypeID;
            instance.ProcessGUID = main.ProcessGUID;
            instance.AppName = main.AppName;
            instance.AppInstanceID = main.AppInstanceID;
            instance.ProcessInstanceID = main.ProcessInstanceID;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedByUserID = main.CreatedByUserID;
            instance.CreatedByUserName = main.CreatedByUserName;
            instance.CreatedDateTime = System.DateTime.Now;
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// 创建活动实例的对象
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activity">活动</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="backSrcActivityInstanceID">退回源活动实例ID</param>
        /// <param name="runner">执行者</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity CreateBackwardActivityInstanceObject(string appName,
            string appInstanceID,
            int processInstanceID,
            ActivityEntity activity,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            WfAppRunner runner)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityGUID = activity.ActivityGUID;
            instance.ActivityName = activity.ActivityName;
            instance.ActivityType = (short)activity.ActivityType;
            instance.WorkItemType = (short)activity.WorkItemType;
            instance.GatewayDirectionTypeID = (short)activity.GatewayDirectionType;
            instance.ProcessGUID = activity.ProcessGUID;
            instance.AppName = appName;
            instance.AppInstanceID = appInstanceID;
            instance.ProcessInstanceID = processInstanceID;
            instance.BackwardType = (short)backwardType;
            instance.BackSrcActivityInstanceID = backSrcActivityInstanceID;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedByUserID = runner.UserID;
            instance.CreatedByUserName = runner.UserName;
            instance.CreatedDateTime = System.DateTime.Now;
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// 更新活动节点的Token数目
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void IncreaseTokensHad(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            ActivityInstanceEntity activityInstance = GetById(activityInstanceID, session);
            activityInstance.TokensHad += 1;

            Update(activityInstance, session);
        }
        #endregion

        #region 活动实例中间状态设置
        /// <summary>
        /// 活动实例被读取
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void SetActivityRead(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Running, runner, session);
        }

        /// <summary>
        /// 撤销活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Withdraw(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Withdrawed, runner, session);
        }

        /// <summary>
        /// 退回活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void SendBack(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Sendbacked, runner, session);
        }

        /// <summary>
        /// 设置活动实例状态
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="nodeState">节点状态</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">数据上下文</param>
        private void SetActivityState(int activityInstanceID,
            ActivityStateEnum nodeState,
            WfAppRunner runner,
            IDbSession session)
        {
            var entity = GetById(activityInstanceID);
            entity.ActivityState = (short)nodeState;
            entity.LastUpdatedByUserID = runner.UserID;
            entity.LastUpdatedByUserName = runner.UserName;
            entity.LastUpdatedDateTime = System.DateTime.Now;
            session.GetRepository<ActivityInstanceEntity>().Update(entity);

            session.SaveChanges();
        }

        /// <summary>
        /// 活动实例完成
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Complete(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            var activityInstance = GetById(activityInstanceID, session);
            activityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            activityInstance.LastUpdatedByUserID = runner.UserID;
            activityInstance.LastUpdatedByUserName = runner.UserName;
            activityInstance.LastUpdatedDateTime = System.DateTime.Now;
            activityInstance.EndedByUserID = runner.UserID;
            activityInstance.EndedByUserName = runner.UserName;
            activityInstance.EndedDateTime = System.DateTime.Now;

            Update(activityInstance, session);
        }
        #endregion

        #region 活动实例记录维护
        /// <summary>
        /// 插入活动实例
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="session">会话</param>
        /// <returns>新的自增长ID键值</returns>
        internal int Insert(ActivityInstanceEntity entity,
            IDbSession session)
        {
            //SET ActivityName When It Is NULL
            if (entity.ActivityType == (short)ActivityTypeEnum.GatewayNode
                && string.IsNullOrEmpty(entity.ActivityName))
            {
                entity.ActivityName = "GATEWAY";
            }
            var repository = session.GetRepository<ActivityInstanceEntity>();
            var newEntity = repository.Insert(entity);
            session.SaveChanges();

            int newID = newEntity.ID;
            return newID;
        }

        /// <summary>
        /// 更新活动实例
        /// </summary>
        /// <param name="entity">活动实例实体</param>
        /// <param name="session">会话</param>
        internal void Update(ActivityInstanceEntity entity,
            IDbSession session)
        {
            session.GetRepository<ActivityInstanceEntity>().Update(entity);
            session.SaveChanges();
        }

        /// <summary>
        /// 更新会签节点中未办理完成的节点状态
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="runner">执行者</param>
        /// <param name="session">会话</param>
        internal void CancelUnCompletedMultipleInstance(int mainActivityInstanceID, 
            WfAppRunner runner,
            IDbSession session)
        {
            //var sql = @"UPDATE 
            //                WfActivityInstance 
            //            SET ActivityState=@activityState,
            //                LastUpdatedByUserID=@lastUpdatedByUserID,
            //                LastUpdatedByUserName=@lastUpdatedByUserName,
            //                LastUpdatedDateTime=@lastUpdatedDateTime 
            //            WHERE MIHostActivityInstanceID=@mainActivityInstanceID 
            //                AND ActivityState in (1,2,5) ";
            var ids = new int[] { 1, 2, 5 };
            var list = session.GetRepository<ActivityInstanceEntity>()
                .Query(e => e.MIHostActivityInstanceID == mainActivityInstanceID
                && ids.Contains(e.ActivityState))
                .ToList();

            list.ForEach(a => {
                a.ActivityState = (short)ActivityStateEnum.Cancelled;
                a.LastUpdatedByUserID = runner.UserID;
                a.LastUpdatedByUserName = runner.UserName;
                a.LastUpdatedDateTime = System.DateTime.Now;
            });
            session.SaveChanges();
        }

        /// <summary>
        /// 撤销主节点及其下面的多实例子节点
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="runner">执行者</param>
        /// <param name="session">会话</param>
        internal void WithdrawMainIncludedChildNodes(int mainActivityInstanceID, 
            WfAppRunner runner, 
            IDbSession session)
        {
            //先更新主节点状态为撤销状态
            SetActivityState(mainActivityInstanceID, ActivityStateEnum.Withdrawed, runner, session);

            //再更新主节点下的多实例子节点状态为撤销状态
            WithdrawMultipleInstance(mainActivityInstanceID, runner, session);
        }

        /// <summary>
        /// 更新会签办理节点的节点状态
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="runner">执行者</param>
        /// <param name="session">数据上下文</param>
        private void WithdrawMultipleInstance(int mainActivityInstanceID, 
            WfAppRunner runner, 
            IDbSession session)
        {
            //var sql = @"UPDATE 
            //                WfActivityInstance 
            //            SET ActivityState=@activityState, 
            //                LastUpdatedByUserID=@lastUpdatedByUserID,
            //                LastUpdatedByUserName=@lastUpdatedByUserName,
            //                LastUpdatedDateTime=@lastUpdatedDateTime
            //            WHERE MIHostActivityInstanceID=@mainActivityInstanceID
            //                AND (ActivityState=1 OR ActivityState=5)";      //准备或挂起状态的节点可以撤销
            var list = session.GetRepository<ActivityInstanceEntity>().Query(e => e.MIHostActivityInstanceID == mainActivityInstanceID)
                .ToList();

            list.ForEach(a =>
            {
                a.ActivityState = (short)ActivityStateEnum.Withdrawed;
                a.LastUpdatedByUserID = runner.UserID;
                a.LastUpdatedByUserName = runner.UserName;
                a.LastUpdatedDateTime = System.DateTime.Now;
            });
            session.SaveChanges();
        }

        /// <summary>
        /// 重新使节点处于挂起状态
        /// 说明：会签最后一个子节点撤销时候用到
        /// </summary>
        /// <param name="activityInstanceID">活动实例节点ID</param>
        /// <param name="runner">执行者</param>
        /// <param name="session">会话</param>
        internal void Resuspend(int activityInstanceID, 
            WfAppRunner runner, 
            IDbSession session)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Suspended, runner, session);
        }

        /// <summary>
        /// 删除活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="session">数据上下文</param>
        internal void Delete(int activityInstanceID,
            IDbSession session)
        {
            session.GetRepository<ActivityInstanceEntity>().Delete(activityInstanceID);
            session.SaveChanges();
        }
        #endregion
    }
}
