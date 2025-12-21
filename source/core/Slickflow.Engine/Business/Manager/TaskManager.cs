
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Config;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Dapper;
using DapperExtensions;
using Slickflow.Engine.Business.Result;
using Slickflow.Engine.Business.SqlProvider;


namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Task manager: including tasks and task view objects
    /// 任务管理类：包括任务及任务视图对象
    /// </summary>
    public class TaskManager : ManagerBase
    {
        #region TaskView 任务视图
        /// <summary>
        /// Retrieve task view based on task Id
        /// 根据任务ID获取任务视图
        /// </summary>
        public TaskViewEntity GetTaskView(int taskId)
        {
            return Repository.GetById<TaskViewEntity>(taskId);
        }

        /// <summary>
        /// Retrieve task view based on task Id
        /// 根据任务ID获取任务视图
        /// </summary>
        public TaskViewEntity GetTaskView(IDbConnection conn, int taskId, IDbTransaction trans)
        {
            return Repository.GetById<TaskViewEntity>(conn, taskId, trans);
        }

        /// <summary>
        /// Retrieve task based on task Id
        /// 获取任务
        /// </summary>
        public TaskEntity GetTask(int taskId)
        {
            return Repository.GetById<TaskEntity>(taskId);
        }

        /// <summary>
        /// Retrieve task based on task Id
        /// 获取任务
        /// </summary>
        public TaskEntity GetTask(IDbConnection conn, int taskId, IDbTransaction trans)
        {
            return Repository.GetById<TaskEntity>(conn, taskId, trans);
        }

        /// <summary>
        /// Obtain tasks based on process information
        /// 根据流程信息获取任务
        /// </summary>
        internal List<TaskEntity> GetTaskListByActivity(IDbConnection conn,
            int processInstanceId,
            int activityInstanceId,
            IDbTransaction trans)
        {
            string sql = @"SELECT
                            * 
                         FROM wf_task 
                         WHERE activity_instance_id=@activityInstanceId
                            AND process_instance_id=@processInstanceId
                        ";
            var list = Repository.Query<TaskEntity>(conn,
                sql,
                new
                {
                    processInstanceId = processInstanceId,
                    activityInstanceId = activityInstanceId
                },
                trans).ToList();

            #region ORMapping support for multiple databases
            //var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pg.Predicates.Add(Predicates.Field<TaskEntity>(f => f.ActivityInstanceId, Operator.Eq, activityInstanceId));
            //pg.Predicates.Add(Predicates.Field<TaskEntity>(f => f.ProcessInstanceId, Operator.Eq, processInstanceId));
            //var list = Repository.GetList<TaskEntity>(conn, pg, null, trans).ToList();
            #endregion

            return list;
        }

        /// <summary>
        /// Obtain tasks based on process information
        /// 根据流程信息获取任务
        /// </summary>
        internal TaskEntity GetTaskByActivity(IDbConnection conn,
            int processInstanceId,
            int activityInstanceId,
            IDbTransaction trans)
        {
            TaskEntity task = null;
            var list = GetTaskListByActivity(conn, processInstanceId, activityInstanceId, trans);
            if (list.Count() > 0)
            {
                task = list[0];
            }
            return task;
        }

        /// <summary>
        /// Obtain tasks based on process information
        /// 根据流程信息获取任务
        /// </summary>
        internal TaskViewEntity GetTaskViewByActivity(int processInstanceId,
            int activityInstanceId)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var taskView = GetTaskViewByActivity(session.Connection, processInstanceId, activityInstanceId, session.Transaction);
                return taskView;
            }
        }

        /// <summary>
        /// Obtain tasks based on process information
        /// 根据流程信息获取任务
        /// </summary>
        internal TaskViewEntity GetTaskViewByActivity(IDbConnection conn,
            int processInstanceId,
            int activityInstanceId,
            IDbTransaction trans)
        {
            var sql = DatabaseSqlProviderFactory.GetTaskViewByActivity_SQL();
            var list = Repository.Query<TaskViewEntity>(sql,
            new
            {
                processInstanceId = processInstanceId,
                activityInstanceId = activityInstanceId

            }).ToList();

            #region ORMapping support for mutiple databases
            //var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pg.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityInstanceId, Operator.Eq, activityInstanceId));
            //pg.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessInstanceId, Operator.Eq, processInstanceId));
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
        /// Determine whether the task is the last task of the current node
        /// Single Node: Return True
        /// Multi instance nodes: judged based on the number of instances
        /// 判断任务是否是当前节点最后一个任务
        /// 单一节点：返回True
        /// 多实例节点：根据实例个数判断
        /// </summary>
        public Boolean IsLastTask(int taskId)
        {
            var isLast = false;
            var task = GetTask(taskId);
            var aim = new ActivityInstanceManager();
            var activityInstance = aim.GetById(task.ActivityInstanceId);

            if (aim.IsMultipleInstanceChildren(activityInstance) == true)
            {
                //多实例会签和加签处理
                //取出会签主节点实例数据
                //Multiple practical meetings for signing and adding signatures
                //Retrieve the primary node instance data for countersignature
                var mainActivityInstance = aim.GetById(activityInstance.MainActivityInstanceId.Value);
                var complexType = EnumHelper.ParseEnum<ComplexTypeEnum>(mainActivityInstance.ComplexType.Value.ToString());
                

                if (complexType == ComplexTypeEnum.SignTogether)        //Sign together
                {
					var mergeType = EnumHelper.ParseEnum<MergeTypeEnum>(mainActivityInstance.MergeType.Value.ToString());
                    if (mergeType == MergeTypeEnum.Sequence)        //Sequence
                    {
                        //取出处于多实例挂起的节点列表
                        //Retrieve the list of nodes that are suspended in multiple instances
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.Id,
                            mainActivityInstance.ProcessInstanceId,
                            (short)ActivityStateEnum.Suspended).ToList<ActivityInstanceEntity>();
                        short allNum = (short)mainActivityInstance.AssignedUserIds.Split(',').Length;
                        short maxOrder = 0;

                        if (sqList != null && sqList.Count > 0)
                        {
                            //取出最大执行节点
                            //Retrieve the maximum execution node
                            maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder.Value);
                        }
                        else if (mainActivityInstance.CompleteOrder <= allNum)
                        {
                            //最后一个执行节点
                            //The last execution node
                            maxOrder = (short)mainActivityInstance.CompleteOrder.Value;
                        }
                        else
                        {
                            maxOrder = allNum;
                        }
                        if (mainActivityInstance.CompareType == null || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Count)
                        {
                            //串行会签通过率（按人数判断）
                            //Serial signature pass rate (judged by number of people)
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
                            //串行会签未设置通过率的判断
                            //Judgment of no set pass rate for serial countersignature
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)
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
                    else if (mergeType == MergeTypeEnum.Parallel)        //Parallel
                    {
                        //取出处于多实例节点列表
                        //Retrieve the list of nodes in multiple instances
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.Id,
                            mainActivityInstance.ProcessInstanceId,
                            null).ToList<ActivityInstanceEntity>();
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed || w.AssignedUserIds == task.AssignedUserId)
                            .ToList<ActivityInstanceEntity>()
                            .Count();
                        if (mainActivityInstance.CompareType == null || (EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Percentage))
                        {
                            //并行会签未设置通过率的判断
                            //Judgment of no set pass rate for parallel signature
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)
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
                else if (complexType == ComplexTypeEnum.SignForward)     //Sign Forward
                {
                    //判断加签是否全部完成，如果是，则流转到下一步，否则不能流转
                    //Check if all signatures have been added. If so, proceed to the next step. Otherwise, do not proceed
                    var signforwardType = EnumHelper.ParseEnum<SignForwardTypeEnum>(activityInstance.SignForwardType.Value.ToString());

                    if (signforwardType == SignForwardTypeEnum.SignForwardBehind
                        || signforwardType == SignForwardTypeEnum.SignForwardBefore)        //Signforward befor, behind
                    {
                        //取出处于多实例节点列表
                        //Retrieve the list of nodes in multiple instances
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.Id,
                            mainActivityInstance.ProcessInstanceId,
                            (short)ActivityStateEnum.Suspended).ToList<ActivityInstanceEntity>();

                        short maxOrder = 0;
                        if (sqList != null && sqList.Count > 0)
                        {
                            //取出最大执行节点
                            //Retrieve the maximum execution node
                            maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder.Value);
                        }
                        else
                        {
                            //最后一个执行节点
                            //The last execution node
                            maxOrder = (short)activityInstance.CompleteOrder;
                        }
                        if (mainActivityInstance.CompareType == null || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Count)
                        {
                            //加签通过率
                            //Approval rate of additional signatures
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
                                //After the last node completes its execution, the main node enters the completion state, and the entire process proceeds downwards
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
                    else if (signforwardType == SignForwardTypeEnum.SignForwardParallel)        //Sign forward parallel
                    {
                        //取出处于多实例节点列表
                        //Retrieve the list of nodes in multiple instances
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.Id,
                            mainActivityInstance.ProcessInstanceId,
                            null).ToList<ActivityInstanceEntity>();

                        //并行加签，按照通过率来决定是否标识当前节点完成
                        //Parallel signing, determining whether to mark the current node completion based on the pass rate
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed || w.AssignedUserIds == task.AssignedUserId)
                            .ToList<ActivityInstanceEntity>()
                            .Count();
                        if (mainActivityInstance.CompareType == null || EnumHelper.ParseEnum<CompareTypeEnum>(mainActivityInstance.CompareType.Value.ToString()) == CompareTypeEnum.Percentage)
                        {
                            //并行加签通过率的判断
                            //Determination of parallel endorsement pass rate
                            if (mainActivityInstance.CompleteOrder > 1)
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
                //Single node type
                isLast = true;
            }
            return isLast;
        }
        #endregion

        #region Get the current user's processing tasks 获取当前用户的办理任务
        /// <summary>
        /// Retrieve the tasks currently running by the user
        /// 获取当前用户运行中的任务
        /// </summary>
        internal IEnumerable<TaskViewEntity> GetRunningTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 2);
        }

        /// <summary>
        /// Retrieve the tasks currently in ready by the user
        /// 获取当前用户待办的任务
        /// </summary>
        internal IEnumerable<TaskViewEntity> GetReadyTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 1);
        }

        /// <summary>
        /// Retrieve the tasks currently running by activity instance id
        /// 获取正在运行的任务
        /// </summary>
        internal TaskViewEntity GetFirstRunningTask(int activityInstanceId)
        {
            //sql query
            var sql = DatabaseSqlProviderFactory.GetFirstRunningTask_SQL();
            var list = Repository.Query<TaskViewEntity>(sql,
                new
                {
                    activityInstanceId = activityInstanceId
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
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityInstanceId, Operator.Eq, activityInstanceId));
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
        /// Retrieve completed tasks
        /// 获取已经完成任务
        /// </summary>
        internal IEnumerable<TaskViewEntity> GetCompletedTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 4);
        }

        /// <summary>
        /// Get task (pagination)
        /// 获取任务（分页）
        /// </summary>
        private IEnumerable<TaskViewEntity> GetTasksPaged(TaskQuery query, int activityState)
        {
            var parameters = new DynamicParameters();
            var sqlWhere = DatabaseSqlProviderFactory.GetTasksPaged_SQL(query, activityState, parameters);
            var pageSize = query.PageSize;
            if (pageSize == 0) pageSize = 100;          //Default number of records per page for pagination
            var list = Repository.Query<TaskViewEntity>(sqlWhere, parameters).Take<TaskViewEntity>(pageSize).ToList();
            
            return list;
        }

        /// <summary>
        /// Get Top 10 task todo list
        /// 仅供用于数据展示使用
        /// </summary>
        public List<TaskViewEntity> GetTaskToDoListTop()
        {
            var sql = DatabaseSqlProviderFactory.GetTaskToDoListTop_SQL();
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
            //sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskId", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).Take<TaskViewEntity>(10).OrderByDescending(tv => tv.TaskId).ToList();
            #endregion
            return list;
        }

        /// <summary>
        /// Get Top 10 task done list
        /// 仅供用于数据展示使用
        /// </summary>
        public List<TaskViewEntity> GetTaskDoneListTop()
        {
            var sql = DatabaseSqlProviderFactory.GetTaskDoneListTop_SQL();
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
            //sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskId", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).Take<TaskViewEntity>(10).OrderByDescending(tv => tv.TaskId).ToList();
            #endregion
            return list;
        }

        /// <summary>
        /// Get my task
        /// 获取我的任务
        /// </summary>
        internal TaskViewEntity GetTaskOfMineByActivityInstance(IDbConnection conn,
            int activityInstanceId, 
            string userId,
            IDbTransaction trans)
        {
            var sql = DatabaseSqlProviderFactory.GetTaskOfMineByActivityInstance_SQL();
            var list = Repository.Query<TaskViewEntity>(conn, sql,
               new
               {
                   activityInstanceId = activityInstanceId,
                   userId = userId
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
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AssignedUserId, Operator.Eq, userId));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityInstanceId, Operator.Eq, activityInstanceId));
            //pgAll.Predicates.Add(pgActivityType);
            //pgAll.Predicates.Add(pgActivityState);

            ////var sortList = new List<DapperExtensions.ISort>();
            ////sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskId", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).OrderByDescending(tv => tv.TaskId).Take(1).ToList();
            # endregion

            if (list.Count == 0)
            {
                var message = LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.error");
                throw new WorkflowException(
                    string.Format("{0}，ActivityInstanceId: {1}", message, activityInstanceId.ToString())
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
        /// Based on the application instance and process GUID, process the user Id to obtain the task list
        /// 根据应用实例、流程GUID，办理用户Id获取任务列表
        /// </summary>
        internal TaskViewEntity GetTaskOfMine(string appInstanceId, 
            string processId, 
            string userId)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetTaskOfMine(session.Connection, appInstanceId, processId, userId, session.Transaction);
            }
        }

        /// <summary>
        /// Based on the application instance and process GUID, process the user Id to obtain the task list
        /// 根据应用实例、流程GUID，办理用户Id获取任务列表
        /// </summary>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn,
            string appInstanceId,
            string processId,
            string userId,
            IDbTransaction trans)
        {

            var sql = DatabaseSqlProviderFactory.GetTaskOfMine_SQL();
            var list = Repository.Query<TaskViewEntity>(conn, sql,
               new
               {
                   appInstanceId = appInstanceId,
                   processId = processId,
                   userId = userId
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
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AssignedUserId, Operator.Eq, userId));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessId, Operator.Eq, processId));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AppInstanceId, Operator.Eq, appInstanceId));
            //pgAll.Predicates.Add(pgActivityType);
            //pgAll.Predicates.Add(pgActivityState);

            ////var sortList = new List<DapperExtensions.ISort>();
            ////sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskId", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).OrderByDescending(tv => tv.TaskId).Take(1).ToList();
            #endregion

            if (list.Count == 0)
            {
                var message = LocalizeHelper.GetEngineMessage("taskmanager.gettaskofmine.error");
                throw new WorkflowException(
                    string.Format("{0}，AppInstanceId: {1}", message, appInstanceId.ToString())
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
        /// Get my taskview
        /// 获取任务视图
        /// </summary>
        internal TaskViewEntity GetTaskOfMine(WfAppRunner runner)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetTaskOfMine(session.Connection, runner, session.Transaction);
            }
        }

        /// <summary>
        /// Get my taskview
        /// 获取任务视图
        /// </summary>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            TaskViewEntity taskView = null;
            if (runner.TaskId != null)
            {
                taskView = GetTaskView(conn, runner.TaskId.Value, trans);
            }
            else
            {
                taskView = GetTaskOfMine(conn, runner.AppInstanceId, runner.ProcessId, runner.UserId, trans);
            }
            return taskView;
        }

        /// <summary>
        /// Determine whether the task belongs to a certain user
        /// 判断任务是否属于某个用户
        /// </summary>
        internal bool IsMine(TaskViewEntity entity, string userId)
        {
            var isMine = false;
            if (entity.AssignedUserId == userId) isMine = true;
            return isMine;
        }

        /// <summary>
        /// Determine if the task is in a running state
        /// 判断任务处于运行状态
        /// </summary>
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
        /// Determine if the task is in a todo state
        /// 判断任务处于待办或运行状态
        /// </summary>
        private Boolean CheckTaskStateInToDoState(TaskViewEntity task)
        {
            var isInToDoState = false;
            if (task != null && (task.TaskState == (short)TaskStateEnum.Waiting
                    || task.TaskState == (short)TaskStateEnum.Reading))
            {
                isInToDoState = true;
            }
            return isInToDoState;
        }

        /// <summary>
        /// Retrieve pending tasks (business instances)
        /// 获取待办任务(业务实例)
        /// </summary>
        internal IEnumerable<TaskViewEntity> GetReadyTaskOfApp(WfAppRunner runner)
        {
            var sql = DatabaseSqlProviderFactory.GetReadyTaskOfApp_SQL();
            var list = Repository.Query<TaskViewEntity>(sql,
                new
                {
                    appInstanceId = runner.AppInstanceId,
                    processId = runner.ProcessId
                });

            #region ORMapping style to support multiple databases
            //var pgActivityType = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityType, Operator.Eq, 4));
            //pgActivityType.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.WorkItemType, Operator.Eq, 1));

            //var pgAll = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessState, Operator.Eq, 2));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ActivityState, Operator.Eq, 1));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.AppInstanceId, Operator.Eq, runner.AppInstanceId));
            //pgAll.Predicates.Add(Predicates.Field<TaskViewEntity>(f => f.ProcessId, Operator.Eq, runner.ProcessId));
            //pgAll.Predicates.Add(pgActivityType);

            ////var sortList = new List<DapperExtensions.ISort>();
            ////sortList.Add(new DapperExtensions.Sort { PropertyName = "TaskId", Ascending = false });
            //var list = Repository.GetList<TaskViewEntity>(pgAll, null).OrderByDescending(tv => tv.TaskId).ToList();
            #endregion

            return list;
        }

        /// <summary>
        /// Get the to-do list of unsent email notifications
        /// 获取未发送邮件通知的待办任务列表
        /// </summary>
        internal IList<TaskViewEntity> GetTaskListEMailUnSent()
        {
            var sql = DatabaseSqlProviderFactory.GetTaskListEMailUnSent_SQL();
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
        /// Insert task
        /// 插入任务数据
        /// </summary>
        private Int32 Insert(TaskEntity entity, 
            IDbSession session)
        {
            int newTaskId = Repository.Insert(session.Connection, entity, session.Transaction);
            return newTaskId;
        }

        /// <summary>
        /// Insert task
        /// 插入任务数据
        /// </summary>
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
        /// Insert Task
        /// 插入任务数据
        /// </summary>
        internal Int32 Insert(ActivityInstanceEntity activityInstance,
            Performer performer,
            WfAppRunner runner,
            IDbSession session)
        {
            return Insert(activityInstance, performer.UserId, performer.UserName, 
                runner.UserId, runner.UserName, session);
        }

        /// <summary>
        /// Insert task data (create task)
        /// 插入任务数据(创建任务)
        /// </summary>
        private Int32 Insert(ActivityInstanceEntity activityInstance,
            string performerId,
            string performerName,
            string runnerId,
            string runnerName,
            IDbSession session,
            int? entrustedTaskId = null)
        {
            TaskEntity entity = new TaskEntity();
            entity.AppName = activityInstance.AppName;
            entity.AppInstanceId = activityInstance.AppInstanceId;
            entity.ActivityInstanceId = activityInstance.Id;
            entity.ProcessInstanceId = activityInstance.ProcessInstanceId;
            entity.ActivityId = activityInstance.ActivityId;
            entity.ActivityName = activityInstance.ActivityName;
            entity.ProcessId = activityInstance.ProcessId;
            entity.TaskType = (short)TaskTypeEnum.Manual;
            entity.AssignedUserId = performerId;
            entity.AssignedUserName = performerName;
            entity.TaskState = 1; //1-ready
            entity.IsEMailSent = 0; //0-unsend status
            entity.CreatedUserId = runnerId;
            entity.CreatedUserName = runnerName;
            entity.CreatedDateTime = System.DateTime.UtcNow;
            entity.RecordStatusInvalid = 0;
            if(entrustedTaskId != null)
                entity.EntrustedTaskId = entrustedTaskId.Value;      //Record the delegated (original) task Id
            //insert task data
            int taskId = Insert(entity, session);
            entity.Id = taskId;

            //发送邮件通知消息
            //Send email notification messages
            SendTaskEMails(entity);

            return taskId;
        }

        /// <summary>
        /// Send email notification messages
        /// email template can be modified by customers
        /// the customers can use their own email html template, this is only a demo
        /// internal static readonly string EMailSendUtility_SendEMailTitle = "新待办事项提醒邮件！";
        /// internal static readonly string EMailSendUtility_SendEMailContent = @" < h1 >新待办任务提醒</h1>" +
        ///        "<p>您有一条新的待办任务： " +
        ///        "<a href='http://localhost/sfmvc/order/{0}'>请在此处填写具体的业务单据名称和单据序号</a>  " +
        ///        "请您及时登录业务系统处理！谢谢！</p>";
        /// 发送任务邮件
        /// </summary>
        private void SendTaskEMails(TaskEntity entity)
        {
            //判断是否需要发送待办邮件
            //Determine whether to send a to-do email
            if (JobAdminConfig.EMailSendUtility_SendEMailFlag == true)
            {
                var userId = entity.AssignedUserId;
                var rm = new ResourceService();
                var user = rm.GetUserById(userId);

                if (user != null && !string.IsNullOrEmpty(user.EMail))
                {
                    var title = LocalizeHelper.GetEngineMessage("taskmanager.emailtemplate.title");
                    var body = LocalizeHelper.GetEngineMessage("taskmanager.emailtemplate.body");
                    body = string.Format(body, JobAdminConfig.EMailSendUtility_LocalHostWebApp, entity.AppName, entity.AppInstanceId);
                    var emailSendUtility = new EMailSendUtility(entity.Id);
                    emailSendUtility.SendEMailAsync(title, body, user.EMail);
                }
            }
        }

        /// <summary>
        /// Regenerate task (limited to co signing child nodes under multiple instances)
        /// 重新生成任务(只限于会签多实例下的子节点)
        /// </summary>
        internal void Renew(ActivityInstanceEntity sourceActivityInstance,
            ActivityInstanceEntity newInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            var performer = new Performer(sourceActivityInstance.AssignedUserIds, 
                sourceActivityInstance.AssignedUserNames);

            Insert(newInstance, performer, runner, session);
        }

        /// <summary>
        /// Update task data
        /// 更新任务数据
        /// </summary>
        internal void Update(TaskEntity entity, IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// Set task type
        /// 设置任务类型
        /// </summary>
        internal void SetTaskType(int taskId, TaskTypeEnum taskType, IDbSession session)
        {
            var task = GetTask(session.Connection, taskId, session.Transaction);
            task.TaskType = (short)taskType;
            Repository.Update<TaskEntity>(session.Connection, task, session.Transaction);
        }

        /// <summary>
        /// Read task, set task to read status
        /// 读取任务，设置任务为已读状态
        /// </summary>
        internal void SetTaskRead(WfAppRunner taskRunner)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //置任务为处理状态
                //Set the task to reading status
                var task = GetTask(taskRunner.TaskId.Value);
                SetTaskState(task, taskRunner.UserId, taskRunner.UserName, TaskStateEnum.Reading, session);

                //置活动为运行状态
                //Set the task to reading status
                (new ActivityInstanceManager()).Read(task.ActivityInstanceId, taskRunner, session);

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
        /// Update the status of task email sending
        /// 更新任务邮件发送状态
        /// </summary>
        internal void SetTaskEMailSent(int taskId)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                var task = GetTask(taskId);
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
        /// Set task state
        /// 设置任务状态
        /// </summary>
        private void SetTaskState(TaskEntity task,
            string userId,
            string userName,
            TaskStateEnum taskState,
            IDbSession session)
        {
            task.TaskState = (short)taskState;
            task.UpdatedUserId = userId;
            task.UpdatedUserName = userName;
            task.UpdatedDateTime = System.DateTime.UtcNow;
            Update(task, session);
        }

        /// <summary>
        /// Set task state to end
        /// 设置任务完成
        /// </summary>
        internal void EndTaskState(long taskId,
            TaskStateEnum taskState,
            WfAppRunner runner,
            IDbSession session)
        {
            TaskEntity task = Repository.GetById<TaskEntity>(session.Connection, taskId, session.Transaction);
            task.TaskState = (byte)taskState;
            task.EndedDateTime = DateTime.UtcNow;
            task.EndedUserId = runner.UserId;
            task.EndedUserName = runner.UserName;

            Update(task, session);
        }

        /// <summary>
        /// Complete task
        /// 设置任务完成
        /// </summary>
        internal void Complete(int taskId,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskId, TaskStateEnum.Completed, runner, session);
        }

        /// <summary>
        /// Withdraw task
        /// 设置任务撤销
        /// </summary>
        internal void Withdraw(long taskId,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskId, TaskStateEnum.Withdrawed, runner, session);
        }

        /// <summary>
        /// Sendback task
        /// 设置任务退回
        /// </summary>
        internal void SendBack(long taskId,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskId, TaskStateEnum.SendBacked, runner, session);
        }

        /// <summary>
        /// Create a new delegated task
        /// 创建新的委托任务
        /// </summary>
        internal WfDataManagedResult Entrust(TaskEntrustedEntity entity, bool cancelOriginalTask = true)
        {
            var wfDataManagedResult = WfDataManagedResult.Default();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                var am = new ActivityInstanceManager();
                var activityInstance = am.GetByTask(entity.TaskId, session);

                if (activityInstance.ActivityState != (short)ActivityStateEnum.Ready
                    && activityInstance.ActivityState != (short)ActivityStateEnum.Running)
                {
                    var exceptionMessage = LocalizeHelper.GetEngineMessage("taskmanager.entrust.warn");
                    wfDataManagedResult = WfDataManagedResult.Exception(exceptionMessage);
                }

                //更新原委托任务的状态为关闭
                //Update the status of the original delegated task to closed
                if (cancelOriginalTask == true)
                {
                    var task = GetTask(entity.TaskId);
                    task.TaskState = (short)TaskStateEnum.Closed;
                    Update(task, session);
                }

                //查询该活动实例下的任务列表
                var taskList = GetTaskListByActivity(session.Connection, activityInstance.ProcessInstanceId, activityInstance.Id, session.Transaction);
                var entrustUserTaskList = taskList.Where<TaskEntity>(t=>t.AssignedUserId == entity.EntrustToUserId).ToList();
                if (entrustUserTaskList.Count > 0)
                {
                    //该用户当前有正在处理待办任务，或者之前曾经被委托过同样任务，不能被再次委托
                    //The user is currently working on a pending task or has previously been delegated the same task,
                    //and cannot be delegated again
                    var notAgainExceptionMessage = LocalizeHelper.GetEngineMessage("taskmanager.entrust.notagain.warn");
                    wfDataManagedResult = WfDataManagedResult.Exception(notAgainExceptionMessage);
                    return wfDataManagedResult;
                }

                //插入委托任务                                      
                //Insert delegated task
                Insert(activityInstance, entity.EntrustToUserId, entity.EntrustToUserName,
                    entity.RunnerId, entity.RunnerName, session, entity.TaskId);

                session.Commit();

                //返回成功标志对象
                wfDataManagedResult = WfDataManagedResult.Success();
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
            return wfDataManagedResult;
        }

        internal void CancelEntrustedTask(int taskId)
        {
            var taskManager = new TaskManager();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //把已经被委托的任务取消掉
                string sql = @"SELECT 
                                * 
                           FROM wf_task 
                           WHERE entrusted_task_id=@entrustedTaskId";
                var list = Repository.Query<TaskEntity>(session.Connection, sql,
                    new
                    {
                        entrustedTaskId = taskId
                    },
                    session.Transaction);
                var entrustedTaskEntity = list.FirstOrDefault();
                entrustedTaskEntity.TaskState = (short)TaskStateEnum.Canceled;
                entrustedTaskEntity.UpdatedDateTime = System.DateTime.UtcNow;
                taskManager.Update(entrustedTaskEntity, session);

                //复制一份之前原始任务，生成新的待办任务
                var originalTask = taskManager.GetTask(session.Connection, taskId, session.Transaction);
                var newTask = new TaskEntity();
                newTask.AppInstanceId = originalTask.AppInstanceId;
                newTask.AppName = originalTask.AppName;
                newTask.ProcessInstanceId = originalTask.ProcessInstanceId;
                newTask.ProcessId = originalTask.ProcessId;
                newTask.ActivityId = originalTask.ActivityId;
                newTask.ActivityName = originalTask.ActivityName;
                newTask.ActivityInstanceId = originalTask.ActivityInstanceId;
                newTask.AssignedUserId = originalTask.AssignedUserId;
                newTask.AssignedUserName = originalTask.AssignedUserName;
                newTask.TaskState = (short)TaskStateEnum.Waiting;
                newTask.TaskType = originalTask.TaskType;
                newTask.CreatedDateTime = System.DateTime.UtcNow;
                newTask.CreatedUserId = originalTask.CreatedUserId;
                newTask.CreatedUserName = originalTask.CreatedUserName;

                //insert new task
                taskManager.Insert(newTask, session);

                session.Commit();
            }
            catch (System.Exception e)
            {
                session.Rollback();
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.entrust.error"),
                    e);
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// Replace the task handlers under the current activity
        /// 取代当前活动下的任务办理人员
        /// </summary>
        internal Boolean Replace(int activityInstanceId, List<TaskReplacedEntity> replaced, WfAppRunner runner)
        {
            var isOk = false;
            var session = SessionFactory.CreateSession();
            try
            {
                CancelTask(activityInstanceId, runner, session);

                var aim = new ActivityInstanceManager();
                var activityInstance = aim.GetById(session.Connection, activityInstanceId, session.Transaction);

                activityInstance.AssignedUserIds = GenerateActivityAssignedUserIDs(replaced);
                activityInstance.AssignedUserNames = GenerateActivityAssignedUserNames(replaced);

                foreach (var user in replaced)
                {
                    Insert(activityInstance, user.ReplacedByUserId, user.ReplacedByUserName, runner.UserId, runner.UserName, session);
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
        /// Generate userid list
        /// 生成UserIds字符串
        /// </summary>
        private string GenerateActivityAssignedUserIDs(List<TaskReplacedEntity> userList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var user in userList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(user.ReplacedByUserId);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// Generate username list
        /// 获取UserNames字符串
        /// </summary>
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
        /// Cancel task
        /// 取消任务办理
        /// </summary>
        private void CancelTask(int activityInstanceId, WfAppRunner runner, IDbSession session)
        {
            var updSql = @"UPDATE wf_task
                        SET task_state=48, 
                            record_status_invalid=1  
                        WHERE activity_instance_id=@activityInstanceId";

            var rows = Repository.Execute(session.Connection, updSql,
                new
                {
                    activityInstanceId = activityInstanceId
                },
                session.Transaction);
        }

        /// <summary>
        /// Delete task
        /// 任务删除
        /// </summary>
        internal bool Delete(IDbConnection conn, long taskId, IDbTransaction trans)
        {
            return Repository.Delete<TaskEntity>(conn, taskId, trans);
        }
        #endregion
    }
}
