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
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 任务管理类：包括任务及任务视图对象
    /// </summary>
    public class TaskManager
    {
        #region TaskManager 任务分配视图
        /// <summary>
        /// 根据任务ID获取任务视图
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>任务视图</returns>
        public TaskViewEntity GetTaskView(int taskID)
        {
            using (var session = DbFactory.CreateSession())
            {
                var list = GetTaskViewList(session).Where<TaskViewEntity>(e => e.ID == taskID).ToList();
                if (list != null && list.Count() == 1) return list[0];
                else return null;
            }
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>任务实体</returns>
        public TaskEntity GetTask(int taskID)
        {
            using (var session = DbFactory.CreateSession())
            {
                return session.GetRepository<TaskEntity>().GetByID(taskID);
            }
        }

        /// <summary>
        /// 判断任务是否是当前节点最后一个任务
        /// 单一节点：返回True
        /// 多实例节点：根据实例个数判断
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>是否最后一条任务</returns>
        public Boolean IsLastTask(int taskID)
        {
            var isLast = false;
            var task = GetTask(taskID);
            var aim = new ActivityInstanceManager();
            var activityInstance = aim.GetById(task.ActivityInstanceID);

            if (activityInstance.MIHostActivityInstanceID != null)
            {
                //多实例会签和加签处理
                //取出会签主节点实例数据
                var mainActivityInstance = aim.GetById(activityInstance.MIHostActivityInstanceID.Value);
                var complexType = EnumHelper.ParseEnum<ComplexTypeEnum>(mainActivityInstance.ComplexType.Value.ToString());
                
                if (complexType == ComplexTypeEnum.SignTogether)        //会签
                {
					var mergeType = EnumHelper.ParseEnum<MergeTypeEnum>(mainActivityInstance.MergeType.Value.ToString());
                    if (mergeType == MergeTypeEnum.Sequence)        //串行会签
                    {
                        //取出处于多实例挂起的节点列表
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
                            (short)ActivityStateEnum.Suspended).ToList<ActivityInstanceEntity>();
                        short allNum = (short)mainActivityInstance.AssignedToUserIDs.Split(',').Length;
                        short maxOrder = 0;

                        if (sqList != null && sqList.Count > 0)
                        {
                            //取出最大执行节点
                            maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder.Value);
                        }
                        else if (mainActivityInstance.CompleteOrder <= allNum)
                        {
                            //最后一个执行节点
                            maxOrder = (short)mainActivityInstance.CompleteOrder.Value;
                        }
                        else
                        {
                            maxOrder = allNum;
                        }
                        if (mainActivityInstance.CompareType == null 
                            || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) 
                            == CompareTypeEnum.Count)
                        {
                            //串行会签通过率（按人数判断）
                            if (mainActivityInstance.CompleteOrder != null && mainActivityInstance.CompleteOrder <= maxOrder)
                            {
                                maxOrder = (short)mainActivityInstance.CompleteOrder;
                            }
                            if (activityInstance.CompleteOrder < maxOrder)
                            {
                                isLast = false;
                            }
                            else if (activityInstance.CompleteOrder == maxOrder)
                            {
                                isLast = true;
                            }
                        }
                        else
                        {
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)//串行会签未设置通过率的判断
                                mainActivityInstance.CompleteOrder = 1;
                            if ((activityInstance.CompleteOrder * 0.01) / (allNum * 0.01) >= mainActivityInstance.CompleteOrder)
                            {
                                isLast = true;
                            }
                            else
                            {
                                isLast = false;
                            }
                        }
                    }
                    else if (mergeType == MergeTypeEnum.Parallel)        //并行会签
                    {
                        //取出处于多实例节点列表
                        var sqList = aim.GetActivityMultipleInstance(mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID)
                            .ToList<ActivityInstanceEntity>();
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed 
                            || w.AssignedToUserIDs == task.AssignedToUserID)
                            .ToList<ActivityInstanceEntity>()
                            .Count();
                        if (mainActivityInstance.CompareType == null 
                            || (EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) 
                            == CompareTypeEnum.Percentage))
                        {
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)//并行会签未设置通过率的判断
                                mainActivityInstance.CompleteOrder = 1;

                            if ((completedCount * 0.01) / (allCount * 0.01) >= mainActivityInstance.CompleteOrder)
                            {
                                isLast = true;
                            }
                            else
                            {
                                isLast = false;
                            }
                        }
                        else
                        {
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > allCount)
                            {
                                mainActivityInstance.CompleteOrder = allCount;
                            }
                            if (mainActivityInstance.CompleteOrder > completedCount)
                            {
                                isLast = false;
                            }
                            else if (mainActivityInstance.CompleteOrder == completedCount)
                            {
                                isLast = true;
                            }
                        }
                    }
                }
                else if (complexType == ComplexTypeEnum.SignForward)     //加签
                {
                    //判断加签是否全部完成，如果是，则流转到下一步，否则不能流转
                    var signforwardType = EnumHelper.ParseEnum<SignForwardTypeEnum>(activityInstance.SignForwardType.Value.ToString());

                    if (signforwardType == SignForwardTypeEnum.SignForwardBehind
                        || signforwardType == SignForwardTypeEnum.SignForwardBefore)        //前加签，后加签
                    {
                        //取出处于多实例节点列表
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
                            (short)ActivityStateEnum.Suspended).ToList<ActivityInstanceEntity>();

                        short maxOrder = 0;
                        if (sqList != null && sqList.Count > 0)
                        {
                            //取出最大执行节点
                            maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder.Value);
                        }
                        else
                        {
                            //最后一个执行节点
                            maxOrder = (short)activityInstance.CompleteOrder;// (short)mainActivityInstance.CompleteOrder.Value;
                        }
                        if (mainActivityInstance.CompareType == null
                            || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) 
                            == CompareTypeEnum.Count)
                        {
                            //加签通过率
                            if (mainActivityInstance.CompleteOrder != null && mainActivityInstance.CompleteOrder <= maxOrder)
                            {
                                maxOrder = (short)mainActivityInstance.CompleteOrder;
                            }

                            if (activityInstance.CompleteOrder == sqList.Count)
                            {
                                isLast = true;
                            }
                            else if (activityInstance.CompleteOrder < maxOrder)
                            {
                                isLast = false;
                            }
                            else if (activityInstance.CompleteOrder == maxOrder)
                            {
                                //最后一个节点执行完，主节点进入完成状态，整个流程向下执行
                                isLast = true;
                            }
                        }
                        else
                        {
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)//串行加签未设置通过率的判断
                                mainActivityInstance.CompleteOrder = 1;
                            if ((activityInstance.CompleteOrder * 0.01) / (maxOrder * 0.01) >= mainActivityInstance.CompleteOrder)
                            {
                                isLast = true;
                            }
                            else
                            {
                                isLast = false;
                            }
                        }
                    }
                    else if (signforwardType == SignForwardTypeEnum.SignForwardParallel)        //并行加签
                    {
                        //取出处于多实例节点列表
                        var sqList = aim.GetActivityMultipleInstance(mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID)
                            .ToList<ActivityInstanceEntity>();

                        //并行加签，按照通过率来决定是否标识当前节点完成
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed 
                            || w.AssignedToUserIDs == task.AssignedToUserID)
                            .ToList<ActivityInstanceEntity>()
                            .Count();
                        if (mainActivityInstance.CompareType == null 
                            || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) 
                            == CompareTypeEnum.Percentage)
                        {
                            if (mainActivityInstance.CompleteOrder > 1)//并行加签通过率的判断
                                mainActivityInstance.CompleteOrder = 1;

                            if ((completedCount * 0.01) / (allCount * 0.01) >= mainActivityInstance.CompleteOrder)
                            {
                                isLast = true;
                            }
                            else
                            {
                                isLast = false;
                            }
                        }
                        else
                        {
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > allCount)
                            {
                                mainActivityInstance.CompleteOrder = allCount;
                            }
                            if (mainActivityInstance.CompleteOrder > completedCount)
                            {
                                isLast = false;
                            }
                            else if (mainActivityInstance.CompleteOrder == completedCount)
                            {
                                isLast = true;
                            }
                        }
                    }
                }
            }
            else
            {
                //单一节点类型
                isLast = true;
            }
            return isLast;
        }
        #endregion

        #region TaskManager 获取当前用户的办理任务
        /// <summary>
        /// 获取当前用户运行中的任务
        /// </summary>
        /// <param name="query"></param>
        /// <param name="allRowsCount">任务记录数</param>
        /// <returns>任务视图列表</returns>
        internal IEnumerable<TaskViewEntity> GetRunningTasks(TaskQuery query, out int allRowsCount)
        {
            return GetTasksPaged(query, 2, out allRowsCount);
        }

        /// <summary>
        /// 获取当前用户待办的任务
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="allRowsCount">任务记录数</param>
        /// <returns>任务列表</returns>
        internal IEnumerable<TaskViewEntity> GetReadyTasks(TaskQuery query, out int allRowsCount)
        {
            return GetTasksPaged(query, 1, out allRowsCount);
        } 

        /// <summary>
        /// 获取已经完成任务
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="allRowsCount">任务记录数</param>
        /// <returns>任务列表</returns>
        internal IEnumerable<TaskViewEntity> GetCompletedTasks(TaskQuery query, out int allRowsCount)
        {
            return GetTasksPaged(query, 4, out allRowsCount);
        }

        /// <summary>
        /// 获取任务（分页）
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="activityState">活动状态</param>
        /// <param name="allRowsCount">任务记录数</param>
        /// <returns>活动列表</returns>
        private IEnumerable<TaskViewEntity> GetTasksPaged(TaskQuery query, 
            int activityState, 
            out int allRowsCount)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（运行）；
            using (var session = DbFactory.CreateSession())
            {
                //         string sql = @"SELECT
                //                     TOP 100 * 
                //                  FROM vwWfActivityInstanceTasks 
                //                  WHERE ProcessState=2 
                //                     AND (ActivityType=4 OR WorkItemType=1)
                //                     AND ActivityState=@activityState
                //                     AND AssignedToUserID=@assignedToUserID
                //AND TaskState<>32";
                allRowsCount = 0;
                var sqlQuery = (from vw in GetTaskViewList(session)
                               where vw.ProcessState == 2
                                   && (vw.ActivityType == 4 || vw.WorkItemType == 1)
                                   && vw.ActivityState == activityState
                                   && vw.AssignedToUserID == query.UserID
                                   && vw.TaskState != 32
                               select vw);
                if (activityState == 1) sqlQuery = sqlQuery.Where(e => e.ActivityState != 4);
                if (!string.IsNullOrEmpty(query.AppInstanceID)) sqlQuery = sqlQuery.Where(e => e.AppInstanceID == query.AppInstanceID);
                if (!string.IsNullOrEmpty(query.ProcessGUID)) sqlQuery = sqlQuery.Where(e => e.ProcessGUID == query.ProcessGUID);
                if (!string.IsNullOrEmpty(query.EndedByUserID)) sqlQuery = sqlQuery.Where(e => e.EndedByUserID == query.EndedByUserID);
                if (!string.IsNullOrEmpty(query.AppName)) sqlQuery = sqlQuery.Where(e => e.AppName.Contains(query.AppName));
                var list = sqlQuery.ToList();
                if (list != null) allRowsCount = list.Count();
                return list;
            }
        }

        /// <summary>
        /// 获取我的任务
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns>任务视图实体</returns>
        internal TaskViewEntity GetTaskOfMine(int activityInstanceID, 
            string userID)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            //将ActivityType 修改为 WorkItemType，以处理多类型的任务节点，包括普通任务，多实例，子流程节点
            //string sql = @"SELECT 
            //                    TOP 1 * 
            //                FROM vwWfActivityInstanceTasks 
            //                WHERE ActivityInstanceID=@activityInstanceID 
            //                    AND AssignedToUserID=@userID 
            //                    AND ProcessState=2 
            //                    AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6 OR WorkItemType=1)
            //                    AND (ActivityState=1 OR ActivityState=2) 
            //                ORDER BY TASKID DESC";
            
            using (var session = DbFactory.CreateSession())
            {
                TaskViewEntity entity = null;
                var list = GetTaskViewList(session).Where<TaskViewEntity>(e => e.ActivityInstanceID == activityInstanceID
                    && e.AssignedToUserID == userID
                    && e.ProcessState == 2
                    && (e.ActivityType == 4 || e.ActivityType == 5 || e.ActivityType == 6 || e.WorkItemType == 1)
                    && (e.ActivityState == 1 || e.ActivityState == 2))
                    .OrderByDescending(e => e.ID)
                    .ToList();
                if (list != null && list.Count() > 0)
                {
                    entity = list[0];
                }
                return entity;
            }
        }

        /// <summary>
        /// 获取任务列表视图
        /// </summary>
        /// <returns></returns>
        private IQueryable<TaskViewEntity> GetTaskViewList(IDbSession session)
        {
            var dbSetAI = session.GetRepository<ActivityInstanceEntity>().GetDbSet();
            var dbSetTask = session.GetRepository<TaskEntity>().GetDbSet();
            var dbSetPI = session.GetRepository<ProcessInstanceEntity>().GetDbSet();
            var list = (from ai in dbSetAI
                        join t in dbSetTask on ai.ID equals t.ActivityInstanceID
                        join pi in dbSetPI on ai.ProcessInstanceID equals pi.ID
                        select new TaskViewEntity
                        {
                            ID = t.ID,
                            AppName = t.AppName,
                            AppInstanceID = t.AppInstanceID,
                            ProcessGUID = ai.ProcessGUID,
                            Version = pi.Version,
                            ProcessInstanceID = t.ProcessInstanceID,
                            ActivityGUID = ai.ActivityGUID,
                            ActivityInstanceID = t.ActivityInstanceID,
                            ActivityName = ai.ActivityName,
                            ActivityType = ai.ActivityType,
                            WorkItemType = ai.WorkItemType,
                            PreviousUserID = ai.CreatedByUserID,
                            PreviousUserName = ai.CreatedByUserName,
                            PreviousDateTime = ai.CreatedDateTime,
                            TaskType = t.TaskType,
                            EntrustedTaskID = t.EntrustedTaskID,
                            AssignedToUserID = t.AssignedToUserID,
                            AssignedToUserName = t.AssignedToUserName,
                            CreatedDateTime = t.CreatedDateTime,
                            LastUpdatedDateTime = t.LastUpdatedDateTime,
                            EndedDateTime = t.EndedDateTime,
                            EndedByUserID = t.EndedByUserID,
                            EndedByUserName = t.EndedByUserName,
                            TaskState = t.TaskState,
                            ActivityState = ai.ActivityState,
                            RecordStatusInvalid = t.RecordStatusInvalid,
                            ProcessState = pi.ProcessState,
                            ComplexType = ai.ComplexType,
                            MIHostActivityInstanceID = ai.MIHostActivityInstanceID,
                            AppInstanceCode = pi.AppInstanceCode,
                            ProcessName = pi.ProcessName,
                            PCreatedByUserID = pi.CreatedByUserID,
                            PCreatedByUserName = pi.CreatedByUserName,
                            PCreatdDateTime = pi.CreatedDateTime,
                            MiHostState = ai.MIHostActivityInstanceID == null
                                ? ai.ActivityState
                                : dbSetAI.SingleOrDefault(x => x.ID == ai.MIHostActivityInstanceID.Value).ActivityState
                        }).AsQueryable<TaskViewEntity>();
            return list;
        }


        /// <summary>
        /// 根据应用实例、流程GUID，办理用户Id获取任务列表
        /// </summary>
        /// <param name="appInstanceID">App实例ID</param>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="userID">用户Id</param>
        /// <returns>任务实体</returns>
        internal TaskViewEntity GetTaskOfMine(string appInstanceID, 
            string processGUID, 
            string userID)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            //将ActivityType 修改为 WorkItemType，以处理多类型的任务节点，包括普通任务，多实例，子流程节点
            //string sql = @"SELECT 
            //                    TOP 1 * 
            //               FROM vwWfActivityInstanceTasks 
            //               WHERE AppInstanceID=@appInstanceID 
            //                    AND ProcessGUID=@processGUID 
            //                    AND AssignedToUserID=@userID 
            //                    AND ProcessState=2 
            //                    AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6 OR WorkItemType=1)
            //                    AND (ActivityState=1 OR ActivityState=2) 
            //               ORDER BY TASKID DESC";
            using (var session = DbFactory.CreateSession())
            {
                var taskList = GetTaskViewList(session).Where<TaskViewEntity>(e => e.AppInstanceID == appInstanceID
                    && e.ProcessGUID == processGUID
                    && e.AssignedToUserID == userID
                    && e.ProcessState == 2
                    && (e.ActivityType == 4 || e.ActivityType == 5 || e.ActivityType == 6 || e.WorkItemType == 1)
                    && (e.ActivityState == 1 || e.ActivityState == 2))
                    .OrderByDescending(e => e.ID)
                    .ToList();

                if (taskList == null || taskList.Count == 0)
                {
                    throw new WorkflowException(
                        string.Format("当前没有你要办理的任务，业务单据标识ID: {0}", appInstanceID.ToString())
                    );
                }
                else if (taskList.Count > 1)
                {
                    throw new WorkflowException(string.Format("当前办理任务的数目: {0} 大于1，无法确定下一步节点！", taskList.Count));
                }
                var task = taskList[0];

                return task;
            }
        }

         /// <summary>
        /// 判断活动实例是否是某个用户的任务
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="userID">用户Id</param>
        /// <returns>是否标志</returns>
        internal bool IsMine(int activityInstanceID, string userID)
        {
            using (var session = DbFactory.CreateSession())
            {
                //string sql = @"SELECT 
                //                    * 
                //               FROM WfTasks 
                //               WHERE ActivityInstanceID=@activityInstanceID
                //                    AND AssignedToUserID=@assignedToUserID";
                var list = session.GetRepository<TaskEntity>().Query(e => e.ActivityInstanceID == activityInstanceID
                    && e.AssignedToUserID == userID)
                    .ToList();
                bool isMine = false;
                if (list != null && list.Count == 1)
                {
                    isMine = true;
                }
                return isMine;
            }
        }

        /// <summary>
        /// 获取待办任务(业务实例)
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <returns>任务列表</returns>
        internal IEnumerable<TaskViewEntity> GetReadyTaskOfApp(WfAppRunner runner)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）
            //string sql = @"SELECT 
            //                    * 
            //               FROM vwWfActivityInstanceTasks 
            //               WHERE AppInstanceID=@appInstanceID 
            //                    AND ProcessGUID=@processGUID 
            //                    AND ProcessState=2 
            //                    AND (ActivityType=4 OR WorkItemType=1)
            //                    AND ActivityState=1";
            using (var session = DbFactory.CreateSession())
            {
                var list = GetTaskViewList(session).Where<TaskViewEntity>(e => e.AppInstanceID == runner.AppInstanceID
                    && e.ProcessGUID == runner.ProcessGUID
                    && (e.ActivityType == 4 || e.WorkItemType == 1)
                    && e.ActivityState == 1)
                    .ToList();
                return list;
                 
            }
        }
        #endregion

        #region TaskManager 任务数据基本操作
        /// <summary>
        /// 插入任务数据
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <param name="session">数据上下文</param>
        private Int32 Insert(TaskEntity entity, 
            IDbSession session)
        {
            session.GetRepository<TaskEntity>().Insert(entity);
            session.SaveChanges();

            return entity.ID;
        }

        /// <summary>
        /// 插入任务数据
        /// </summary>
        /// <param name="activityInstance">活动实体</param>
        /// <param name="performers">执行者列表</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Insert(ActivityInstanceEntity activityInstance,
            PerformerList performers, 
            WfAppRunner runner,
            IDbSession session)
        {
            foreach (Performer performer in performers)
            {
                Insert(activityInstance, performer, runner, session);
            }
        }

        /// <summary>
        /// 插入任务数据
        /// </summary>
        /// <param name="activityInstance">活动实例</param>
        /// <param name="performer">执行者</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal Int32 Insert(ActivityInstanceEntity activityInstance,
            Performer performer,
            WfAppRunner runner,
            IDbSession session)
        {
            return Insert(activityInstance, performer.UserID, performer.UserName, 
                runner.UserID, runner.UserName, session);
        }

        /// <summary>
        /// 插入任务数据(创建任务)
        /// </summary>
        /// <param name="activityInstance">活动实例</param>
        /// <param name="performerID">执行者Id</param>
        /// <param name="performerName">执行者名称</param>
        /// <param name="runnerID">运行者ID</param>
        /// <param name="runnerName">运行者名称</param>
        /// <param name="session">数据上下文</param>
        /// <param name="entrustedTaskID">被委托（原始）任务ID</param>
        private Int32 Insert(ActivityInstanceEntity activityInstance,
            string performerID,
            string performerName,
            string runnerID,
            string runnerName,
            IDbSession session,
            int? entrustedTaskID = null)
        {

            TaskEntity entity = new TaskEntity();
            entity.AppName = activityInstance.AppName;
            entity.AppInstanceID = activityInstance.AppInstanceID;
            entity.ActivityInstanceID = activityInstance.ID;
            entity.ProcessInstanceID = activityInstance.ProcessInstanceID;
            entity.ActivityGUID = activityInstance.ActivityGUID;
            entity.ActivityName = activityInstance.ActivityName;
            entity.ProcessGUID = activityInstance.ProcessGUID;
            entity.TaskType = (short)TaskTypeEnum.Manual;
            entity.AssignedToUserID = performerID;
            entity.AssignedToUserName = performerName;
            entity.TaskState = 1; //1-待办状态
            entity.CreatedByUserID = runnerID;
            entity.CreatedByUserName = runnerName;
            entity.CreatedDateTime = System.DateTime.Now;
            entity.RecordStatusInvalid = 0;
            if(entrustedTaskID != null)
                entity.EntrustedTaskID = entrustedTaskID.Value;      //记录被委托(原始)任务ID
            //插入任务数据
            int taskID = Insert(entity, session);
            return taskID;
        }

        /// <summary>
        /// 重新生成任务(只限于会签多实例下的子节点)
        /// </summary>
        /// <param name="sourceActivityInstance">原活动实例</param>
        /// <param name="newInstance">新活动实例</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">数据上下文</param>
        internal void Renew(ActivityInstanceEntity sourceActivityInstance,
            ActivityInstanceEntity newInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            var performer = new Performer(sourceActivityInstance.AssignedToUserIDs, 
                sourceActivityInstance.AssignedToUserNames);

            Insert(newInstance, performer, runner, session);
        }

        /// <summary>
        /// 更新任务数据
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <param name="session">数据上下文</param>
        internal void Update(TaskEntity entity, IDbSession session)
        {
            session.GetRepository<TaskEntity>().Update(entity);
            session.SaveChanges();
        }


        /// <summary>
        /// 读取任务，设置任务为已读状态
        /// </summary>
        /// <param name="taskRunner">运行者实体</param>
        internal void SetTaskRead(WfAppRunner taskRunner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var transaction = session.DbContext.Database.BeginTransaction();
                try
                {
                    var task = GetTask(taskRunner.TaskID.Value);
                    SetTaskState(task, taskRunner.UserID, taskRunner.UserName, TaskStateEnum.Handling, session);

                    //置活动为运行状态
                    (new ActivityInstanceManager()).SetActivityRead(task.ActivityInstanceID, taskRunner, session);
                    session.SaveChanges();

                    transaction.Commit();
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                    throw new WorkflowException(string.Format("阅读待办任务时出错！，详细错误：{0}", ex.Message), ex);
                }
            }
        }

        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="task">任务实体</param>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <param name="taskState">任务状态</param>
        /// <param name="session">会话</param>
        private void SetTaskState(TaskEntity task,
            string userID,
            string userName,
            TaskStateEnum taskState,
            IDbSession session)
        {
            task.TaskState = (short)taskState;
            task.LastUpdatedByUserID = userID;
            task.LastUpdatedByUserName = userName;
            task.LastUpdatedDateTime = System.DateTime.Now;
            Update(task, session);
        }

        /// <summary>
        /// 设置任务完成
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Complete(int taskID,
            WfAppRunner runner,
            IDbSession session)
        {
            TaskEntity task = session.GetRepository<TaskEntity>().GetByID(taskID);
            task.TaskState = (byte)TaskStateEnum.Completed;
            task.EndedDateTime = DateTime.Now;
            task.EndedByUserID = runner.UserID;
            task.EndedByUserName = runner.UserName;

            Update(task, session);
        }

        /// <summary>
        /// 创建新的委托任务
        /// </summary>
        /// <param name="entity">任务委托实体</param>
        /// <param name="cancalOriginalTask">是否取消原始任务</param>
        internal Boolean Entrust(TaskEntrustedEntity entity, bool cancalOriginalTask = true)
        {
            var isOk = false;
            using (var session = DbFactory.CreateSession())
            {
                var transaction = session.DbContext.Database.BeginTransaction();
                try
                {
                    var am = new ActivityInstanceManager();
                    var activityInstance = am.GetByTask(entity.TaskID, session);

                    if (activityInstance.ActivityState != (short)ActivityStateEnum.Ready
                        && activityInstance.ActivityState != (short)ActivityStateEnum.Running)
                    {
                        throw new WorkflowException("没有可以委托的任务，因为活动实例的状态不在运行状态！");
                    }

                    //更新AssignedToUsers 信息
                    activityInstance.AssignedToUserIDs = activityInstance.AssignedToUserIDs + "," + entity.EntrustToUserID;
                    activityInstance.AssignedToUserNames = activityInstance.AssignedToUserNames + "," + entity.EntrustToUserName;
                    activityInstance.ActivityState = (int)ActivityStateEnum.Ready;
                    am.Update(activityInstance, session);

                    //更新原委托任务的状态为关闭
                    if (cancalOriginalTask == true)
                    {
                        var task = GetTask(entity.TaskID);
                        task.TaskState = (short)TaskStateEnum.Closed;
                        Update(task, session);
                    }

                    //插入委托任务
                    Insert(activityInstance, entity.EntrustToUserID, entity.EntrustToUserName,
                        entity.RunnerID, entity.RunnerName, session, entity.TaskID);

                    transaction.Commit();
                    isOk = true;
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                    throw new WorkflowException(string.Format("委托任务时出错！，详细错误：{0}", ex.Message), ex);
                }
            }
            return isOk;
        }

        /// <summary>
        /// 任务删除
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="session">会话</param>
        internal void Delete(int taskID, IDbSession session)
        {
            session.GetRepository<TaskEntity>().Delete(taskID);
            session.SaveChanges();
        }
        #endregion
    }
}
