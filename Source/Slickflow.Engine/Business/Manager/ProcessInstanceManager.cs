using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Business.Manager
{
    internal class ProcessInstanceManager : ManagerBase
    {
        #region ProcessInstanceManager 基本数据操作
        /// <summary>
        /// 根据GUID获取流程实例数据
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetById(int processInstanceID)
        {
            return Repository.GetById<ProcessInstanceEntity>(processInstanceID);
        }

        /// <summary>
        /// 根据流程完成状态获取流程实例数据
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(String appName,
            String appInstanceID,
            String processGUID)
        {
            return Repository.Query<ProcessInstanceEntity>(
                        @"SELECT * FROM WfProcessInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND RecordStatusInvalid = 0
                        ORDER BY CreatedDateTime DESC",
                        new { appName = appName, 
                            appInstanceID = appInstanceID, 
                            processGUID = processGUID
                        });
        }

        /// <summary>
        /// 获取流程的发起人
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        internal Performer GetProcessInitiator(int processInstanceID)
        {
            var entity = GetById(processInstanceID);
            Performer performer = new Performer(entity.CreatedByUserID, entity.CreatedByUserName);
            return performer;
        }

        /// <summary>
        /// 获取最近的流程运行实例
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetProcessInstanceLatest(String appName,
            String appInstanceID,
            String processGUID)
        {
            ProcessInstanceEntity entity = null;
            var processInstanceList = GetProcessInstance(appName, appInstanceID, processGUID).ToList();
            if (processInstanceList != null && processInstanceList.Count > 0)
            {
                entity = processInstanceList[0];
            }
            return entity;
        }

        internal ProcessInstanceEntity GetRunningProcessInstance(IDbConnection conn, 
            string appName,
            string appInstanceID,
            string processGUID)
        {
            var list = Repository.Query<ProcessInstanceEntity>(
                    conn,
                    @"SELECT * FROM WfProcessInstance, 
                                WHERE AppInstanceID=@appInstanceID 
                                    AND ProcessGUID=@processGUID 
                                    AND IsProcessComplted=0 
                                    AND RecordStatusInvalid=0
                                    AND (ProcessState=1 OR ProcessState=2)
                                ORDER BY CreatedDateTime DESC",
                    new
                    {
                        appName = appName,
                        appInstanceID = appInstanceID,
                        processGUID = processGUID
                    }).ToList();

            if (list.Count == 1)
            {
                return list[0];
            } 
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 检查子流程是否结束
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        internal bool CheckSubProcessInstanceCompleted(IDbConnection conn,
            int activityInstanceID,
            String activityGUID,
            IDbTransaction trans)
        {
            bool isCompleted = false;
            var list = Repository.Query<ProcessInstanceEntity>(
                    conn,
                    @"SELECT * FROM WfProcessInstance
                                WHERE InvokedActivityInstanceID=@invokedActivityInstanceID 
                                    AND InvokedActivityGUID=@invokedActivityGUID 
                                    AND RecordStatusInvalid=0
                                    AND ProcessState=4
                                ORDER BY CreatedDateTime DESC",
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
        /// 流程数据插入
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wfLinqDataContext"></param>
        /// <returns></returns>
        internal void Insert(IDbConnection conn, ProcessInstanceEntity entity, IDbTransaction trans)
        {
            int newID = Repository.Insert(conn, entity, trans);
            entity.ID = newID;

            Debug.WriteLine(string.Format("process instance inserted, Guid:{0}, time:{1}", entity.ID.ToString(), System.DateTime.Now.ToString()));
        }

        /// <summary>
        /// 流程实例更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wfLinqDataContext"></param>
        internal void Update(ProcessInstanceEntity entity, 
            IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// 根据流程定义，创建新的流程实例
        /// </summary>
        /// <param name="processID">流程定义ID</param>
        /// <returns>流程实例的ID</returns>
        internal ProcessInstanceEntity CreateNewProcessInstanceObject(WfAppRunner runner,
            ProcessEntity processEntity,
            ProcessInstanceEntity parentProcessInstance,
            ActivityInstanceEntity subProcessNode)
        {
            ProcessInstanceEntity entity = new ProcessInstanceEntity();
            entity.ProcessGUID = processEntity.ProcessGUID;
            entity.ProcessName = processEntity.ProcessName;
            entity.AppName = runner.AppName;
            entity.AppInstanceID = runner.AppInstanceID;
            entity.ProcessState = (int)ProcessStateEnum.Running;
            if (parentProcessInstance != null)
            {
                //流程的Parent信息
                entity.ParentProcessInstanceID = parentProcessInstance.ID;
                entity.ParentProcessGUID = parentProcessInstance.ProcessGUID;
                entity.InvokedActivityInstanceID = subProcessNode.ID;
                entity.InvokedActivityGUID = subProcessNode.ActivityGUID;
            }
            entity.CreatedByUserID = runner.UserID;
            entity.CreatedByUserName = runner.UserName;
            entity.CreatedDateTime = System.DateTime.Now;
            entity.LastUpdatedByUserID = runner.UserID;
            entity.LastUpdatedByUserName = runner.UserName;
            entity.LastUpdatedDateTime = System.DateTime.Now;
           
            return entity;
        }
        #endregion

        #region 流程业务规则处理
        /// <summary>
        /// 流程完成，设置流程的状态为完成
        /// </summary>
        /// <returns>是否成功</returns>
        internal void Complete(int processInstanceID, 
            WfAppRunner runner, 
            IDbSession session)
        {
            var bEntity = GetById(processInstanceID);
            var processState = (ProcessStateEnum)Enum.Parse(typeof(ProcessStateEnum), bEntity.ProcessState.ToString());
            if ((processState | ProcessStateEnum.Running) == ProcessStateEnum.Running)
            {
                bEntity.ProcessState = (short)ProcessStateEnum.Completed;
                bEntity.EndedDateTime = System.DateTime.Now;
                bEntity.EndedByUserID = runner.UserID;
                bEntity.EndedByUserName = runner.UserName;

                Update(bEntity, session);
            }
            else
            {
                throw new ProcessInstanceException("流程不在运行状态，不能被完成！");
            }
        }

        /// <summary>
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
                throw new ProcessInstanceException("流程不在运行状态，不能被完成！");
            }
        }

        /// <summary>
        /// 恢复流程实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Resume(int invokedActivityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            var list = Repository.Query<ProcessInstanceEntity>(
                   session.Connection,
                   @"SELECT * FROM WfProcessInstance
                                WHERE InvokedActivityInstanceID=@invokedActivityInstanceID 
                                    AND ProcessState=5
                                ORDER BY CreatedDateTime DESC",
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
                throw new ProcessInstanceException("流程不在挂起状态，不能被完成！");
            }
        }


        /// <summary>
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
                throw new ProcessInstanceException("流程不在运行状态，不能被完成！");
            }
        }

        /// <summary>
        /// 流程的取消操作
        /// </summary>
        /// <returns>是否成功</returns>
        internal bool Cancel(WfAppRunner runner, IDbConnection conn = null)
        {
            bool isCanceled = false;

            if (conn == null)
            {
                conn = SessionFactory.CreateConnection();
            }
            try
            {
                var entity = GetProcessInstanceLatest(runner.AppName,
                    runner.AppInstanceID, 
                    runner.ProcessGUID);

                if (entity == null || entity.ProcessState != (short)ProcessStateEnum.Running)
                {
                    throw new WorkflowException("无法取消流程，错误原因：当前没有运行中的流程实例，或者同时有多个运行中的流程实例（不合法数据）!");
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
                throw new WorkflowException(string.Format("取消流程实例失败，错误原因：{0}", e.Message));
            }
            finally
            {
                conn.Close();
            }
            return isCanceled;
        }

        /// <summary>
        /// 废弃单据下所有流程的信息
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal bool Discard(WfAppRunner runner, IDbConnection conn=null)
        {
            var isDiscarded = false;
            if (conn == null)
            {
                conn = SessionFactory.CreateConnection();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {
                try
                {
                    string updSql = @"UPDATE WfProcessInstance
		                         SET [ProcessState] = 7, --废弃状态
			                         [RecordStatusInvalid] = 1, --设置记录为无效状态
			                         [LastUpdatedDateTime] = GETDATE(),
			                         [LastUpdatedByUserID] = @userID,
			                         [LastUpdatedByUserName] = @userName
		                        WHERE AppInstanceID = @appInstanceID
			                        AND ProcessGUID = @processGUID
			                        AND ProcessState <> 32";

                    cmd.CommandText = updSql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = conn.BeginTransaction();

                    cmd.Parameters.Add(new SqlParameter("@userID", runner.UserID));
                    cmd.Parameters.Add(new SqlParameter("@userName", runner.UserName));
                    cmd.Parameters.Add(new SqlParameter("@appInstanceID", runner.AppInstanceID));
                    cmd.Parameters.Add(new SqlParameter("@processGUID", runner.ProcessGUID));

                    int result = Repository.ExecuteCommand(cmd);
                    cmd.Transaction.Commit();

                    //返回结果大于0，表示有记录更新
                    if (result > 0)
                    {
                        isDiscarded = true;
                    }
                }
                catch (System.Exception e)
                {
                    cmd.Transaction.Rollback();
                    throw new WorkflowException(string.Format("执行废弃流程的操作失败，错误原因：{0}", e.Message));
                }
                finally
                {
                    conn.Close();
                }
            }
            return isDiscarded;
        }

        /// <summary>
        /// 流程终止操作
        /// </summary>
        /// <returns></returns>
        internal bool Terminate(int processInstanceID)
        {
            ProcessInstanceEntity entity = Repository.GetById<ProcessInstanceEntity>(processInstanceID);

            if (entity.ProcessState == (int)ProcessStateEnum.Running
                || entity.ProcessState == (int)ProcessStateEnum.Ready
                || entity.ProcessState == (int)ProcessStateEnum.Suspended)
            {

                return true;
            }
            else
            {
                throw new ProcessInstanceException("流程已经结束，或者已经被取消！");
            }
        }

        /// <summary>
        /// 删除不正常的流程实例（流程在取消状态，才可以进行删除！）
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        internal bool Delete(int processInstanceID)
        {
            bool isDeleted = false;
            IDbSession session = SessionFactory.CreateSession();

            try
            {
                ProcessInstanceEntity entity = Repository.GetById<ProcessInstanceEntity>(processInstanceID);

                if (entity.ProcessState == (int)ProcessStateEnum.Canceled)
                {
                    Repository.Delete<ProcessInstanceEntity>(session.Connection, processInstanceID, session.Transaction);
                    session.Commit();
                    isDeleted = true;
                }
                else
                {
                    throw new ProcessInstanceException("流程只有先取消，才可以删除！");
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
        #endregion

    }
}
