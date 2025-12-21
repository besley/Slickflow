
using Dapper;
using DnsClient.Protocol;
using Google.Protobuf.Compiler;
using MongoDB.Driver;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Process instance manager
    /// 流程实例管理者类
    /// </summary>
    internal class ProcessInstanceManager : ManagerBase
    {
        #region ProcessInstanceManager Basic Data Operations 基本数据操作
        /// <summary>
        /// Get the current running process instance latest
        /// 获取当前运行的流程实例
        /// </summary>
        internal ProcessInstanceEntity GetProcessInstanceLatest(String appInstanceId,
           String processId,
           String version)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInstanceCurrent(session.Connection, appInstanceId, processId, version, session.Transaction);
            }
        }

        /// <summary>
        /// Get the current running process instance latest
        /// 获取当前运行的流程实例
        /// </summary>
        internal ProcessInstanceEntity GetProcessInstanceLatest(IDbConnection conn,
           String appInstanceId,
           String processId,
           String version,
           IDbTransaction trans)
        {
            return GetProcessInstanceCurrent(conn, appInstanceId, processId, version, trans);
        }

        /// <summary>
        /// Obtain process instance data based on Id
        /// 根据ID获取流程实例数据
        /// </summary>
        internal ProcessInstanceEntity GetById(int processInstanceId)
        {
            return Repository.GetById<ProcessInstanceEntity>(processInstanceId);
        }

        /// <summary>
        /// Obtain process instance data based on Id
        /// 根据ID获取流程实例数据
        /// </summary>
        internal ProcessInstanceEntity GetById(IDbConnection conn, int processInstanceId, IDbTransaction trans)
        {
            return Repository.GetById<ProcessInstanceEntity>(conn, processInstanceId, trans);
        }

        /// <summary>
        /// Obtain process instance data by Version
        /// 根据版本获取流程实例
        /// </summary>
        private ProcessInstanceEntity GetByVersion(string processId, 
            string version)
        {
            //var list = Repository.GetAll<ProcessInstanceEntity>()
            //    .Where<ProcessInstanceEntity>(
            //        p => p.ProcessId == processId &&
            //            p.Version == version)
            //    .OrderByDescending(p => p.Id)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance
                        WHERE process_id=@processId
                            AND version=@version
                        ORDER BY id DESC";
            var list = Repository.Query<ProcessInstanceEntity>(sql,
                        new
                        {
                            processId = processId,
                            version = version
                        }).ToList();

            var entity = EnumHelper.GetFirst<ProcessInstanceEntity>(list);
            return entity;
        }

        /// <summary>
        ///  Obtain process instance data by activity instance id
        /// 根据活动实例ID查询流程实例
        /// </summary>
        internal ProcessInstanceEntity GetByActivity(IDbConnection conn,
            int activityInstanceId,
            IDbTransaction trans)
        {
            var activityInstance = Repository.GetById<ActivityInstanceEntity>(conn, activityInstanceId, trans);
            var processInstance = Repository.GetById<ProcessInstanceEntity>(conn, activityInstance.ProcessInstanceId, trans);

            return processInstance;
        }

        /// <summary>
        /// Determine whether the process instance is a subprocess
        /// 判断流程实例是否为子流程
        /// </summary>
        internal Boolean IsSubProcess(ProcessInstanceEntity entity)
        {
            var isSub = false;
            if (entity.InvokedActivityInstanceId > 0 
                && !string.IsNullOrEmpty(entity.InvokedActivityId))
            {
                isSub = true;
            }
            return isSub;
        }

        /// <summary>
        /// Obtain the initiator of the process
        /// 获取流程的发起人
        /// </summary>
        internal User GetProcessInitiator(int processInstanceId)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInitiator(session.Connection, processInstanceId, session.Transaction);
            }
        }

        /// <summary>
        /// Obtain the initiator of the process
        /// 获取流程的发起人
        /// </summary>
        internal User GetProcessInitiator(IDbConnection conn,
            int processInstanceId,
            IDbTransaction trans)
        {
            var entity = GetById(processInstanceId);
            var initiator = new User { UserId = entity.CreatedUserId, UserName = entity.CreatedUserName };
            return initiator;
        }

        /// <summary>
        /// Get the current process running instance
        /// 获取当前的流程运行实例
        /// </summary>
        internal ProcessInstanceEntity GetProcessInstanceCurrent(String appInstanceId,
            String processId,
            String version)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInstanceCurrent(session.Connection, appInstanceId, processId, version, session.Transaction);
            }
        }

        /// <summary>
        /// Get the current process running instance
        /// 获取当前的流程运行实例
        /// </summary>
        internal ProcessInstanceEntity GetProcessInstanceCurrent(IDbConnection conn,
            String appInstanceId,
            String processId,
            String version,
            IDbTransaction trans)
        {
            ProcessInstanceEntity entity = null;
            var processInstanceList = GetProcessInstance(conn, appInstanceId, processId, version, trans).ToList();
            if (processInstanceList != null && processInstanceList.Count > 0)
            {
                entity = processInstanceList[0];
            }
            return entity;
        }

        /// <summary>
        /// Query process instances based on application instance IDs
        /// Explanation: The appInstanceId here only determines uniqueness if it is of type UUID. Otherwise, ProcessGUID is added to determine uniqueness
        /// 根据应用实例ID查询流程实例
        /// 说明：此处appInstanceId 只有GUID类型，才确定唯一性，否则就加入ProcessID来确定唯一性
        /// </summary>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(String appInstanceId)
        {
            //var list = Repository.GetAll<ProcessInstanceEntity>()
            //            .Where<ProcessInstanceEntity>(
            //                p => p.AppInstanceId == appInstanceId &&
            //                    p.RecordStatusInvalid == 0)
            //            .OrderByDescending(p => p.Id)
            //            .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance 
                        WHERE app_instance_id=@appInstanceId 
                            AND record_status_invalid = 0 
                        ORDER BY id DESC";

            var list = Repository.Query<ProcessInstanceEntity>(
                        sql,
                        new
                        {
                            appInstanceId = appInstanceId
                        });

            return list;
        }

        /// <summary>
        /// Query process instance
        /// 查询流程实例
        /// </summary>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(IDbConnection conn,
            String appInstanceId,
            String processId,
            String version,
            IDbTransaction trans)
        {
            //var list = Repository.GetAll<ProcessInstanceEntity>(conn, trans)
            //    .Where<ProcessInstanceEntity>(
            //        p => p.AppInstanceId == appInstanceId &&
            //            p.Version == version &&
            //            p.RecordStatusInvalid == 0)
            //    .OrderByDescending(p => p.Id)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance 
                        WHERE app_instance_id=@appInstanceId 
                            AND process_id=@processId 
                            AND version=@version 
                            AND record_status_invalid = 0
                        ORDER BY id DESC";
            var list = Repository.Query<ProcessInstanceEntity>(conn,
            sql,
            new
            {
                appInstanceId = appInstanceId,
                processId = processId,
                version = version
            },
            trans);

            return list;
        }

        /// <summary>
        /// Obtain sub process data
        /// 获取子流程数据
        /// </summary>
        internal ProcessInstanceEntity GetSubProcessInstance(IDbConnection conn,
            String appInstanceId,
            String processId,
            String subProcessId,
            IDbTransaction trans)
        {
            //var list = Repository.GetAll<ProcessInstanceEntity>(conn, trans)
            //    .Where<ProcessInstanceEntity>(
            //        p => p.AppInstanceId == appInstanceId &&
            //            p.ProcessId == processId &&
            //            p.SubProcessId == subProcessId &&
            //            p.RecordStatusInvalid == 0)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance 
                        WHERE app_instance_id=@appInstanceId 
                            AND process_id=@processId 
                            AND sub_process_id=@subProcessId 
                            AND record_status_invalid = 0
                        ORDER BY id DESC";
            var list = Repository.Query<ProcessInstanceEntity>(conn,
            sql,
            new
            {
                appInstanceId = appInstanceId,
                processId = processId,
                subProcessId = subProcessId
            },
            trans).ToList();

            if (list.Count() == 1)
            {
                return list[0];
            }
            else if (list.Count() == 0)
            {
                return null;
            }
            else
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("processinstancemanager.GetSubProcessInstance.error"));
            }
        }

        /// <summary>
        /// Get process instances in running state
        /// 获取处于运行状态的流程实例
        /// </summary>
        internal ProcessInstanceEntity GetRunningProcessInstance(string appInstanceId,
            string processId)
        {
            //var list = Repository.GetAll<ProcessInstanceEntity>()
            //    .Where<ProcessInstanceEntity>(
            //        p => p.AppInstanceId == appInstanceId &&
            //            p.ProcessId == processId &&
            //            (p.ProcessState == 1 || p.ProcessState == 2) &&
            //            p.RecordStatusInvalid == 0)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance
                        WHERE app_instance_id=@appInstanceId 
                            AND process_id=@processId 
                            AND record_status_invalid=0
                            AND (process_state=1 OR process_state=2)
                        ORDER BY id DESC";
            var list = Repository.Query<ProcessInstanceEntity>(
                sql,
                new
                {
                    appInstanceId = appInstanceId,
                    processId = processId
                }).ToList();


            var entity = EnumHelper.GetFirst<ProcessInstanceEntity>(list);
            return entity;
        }

        /// <summary>
        /// Obtain the number of process instances
        /// 获取流程实例的数目
        /// </summary>
        internal Int32 GetProcessInstanceCount(string processId, string version)
        {
            //var count = Repository.GetAll<ProcessInstanceEntity>()
            //    .Where<ProcessInstanceEntity>(
            //        p => p.ProcessId == processId &&
            //            p.Version == version &&
            //            p.RecordStatusInvalid == 0)
            //    .Count();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance
                        WHERE process_id=@processId
                            AND version=@version";
            var parameters = new DynamicParameters();
            parameters.Add("@processId", processId);
            parameters.Add("@version", version);
            var count = Repository.Count(sql, parameters);

            return count;
        }

        /// <summary>
        /// Check if the sub process has ended
        /// 检查子流程是否结束
        /// </summary>
        internal bool CheckSubProcessInstanceCompleted(IDbConnection conn,
            int activityInstanceId,
            String activityId,
            IDbTransaction trans)
        {
            bool isCompleted = false;
            //var list = Repository.GetAll<ProcessInstanceEntity>(conn, trans)
            //    .Where<ProcessInstanceEntity>(
            //        p => p.InvokedActivityInstanceId == activityInstanceId &&
            //            p.InvokedActivityId == activityId &&
            //            p.ProcessState == 4 &&
            //            p.RecordStatusInvalid == 0)
            //    .OrderByDescending(p => p.Id)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance
                        WHERE invoked_activity_instance_id=@invokedActivityInstanceId 
                            AND invoked_activity_id=@invokedActivityId 
                            AND record_status_invalid=0
                            AND process_state=4
                        ORDER BY created_dateTime DESC";
            var list = Repository.Query<ProcessInstanceEntity>(
                    conn,
                    sql,
                    new
                    {
                        invokedActivityInstanceId = activityInstanceId,
                        invokedActivityId = activityId
                    },
                    trans).ToList();


            if (list.Count == 1)
            {
                isCompleted = true;
            }

            return isCompleted;
        }


        /// <summary>
        /// Insert process instance
        /// 流程数据插入
        /// </summary>
        internal Int32 Insert(IDbConnection conn, ProcessInstanceEntity entity, IDbTransaction trans)
        {
            int newId = Repository.Insert(conn, entity, trans);
            entity.Id = newId;
            return newId;
        }

        /// <summary>
        /// Update process instance
        /// 流程实例更新
        /// </summary>
        internal void Update(ProcessInstanceEntity entity, 
            IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// Update process instance
        /// 流程实例更新
        /// </summary>
        internal void Update(ProcessInstanceEntity entity)
        {
            Repository.Update<ProcessInstanceEntity>(entity);
        }

        /// <summary>
        /// Update process instance
        /// 流程数据更新
        /// </summary>
        internal void Update(IDbConnection conn, ProcessInstanceEntity entity, IDbTransaction trans)
        {
            Repository.Update<ProcessInstanceEntity>(conn, entity, trans);
        }

        /// <summary>
        /// Create a new process instance according to the process definition
        /// 根据流程定义，创建新的流程实例
        /// </summary>
        internal ProcessInstanceEntity CreateNewProcessInstanceObject(WfAppRunner runner,
            ProcessEntity processEntity,
            SubProcessNode subProcessNode = null)
        {
            ProcessInstanceEntity entity = new ProcessInstanceEntity();
            entity.ProcessId = processEntity.ProcessId;
            entity.ProcessName = processEntity.ProcessName;
            entity.Version = processEntity.Version;
            entity.AppName = runner.AppName;
            entity.AppInstanceId = runner.AppInstanceId;
            entity.AppInstanceCode = runner.AppInstanceCode;
            entity.ProcessState = (int)ProcessStateEnum.Running;
            if (subProcessNode != null)
            {
                entity.SubProcessId = subProcessNode.SubProcessId;
                entity.InvokedActivityId = subProcessNode.ActivityInstance.ActivityId;
                entity.InvokedActivityInstanceId = subProcessNode.ActivityInstance.Id;
                if (subProcessNode.SubProcessNested != null)
                {
                    //内嵌子流程，子流程的流程实例记录保存以主流程记录为主
                    //内嵌子流程没有SubProcessID属性
                    //Embedded sub processes, the process instance records of sub processes are mainly saved based on the main process records
                    //The embedded subprocess does not have the SubProcessId attribute
                    entity.ProcessId = subProcessNode.ActivityInstance.ProcessId;
                    entity.SubProcessType = (short)SubProcessTypeEnum.Nested;
                }
                else
                {
                    //外部引用子流程在wf_rocess表中有记录存在
                    //There are records of external reference subprocesses in the wf_process table
                    entity.SubProcessType = (short)SubProcessTypeEnum.Referenced;
                    entity.SubProcessDefId = subProcessNode.SubProcessDefId;
                }
            }
            entity.CreatedUserId = runner.UserId;
            entity.CreatedUserName = runner.UserName;
            entity.CreatedDateTime = System.DateTime.UtcNow;

            //过期时间设置
            //Expiration time setting
            if (processEntity.EndType == (short)ProcessEndTypeEnum.Timer)
            {
                entity.OverdueDateTime = CalculateOverdueDateTime(entity.CreatedDateTime, processEntity.EndExpression);
            }
            entity.UpdatedUserId = runner.UserId;
            entity.UpdatedUserName = runner.UserName;
            entity.UpdatedDateTime = System.DateTime.UtcNow;
           
            return entity;
        }

        /// <summary>
        /// Calculate the expiration time of process instances
        /// 计算流程实例过期时间
        /// XmlConvert.ToTimeSpan()
        /// https://stackoverflow.com/questions/12466188/how-do-i-convert-an-iso8601-timespan-to-a-c-sharp-timespan
        /// </summary>
        private DateTime CalculateOverdueDateTime(DateTime current, string expression)
        {
            var timeSpan = System.Xml.XmlConvert.ToTimeSpan(expression);
            var overdueDaeTime = current.Add(timeSpan);
            return overdueDaeTime;
        }
        #endregion

        #region Process business rule processing 流程业务规则处理
        /// <summary>
        /// Process completed, set the status of the process to complete
        /// 流程完成，设置流程的状态为完成
        /// </summary>
        internal ProcessInstanceEntity Complete(int processInstanceId, 
            WfAppRunner runner, 
            IDbSession session)
        {
            var processInstance = GetById(processInstanceId);
            var processState = (ProcessStateEnum)Enum.Parse(typeof(ProcessStateEnum), processInstance.ProcessState.ToString());
            if ((processState | ProcessStateEnum.Running) == ProcessStateEnum.Running)
            {
                processInstance.ProcessState = (short)ProcessStateEnum.Completed;
                processInstance.EndedDateTime = System.DateTime.UtcNow;
                processInstance.EndedUserId = runner.UserId;
                processInstance.EndedUserName = runner.UserName;

                Update(processInstance, session);
            }
            else
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.complete.error"));
            }
            return processInstance;
        }

        /// <summary>
        /// Suspend process instance
        /// 挂起流程实例
        /// </summary>
        internal void Suspend(int processInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            var bEntity = GetById(processInstanceId);
            var processState = (ProcessStateEnum)Enum.Parse(typeof(ProcessStateEnum), bEntity.ProcessState.ToString());
            if ((processState | ProcessStateEnum.Running) == ProcessStateEnum.Running)
            {
                bEntity.ProcessState = (short)ProcessStateEnum.Suspended;
                bEntity.UpdatedDateTime = System.DateTime.UtcNow;
                bEntity.UpdatedUserId = runner.UserId;
                bEntity.UpdatedUserName = runner.UserName;

                Update(bEntity, session);
            }
            else
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.suspend.error"));
            }
        }

        /// <summary>
        /// Resume process instance
        /// 恢复流程实例
        /// </summary>
        internal void Resume(int processInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            var bEntity = GetById(processInstanceId);
            var processState = (ProcessStateEnum)Enum.Parse(typeof(ProcessStateEnum), bEntity.ProcessState.ToString());
            if ((processState | ProcessStateEnum.Suspended) == ProcessStateEnum.Suspended)
            {
                bEntity.ProcessState = (short)ProcessStateEnum.Running;
                bEntity.UpdatedDateTime = System.DateTime.UtcNow;
                bEntity.UpdatedUserId = runner.UserId;
                bEntity.UpdatedUserName = runner.UserName;

                Update(bEntity, session);
            }
            else
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.resume.error"));
            }
        }
        /// <summary>
        /// Recall sub process
        /// 恢复子流程
        /// </summary>
        internal void RecallSubProcess(int invokedActivityInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            //var list = Repository.GetAll<ProcessInstanceEntity>(session.Connection, session.Transaction)
            //    .Where<ProcessInstanceEntity>(
            //        p => p.InvokedActivityInstanceId == invokedActivityInstanceId &&
            //            p.ProcessState == 5)
            //    .OrderByDescending(p => p.Id)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_process_instance
                        WHERE invoked_activity_anstance_id=@invokedActivityInstanceId 
                            AND process_state=5
                        ORDER BY id DESC";
            var list = Repository.Query<ProcessInstanceEntity>(
                   session.Connection,
                   sql,
                   new
                   {
                       invokedActivityInstanceId = invokedActivityInstanceId
                   },
                   session.Transaction).ToList();

            if (list != null && list.Count() == 1)
            {
                var bEntity = list[0];

                bEntity.ProcessState = (short)ProcessStateEnum.Running;
                bEntity.UpdatedDateTime = System.DateTime.UtcNow;
                bEntity.UpdatedUserId = runner.UserId;
                bEntity.UpdatedUserName = runner.UserName;

                Update(bEntity, session);
            }
            else
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.recallsubprocess.error"));
            }
        }

        /// <summary>
        /// Reverse process, set the process status to reverse and modify the process incomplete flag
        /// 返签流程，将流程状态置为返签，并修改流程未完成标志
        /// </summary>
        internal void Reverse(int processInstanceId, 
            WfAppRunner currentUser, 
            IDbSession session)
        {
            var bEntity = GetById(processInstanceId);
            if (bEntity.ProcessState == (short)ProcessStateEnum.Completed)
            {
                bEntity.ProcessState = (short)ProcessStateEnum.Running;
                bEntity.UpdatedUserId = currentUser.UserId;
                bEntity.UpdatedUserName = currentUser.UserName;
                bEntity.UpdatedDateTime = System.DateTime.UtcNow;
                
                Update(bEntity, session);
            }
            else
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.reverse.error"));
            }
        }

        /// <summary>
        /// Cancel process instance
        /// 流程的取消操作
        /// </summary>
        internal bool Cancel(WfAppRunner runner, IDbConnection conn = null)
        {
            bool isCanceled = false;

            if (conn == null)
            {
                conn = SessionFactory.CreateConnection();
            }
            try
            {
                var entity = GetProcessInstanceLatest(runner.AppInstanceId, 
                    runner.ProcessId,
                    runner.Version);

                if (entity == null || entity.ProcessState != (short)ProcessStateEnum.Running)
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("processinstancemanager.cancel.error"));
                }

                IDbSession session = SessionFactory.CreateSession();
                entity.ProcessState = (short)ProcessStateEnum.Canceled;
                entity.RecordStatusInvalid = 1;
                entity.UpdatedUserId = runner.UserId;
                entity.UpdatedUserName = runner.UserName;
                entity.UpdatedDateTime = System.DateTime.UtcNow;

                Update(entity, session);

                isCanceled = true;
            }
            catch (System.Exception e)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("processinstancemanager.cancel.error", e.Message));
            }
            finally
            {
                conn.Close();
            }
            return isCanceled;
        }

        /// <summary>
        /// Discard process instance
        /// 废弃单据下所有流程的信息
        /// </summary>
        internal bool Discard(WfAppRunner runner)
        {
            var isDiscarded = false;
            IDbConnection conn = SessionFactory.CreateConnection();
            var transaction = conn.BeginTransaction();
            try
            {
                //process state:7--discard status
                //record status:1 --invalid status
                string updSql = @"UPDATE wf_process_instance
		                         SET process_state = 7, 
			                         record_status_invalid = 1,
			                         updated_datetime = @currentDate,
			                         updated_user_id = @userId,
			                         updated_user_name = @userName
		                        WHERE app_instance_id = @appInstanceId
			                        AND process_id = @processId
                                    AND version = @version";
                int result = Repository.Execute(conn, 
                    updSql, 
                    new{
                        appInstanceId = runner.AppInstanceId,
                        processId = runner.ProcessId,
                        version = runner.Version,
                        userId = runner.UserId,
                        userName = runner.UserName,
                        currentDate = System.DateTime.UtcNow
                    },
                transaction);
                transaction.Commit();
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("processinstancemanager.discard.error", e.Message));
            }
            finally
            {
                conn.Close();
            }
            return isDiscarded;
        }

        /// <summary>
        /// Terminate process instance
        /// 流程终止操作
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        internal bool Terminate(WfAppRunner runner)
        {
            var isTerminated = false;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetProcessInstanceLatest(session.Connection , runner.AppInstanceId, runner.ProcessId, runner.Version, session.Transaction);
                isTerminated = Terminate(session.Connection, entity, runner.UserId, runner.UserName, session.Transaction);
                session.Commit();
            }
            catch (System.Exception ex)
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.terminate.error", ex.Message));
            }
            finally
            {
                session.Dispose();
            }
            return isTerminated;
        }

        /// <summary>
        /// Terminate process instance
        /// 终结流程实例
        /// </summary>
        internal bool Terminate(IDbConnection conn, ProcessInstanceEntity entity, string userId, 
            string userName, IDbTransaction trans)
        {
            var isTerminated = false;
            if (entity.ProcessState == (int)ProcessStateEnum.Running
                || entity.ProcessState == (int)ProcessStateEnum.Ready
                || entity.ProcessState == (int)ProcessStateEnum.Suspended)
            {
                entity.ProcessState = (short)ProcessStateEnum.Terminated;
                entity.EndedUserId =userId;
                entity.EndedUserName = userName;
                entity.EndedDateTime = DateTime.UtcNow;

                isTerminated = Repository.Update<ProcessInstanceEntity>(conn, entity, trans);
            }
            else
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.terminate.error"));
            }
            return isTerminated;
        }

        /// <summary>
        /// Set process overdue time
        /// 设置流程过期时间
        /// </summary>
        internal bool SetOverdue(int processInstanceId, DateTime overdueDateTime, WfAppRunner runner)
        {
            ProcessInstanceEntity entity = Repository.GetById<ProcessInstanceEntity>(processInstanceId);

            if (entity.ProcessState == (int)ProcessStateEnum.Running
                || entity.ProcessState == (int)ProcessStateEnum.Ready
                || entity.ProcessState == (int)ProcessStateEnum.Suspended)
            {
                entity.OverdueDateTime = overdueDateTime;
                entity.UpdatedUserId = runner.UserId;
                entity.UpdatedUserName = runner.UserName;
                entity.UpdatedDateTime = DateTime.UtcNow;

                Repository.Update<ProcessInstanceEntity>(entity);

                return true;
            }
            else
            {
                throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.setoverdue.error"));
            }
        }

        /// <summary>
        /// Delete abnormal process instances 
        /// Notes:the process can only be deleted when it is in a cancelled state!
        /// Or, delete it forcedly
        /// 删除流程实例
        /// 备注:流程在取消状态，才可以进行删除！, 或者强制删除
        /// </summary>
        internal bool Delete(int processInstanceId, bool isForced = false)
        {
            bool isDeleted = false;
            IDbSession session = SessionFactory.CreateSession();

            try
            {
                session.BeginTrans();
                var entity = Repository.GetById<ProcessInstanceEntity>(processInstanceId);

                if (entity.ProcessState == (int)ProcessStateEnum.Canceled
                    || isForced == true)
                {
                    //delete tasks
                    var sqlTasks = @"DELETE 
                                    FROM wf_task
                                    WHERE process_instance_id=@processInstanceId";
                    Repository.Execute(session.Connection, sqlTasks,
                        new { processInstanceId = processInstanceId },
                        session.Transaction);

                    //delete transitioninstance
                    var sqlTransitionInstance = @"DELETE 
                                                FROM wf_transition_instance 
                                                WHERE process_instance_id=@processInstanceId";
                    Repository.Execute(session.Connection, sqlTransitionInstance,
                        new { processInstanceId = processInstanceId },
                        session.Transaction);

                    //delete activityinstance
                    var sqlActivityInstance = @"DELETE 
                                                FROM wf_activity_instance 
                                                WHERE process_instance_id=@processInstanceId";
                    Repository.Execute(session.Connection, sqlActivityInstance,
                        new { processInstanceId = processInstanceId },
                        session.Transaction);

                    //delete process variable
                    var sqlProcessVariable = @"DELETE
                                               FROM wf_process_variable
                                               WHERE process_instance_id=@processInstanceId";
                    Repository.Execute(session.Connection, sqlProcessVariable,
                        new { processInstanceId = processInstanceId },
                        session.Transaction);

                    //delete processinstance
                    Repository.Delete<ProcessInstanceEntity>(session.Connection, processInstanceId, session.Transaction);

                    session.Commit();
                    isDeleted = true;
                }
                else
                {
                    throw new ProcessInstanceException(LocalizeHelper.GetEngineMessage("processinstancemanager.delete.error"));
                }
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }

            return isDeleted;
        }

        /// <summary>
        /// Delete process instance
        /// 删除流程实例
        /// </summary>
        internal bool Delete(string processId, string version)
        {
            var entity = GetByVersion(processId, version);
            if (entity != null)
            {
                return Delete(entity.Id, true);
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
