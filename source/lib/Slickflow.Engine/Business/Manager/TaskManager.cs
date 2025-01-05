
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Data.DataProvider;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Config;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;


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
        /// Retrieve task view based on task ID
        /// 根据任务ID获取任务视图
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public TaskViewEntity GetTaskView(int taskID)
        {
            return Repository.GetById<TaskViewEntity>(taskID);
        }

        /// <summary>
        /// Retrieve task view based on task ID
        /// 根据任务ID获取任务视图
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="taskID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public TaskViewEntity GetTaskView(IDbConnection conn, int taskID, IDbTransaction trans)
        {
            return Repository.GetById<TaskViewEntity>(conn, taskID, trans);
        }

        /// <summary>
        /// Retrieve task based on task ID
        /// 获取任务
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public TaskEntity GetTask(int taskID)
        {
            return Repository.GetById<TaskEntity>(taskID);
        }

        /// <summary>
        /// Retrieve task based on task ID
        /// 获取任务
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="taskID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public TaskEntity GetTask(IDbConnection conn, int taskID, IDbTransaction trans)
        {
            return Repository.GetById<TaskEntity>(conn, taskID, trans);
        }

        /// <summary>
        /// Obtain tasks based on process information
        /// 根据流程信息获取任务
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
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
        /// Obtain tasks based on process information
        /// 根据流程信息获取任务
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="activityInstanceID"></param>
        /// <returns></returns>
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
        /// Obtain tasks based on process information
        /// 根据流程信息获取任务
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
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
        /// Determine whether the task is the last task of the current node
        /// Single Node: Return True
        /// Multi instance nodes: judged based on the number of instances
        /// 判断任务是否是当前节点最后一个任务
        /// 单一节点：返回True
        /// 多实例节点：根据实例个数判断
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
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
                //Multiple practical meetings for signing and adding signatures
                //Retrieve the primary node instance data for countersignature
                var mainActivityInstance = aim.GetById(activityInstance.MIHostActivityInstanceID.Value);
                var complexType = EnumHelper.ParseEnum<ComplexTypeEnum>(mainActivityInstance.ComplexType.Value.ToString());
                

                if (complexType == ComplexTypeEnum.SignTogether)        //Sign together
                {
					var mergeType = EnumHelper.ParseEnum<MergeTypeEnum>(mainActivityInstance.MergeType.Value.ToString());
                    if (mergeType == MergeTypeEnum.Sequence)        //Sequence
                    {
                        //取出处于多实例挂起的节点列表
                        //Retrieve the list of nodes that are suspended in multiple instances
                        var sqList = aim.GetActivityMulitipleInstanceWithState(
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
                            (short)ActivityStateEnum.Suspended).ToList<ActivityInstanceEntity>();
                        short allNum = (short)mainActivityInstance.AssignedToUserIDs.Split(',').Length;
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
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
                            null).ToList<ActivityInstanceEntity>();
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed || w.AssignedToUserIDs == task.AssignedToUserID)
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
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
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
                            mainActivityInstance.ID,
                            mainActivityInstance.ProcessInstanceID,
                            null).ToList<ActivityInstanceEntity>();

                        //并行加签，按照通过率来决定是否标识当前节点完成
                        //Parallel signing, determining whether to mark the current node completion based on the pass rate
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed || w.AssignedToUserIDs == task.AssignedToUserID)
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
        /// <param name="query"></param>
        /// <returns></returns>
        internal IEnumerable<TaskViewEntity> GetRunningTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 2);
        }

        /// <summary>
        /// Retrieve the tasks currently in ready by the user
        /// 获取当前用户待办的任务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal IEnumerable<TaskViewEntity> GetReadyTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 1);
        }

        /// <summary>
        /// Retrieve the tasks currently running by activity instance id
        /// 获取正在运行的任务
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <returns></returns>
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
        /// Retrieve completed tasks
        /// 获取已经完成任务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal IEnumerable<TaskViewEntity> GetCompletedTasks(TaskQuery query)
        {
            return GetTasksPaged(query, 4);
        }

        /// <summary>
        /// Get task (pagination)
        /// 获取任务（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="activityState"></param>
        /// <returns></returns>
        private IEnumerable<TaskViewEntity> GetTasksPaged(TaskQuery query, int activityState)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -task type 表示“任务”类型的节点
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
                parameters.Add("@appName", string.Format("%{0}%", query.AppName));
            }
            sqlBuilder.OrderBy("TaskID", true);

            var pageSize = query.PageSize;
            if (pageSize == 0) pageSize = 100;          //Default number of records per page for pagination
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
        /// Get my task
        /// 获取我的任务
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="userID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn,
            int activityInstanceID, 
            string userID,
            IDbTransaction trans)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -task type 表示“任务”类型的节点
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
        /// Get my task
        /// 获取我的任务
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="userID"></param>
        /// <param name="notThrowException"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn,
            int activityInstanceID,
            string userID,
            bool notThrowException,
            IDbTransaction trans)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -task type 表示“任务”类型的节点
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
        /// Based on the application instance and process UID, process the user ID to obtain the task list
        /// 根据应用实例、流程GUID，办理用户Id获取任务列表
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
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
        /// Based on the application instance and process UID, process the user ID to obtain the task list
        /// 根据应用实例、流程GUID，办理用户Id获取任务列表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal TaskViewEntity GetTaskOfMine(IDbConnection conn,
            string appInstanceID,
            string processGUID,
            string userID,
            IDbTransaction trans)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -task type 表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            //2015.09.10 besley
            //added ActivityType to WorkItemType
            //To handle multiple types of task nodes, including regular tasks, multiple instances, and subprocess nodes
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
        /// Get my taskview
        /// 获取任务视图
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
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
        /// <param name="conn"></param>
        /// <param name="runner"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
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
        /// Determine whether the task belongs to a certain user
        /// 判断任务是否属于某个用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        internal bool IsMine(TaskViewEntity entity, string userID)
        {
            var isMine = false;
            if (entity.AssignedToUserID == userID) isMine = true;
            return isMine;
        }

        /// <summary>
        /// Determine if the task is in a running state
        /// 判断任务处于运行状态
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
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
        /// Retrieve pending tasks (business instances)
        /// 获取待办任务(业务实例)
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        internal IEnumerable<TaskViewEntity> GetReadyTaskOfApp(WfAppRunner runner)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -task type 表示“任务”类型的节点
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
        /// Get the to-do list of unsent email notifications
        /// 获取未发送邮件通知的待办任务列表
        /// </summary>
        /// <returns></returns>
        internal IList<TaskViewEntity> GetTaskListEMailUnSent()
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -task type 表示“任务”类型的节点
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
        /// Insert task
        /// 插入任务数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="session"></param>
        private Int32 Insert(TaskEntity entity, 
            IDbSession session)
        {
            int newTaskID = Repository.Insert(session.Connection, entity, session.Transaction);
            return newTaskID;
        }

        /// <summary>
        /// Insert task
        /// 插入任务数据
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="performers"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
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
        /// <param name="activityInstance"></param>
        /// <param name="performer"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal Int32 Insert(ActivityInstanceEntity activityInstance,
            Performer performer,
            WfAppRunner runner,
            IDbSession session)
        {
            return Insert(activityInstance, performer.UserID, performer.UserName, 
                runner.UserID, runner.UserName, session);
        }

        /// <summary>
        /// Insert task data (create task)
        /// 插入任务数据(创建任务)
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="performerID"></param>
        /// <param name="performerName"></param>
        /// <param name="runnerID"></param>
        /// <param name="runnerName"></param>
        /// <param name="session"></param>
        /// <param name="entrustedTaskID"></param>
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
            entity.TaskState = 1; //1-ready
            entity.IsEMailSent = 0; //0-unsend status
            entity.CreatedByUserID = runnerID;
            entity.CreatedByUserName = runnerName;
            entity.CreatedDateTime = System.DateTime.Now;
            entity.RecordStatusInvalid = 0;
            if(entrustedTaskID != null)
                entity.EntrustedTaskID = entrustedTaskID.Value;      //Record the delegated (original) task ID
            //insert task data
            int taskID = Insert(entity, session);
            entity.ID = taskID;

            //发送邮件通知消息
            //Send email notification messages
            SendTaskEMails(entity);

            return taskID;
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
        /// <param name="entity"></param>
        private void SendTaskEMails(TaskEntity entity)
        {
            //判断是否需要发送待办邮件
            //Determine whether to send a to-do email
            if (JobAdminConfig.EMailSendUtility_SendEMailFlag == true)
            {
                var userID = entity.AssignedToUserID;
                var rm = new ResourceService();
                var user = rm.GetUserById(userID);

                if (user != null && !string.IsNullOrEmpty(user.EMail))
                {
                    var title = LocalizeHelper.GetEngineMessage("taskmanager.emailtemplate.title");
                    var body = LocalizeHelper.GetEngineMessage("taskmanager.emailtemplate.body");
                    body = string.Format(body, JobAdminConfig.EMailSendUtility_LocalHostWebApp, entity.AppName, entity.AppInstanceID);
                    var emailSendUtility = new EMailSendUtility(entity.ID);
                    emailSendUtility.SendEMailAsync(title, body, user.EMail);
                }
            }
        }

        /// <summary>
        /// Regenerate task (limited to co signing child nodes under multiple instances)
        /// 重新生成任务(只限于会签多实例下的子节点)
        /// </summary>
        /// <param name="sourceActivityInstance"></param>
        /// <param name="newInstance"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
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
        /// Update task data
        /// 更新任务数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="session"></param>
        internal void Update(TaskEntity entity, IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// Set task type
        /// 设置任务类型
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="taskType"></param>
        /// <param name="session"></param>
        internal void SetTaskType(int taskID, TaskTypeEnum taskType, IDbSession session)
        {
            var task = GetTask(session.Connection, taskID, session.Transaction);
            task.TaskType = (short)taskType;
            Repository.Update<TaskEntity>(session.Connection, task, session.Transaction);
        }

        /// <summary>
        /// Read task, set task to read status
        /// 读取任务，设置任务为已读状态
        /// </summary>
        /// <param name="taskRunner"></param>
        internal void SetTaskRead(WfAppRunner taskRunner)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //置任务为处理状态
                //Set the task to reading status
                var task = GetTask(taskRunner.TaskID.Value);
                SetTaskState(task, taskRunner.UserID, taskRunner.UserName, TaskStateEnum.Reading, session);

                //置活动为运行状态
                //Set the task to reading status
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
        /// Update the status of task email sending
        /// 更新任务邮件发送状态
        /// </summary>
        /// <param name="taskID"></param>
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
        /// Set task state
        /// 设置任务状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="taskState"></param>
        /// <param name="session"></param>
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
        /// Set task state to end
        /// 设置任务完成
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="taskState"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
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
        /// Complete task
        /// 设置任务完成
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Complete(int taskID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskID, TaskStateEnum.Completed, runner, session);
        }

        /// <summary>
        /// Withdraw task
        /// 设置任务撤销
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Withdraw(long taskID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskID, TaskStateEnum.Withdrawed, runner, session);
        }

        /// <summary>
        /// Sendback task
        /// 设置任务退回
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void SendBack(long taskID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndTaskState(taskID, TaskStateEnum.SendBacked, runner, session);
        }

        /// <summary>
        /// Create a new delegated task
        /// 创建新的委托任务
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancalOriginalTask"></param>
        internal bool Entrust(TaskEntrustedEntity entity, bool cancalOriginalTask = true)
        {
            var isOk = false;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                var am = new ActivityInstanceManager();
                var activityInstance = am.GetByTask(entity.TaskID, session);

                if (activityInstance.ActivityState != (short)ActivityStateEnum.Ready
                    && activityInstance.ActivityState != (short)ActivityStateEnum.Running)
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("taskmanager.entrust.warn"));
                }

                //更新AssignedToUsers 信息
                //Update Assigned To Users information
                activityInstance.AssignedToUserIDs = activityInstance.AssignedToUserIDs + "," + entity.EntrustToUserID;
                activityInstance.AssignedToUserNames = activityInstance.AssignedToUserNames + "," + entity.EntrustToUserName;
				activityInstance.ActivityState = (int)ActivityStateEnum.Ready;
                am.Update(activityInstance, session);

                //更新原委托任务的状态为关闭
                //Update the status of the original delegated task to closed
                if (cancalOriginalTask == true)
                {
                    var task = GetTask(entity.TaskID);
                    task.TaskState = (short)TaskStateEnum.Closed;
                    Update(task, session);
                }


                //查询被委托人是否已经有待办任务存在
                //Check if the delegated person already has pending tasks
                var todoTask = GetTaskOfMine(session.Connection, activityInstance.ID, entity.EntrustToUserID, true, session.Transaction);
                if (todoTask != null)
                {
                    //更新委托用户信息
                    //Update entrusted user information
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
                    //Insert delegated task
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
        /// Replace the task handlers under the current activity
        /// 取代当前活动下的任务办理人员
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="replaced"></param>
        /// <param name="runner"></param>
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
        /// Generate userid list
        /// 生成UserIds字符串
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
        /// Generate username list
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
        /// Cancel task
        /// 取消任务办理
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
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
        /// Delete task
        /// 任务删除
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="taskID"></param>
        /// <param name="trans"></param>
        internal bool Delete(IDbConnection conn, long taskID, IDbTransaction trans)
        {
            return Repository.Delete<TaskEntity>(conn, taskID, trans);
        }
        #endregion
    }
}
