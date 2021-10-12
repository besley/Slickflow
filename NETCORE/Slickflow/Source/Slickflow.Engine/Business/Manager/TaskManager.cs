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
using System.Text;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 任务管理类：包括任务及任务视图对象
    /// </summary>
    public class TaskManager : ManagerBase
    {
        #region TaskManager 任务分配视图
        /// <summary>
        /// 根据任务ID获取任务视图
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>任务视图</returns>
        public TaskViewEntity GetTaskView(int taskID)
        {
            return Repository.GetById<TaskViewEntity>(taskID);
        }

        /// <summary>
        /// 根据任务ID获取任务视图
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="taskID">任务ID</param>
        /// <param name="trans">事务</param>
        /// <returns>任务视图</returns>
        public TaskViewEntity GetTaskView(IDbConnection conn, int taskID, IDbTransaction trans)
        {
            return Repository.GetById<TaskViewEntity>(conn, taskID, trans);
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>任务实体</returns>
        public TaskEntity GetTask(int taskID)
        {
            return Repository.GetById<TaskEntity>(taskID);
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="taskID">任务ID</param>
        /// <param name="trans">事务</param>
        /// <returns>任务实体</returns>
        public TaskEntity GetTask(IDbConnection conn, int taskID, IDbTransaction trans)
        {
            return Repository.GetById<TaskEntity>(conn, taskID, trans);
        }

        /// <summary>
        /// 根据流程信息获取任务
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="trans">事务</param>
        /// <returns>任务实体</returns>
        internal TaskEntity GetTaskByActivity(IDbConnection conn,
            int processInstanceID,
            int activityInstanceID,
            IDbTransaction trans)
        {
            TaskEntity task = null;
            //string sql = @"SELECT
            //                * 
            //             FROM WfTasks 
            //             WHERE ActivityInstanceID=@activityInstanceID
            //                AND ProcessInstanceID=@processInstanceID
            //            ";
            //var list = Repository.Query<TaskEntity>(conn,
            //    sql,
            //    new
            //    {
            //        processInstanceID = processInstanceID,
            //        activityInstanceID = activityInstanceID

            //    },
            //    trans).ToList();
            var sqlQuery = (from t in Repository.GetAll<TaskEntity>(conn, trans)
                            where t.ActivityInstanceID == activityInstanceID
                                && t.ProcessInstanceID == processInstanceID
                            select t
                            );
            var list = sqlQuery.ToList<TaskEntity>();

            if (list.Count() > 0)
            {
                task = list[0];
            }
            return task;
        }

        /// <summary>
        /// 根据流程信息获取任务
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>任务实体</returns>
        internal TaskViewEntity GetTaskViewByActivity(int processInstanceID,
            int activityInstanceID)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var taskView = GetTaskViewByActivity(session.Connection, processInstanceID, activityInstanceID, session.Transaction);
                return taskView;
            }
        }

        /// <summary>
        /// 根据流程信息获取任务
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="trans">事务</param>
        /// <returns>任务实体</returns>
        internal TaskViewEntity GetTaskViewByActivity(IDbConnection conn,
            int processInstanceID,
            int activityInstanceID,
            IDbTransaction trans)
        {
            TaskViewEntity taskView = null;
            //string sql = @"SELECT
            //                * 
            //             FROM vwWfActivityInstanceTasks 
            //             WHERE ActivityInstanceID=@activityInstanceID
            //                AND ProcessInstanceID=@processInstanceID
            //            ";

            //var list = Repository.Query<TaskViewEntity>(sql,
            //new
            //{
            //    processInstanceID = processInstanceID,
            //    activityInstanceID = activityInstanceID

            //}).ToList();
            var sqlQuery = (from tv in Repository.GetAll<TaskViewEntity>(conn, trans)
                            where tv.ActivityInstanceID == activityInstanceID
                                && tv.ProcessInstanceID == processInstanceID
                            select tv
                            );
            var list = sqlQuery.ToList<TaskViewEntity>();
            if (list.Count() > 0)
            {
                taskView = list[0];
            }
            return taskView;
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

            if (aim.IsMultipleInstanceChildren(activityInstance) == true)
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
                        if (mainActivityInstance.CompareType == null || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Count)
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
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
                            null).ToList<ActivityInstanceEntity>();
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed || w.AssignedToUserIDs == task.AssignedToUserID)
                            .ToList<ActivityInstanceEntity>()
                            .Count();
                        if (mainActivityInstance.CompareType == null || (EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Percentage))
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
                        if (mainActivityInstance.CompareType == null || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Count)
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
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
                            null).ToList<ActivityInstanceEntity>();

                        //并行加签，按照通过率来决定是否标识当前节点完成
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed || w.AssignedToUserIDs == task.AssignedToUserID)
                            .ToList<ActivityInstanceEntity>()
                            .Count();
                        if (mainActivityInstance.CompareType == null || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Percentage)
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
        /// 获取正在运行的任务
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>任务视图</returns>
        internal TaskViewEntity GetFirstRunningTask(int activityInstanceID)
        {
            string sql = @"SELECT
                            * 
                         FROM vwWfActivityInstanceTasks 
                         WHERE ProcessState=2 
                            AND ActivityInstanceID=@activityInstanceID 
                            AND (ActivityType=4 OR WorkItemType=1)
                            AND (ActivityState=1 OR ActivityState=2)
                            AND (TaskState=1 OR TaskState=2)
                            ORDER BY TaskState DESC
                        ";
            var list = Repository.Query<TaskViewEntity>(sql,
                new
                {
                    activityInstanceID = activityInstanceID
                }).ToList();
            //var sqlQuery = (from tv in Repository.GetAll<TaskViewEntity>()
            //                where tv.ProcessState == 2
            //                    && tv.ActivityInstanceID == activityInstanceID
            //                    && (tv.ActivityType == 4 || tv.WorkItemType == 1)
            //                    && (tv.ActivityState == 1 || tv.ActivityState == 2)
            //                    && (tv.TaskState == 1 || tv.TaskState == 2)
            //                select tv
            //                );
            //var list = sqlQuery.OrderByDescending(tv => tv.TaskState).ToList<TaskViewEntity>();

            if (list.Count() > 0)
            {
                return list[0];
            }
            return null;
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
        private IEnumerable<TaskViewEntity> GetTasksPaged(TaskQuery query, int activityState, out int allRowsCount)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（运行）；

            //         string sql = @"SELECT
            //                     TOP 100 * 
            //                  FROM vwWfActivityInstanceTasks 
            //                  WHERE ProcessState=2 
            //                     AND (ActivityType=4 OR WorkItemType=1)
            //                     AND ActivityState=@activityState
            //                     AND TaskState<>32";
            allRowsCount = 0;
            var sqlQuery = (from vw in Repository.GetAll<TaskViewEntity>()
                            where vw.ProcessState == 2
                                && (vw.ActivityType == 4 || vw.WorkItemType == 1)
                                && vw.ActivityState == activityState
                                && vw.TaskState != 32
                            select vw);
            if (activityState == 1) sqlQuery = sqlQuery.Where(e => e.ActivityState != 4);
            if (!string.IsNullOrEmpty(query.AppInstanceID)) sqlQuery = sqlQuery.Where(e => e.AppInstanceID == query.AppInstanceID);
            if (!string.IsNullOrEmpty(query.ProcessGUID)) sqlQuery = sqlQuery.Where(e => e.ProcessGUID == query.ProcessGUID);
            if (!string.IsNullOrEmpty(query.UserID)) sqlQuery = sqlQuery.Where(e => e.AssignedToUserID == query.UserID);
            if (!string.IsNullOrEmpty(query.EndedByUserID)) sqlQuery = sqlQuery.Where(e => e.EndedByUserID == query.EndedByUserID);
            if (!string.IsNullOrEmpty(query.AppName)) sqlQuery = sqlQuery.Where(e => e.AppName.Contains(query.AppName));
            var list = sqlQuery.OrderByDescending(vw=>vw.TaskID).Take(100).ToList();
            if (list != null) allRowsCount = list.Count();
            return list;
        }

        /// <summary>
        /// Get Top 10 task todo list
        /// </summary>
        /// <returns>task list</returns>
        public List<TaskViewEntity> GetTaskToDoListTop()
        {
            var sqlQuery = (from vw in Repository.GetAll<TaskViewEntity>()
                            where vw.ProcessState == 2
                                && (vw.ActivityType == 4 || vw.WorkItemType == 1)
                                && vw.ActivityState == 1        //ready
                                && vw.TaskState != 32
                            select vw);
            var list = sqlQuery.Take(10).OrderByDescending(t=>t.TaskID).ToList();
            return list;
        }

        /// <summary>
        /// Get Top 10 task done list
        /// </summary>
        /// <returns>task list</returns>
        public List<TaskViewEntity> GetTaskDoneListTop()
        {
            var sqlQuery = (from vw in Repository.GetAll<TaskViewEntity>()
                            where vw.ProcessState == 2
                                && (vw.ActivityType == 4 || vw.WorkItemType == 1)
                                && vw.ActivityState == 4        //completed
                                && vw.TaskState != 32
                            select vw);
            var list = sqlQuery.Take(10).OrderByDescending(t=>t.TaskID).ToList();
            return list;
        }

        /// <summary>
        /// 获取我的任务
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="trans">数据库事务</param>
        /// <returns>任务视图实体</returns>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn,
            int activityInstanceID, 
            string userID,
            IDbTransaction trans)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            //            string whereSql = @"SELECT 
            //                                    TOP 1 *
            //                                FROM vwWfActivityInstanceTasks 
            //                                WHERE ActivityInstanceID=@activityInstanceID 
            //                                    AND AssignedToUserID=@userID 
            //                                    AND ProcessState=2 
            //                                    AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6) 
            //                                    AND (ActivityState=1 OR ActivityState=2) 
            //                                ORDER BY TASKID DESC";
            TaskViewEntity entity = null;
            var list = Repository.GetAll<TaskViewEntity>(conn, trans).Where<TaskViewEntity>(e => e.ActivityInstanceID == activityInstanceID
                && e.AssignedToUserID == userID
                && e.ProcessState == 2
                && (e.ActivityType == 3 || e.ActivityType == 4 || e.ActivityType == 5 || e.ActivityType == 6 || e.WorkItemType == 1)
                && (e.ActivityState == 1 || e.ActivityState == 2))
                .OrderByDescending(e => e.TaskID)
                .ToList();

            if (list.Count() > 0)
            {
                entity = list[0];
            }
            return entity;
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
            using (var session = SessionFactory.CreateSession())
            {
                return GetTaskOfMine(session.Connection, appInstanceID, processGUID, userID, session.Transaction);
            }
        }

        /// <summary>
        /// 根据应用实例、流程GUID，办理用户Id获取任务列表
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="appInstanceID">App实例ID</param>
        /// <param name="processGUID">流程定义GUID</param>
        /// <param name="userID">用户Id</param>
        /// <param name="trans">事务</param>
        /// <returns>任务实体</returns>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn,
            string appInstanceID,
            string processGUID,
            string userID,
            IDbTransaction trans)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            //2015.09.10 besley
            //将ActivityType 修改为 WorkItemType，以处理多类型的任务节点，包括普通任务，多实例，子流程节点
            //string sql = @"SELECT 
            //                TOP 1 * 
            //           FROM vwWfActivityInstanceTasks 
            //           WHERE AppInstanceID=@appInstanceID 
            //                AND ProcessGUID=@processGUID 
            //                AND AssignedToUserID=@userID 
            //                AND ProcessState=2 
            //                AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6 OR WorkItemType=1)
            //                AND (ActivityState=1 OR ActivityState=2) 
            //           ORDER BY TASKID DESC";
            var taskList = Repository.GetAll<TaskViewEntity>(conn, trans).Where<TaskViewEntity>(e => e.AppInstanceID == appInstanceID
                && e.ProcessGUID == processGUID
                && e.AssignedToUserID == userID
                && e.ProcessState == 2
                && (e.ActivityType == 4 || e.ActivityType == 5 || e.ActivityType == 6 || e.WorkItemType == 1)
                && (e.ActivityState == 1 || e.ActivityState == 2))
                .OrderByDescending(e => e.TaskID)
                .ToList();

            if (taskList.Count == 0)
            {
                var message = LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.error");
                throw new WorkflowException(
                    string.Format("{0}，AppInstanceID: {1}", message, appInstanceID.ToString())
                );
            }
            else if (taskList.Count > 1)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.toomoretasks.error"));
            }
            else
            {
                var taskView = taskList[0];
                return taskView;
            }
        }

        /// <summary>
        /// 获取任务视图
        /// </summary>
        /// <param name="runner">当前运行者</param>
        /// <returns>任务视图</returns>
        internal TaskViewEntity GetTaskOfMine(WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetTaskOfMine(session.Connection, runner, session.Transaction);
            }
        }

        /// <summary>
        /// 获取任务视图
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="runner">当前运行者</param>
        /// <param name="trans">事务</param>
        /// <returns>任务视图</returns>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            TaskViewEntity taskView = null;
            if (runner.TaskID != null)
            {
                taskView = GetTaskView(conn, runner.TaskID.Value, trans);
            }
            else
            {
                taskView = GetTaskOfMine(conn, runner.AppInstanceID, runner.ProcessGUID, runner.UserID, trans);
            }
            return taskView;
        }

        /// <summary>
        /// 判断任务是否属于某个用户
        /// </summary>
        /// <param name="entity">任务</param>
        /// <param name="userID">用户Id</param>
        /// <returns>是否标志</returns>
        internal bool IsMine(TaskViewEntity entity, string userID)
        {
            var isMine = false;
            if (entity.AssignedToUserID == userID) isMine = true;
            return isMine;
        }

        /// <summary>
        /// 判断任务处于运行状态
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>是否可运行</returns>
        internal Boolean CheckTaskStateInRunningState(TaskViewEntity task)
        {
            var isRunning = true;
            if (task.TaskState == (short)TaskStateEnum.Completed)
            {
                isRunning = false;
            }
            else if (task.ActivityState == (short)ActivityStateEnum.Completed){
                isRunning = false;
            }
            return isRunning;
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
            //                    AND ActivityState=1
            //               ORDER BY TaskID DESC";
            //var list = Repository.Query<TaskViewEntity>(sql,
            //    new
            //    {
            //        appInstanceID = runner.AppInstanceID,
            //        processGUID = runner.ProcessGUID
            //    });
            var sqlQuery = (from tv in Repository.GetAll<TaskViewEntity>()
                            where tv.AppInstanceID == runner.AppInstanceID
                                && tv.ProcessGUID == runner.ProcessGUID
                                && (tv.ActivityType == 4 || tv.WorkItemType == 1)
                                && tv.ActivityState == 1
                            orderby tv.TaskID descending
                            select tv
                            );
            var list = sqlQuery.ToList<TaskViewEntity>();
            return list;
        }

        /// <summary>
        /// 获取未发送邮件通知的待办任务列表
        /// </summary>
        /// <returns></returns>
        internal IList<TaskViewEntity> GetTaskListEMailUnSent()
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（运行）
            //isEMailSent: 0-邮件未发送, 1-发送成功, -1-发送失败
       //     string sql = @"SELECT * 
       //                  FROM vwWfActivityInstanceTasks 
       //                  WHERE ProcessState=2 
       //                     AND (ActivityType=4 OR WorkItemType=1)
       //                     AND ActivityState=1
       //                     AND IsEMailSent=0
							//AND TaskState<>32
       //                 ";
       //     var list = Repository.Query<TaskViewEntity>(sql).ToList<TaskViewEntity>();
            var sqlQuery = (from tv in Repository.GetAll<TaskViewEntity>()
                            where tv.ProcessState == 2
                                && (tv.ActivityType == 4 || tv.WorkItemType == 1)
                                && tv.ActivityState == 1
                                && tv.IsEMailSent == 0
                                && tv.TaskState != 32
                            select tv
                            );
            var list = sqlQuery.ToList<TaskViewEntity>();
            return list;
        }
        #endregion

        #region TaskManager 任务数据基本操作
        /// <summary>
        /// 插入任务数据
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <param name="session">会话</param>
        private Int32 Insert(TaskEntity entity, 
            IDbSession session)
        {
            int newTaskID = Repository.Insert(session.Connection, entity, session.Transaction);
            return newTaskID;
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
        /// <param name="session">会话</param>
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
            entity.IsEMailSent = 0; //0-默认邮件未发送
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
        /// <param name="session">会话</param>
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
        /// <param name="session">会话</param>
        internal void Update(TaskEntity entity, IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// 设置任务类型
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="taskType">任务类型</param>
        /// <param name="session">会话</param>
        internal void SetTaskType(int taskID, TaskTypeEnum taskType, IDbSession session)
        {
            var task = GetTask(session.Connection, taskID, session.Transaction);
            task.TaskType = (short)taskType;
            Repository.Update<TaskEntity>(session.Connection, task, session.Transaction);
        }

        /// <summary>
        /// 读取任务，设置任务为已读状态
        /// </summary>
        /// <param name="taskRunner">运行者实体</param>
        internal void SetTaskRead(WfAppRunner taskRunner)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //置任务为处理状态
                var task = GetTask(taskRunner.TaskID.Value);
                SetTaskState(task, taskRunner.UserID, taskRunner.UserName, TaskStateEnum.Reading, session);

                //置活动为运行状态
                (new ActivityInstanceManager()).Read(task.ActivityInstanceID, taskRunner, session);

                session.Commit();
            }
            catch (System.Exception e)
            {
                session.Rollback();
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.settaskread.error", e.Message), 
                    e);
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 更新任务邮件发送状态
        /// </summary>
        /// <param name="taskID">任务ID</param>
        internal void SetTaskEMailSent(int taskID)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                var task = GetTask(taskID);
                session.BeginTrans();
                task.IsEMailSent = (byte)TaskEMailSentStatusEnum.Sent;
                Update(task, session);
                session.Commit();
            }
            catch (System.Exception e)
            {
                session.Rollback();
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.settaskemailsent.error", e.Message),
                    e);
            }
            finally
            {
                session.Dispose();
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
        /// <param name="taskState">任务状态</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void EndTaskState(long taskID,
            TaskStateEnum taskState,
            WfAppRunner runner,
            IDbSession session)
        {
            TaskEntity task = Repository.GetById<TaskEntity>(session.Connection, taskID, session.Transaction);
            task.TaskState = (byte)taskState;
            task.EndedDateTime = DateTime.Now;
            task.EndedByUserID = runner.UserID;
            task.EndedByUserName = runner.UserName;

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
            EndTaskState(taskID, TaskStateEnum.Completed, runner, session);
        }

        /// <summary>
        /// 设置任务撤销
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Withdraw(long taskID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskID, TaskStateEnum.Withdrawed, runner, session);
        }

        /// <summary>
        /// 设置任务退回
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void SendBack(long taskID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskID, TaskStateEnum.SendBacked, runner, session);
        }

        /// <summary>
        /// 创建新的委托任务
        /// </summary>
        /// <param name="entity">任务委托实体</param>
        /// <param name="cancalOriginalTask">是否取消原始任务</param>
        internal bool Entrust(TaskEntrustedEntity entity, bool cancalOriginalTask = true)
        {
            var isOk = false;
            var session = SessionFactory.CreateSession();
            try
            {
                //获取活动实例信息
                session.BeginTrans();

                var am = new ActivityInstanceManager();
                var activityInstance = am.GetByTask(entity.TaskID, session);

                if (activityInstance.ActivityState != (short)ActivityStateEnum.Ready
                    && activityInstance.ActivityState != (short)ActivityStateEnum.Running)
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.entrust.warn"));
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

                session.Commit();

                isOk = true;
            }
            catch(System.Exception e)
            {
                session.Rollback();
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.entrust.error"),
                    e);
            }
            finally
            {
                session.Dispose();
            }
            return isOk;
        }

        /// <summary>
        /// 取代当前活动下的任务办理人员
        /// </summary>
        /// <param name="activityInstanceID">活动ID</param>
        /// <param name="replaced">替代用户</param>
        /// <param name="runner">运行用户</param>
        /// <returns></returns>
        internal Boolean Replace(int activityInstanceID, List<TaskReplacedEntity> replaced, WfAppRunner runner)
        {
            var isOk = false;
            var session = SessionFactory.CreateSession();
            try
            {
                CancelTask(activityInstanceID, runner, session);

                var aim = new ActivityInstanceManager();
                var activityInstance = aim.GetById(session.Connection, activityInstanceID, session.Transaction);

                activityInstance.AssignedToUserIDs = GenerateActivityAssignedUserIDs(replaced);
                activityInstance.AssignedToUserNames = GenerateActivityAssignedUserNames(replaced);

                foreach (var user in replaced)
                {
                    Insert(activityInstance, user.ReplacedByUserID, user.ReplacedByUserName, runner.UserID, runner.UserName, session);
                }

                session.Commit();

                isOk = true;
            }
            catch (System.Exception e)
            {
                session.Rollback();
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.replace.error"),
                    e);
            }
            finally
            {
                session.Dispose();
            }
            return isOk;
        }

        /// <summary>
        /// 获取UserIds字符串
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        private string GenerateActivityAssignedUserIDs(List<TaskReplacedEntity> userList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var user in userList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(user.ReplacedByUserID);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 获取UserNames字符串
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        private string GenerateActivityAssignedUserNames(List<TaskReplacedEntity> userList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var user in userList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(user.ReplacedByUserName);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 取消任务办理
        /// </summary>
        /// <param name="activityInstanceID">活动实例id</param>
        /// <param name="runner">运行着</param>
        /// <param name="session">会话</param>
        private void CancelTask(int activityInstanceID, WfAppRunner runner, IDbSession session)
        {
            var sqlQuery = (from t in Repository.GetAll<TaskEntity>(session.Connection, session.Transaction)
                            where t.ActivityInstanceID == activityInstanceID
                            select t);
            var list = sqlQuery.ToList();
            foreach (var task in list)
            {
                task.TaskState = (short)TaskStateEnum.Canceled;
                task.RecordStatusInvalid = 1;
                Update(task, session);
            }
        }

        /// <summary>
        /// 任务删除
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="taskID">任务ID</param>
        /// <param name="trans">事务</param>
        internal bool Delete(IDbConnection conn, long taskID, IDbTransaction trans)
        {
            return Repository.Delete<TaskEntity>(conn, taskID, trans);
        }
        #endregion
    }
}
