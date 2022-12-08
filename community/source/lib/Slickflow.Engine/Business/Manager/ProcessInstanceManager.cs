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
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Module.Resource;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程实例管理者类
    /// </summary>
    internal class ProcessInstanceManager : ManagerBase
    {
        #region ProcessInstanceManager 基本数据操作
        /// <summary>
        /// 获取当前运行的流程实例
        /// </summary>
        /// <param name="appInstanceID">应用ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetProcessInstanceLatest(String appInstanceID,
           String processGUID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInstanceCurrent(session.Connection, appInstanceID, processGUID, session.Transaction);
            }
        }

        /// <summary>
        /// 获取当前运行的流程实例
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="appInstanceID">应用ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetProcessInstanceLatest(IDbConnection conn,
           String appInstanceID,
           String processGUID,
           IDbTransaction trans)
        {
            return GetProcessInstanceCurrent(conn, appInstanceID, processGUID, trans);
        }

        /// <summary>
        /// 根据GUID获取流程实例数据
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <returns>流程实例实体</returns>
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
        /// 获取流程实例
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程实例</returns>
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
        /// 根据活动实例查询流程实例
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetByActivity(IDbConnection conn,
            int activityInstanceID,
            IDbTransaction trans)
        {
            var activityInstance = Repository.GetById<ActivityInstanceEntity>(conn, activityInstanceID, trans);
            var processInstance = Repository.GetById<ProcessInstanceEntity>(conn, activityInstance.ProcessInstanceID, trans);

            return processInstance;
        }

        /// <summary>
        /// 判断流程实例是否为子流程
        /// </summary>
        /// <param name="entity">流程实例</param>
        /// <returns>子流程标志</returns>
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
        /// 获取流程的发起人
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>流程发起人</returns>
        internal User GetProcessInitiator(int processInstanceID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInitiator(session.Connection, processInstanceID, session.Transaction);
            }
        }

        /// <summary>
        /// 获取流程的发起人
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程发起人</returns>
        internal User GetProcessInitiator(IDbConnection conn,
            int processInstanceID,
            IDbTransaction trans)
        {
            var entity = GetById(processInstanceID);
            var initiator = new User { UserID = entity.CreatedByUserID, UserName = entity.CreatedByUserName };
            return initiator;
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
            using (var session = SessionFactory.CreateSession())
            {
                return GetProcessInstanceCurrent(session.Connection, appInstanceID, processGUID, session.Transaction);
            }
        }

        /// <summary>
        /// 获取最近的流程运行实例
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="trans">事务</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetProcessInstanceCurrent(IDbConnection conn,
            String appInstanceID,
            String processGUID,
            IDbTransaction trans)
        {
            ProcessInstanceEntity entity = null;
            var processInstanceList = GetProcessInstance(conn, appInstanceID, processGUID, trans).ToList();
            if (processInstanceList != null && processInstanceList.Count > 0)
            {
                entity = processInstanceList[0];
            }
            return entity;
        }

        /// <summary>
        /// 根据业务实例ID查询流程实例
        /// 说明：此处appInstanceID 只有GUID类型，才确定唯一性，否则就加入ProcessGUID来确定唯一性
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <returns>流程实例实体</returns>
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
        /// 根据流程完成状态获取流程实例数据
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="trans">事务</param>
        /// <returns>流程实例列表</returns>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(IDbConnection conn,
            String appInstanceID,
            String processGUID,
            IDbTransaction trans)
        {
            var sql = @"SELECT 
                            * 
                        FROM WfProcessInstance 
                        WHERE AppInstanceID=@appInstanceID 
                            AND ProcessGUID=@processGUID 
                            AND SubProcessGUID IS NULL  
                            AND RecordStatusInvalid = 0
                        ORDER BY CreatedDateTime DESC";
            var list = Repository.Query<ProcessInstanceEntity>(conn,
                        sql,
                        new
                        {
                            appInstanceID = appInstanceID,
                            processGUID = processGUID
                        },
                        trans);
            return list;
        }

        /// <summary>
        /// 获取子流程数据
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="subProcessGUID"></param>
        /// <param name="trans">事务</param>
        /// <returns>流程实例列表</returns>
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
        /// 获取处于运行状态的流程实例
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程实例实体</returns>
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
        /// 判断流程实例是否存在
        /// </summary>
        /// <param name="processGUID">流程GUId</param>
        /// <param name="version">版本</param>
        /// <returns>流程实例记录数</returns>
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
        /// 检查子流程是否结束
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="trans">事务</param>
        /// <returns>是否结束标志</returns>
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
        /// 流程数据插入
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">流程实例实体</param>
        /// <param name="trans">事务</param>
        /// <returns>新实例ID</returns>
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
        /// 流程实例更新
        /// </summary>
        /// <param name="entity">实体</param>
        internal void Update(ProcessInstanceEntity entity)
        {
            Repository.Update<ProcessInstanceEntity>(entity);
        }

        /// <summary>
        /// 流程数据插入
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">流程实例实体</param>
        /// <param name="trans">事务</param>
        /// <returns>新实例ID</returns>
        internal void Update(IDbConnection conn, ProcessInstanceEntity entity, IDbTransaction trans)
        {
            Repository.Update<ProcessInstanceEntity>(conn, entity, trans);
        }

        /// <summary>
        /// 根据流程定义，创建新的流程实例
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="processEntity">流程定义</param>
        /// <param name="subProcessNode">子流程节点</param>
        /// <returns>流程实例的ID</returns>
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
            }
            entity.CreatedByUserID = runner.UserID;
            entity.CreatedByUserName = runner.UserName;
            entity.CreatedDateTime = System.DateTime.Now;

            //过期时间设置
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
        /// 计算流程实例过期时间
        /// XmlConvert.ToTimeSpan()
        /// https://stackoverflow.com/questions/12466188/how-do-i-convert-an-iso8601-timespan-to-a-c-sharp-timespan
        /// </summary>
        /// <param name="current">当前时间</param>
        /// <param name="expression">表达式</param>
        /// <returns>过期时间</returns>
        private DateTime CalculateOverdueDateTime(DateTime current, string expression)
        {
            var timeSpan = System.Xml.XmlConvert.ToTimeSpan(expression);
            var overdueDaeTime = current.Add(timeSpan);
            return overdueDaeTime;
        }
        #endregion

        #region 流程业务规则处理
        /// <summary>
        /// 流程完成，设置流程的状态为完成
        /// </summary>
        /// <returns>是否成功</returns>
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
        /// 恢复流程实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
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
        /// 恢复子流程
        /// </summary>
        /// <param name="invokedActivityInstanceID">激活活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
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
        /// 返签流程，将流程状态置为返签，并修改流程未完成标志
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="currentUser">当前用户</param>
        /// <param name="session">会话</param>
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
        /// 流程的取消操作
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="conn">数据库链接</param>
        /// <returns>设置成功标识</returns>
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
        /// 废弃单据下所有流程的信息
        /// </summary>
        /// <param name="runner">运行者</param>

        /// <returns>设置成功标识</returns>
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
        /// 流程终止操作
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <returns>设置成功标识</returns>
        internal bool Terminate(WfAppRunner runner)
        {
            var isTerminated = false;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                var entity = GetProcessInstanceLatest(session.Connection , runner.AppInstanceID, runner.ProcessGUID, session.Transaction);
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
        /// 终结流程实例
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">流程实例</param>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        internal bool Terminate(IDbConnection conn, ProcessInstanceEntity entity, string userID, string userName, IDbTransaction trans)
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
        /// 删除不正常的流程实例（流程在取消状态，才可以进行删除！）
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="isForced">不取消，直接删除</param>
        /// <returns>是否删除标识</returns>
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
        /// 删除流程实例包括其关联数据
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>是否删除</returns>
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
