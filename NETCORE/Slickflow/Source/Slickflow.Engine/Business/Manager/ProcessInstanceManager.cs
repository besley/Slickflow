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
using System.Diagnostics;
using System.Text.RegularExpressions;
using Dapper;
using DapperExtensions;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
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
            var processInstanceList = Repository.Query<ProcessInstanceEntity>(sql,
                    new
                    {
                        processGUID = processGUID,
                        version = version
                    }).ToList();
            var entity = EnumHelper.GetFirst<ProcessInstanceEntity>(processInstanceList);

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

            return Repository.Query<ProcessInstanceEntity>(
                        sql,
                        new
                        {
                            appInstanceID = appInstanceID
                        });
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
                            AND RecordStatusInvalid = 0
                        ORDER BY CreatedDateTime DESC";

            return Repository.Query<ProcessInstanceEntity>(conn,
                        sql,
                        new {
                            appInstanceID = appInstanceID, 
                            processGUID = processGUID
                        },
                        trans);
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
                        FROM WfProcessInstance
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
        /// <param name="trans">事务</param>
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
        /// 根据流程定义，创建新的流程实例
        /// </summary>
        /// <param name="runner">运行者</param>
        /// <param name="processEntity">流程定义</param>
        /// <param name="parentProcessInstance">父流程实例</param>
        /// <param name="subProcessNode">子流程节点</param>
        /// <returns>流程实例的ID</returns>
        internal ProcessInstanceEntity CreateNewProcessInstanceObject(WfAppRunner runner,
            ProcessEntity processEntity,
            ProcessInstanceEntity parentProcessInstance = null,
            ActivityInstanceEntity subProcessNode = null)
        {
            ProcessInstanceEntity entity = new ProcessInstanceEntity();
            entity.ProcessGUID = processEntity.ProcessGUID;
            entity.ProcessName = processEntity.ProcessName;
            entity.Version = processEntity.Version;
            entity.AppName = runner.AppName;
            entity.AppInstanceID = runner.AppInstanceID;
            entity.AppInstanceCode = runner.AppInstanceCode;
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
                throw new ProcessInstanceException("流程不在运行状态，不能被完成！");
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
        /// <param name="invokedActivityInstanceID">激活活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
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
                throw new ProcessInstanceException("流程不在运行状态，不能被完成！");
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
        /// <param name="runner">运行者</param>
        /// <param name="conn">数据库链接</param>
        /// <returns>设置成功标识</returns>
        internal bool Discard(WfAppRunner runner, IDbConnection conn=null)
        {
            var isDiscarded = false;
            if (conn == null)
            {
                conn = SessionFactory.CreateConnection();
            }

            var transaction = conn.BeginTransaction();
            try
            {
                string updSql = @"UPDATE WfProcessInstance
		                         SET [ProcessState] = 7, --废弃状态
			                         [RecordStatusInvalid] = 1, --设置记录为无效状态
			                         [LastUpdatedDateTime] = GETDATE(),
			                         [LastUpdatedByUserID] = @userID,
			                         [LastUpdatedByUserName] = @userName
		                        WHERE AppInstanceID = @appInstanceID
			                        AND ProcessGUID = @processGUID";

                int result = Repository.Execute(conn, updSql, new
                    {
                        appInstanceID = runner.AppInstanceID,
                        processGUID = runner.ProcessGUID,
                        userID = runner.UserID,
                        userName = runner.UserName
                    },
                transaction);
                transaction.Commit();

                //返回结果大于0，表示有记录更新
                if (result > 0)
                {
                    isDiscarded = true;
                }
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
                throw new WorkflowException(string.Format("执行废弃流程的操作失败，错误原因：{0}", e.Message));
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
                entity.OverdueDateTime = overdueDateTime;
                entity.LastUpdatedByUserID = runner.UserID;
                entity.LastUpdatedByUserName = runner.UserName;
                entity.LastUpdatedDateTime = DateTime.Now;

                Repository.Update<ProcessInstanceEntity>(entity);

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
