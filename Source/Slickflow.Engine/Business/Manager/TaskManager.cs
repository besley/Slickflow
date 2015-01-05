using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using Dapper;
using DapperExtensions;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 任务管理类：包括任务及任务视图对象
    /// </summary>
    public class TaskManager : ManagerBase
    {
        #region TaskManager 任务分配视图
        public TaskViewEntity GetTaskView(int taskID)
        {
            return Repository.GetById<TaskViewEntity>(taskID);
        }

        public TaskEntity GetTask(int taskID)
        {
            return Repository.GetById<TaskEntity>(taskID);
        }


        #region TaskManager 获取当前用户的办理任务
        /// <summary>
        /// 获取当前用户运行中的任务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal IEnumerable<TaskViewEntity> GetRunningTasks(TaskQueryEntity query, out int allRowsCount)
        {
            return GetTasksPaged(query, 2, out allRowsCount);
        }

        /// <summary>
        /// 获取当前用户待办的任务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal IEnumerable<TaskViewEntity> GetReadyTasks(TaskQueryEntity query, out int allRowsCount)
        {
            return GetTasksPaged(query, 1, out allRowsCount);
        } 

        /// <summary>
        /// 获取任务（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="activityState"></param>
        /// <returns></returns>
        private IEnumerable<TaskViewEntity> GetTasksPaged(TaskQueryEntity query, int activityState, out int allRowsCount)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（运行）；

            StringBuilder sql = new StringBuilder(512);
            sql.Append(@"SELECT 
                            Top 100 * 
                         FROM vwWfActivityInstanceTasks 
                         WHERE ProcessState=2 
                            AND ActivityType=4
                            AND ActivityState=@activityState
                            AND AssignedToUserID=@assignedToUserID
                        ");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@activityState", activityState);
            parameters.Add("@assignedToUserID", query.UserID);

            if (!string.IsNullOrEmpty(query.AppInstanceID))
            {
                sql.Append(" AND AppInstanceID=@appInstanceID");
                parameters.Add("@appinstanceID", query.AppInstanceID);
            }

            if (!string.IsNullOrEmpty(query.ProcessGUID))
            {
                sql.Append(" AND ProcessGUID=@processGUID");
                parameters.Add("@processGUID", query.ProcessGUID);
            }

            if (!string.IsNullOrEmpty(query.AppName))
            {
                sql.AppendFormat(" AND AppName like '%' + @appName + '%' ");
                parameters.Add("@appName", query.AppName);
            }

            sql.Append(" ORDER BY TASKID DESC ");

            //如果数据记录数为0，则不用查询列表
            StringBuilder sqlCount = new StringBuilder(1024);
            sqlCount.Append("SELECT COUNT(1) FROM (");
            sqlCount.Append(sql.ToString());
            sqlCount.Append(")T");

            allRowsCount = Repository.Count(sqlCount.ToString(), parameters);
            if (allRowsCount == 0)
            {
                return null;
            }

            //查询列表数据并返回结果集
            var list = Repository.Query<TaskViewEntity>(sql.ToString(),
                parameters);

            return list;
        }

        /// <summary>
        /// 获取我的任务
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        internal TaskViewEntity GetTaskOfMine(int activityInstanceID, 
            string userID)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            string whereSql = @"SELECT TOP 1 * FROM vwWfActivityInstanceTasks 
                           WHERE ActivityInstanceID=@activityInstanceID 
                                AND AssignedToUserID=@userID 
                                AND ProcessState=2 AND (ActivityType=4 OR ActivityType=5) 
                                AND (ActivityState=1 OR ActivityState=2) 
                           ORDER BY TASKID DESC";

            var list = Repository.Query<TaskViewEntity>(
                whereSql,
                new
                {
                    activityInstanceID = activityInstanceID,
                    userID = userID
                }).ToList<TaskViewEntity>();

            //取出唯一待办任务记录，并返回。
            TaskViewEntity task = null;
            if (list != null && list.Count == 1)
            {
                task = list[0];
            }
            return task;
        }

        /// <summary>
        /// 根据应用实例、流程GUID，办理用户Id获取任务列表
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal TaskViewEntity GetTaskOfMine(string appInstanceID, 
            string processGUID, 
            string userID)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）, 2-running（）运行；
            string sql = @"SELECT TOP 1 * FROM vwWfActivityInstanceTasks 
                           WHERE AppInstanceID=@appInstanceID 
                                AND ProcessGUID=@processGUID 
                                AND AssignedToUserID=@userID 
                                AND ProcessState=2 
                                AND ActivityType=4 
                                AND (ActivityState=1 OR ActivityState=2) 
                           ORDER BY TASKID DESC";
            var taskList = Repository.Query<TaskViewEntity>(sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID,
                    userID = userID
                }).ToList();

            if (taskList == null || taskList.Count == 0)
            {
                throw new WorkflowException(
                    string.Format("当前没有你要办理的任务！业务单据标识:{0}", appInstanceID.ToString())
                );
            }
            else if (taskList.Count > 1)
            {
                throw new WorkflowException(string.Format("当前办理任务的数目: {0} 大于1，无法确定下一步节点！", taskList.Count));
            }

            var task = taskList[0];

            return task;
        }

         /// <summary>
        /// 判断活动实例是否是某个用户的任务
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="userID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal bool IsMine(int activityInstanceID, int userID)
        {
            string whereSql = @"WHERE ActivityInstanceID=@activityInstanceID
                             AND AssignedToUserID=@assignedToUserID";

            var list = Repository.Query<TaskEntity>(whereSql,
                new
                {
                    activityInstanceID = activityInstanceID,
                    assignedToUserID = userID
                }).ToList<TaskEntity>();

            bool isMine = false;
            if (list != null && list.Count == 1)
            {
                isMine = true;
            }
            return isMine;
        }

        internal IEnumerable<TaskViewEntity> GetReadyTaskOfApp(WfAppRunner runner)
        {
            //processState:2 -running 流程处于运行状态
            //activityType:4 -表示“任务”类型的节点
            //activityState: 1-ready（准备）
            string sql = @"SELECT * FROM vwWfActivityInstanceTasks 
                           WHERE AppInstanceID=@appInstanceID 
                                    AND ProcessGUID=@processGUID 
                                    AND ProcessState=2 AND ActivityType=4 AND ActivityState=1";

            var list = Repository.Query<TaskViewEntity>(sql,
                new
                {
                    appInstanceID = runner.AppInstanceID,
                    processGUID = runner.ProcessGUID
                });
            return list;
        }
        #endregion


        /// <summary>
        /// 获取流程实例下的任务数据
        /// </summary>
        /// <param name="appInstanceID">应用ID</param>
        /// <param name="ProcessInstanceID">流程实例ID</param>
        /// <returns>任务列表数据</returns>
        internal IEnumerable<TaskViewEntity> GetProcessTasks(int appInstanceID,
            int processInstanceID)
        {
            string whereSql = @"WHERE ApplicationInstaceID=@appInstanceID 
                            AND ProcessInstanceID=@processInstanceID";
            var list = Repository.Query<TaskViewEntity>(whereSql,
                new
                {
                    appInstanceID = appInstanceID,
                    processInstanceID = processInstanceID
                });
            return list;

        }

        internal IEnumerable<TaskViewEntity> GetProcessTasksWithState(int appInstanceID,
            int processInstanceID,
            ActivityStateEnum state)
        {
            string whereSql = @"WHERE ApplicationInstaceID=@appInstanceID 
                            AND ProcessInstanceID=@processInstanceID 
                            AND ActivityState=@state";
            var list = Repository.Query<TaskViewEntity>(whereSql,
                new
                {
                    appInstanceID = appInstanceID,
                    processInstanceID = processInstanceID,
                    state = state
                });
            return list;
        }

       
        #endregion

        #region TaskManager 任务数据基本操作
        /// <summary>
        /// 插入任务数据
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <param name="wfLinqDataContext">linq上下文</param>
        internal void Insert(TaskEntity entity, 
            IDbSession session)
        {
            int result = Repository.Insert(session.Connection, entity, session.Transaction);
            Debug.WriteLine(string.Format("task instance inserted, time:{0}", System.DateTime.Now.ToString()));
        }

        /// <summary>
        /// 插入任务数据
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="performers"></param>
        /// <param name="wfLinqDataContext"></param>
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
        /// <param name="activityInstance"></param>
        /// <param name="performer"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Insert(ActivityInstanceEntity activityInstance,
            Performer performer,
            WfAppRunner runner,
            IDbSession session)
        {
            Insert(activityInstance, performer.UserID, performer.UserName, 
                runner.UserID, runner.UserName, session);
        }

        /// <summary>
        /// 插入任务数据(创建任务)
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="performerID"></param>
        /// <param name="performerName"></param>
        /// <param name="runnerID"></param>
        /// <param name="runnerName"></param>
        /// <param name="session"></param>
        private void Insert(ActivityInstanceEntity activityInstance,
            string performerID,
            string performerName,
            string runnerID,
            string runnerName,
            IDbSession session)
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
            //插入任务数据
            Insert(entity, session);
        }

        /// <summary>
        /// 更新任务数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wfLinqDataContext"></param>
        internal void Update(TaskEntity entity, IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }


        /// <summary>
        /// 读取任务，设置任务为已读状态
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        internal void SetTaskRead(WfAppRunner taskRunner)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //置任务为处理状态
                var task = GetTask(taskRunner.TaskID.Value);
                SetTaskState(task, taskRunner.UserID, taskRunner.UserName, TaskStateEnum.Handling, session);

                //置活动为运行状态
                (new ActivityInstanceManager()).SetActivityRead(task.ActivityInstanceID, taskRunner.UserID, taskRunner.UserName, session);

                session.Commit();
            }
            catch (System.Exception e)
            {
                session.Rollback();
                throw new WorkflowException(string.Format("阅读待办任务时出错！，详细错误：{0}", e.Message), e);
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="logonUser"></param>
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
        /// 设置任务完成
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="runner"></param>
        /// <param name="wfLinqDataContext"></param>
        internal void Complete(long taskID,
            WfAppRunner runner,
            IDbSession session)
        {
            TaskEntity task = Repository.GetById<TaskEntity>(taskID);
            task.TaskState = (byte)TaskStateEnum.Completed;
            task.EndedDateTime = DateTime.Now;
            task.EndedByUserID = runner.UserID;
            task.EndedByUserName = runner.UserName;

            Update(task, session);
        }

        /// <summary>
        /// 创建新的委托任务
        /// </summary>
        /// <param name="entity"></param>
        internal void Entrust(TaskEntrustedEntity entity)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                //获取活动实例信息
                session.BeginTrans();

                var am = new ActivityInstanceManager();
                var activityInstance = am.GetByTask(entity.ID, session);

                if (activityInstance.ActivityState != (short)ActivityStateEnum.Ready
                    && activityInstance.ActivityState != (short)ActivityStateEnum.Running)
                {
                    throw new WorkflowException("没有可以委托的任务，因为活动实例的状态不在运行状态！");
                }

                //更新AssignedToUsers 信息
                activityInstance.AssignedToUsers = activityInstance.AssignedToUsers + "," + entity.EntrustToUserID;
                am.Update(activityInstance, session);

                //插入委托任务
                Insert(activityInstance, entity.EntrustToUserID, entity.EntrustToUserName,
                    entity.RunnerID, entity.RunnerName, session);

                session.Commit();
            }
            catch(System.Exception e)
            {
                session.Rollback();
                throw new WorkflowException("任务委托失败，请查看异常信息！", e);
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 任务删除
        /// </summary>
        /// <param name="taskID">任务ID</param>
        internal bool Delete(IDbConnection conn, long taskID, IDbTransaction trans)
        {
            return Repository.Delete<TaskEntity>(conn, taskID, trans);
        }
        #endregion
    }
}
