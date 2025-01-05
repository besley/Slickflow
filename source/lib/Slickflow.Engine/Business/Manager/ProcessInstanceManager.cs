
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using DapperExtensions;
using System.Threading;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Module.Resource;
using Slickflow.Engine.Xpdl.Node;
using DnsClient.Protocol;
using Org.BouncyCastle.Asn1;

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
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetProcessInstanceLatest(String appInstanceID,
           String processGUID,
           String version)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInstanceCurrent(session.Connection, appInstanceID, processGUID, version, session.Transaction);
            }
        }

        /// <summary>
        /// Get the current running process instance latest
        /// 获取当前运行的流程实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetProcessInstanceLatest(IDbConnection conn,
           String appInstanceID,
           String processGUID,
           String version,
           IDbTransaction trans)
        {
            return GetProcessInstanceCurrent(conn, appInstanceID, processGUID, version, trans);
        }

        /// <summary>
        /// Obtain process instance data based on ID
        /// 根据ID获取流程实例数据
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetById(int processInstanceID)
        {
            return Repository.GetById<ProcessInstanceEntity>(processInstanceID);
        }

        /// <summary>
        /// Obtain process instance data based on ID
        /// 根据ID获取流程实例数据
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetById(IDbConnection conn, int processInstanceID, IDbTransaction trans)
        {
            return Repository.GetById<ProcessInstanceEntity>(conn, processInstanceID, trans);
        }

        /// <summary>
        /// Obtain process instance data by Version
        /// 根据版本获取流程实例
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private ProcessInstanceEntity GetByVersion(string processGUID, 
            string version)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance
                        WHERE ProcessGUID=@processGUID
                            AND Version=@version
                        ORDER BY ID DESC";
            var list = Repository.Query<ProcessInstanceEntity>(sql,
                    new
                    {
                        processGUID = processGUID,
                        version = version
                    }).ToList();
            var entity = EnumHelper.GetFirst<ProcessInstanceEntity>(list);
            return entity;
        }

        /// <summary>
        ///  Obtain process instance data by activity instance id
        /// 根据活动实例ID查询流程实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetByActivity(IDbConnection conn,
            int activityInstanceID,
            IDbTransaction trans)
        {
            var activityInstance = Repository.GetById<ActivityInstanceEntity>(conn, activityInstanceID, trans);
            var processInstance = Repository.GetById<ProcessInstanceEntity>(conn, activityInstance.ProcessInstanceID, trans);

            return processInstance;
        }

        /// <summary>
        /// Determine whether the process instance is a subprocess
        /// 判断流程实例是否为子流程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal Boolean IsSubProcess(ProcessInstanceEntity entity)
        {
            var isSub = false;
            if (entity.InvokedActivityInstanceID > 0 
                && !string.IsNullOrEmpty(entity.InvokedActivityGUID))
            {
                isSub = true;
            }
            return isSub;
        }

        /// <summary>
        /// Obtain the initiator of the process
        /// 获取流程的发起人
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        internal User GetProcessInitiator(int processInstanceID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInitiator(session.Connection, processInstanceID, session.Transaction);
            }
        }

        /// <summary>
        /// Obtain the initiator of the process
        /// 获取流程的发起人
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal User GetProcessInitiator(IDbConnection conn,
            int processInstanceID,
            IDbTransaction trans)
        {
            var entity = GetById(processInstanceID);
            var initiator = new User { UserID = entity.CreatedByUserID, UserName = entity.CreatedByUserName };
            return initiator;
        }

        /// <summary>
        /// Get the current process running instance
        /// 获取当前的流程运行实例
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetProcessInstanceCurrent(String appInstanceID,
            String processGUID,
            String version)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInstanceCurrent(session.Connection, appInstanceID, processGUID, version, session.Transaction);
            }
        }

        /// <summary>
        /// Get the current process running instance
        /// 获取当前的流程运行实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetProcessInstanceCurrent(IDbConnection conn,
            String appInstanceID,
            String processGUID,
            String version,
            IDbTransaction trans)
        {
            ProcessInstanceEntity entity = null;
            var processInstanceList = GetProcessInstance(conn, appInstanceID, processGUID, version, trans).ToList();
            if (processInstanceList != null && processInstanceList.Count > 0)
            {
                entity = processInstanceList[0];
            }
            return entity;
        }

        /// <summary>
        /// Query process instances based on application instance IDs
        /// Explanation: The appInstanceID here only determines uniqueness if it is of type UUID. Otherwise, ProcessUID is added to determine uniqueness
        /// 根据应用实例ID查询流程实例
        /// 说明：此处appInstanceID 只有GUID类型，才确定唯一性，否则就加入ProcessGUID来确定唯一性
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <returns></returns>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(String appInstanceID)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND RecordStatusInvalid = 0 
                        ORDER BY CreatedDateTime DESC";

            var list = Repository.Query<ProcessInstanceEntity>(
                        sql,
                        new
                        {
                            appInstanceID = appInstanceID
                        });
            return list;
        }

        /// <summary>
        /// Query process instance
        /// 查询流程实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(IDbConnection conn,
            String appInstanceID,
            String processGUID,
            String version,
            IDbTransaction trans)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND Version=@version 
                            AND RecordStatusInvalid = 0
                        ORDER BY CreatedDateTime DESC";
            var list = Repository.Query<ProcessInstanceEntity>(conn,
                        sql,
                        new
                        {
                            appInstanceID = appInstanceID,
                            processGUID = processGUID,
                            version= version
                        },
                        trans);
            return list;
        }

        /// <summary>
        /// Obtain sub process data
        /// 获取子流程数据
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="subProcessGUID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetSubProcessInstance(IDbConnection conn,
            String appInstanceID,
            String processGUID,
            String subProcessGUID,
            IDbTransaction trans)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND SubProcessGUID=@subProcessGUID 
                            AND RecordStatusInvalid = 0
                        ORDER BY CreatedDateTime DESC";
            var list = Repository.Query<ProcessInstanceEntity>(conn,
                        sql,
                        new
                        {
                            appInstanceID = appInstanceID,
                            processGUID = processGUID,
                            subProcessGUID = subProcessGUID
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
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetRunningProcessInstance(string appInstanceID,
            string processGUID)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND RecordStatusInvalid=0
                            AND (ProcessState=1 OR ProcessState=2)
                        ORDER BY CreatedDateTime DESC";

            var list = Repository.Query<ProcessInstanceEntity>(
                    sql,
                    new
                    {
                        appInstanceID = appInstanceID,
                        processGUID = processGUID
                    }).ToList();
            var entity = EnumHelper.GetFirst<ProcessInstanceEntity>(list);
            return entity;
        }

        /// <summary>
        /// Obtain the number of process instances
        /// 获取流程实例的数目
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal Int32 GetProcessInstanceCount(string processGUID, string version)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance
                        WHERE ProcessGUID=@processGUID
                            AND Version=@version";
            var parameters = new DynamicParameters();
            parameters.Add("@processGUID", processGUID);
            parameters.Add("@version", version);
            var count = Repository.Count(sql, parameters);
            return count;
        }

        /// <summary>
        /// Check if the sub process has ended
        /// 检查子流程是否结束
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="activityGUID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal bool CheckSubProcessInstanceCompleted(IDbConnection conn,
            int activityInstanceID,
            String activityGUID,
            IDbTransaction trans)
        {
            bool isCompleted = false;
            var sql = @"SELECT * FROM WfProcessInstance
                                WHERE InvokedActivityInstanceID=@invokedActivityInstanceID 
                                    AND InvokedActivityGUID=@invokedActivityGUID 
                                    AND RecordStatusInvalid=0
                                    AND ProcessState=4
                                ORDER BY CreatedDateTime DESC";
            var list = Repository.Query<ProcessInstanceEntity>(
                    conn,
                    sql,
                    new
                    {
                        invokedActivityInstanceID = activityInstanceID,
                        invokedActivityGUID = activityGUID
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
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal Int32 Insert(IDbConnection conn, ProcessInstanceEntity entity, IDbTransaction trans)
        {
            int newID = Repository.Insert(conn, entity, trans);
            entity.ID = newID;
            return newID;
        }

        /// <summary>
        /// Update process instance
        /// 流程实例更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="session"></param>
        internal void Update(ProcessInstanceEntity entity, 
            IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// Update process instance
        /// 流程实例更新
        /// </summary>
        /// <param name="entity"></param>
        internal void Update(ProcessInstanceEntity entity)
        {
            Repository.Update<ProcessInstanceEntity>(entity);
        }

        /// <summary>
        /// Update process instance
        /// 流程数据更新
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal void Update(IDbConnection conn, ProcessInstanceEntity entity, IDbTransaction trans)
        {
            Repository.Update<ProcessInstanceEntity>(conn, entity, trans);
        }

        /// <summary>
        /// Create a new process instance according to the process definition
        /// 根据流程定义，创建新的流程实例
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="processEntity"></param>
        /// <param name="subProcessNode"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity CreateNewProcessInstanceObject(WfAppRunner runner,
            ProcessEntity processEntity,
            SubProcessNode subProcessNode = null)
        {
            ProcessInstanceEntity entity = new ProcessInstanceEntity();
            entity.ProcessGUID = processEntity.ProcessGUID;
            entity.ProcessName = processEntity.ProcessName;
            entity.Version = processEntity.Version;
            entity.AppName = runner.AppName;
            entity.AppInstanceID = runner.AppInstanceID;
            entity.AppInstanceCode = runner.AppInstanceCode;
            entity.ProcessState = (int)ProcessStateEnum.Running;
            if (subProcessNode != null)
            {
                entity.SubProcessGUID = subProcessNode.SubProcessGUID;
                entity.InvokedActivityGUID = subProcessNode.ActivityInstance.ActivityGUID;
                entity.InvokedActivityInstanceID = subProcessNode.ActivityInstance.ID;
                if (subProcessNode.SubProcessNested != null)
                {
                    //内嵌子流程，子流程的流程实例记录保存以主流程记录为主
                    //内嵌子流程没有SubProcessID属性
                    //Embedded sub processes, the process instance records of sub processes are mainly saved based on the main process records
                    //The embedded subprocess does not have the SubProcessID attribute
                    entity.ProcessGUID = subProcessNode.ActivityInstance.ProcessGUID;
                    entity.SubProcessType = (short)SubProcessTypeEnum.Nested;
                }
                else
                {
                    //外部引用子流程在WfProcess表中有记录存在
                    //There are records of external reference subprocesses in the WfProcess table
                    entity.SubProcessType = (short)SubProcessTypeEnum.Referenced;
                    entity.SubProcessID = subProcessNode.SubProcessID;
                }
            }
            entity.CreatedByUserID = runner.UserID;
            entity.CreatedByUserName = runner.UserName;
            entity.CreatedDateTime = System.DateTime.Now;

            //过期时间设置
            //Expiration time setting
            if (processEntity.EndType == (short)ProcessEndTypeEnum.Timer)
            {
                entity.OverdueDateTime = CalculateOverdueDateTime(entity.CreatedDateTime, processEntity.EndExpression);
            }
            entity.LastUpdatedByUserID = runner.UserID;
            entity.LastUpdatedByUserName = runner.UserName;
            entity.LastUpdatedDateTime = System.DateTime.Now;
           
            return entity;
        }

        /// <summary>
        /// Calculate the expiration time of process instances
        /// 计算流程实例过期时间
        /// XmlConvert.ToTimeSpan()
        /// https://stackoverflow.com/questions/12466188/how-do-i-convert-an-iso8601-timespan-to-a-c-sharp-timespan
        /// </summary>
        /// <param name="current"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        internal ProcessInstanceEntity Complete(int processInstanceID, 
            WfAppRunner runner, 
            IDbSession session)
        {
            var processInstance = GetById(processInstanceID);
            var processState = (ProcessStateEnum)Enum.Parse(typeof(ProcessStateEnum), processInstance.ProcessState.ToString());
            if ((processState | ProcessStateEnum.Running) == ProcessStateEnum.Running)
            {
                processInstance.ProcessState = (short)ProcessStateEnum.Completed;
                processInstance.EndedDateTime = System.DateTime.Now;
                processInstance.EndedByUserID = runner.UserID;
                processInstance.EndedByUserName = runner.UserName;

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
        /// <param name="processInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Suspend(int processInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            var bEntity = GetById(processInstanceID);
            var processState = (ProcessStateEnum)Enum.Parse(typeof(ProcessStateEnum), bEntity.ProcessState.ToString());
            if ((processState | ProcessStateEnum.Running) == ProcessStateEnum.Running)
            {
                bEntity.ProcessState = (short)ProcessStateEnum.Suspended;
                bEntity.LastUpdatedDateTime = System.DateTime.Now;
                bEntity.LastUpdatedByUserID = runner.UserID;
                bEntity.LastUpdatedByUserName = runner.UserName;

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
        /// <param name="processInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Resume(int processInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            var bEntity = GetById(processInstanceID);
            var processState = (ProcessStateEnum)Enum.Parse(typeof(ProcessStateEnum), bEntity.ProcessState.ToString());
            if ((processState | ProcessStateEnum.Suspended) == ProcessStateEnum.Suspended)
            {
                bEntity.ProcessState = (short)ProcessStateEnum.Running;
                bEntity.LastUpdatedDateTime = System.DateTime.Now;
                bEntity.LastUpdatedByUserID = runner.UserID;
                bEntity.LastUpdatedByUserName = runner.UserName;

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
        /// <param name="invokedActivityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void RecallSubProcess(int invokedActivityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            var sql = @"SELECT * FROM WfProcessInstance
                                WHERE InvokedActivityInstanceID=@invokedActivityInstanceID 
                                    AND ProcessState=5
                                ORDER BY CreatedDateTime DESC";
            var list = Repository.Query<ProcessInstanceEntity>(
                   session.Connection,
                   sql,
                   new
                   {
                       invokedActivityInstanceID = invokedActivityInstanceID
                   },
                   session.Transaction).ToList();


            if (list != null && list.Count() == 1)
            {
                var bEntity = list[0];

                bEntity.ProcessState = (short)ProcessStateEnum.Running;
                bEntity.LastUpdatedDateTime = System.DateTime.Now;
                bEntity.LastUpdatedByUserID = runner.UserID;
                bEntity.LastUpdatedByUserName = runner.UserName;

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
        /// <param name="processInstanceID"></param>
        /// <param name="currentUser"></param>
        /// <param name="session"></param>
        internal void Reverse(int processInstanceID, 
            WfAppRunner currentUser, 
            IDbSession session)
        {
            var bEntity = GetById(processInstanceID);
            if (bEntity.ProcessState == (short)ProcessStateEnum.Completed)
            {
                bEntity.ProcessState = (short)ProcessStateEnum.Running;
                bEntity.LastUpdatedByUserID = currentUser.UserID;
                bEntity.LastUpdatedByUserName = currentUser.UserName;
                bEntity.LastUpdatedDateTime = System.DateTime.Now;
                
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
        /// <param name="runner"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        internal bool Cancel(WfAppRunner runner, IDbConnection conn = null)
        {
            bool isCanceled = false;

            if (conn == null)
            {
                conn = SessionFactory.CreateConnection();
            }
            try
            {
                var entity = GetProcessInstanceLatest(runner.AppInstanceID, 
                    runner.ProcessGUID,
                    runner.Version);

                if (entity == null || entity.ProcessState != (short)ProcessStateEnum.Running)
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("processinstancemanager.cancel.error"));
                }

                IDbSession session = SessionFactory.CreateSession();
                entity.ProcessState = (short)ProcessStateEnum.Canceled;
                entity.RecordStatusInvalid = 1;
                entity.LastUpdatedByUserID = runner.UserID;
                entity.LastUpdatedByUserName = runner.UserName;
                entity.LastUpdatedDateTime = System.DateTime.Now;

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
        /// <param name="runner"></param>
        /// <returns></returns>
        internal bool Discard(WfAppRunner runner)
        {
            var isDiscarded = false;
            IDbConnection conn = SessionFactory.CreateConnection();
            var transaction = conn.BeginTransaction();
            try
            {
                //process state:7--discard status
                //record status:1 --invalid status
                string updSql = @"UPDATE WfProcessInstance
		                         SET [ProcessState] = 7, 
			                         [RecordStatusInvalid] = 1,
			                         [LastUpdatedDateTime] = @currentDate,
			                         [LastUpdatedByUserID] = @userID,
			                         [LastUpdatedByUserName] = @userName
		                        WHERE AppInstanceID = @appInstanceID
			                        AND ProcessGUID = @processGUID
                                    AND Version = @version";
                int result = Repository.Execute(conn, 
                    updSql, 
                    new{
                        appInstanceID = runner.AppInstanceID,
                        processGUID = runner.ProcessGUID,
                        version = runner.Version,
                        userID = runner.UserID,
                        userName = runner.UserName,
                        currentDate = System.DateTime.Now
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
                var entity = GetProcessInstanceLatest(session.Connection , runner.AppInstanceID, runner.ProcessGUID, runner.Version, session.Transaction);
                isTerminated = Terminate(session.Connection, entity, runner.UserID, runner.UserName, session.Transaction);
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
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal bool Terminate(IDbConnection conn, ProcessInstanceEntity entity, string userID, 
            string userName, IDbTransaction trans)
        {
            var isTerminated = false;
            if (entity.ProcessState == (int)ProcessStateEnum.Running
                || entity.ProcessState == (int)ProcessStateEnum.Ready
                || entity.ProcessState == (int)ProcessStateEnum.Suspended)
            {
                entity.ProcessState = (short)ProcessStateEnum.Terminated;
                entity.EndedByUserID =userID;
                entity.EndedByUserName = userName;
                entity.EndedDateTime = DateTime.Now;

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
        /// <param name="processInstanceID"></param>
        /// <param name="overdueDateTime"></param>
        /// <param name="runner"></param>
        /// <returns></returns>
        internal bool SetOverdue(int processInstanceID, DateTime overdueDateTime, WfAppRunner runner)
        {
            ProcessInstanceEntity entity = Repository.GetById<ProcessInstanceEntity>(processInstanceID);

            if (entity.ProcessState == (int)ProcessStateEnum.Running
                || entity.ProcessState == (int)ProcessStateEnum.Ready
                || entity.ProcessState == (int)ProcessStateEnum.Suspended)
            {
                entity.OverdueDateTime = overdueDateTime;
                entity.LastUpdatedByUserID = runner.UserID;
                entity.LastUpdatedByUserName = runner.UserName;
                entity.LastUpdatedDateTime = DateTime.Now;

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
        /// <param name="processInstanceID"></param>
        /// <param name="isForced"></param>
        /// <returns></returns>
        internal bool Delete(int processInstanceID, bool isForced = false)
        {
            bool isDeleted = false;
            IDbSession session = SessionFactory.CreateSession();

            try
            {
                session.BeginTrans();
                var entity = Repository.GetById<ProcessInstanceEntity>(processInstanceID);

                if (entity.ProcessState == (int)ProcessStateEnum.Canceled
                    || isForced == true)
                {
                    //delete tasks
                    var sqlTasks = @"DELETE 
                                    FROM WfTasks 
                                    WHERE ProcessInstanceID=@processInstanceID";
                    Repository.Execute(session.Connection, sqlTasks,
                        new { processInstanceID = processInstanceID },
                        session.Transaction);

                    //delete transitioninstance
                    var sqlTransitionInstance = @"DELETE 
                                                FROM WfTransitionInstance 
                                                WHERE ProcessInstanceID=@processInstanceID";
                    Repository.Execute(session.Connection, sqlTransitionInstance,
                        new { processInstanceID = processInstanceID },
                        session.Transaction);

                    //delete activityinstance
                    var sqlActivityInstance = @"DELETE 
                                                FROM WfActivityInstance 
                                                WHERE ProcessInstanceID=@processInstanceID";
                    Repository.Execute(session.Connection, sqlActivityInstance,
                        new { processInstanceID = processInstanceID },
                        session.Transaction);

                    //delete process variable
                    var sqlProcessVariable = @"DELETE
                                               FROM WfProcessVariable
                                               WHERE ProcessInstanceID=@processInstanceID";
                    Repository.Execute(session.Connection, sqlProcessVariable,
                        new { processInstanceID = processInstanceID },
                        session.Transaction);

                    //delete processinstance
                    Repository.Delete<ProcessInstanceEntity>(session.Connection, processInstanceID, session.Transaction);

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
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal bool Delete(string processGUID, string version)
        {
            var entity = GetByVersion(processGUID, version);
            if (entity != null)
            {
                return Delete(entity.ID, true);
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
