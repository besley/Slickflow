﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Data.DataProvider;

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
            string sql = @"SELECT
                            * 
                         FROM WfTasks 
                         WHERE ActivityInstanceID=@activityInstanceID
                            AND ProcessInstanceID=@processInstanceID
                        ";
            var list = Repository.Query<TaskEntity>(conn,
                sql,
                new
                {
                    processInstanceID = processInstanceID,
                    activityInstanceID = activityInstanceID
                },
                trans).ToList();

            #region ORMapping support for multiple databases
            //var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pg.Predicates.Add(Predicates.Field<TaskEntity>(f => f.ActivityInstanceID, Operator.Eq, activityInstanceID));
            //pg.Predicates.Add(Predicates.Field<TaskEntity>(f => f.ProcessInstanceID, Operator.Eq, processInstanceID));
            //var list = Repository.GetList<TaskEntity>(conn, pg, null, trans).ToList();
            #endregion

            TaskEntity task = null;
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
            string sql = @"SELECT
                            * 
                         FROM vwWfActivityInstanceTasks 
                         WHERE ActivityInstanceID=@activityInstanceID
                            AND ProcessInstanceID=@processInstanceID
                        ";

            var list = Repository.Query<TaskViewEntity>(sql,
            new
            {
                processInstanceID = processInstanceID,
                activityInstanceID = activityInstanceID

            }).ToList();

            #region ORMapping support for mutiple databases
            //var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pg.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityInstanceID, Operator.Eq, activityInstanceID));
            //pg.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessInstanceID, Operator.Eq, processInstanceID));
            //var list = Repository.GetList<TaskViewEntity>(conn, pg, null, trans).ToList();
            #endregion

            TaskViewEntity taskView = null;
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
        internal IEnumerable<TaskViewEntity> GetRunningTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 2);
        }

        /// <summary>
        /// 获取当前用户待办的任务
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <returns>任务列表</returns>
        internal IEnumerable<TaskViewEntity> GetReadyTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 1);
        } 

        /// <summary>
        /// 获取正在运行的任务
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>任务视图</returns>
        internal TaskViewEntity GetFirstRunningTask(int activityInstanceID)
        {
            //sql query
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

            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));
            //var pgActivityState = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 1));
            //pgActivityState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 2));
            //var pgTaskState = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgTaskState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.TaskState, Operator.Eq, 1));
            //pgTaskState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.TaskState, Operator.Eq, 2));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityInstanceID, Operator.Eq, activityInstanceID));
            //pgAll.Predicates.Add(pgActivityType);
            //pgAll.Predicates.Add(pgActivityState);
            //pgAll.Predicates.Add(pgTaskState);

            //var sortList = new List<DapperExtensions.ISort>();
            //sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskState", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).OrderByDescending(tv => tv.TaskState).ToList();
            #endregion

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
        /// <returns>任务列表</returns>
        internal IEnumerable<TaskViewEntity> GetCompletedTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 4);
        }

        /// <summary>
        /// 获取任务（分页）
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="activityState">活动状态</param>
        /// <returns>活动列表</returns>
        private IEnumerable<TaskViewEntity> GetTasksPaged(TaskQuery query, int activityState)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（运行）；

            string sql = @"SELECT
                                * 
                            FROM vwWfActivityInstanceTasks 
                            WHERE ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=@activityState
                                AND TaskState<>32";
            sql = SqlDataProviderFactory.GetSqlTaskPaged(sql);

            var parameters = new DynamicParameters();
            parameters.Add("@activityState", activityState);

            var sqlBuilder = new StringSQLBuilder(sql);
            if (activityState == 1)
            {
                sqlBuilder.And("MiHostState", Data.Operator.Nq, 4);
            }

            if (!string.IsNullOrEmpty(query.AppInstanceID))
            {
                sqlBuilder.And("AppInstanceID", Data.Operator.Eq, "@appInstanceID");
                parameters.Add("@appInstanceID", query.AppInstanceID);
            }

            if (!string.IsNullOrEmpty(query.ProcessGUID))
            {
                sqlBuilder.And("ProcessGUID", Data.Operator.Eq, "@processGUID");
                parameters.Add("@processGUID", query.ProcessGUID);
            }

            if (!string.IsNullOrEmpty(query.Version))
            {
                sqlBuilder.And("Version", Data.Operator.Eq, "@version");
                parameters.Add("@version", query.Version);
            }

            if (!string.IsNullOrEmpty(query.UserID))
            {
                sqlBuilder.And("AssignedToUserID", Data.Operator.Eq, "@assignedToUserID");
                parameters.Add("@assignedToUserID", query.UserID);
            }

            if (!string.IsNullOrEmpty(query.EndedByUserID))
            {
                sqlBuilder.And("EndedByUserID", Data.Operator.Eq, "@endedByUserID");
                parameters.Add("@endedByUserID", query.EndedByUserID);
            }

            if (!string.IsNullOrEmpty(query.AppName))
            {
                sqlBuilder.And("AppName", Data.Operator.Like, "@appName");
                parameters.Add("@appName", query.AppName);
            }
            sqlBuilder.OrderBy("TaskID", true);

            var pageSize = query.PageSize;
            if (pageSize == 0) pageSize = 100;          //缺省分页每页的记录数
            var sqlWhere = sqlBuilder.GetSQL();
            var list = Repository.Query<TaskViewEntity>(sqlWhere, parameters).Take<TaskViewEntity>(pageSize).ToList();
            
            return list;
        }

        /// <summary>
        /// Get Top 10 task todo list
        /// 仅供用于数据展示使用
        /// </summary>
        /// <returns>task list</returns>
        public List<TaskViewEntity> GetTaskToDoListTop()
        {
            string sql = @"SELECT
                                TOP 10 * 
                            FROM vwWfActivityInstanceTasks 
                            WHERE ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=1
                                AND TaskState<>32
                            ORDER BY TaskID DESC";
            var list = Repository.Query<TaskViewEntity>(sql).ToList();

            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 1));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.TaskState, Operator.Eq, 32, true));
            //pgAll.Predicates.Add(pgActivityType);

            //var sortList = new List<DapperExtensions.ISort>();
            //sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskID", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).Take<TaskViewEntity>(10).OrderByDescending(tv => tv.TaskID).ToList();
            #endregion
            return list;
        }

        /// <summary>
        /// Get Top 10 task done list
        /// 仅供用于数据展示使用
        /// </summary>
        /// <returns>task list</returns>
        public List<TaskViewEntity> GetTaskDoneListTop()
        {
            string sql = @"SELECT
                                TOP 10 * 
                            FROM vwWfActivityInstanceTasks 
                            WHERE ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=4
                                AND TaskState<>32
                            ORDER BY TaskID DESC";
            var list = Repository.Query<TaskViewEntity>(sql).ToList();

            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 4));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.TaskState, Operator.Eq, 32, true));
            //pgAll.Predicates.Add(pgActivityType);

            //var sortList = new List<DapperExtensions.ISort>();
            //sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskID", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).Take<TaskViewEntity>(10).OrderByDescending(tv => tv.TaskID).ToList();
            #endregion
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
            string sql = @"SELECT 
                                TOP 1 *
                            FROM vwWfActivityInstanceTasks 
                            WHERE ActivityInstanceID=@activityInstanceID 
                                AND AssignedToUserID=@userID 
                                AND ProcessState=2 
                                AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6) 
                                AND (ActivityState=1 OR ActivityState=2) 
                            ORDER BY TASKID DESC";
            sql = SqlDataProviderFactory.GetSqlTaskOfMineByAtcitivityInstance(sql);
            var list = Repository.Query<TaskViewEntity>(conn, sql,
               new
               {
                   activityInstanceID = activityInstanceID,
                   userID = userID
               },
               trans).ToList();

            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 5));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 6));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));

            //var pgActivityState = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 1));
            //pgActivityState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 2));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AssignedToUserID, Operator.Eq, userID));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityInstanceID, Operator.Eq, activityInstanceID));
            //pgAll.Predicates.Add(pgActivityType);
            //pgAll.Predicates.Add(pgActivityState);

            ////var sortList = new List<DapperExtensions.ISort>();
            ////sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskID", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).OrderByDescending(tv => tv.TaskID).Take(1).ToList();
            # endregion

            if (list.Count == 0)
            {
                var message = LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.error");
                throw new WorkflowException(
                    string.Format("{0}，ActivityInstanceID: {1}", message, activityInstanceID.ToString())
                );
            }
            else if (list.Count > 1)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.toomoretasks.error"));
            }
            else
            {
                var taskView = list[0];
                return taskView;
            }
        }

        /// <summary>
        /// 获取我的任务
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="notThrowException">是否抛出异常</param>
        /// <param name="trans">数据库事务</param>
        /// <returns>任务视图实体</returns>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn,
            int activityInstanceID,
            string userID,
            bool notThrowException,
            IDbTransaction trans)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            string sql = @"SELECT 
                                TOP 1 *
                            FROM vwWfActivityInstanceTasks 
                            WHERE ActivityInstanceID=@activityInstanceID 
                                AND AssignedToUserID=@userID 
                                AND ProcessState=2 
                                AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6) 
                                AND (ActivityState=1 OR ActivityState=2) 
                            ORDER BY TASKID DESC";
            sql = SqlDataProviderFactory.GetSqlTaskOfMineByAtcitivityInstance(sql);
            var list = Repository.Query<TaskViewEntity>(conn, sql,
               new
               {
                   activityInstanceID = activityInstanceID,
                   userID = userID
               },
               trans).ToList();


            if (list.Count == 0)
            {
                if (notThrowException)
                {
                    return null;
                }
                else
                {
                    var message = LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.error");
                    throw new WorkflowException(
                        string.Format("{0}，ActivityInstanceID: {1}", message, activityInstanceID.ToString())
                    );
                }
            }
            else if (list.Count > 1)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.toomoretasks.error"));
            }
            else
            {
                var taskView = list[0];
                return taskView;
            }
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
            string sql = @"SELECT 
                            TOP 1 * 
                       FROM vwWfActivityInstanceTasks 
                       WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND AssignedToUserID=@userID 
                            AND ProcessState=2 
                            AND (ActivityType=4 OR ActivityType=5 OR ActivityType=6 OR WorkItemType=1)
                            AND (ActivityState=1 OR ActivityState=2) 
                       ORDER BY TASKID DESC";

                sql = SqlDataProviderFactory.GetSqlTaskOfMineByAppInstance(sql);
            var list = Repository.Query<TaskViewEntity>(conn, sql,
               new
               {
                   appInstanceID = appInstanceID,
                   processGUID = processGUID,
                   userID = userID
               },
               trans).ToList();


            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 5));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 6));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));

            //var pgActivityState = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 1));
            //pgActivityState.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 2));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AssignedToUserID, Operator.Eq, userID));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessGUID, Operator.Eq, processGUID));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AppInstanceID, Operator.Eq, appInstanceID));
            //pgAll.Predicates.Add(pgActivityType);
            //pgAll.Predicates.Add(pgActivityState);

            ////var sortList = new List<DapperExtensions.ISort>();
            ////sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskID", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).OrderByDescending(tv => tv.TaskID).Take(1).ToList();
            #endregion

            if (list.Count == 0)
            {
                var message = LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.error");
                throw new WorkflowException(
                    string.Format("{0}，AppInstanceID: {1}", message, appInstanceID.ToString())
                );
            }
            else if (list.Count > 1)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.toomoretasks.error"));
            }
            else
            {
                var taskView = list[0];
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
            string sql = @"SELECT 
                                * 
                           FROM vwWfActivityInstanceTasks 
                           WHERE AppInstanceID=@appInstanceID 
                                AND ProcessGUID=@processGUID 
                                AND ProcessState=2 
                                AND (ActivityType=4 OR WorkItemType=1)
                                AND ActivityState=1
                           ORDER BY TaskID DESC";
            var list = Repository.Query<TaskViewEntity>(sql,
                new
                {
                    appInstanceID = runner.AppInstanceID,
                    processGUID = runner.ProcessGUID
                });

            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 1));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AppInstanceID, Operator.Eq, runner.AppInstanceID));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessGUID, Operator.Eq, runner.ProcessGUID));
            //pgAll.Predicates.Add(pgActivityType);

            ////var sortList = new List<DapperExtensions.ISort>();
            ////sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskID", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).OrderByDescending(tv => tv.TaskID).ToList();
            #endregion

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
            string sql = @"SELECT * 
                         FROM vwWfActivityInstanceTasks 
                         WHERE ProcessState=2 
                            AND (ActivityType=4 OR WorkItemType=1)
                            AND ActivityState=1
                            AND IsEMailSent=0
							AND TaskState<>32
                        ";
            var list = Repository.Query<TaskViewEntity>(sql).ToList<TaskViewEntity>();

            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 1));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.IsEMailSent, Operator.Eq, 0));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.TaskState, Operator.Eq, 32, true));
            //pgAll.Predicates.Add(pgActivityType);

            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).ToList();
            #endregion
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


                //查询被委托人是否已经有待办任务存在
                var todoTask = GetTaskOfMine(session.Connection, activityInstance.ID, entity.EntrustToUserID, true, session.Transaction);
                if (todoTask != null)
                {
                    //更新委托用户信息
                    var originalTask = GetTask(todoTask.TaskID);
                    originalTask.EntrustedTaskID = entity.TaskID;
                    originalTask.LastUpdatedByUserID = entity.RunnerID;
                    originalTask.LastUpdatedByUserName = entity.RunnerName;
                    originalTask.LastUpdatedDateTime = DateTime.Now;
                    Update(originalTask, session);
                }
                else
                {
                    //插入委托任务
                    Insert(activityInstance, entity.EntrustToUserID, entity.EntrustToUserName,
                        entity.RunnerID, entity.RunnerName, session, entity.TaskID);
                }

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
            var updSql = @"UPDATE WfTasks 
                        SET TaskState=48, 
                            RecordStatusInvalid=1  
                        WHERE ActivityInstanceID=@activityInstanceID";

            var rows = Repository.Execute(session.Connection, updSql,
                new
                {
                    activityInstanceID = activityInstanceID
                },
                session.Transaction);
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
