/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using DapperExtensions;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Module.Resource;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程实例管理者类
    /// </summary>
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
        /// 根据ID获取流程实例数据
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetById(IDbConnection conn, int processInstanceID, IDbTransaction trans)
        {
            return Repository.GetById<ProcessInstanceEntity>(conn, processInstanceID, trans);
        }

        /// <summary>
        /// 根据活动实例查询流程实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetByActivity(int activityInstanceID)
        {
            var activityInstance = Repository.GetById<ActivityInstanceEntity>(activityInstanceID);
            var processInstance = Repository.GetById<ProcessInstanceEntity>(activityInstance.ProcessInstanceID);
            return processInstance;
        }

        /// <summary>
        /// 获取最近的流程运行实例
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetProcessInstanceCurrent(String appInstanceID,
            String processGUID)
        {
            ProcessInstanceEntity entity = null;
            var processInstanceList = GetProcessInstance(appInstanceID, processGUID).ToList();
            if (processInstanceList != null && processInstanceList.Count > 0)
            {
                entity = processInstanceList[0];
            }
            return entity;
        }

        /// <summary>
        /// 根据流程完成状态获取流程实例数据
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(String appInstanceID,
            String processGUID)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND RecordStatusInvalid = 0
                        ORDER BY CreatedDateTime DESC";

            return Repository.Query<ProcessInstanceEntity>(
                        sql,
                        new {
                            appInstanceID = appInstanceID, 
                            processGUID = processGUID
                        });
        }

        /// <summary>
        /// 获取流程的发起人
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>流程发起人</returns>
        internal User GetProcessInitiator(int processInstanceID)
        {
            var entity = GetById(processInstanceID);
            var initiator = new User { UserID = entity.CreatedByUserID, UserName = entity.CreatedByUserName };
            return initiator;
        }

        /// <summary>
        /// 获取最近的流程运行实例
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        internal ProcessInstanceEntity GetProcessInstanceLatest(String appInstanceID,
            String processGUID)
        {
            ProcessInstanceEntity entity = null;
            var processInstanceList = GetProcessInstance(appInstanceID, processGUID).ToList();
            if (processInstanceList != null && processInstanceList.Count > 0)
            {
                entity = processInstanceList[0];
            }
            return entity;
        }

        /// <summary>
        /// 获取处于运行状态的流程实例
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetRunningProcessInstance(IDbConnection conn, 
            string appInstanceID,
            string processGUID)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance WITH(NOLOCK)
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND RecordStatusInvalid=0
                            AND (ProcessState=1 OR ProcessState=2)
                        ORDER BY CreatedDateTime DESC";

            var list = Repository.Query<ProcessInstanceEntity>(
                    conn,
                    sql,
                    new
                    {
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
        /// 判断流程实例是否存在
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="processGUID">流程GUId</param>
        /// <param name="version">版本</param>
        /// <returns>流程实例记录数</returns>
        internal Int32 GetProcessInstanceCount(IDbConnection conn, string processGUID, string version)
        {
            var sql = @"SELECT 
                            COUNT(1) 
                        FROM WfProcessInstance
                        WHERE ProcessGUID=@processGUID
                            AND Version=@version";
            var parameters = new DynamicParameters();
            parameters.Add("@processGUID", processGUID);
            parameters.Add("@version", version);

            return Repository.Count(sql, parameters);
        }

        /// <summary>
        /// 检查子流程是否结束
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>是否结束标志</returns>
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
        /// <param name="conn">链接</param>
        /// <param name="entity">流程实例实体</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        internal Int32 Insert(IDbConnection conn, ProcessInstanceEntity entity, IDbTransaction trans)
        {
            int newID = Repository.Insert(conn, entity, trans);
            entity.ID = newID;

            return newID;
        }

        /// <summary>
        /// 流程实例更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="session">会话</param>
        internal void Update(ProcessInstanceEntity entity, 
            IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// 根据流程定义，创建新的流程实例
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="processEntity">流程定义</param>
        /// <param name="parentProcessInstance">父流程实例</param>
        /// <param name="subProcessNode">子流程节点</param>
        /// <returns>流程实例的ID</returns>
        internal ProcessInstanceEntity CreateNewProcessInstanceObject(WfAppRunner runner,
            ProcessEntity processEntity,
            ProcessInstanceEntity parentProcessInstance,
            ActivityInstanceEntity subProcessNode)
        {
            ProcessInstanceEntity entity = new ProcessInstanceEntity();
            entity.ProcessGUID = processEntity.ProcessGUID;
            entity.ProcessName = processEntity.ProcessName;
            entity.Version = processEntity.Version;
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
                throw new ProcessInstanceException("流程不在挂起状态，不能被完成！");
            }
        }
        /// <summary>
        /// 恢复子流程
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void RecallSubProcess(int invokedActivityInstanceID,
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
                var entity = GetProcessInstanceLatest(runner.AppInstanceID, 
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
        /// <param name="runner">执行者</param>
        /// <returns>设置成功标识</returns>
        internal bool Terminate(WfAppRunner runner)
        {
            var entity = GetProcessInstanceLatest(runner.AppInstanceID, runner.ProcessGUID);

            if (entity.ProcessState == (int)ProcessStateEnum.Running
                || entity.ProcessState == (int)ProcessStateEnum.Ready
                || entity.ProcessState == (int)ProcessStateEnum.Suspended)
            {
                entity.ProcessState = (short)ProcessStateEnum.Terminated;
                entity.EndedByUserID = runner.UserID;
                entity.EndedByUserName = runner.UserName;
                entity.EndedDateTime = DateTime.Now;

                Repository.Update<ProcessInstanceEntity>(entity);

                return true;
            }
            else
            {
                throw new ProcessInstanceException("流程已经结束，或者已经被取消！");
            }
        }

        /// <summary>
        /// 设置流程过期时间
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="overdueDateTime">过期时间</param>
        /// <param name="runner">当前运行用户</param>
        /// <returns>设置成功标识</returns>
        internal bool SetOverdue(int processInstanceID, DateTime overdueDateTime, WfAppRunner runner)
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
