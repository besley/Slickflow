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
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Module.Resource;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 流程实例管理者类
    /// </summary>
    internal class ProcessInstanceManager
    {
        #region ProcessInstanceManager 基本数据操作
		/// <summary>
        /// 获取最近的流程运行实例
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>流程实例</returns>
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
        /// 根据GUID获取流程实例数据
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>流程实例</returns>
        internal ProcessInstanceEntity GetById(int processInstanceID)
        {
            using (var session = DbFactory.CreateSession())
            {
                return session.GetRepository<ProcessInstanceEntity>().GetByID(processInstanceID);
            }
        }

        /// <summary>
        /// 根据活动实例查询流程实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetByActivity(int activityInstanceID)
        {
            using (var session = DbFactory.CreateSession())
            {
                var activityInstance = session.GetRepository<ActivityInstanceEntity>().GetByID(activityInstanceID);
                var processInstance = session.GetRepository<ProcessInstanceEntity>().GetByID(activityInstance.ProcessInstanceID);
                return processInstance;
            }
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
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        internal IEnumerable<ProcessInstanceEntity> GetProcessInstance(String appInstanceID,
            String processGUID)
        {
            //var sql = @"SELECT 
            //                * 
            //            FROM WfProcessInstance 
            //            WHERE AppInstanceID=@appInstanceID 
            //                AND ProcessGUID=@processGUID 
            //                AND RecordStatusInvalid = 0
            //            ORDER BY CreatedDateTime DESC";
            using (var session = DbFactory.CreateSession())
            {
                var instanceList = session.GetRepository<ProcessInstanceEntity>().Query(e => e.AppInstanceID == appInstanceID
                    && e.ProcessGUID == processGUID
                    && e.RecordStatusInvalid == 0)
                    .OrderByDescending(e => e.CreatedDateTime)
                    .ToList();
                return instanceList;
            }
        }
        /// <summary>
        /// 获取处于运行状态的流程实例
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="session">会话</param>
        /// <returns>流程实例实体</returns>
        internal ProcessInstanceEntity GetRunningProcessInstance(string appInstanceID,
            string processGUID,
            IDbSession session)
        {
            //var sql = @"SELECT 
            //                * 
            //            FROM WfProcessInstance
            //            WHERE AppInstanceID=@appInstanceID 
            //                AND ProcessGUID=@processGUID 
            //                AND RecordStatusInvalid=0
            //                AND (ProcessState=1 OR ProcessState=2)
            //            ORDER BY CreatedDateTime DESC";
            ProcessInstanceEntity entity = null;
            var instanceList = session.GetRepository<ProcessInstanceEntity>().Query(e => e.AppInstanceID == appInstanceID
                    && e.ProcessGUID == processGUID
                    && e.RecordStatusInvalid == 0
                    && (e.ProcessState == 1 || e.ProcessState == 2))
                    .OrderByDescending(e => e.CreatedDateTime)
                    .ToList();
            if (instanceList != null && instanceList.Count() == 1)
            {
                entity = instanceList[0];
            }
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
            //var sql = @"SELECT 
            //                COUNT(1) 
            //            FROM WfProcessInstance
            //            WHERE ProcessGUID=@processGUID
            //                AND Version=@version";
            using (var session = DbFactory.CreateSession())
            {
                var count = session.GetRepository<ProcessInstanceEntity>().Count(e => e.ProcessGUID == processGUID
                    && e.Version == version);
                return count;
            }
        }

        /// <summary>
        /// 检查子流程是否结束
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="session">会话</param>
        /// <returns>是否结束标志</returns>
        internal bool CheckSubProcessInstanceCompleted(int activityInstanceID,
            String activityGUID,
            IDbSession session)
        {
            bool isCompleted = false;
            var list = session.GetRepository<ProcessInstanceEntity>().Query(e => e.InvokedActivityInstanceID == activityInstanceID
                && e.InvokedActivityGUID == activityGUID
                && e.RecordStatusInvalid == 0
                && e.ProcessState == 4)
                .OrderByDescending(e => e.CreatedDateTime)
                .ToList();
            if (list != null && list.Count() == 1) isCompleted = true;

            return isCompleted;
        }


        /// <summary>
        /// 流程数据插入
        /// </summary>
        /// <param name="entity">流程实例实体</param>
        /// <param name="session">会话</param>
        /// <returns>新实例ID</returns>
        internal Int32 Insert( ProcessInstanceEntity entity, 
            IDbSession session)
        {
            var newEntity = session.GetRepository<ProcessInstanceEntity>().Insert(entity);
            session.SaveChanges();

            return newEntity.ID;
        }

        /// <summary>
        /// 流程实例更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="session">数据上下文</param>
        internal void Update(ProcessInstanceEntity entity, 
            IDbSession session)
        {
            session.GetRepository<ProcessInstanceEntity>().Update(entity);
            session.SaveChanges();
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
            entity.ProcessState = (int)ProcessStateEnum.Running;
            if (parentProcessInstance != null)
            {
                //流程的Parent信息
                entity.ParentProcessInstanceID = parentProcessInstance.ID;
                entity.ParentProcessGUID = parentProcessInstance.ProcessGUID;
                entity.InvokedActivityInstanceID = subProcessNode.ID;
                entity.InvokedActivityGUID = subProcessNode.ActivityGUID;
            }

            //过期时间设置
            if (processEntity.EndType == (short)EventTriggerEnum.Timer) {
                entity.OverdueDateTime = CalculateOverdueDateTime(processEntity.EndExpression);
            }
            entity.CreatedByUserID = runner.UserID;
            entity.CreatedByUserName = runner.UserName;
            entity.CreatedDateTime = System.DateTime.Now;
            entity.LastUpdatedByUserID = runner.UserID;
            entity.LastUpdatedByUserName = runner.UserName;
            entity.LastUpdatedDateTime = System.DateTime.Now;
            entity.EndedDateTime = (DateTime?)null;

            return entity;
        }

        /// <summary>
        /// 过期时间计算
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>过期时间</returns>
        private DateTime CalculateOverdueDateTime(string expression)
        {
            var newDate = System.DateTime.Now;
            foreach (Match part in Regex.Matches(expression, @"[0-9]+M|[0-9]+D|[0-9]+H|[0-9]+m|"))
            {
                if (part.Success)
                {
                    Match m = Regex.Match(part.Value, @"[0-9]+");
                    if (m.Success)
                    {
                        var type = part.Value.Substring(part.Value.Length - 1);
                        if (type == "M") newDate = newDate.AddMonths(int.Parse(m.Value));
                        if (type == "D") newDate = newDate.AddDays(int.Parse(m.Value));
                        if (type == "H") newDate = newDate.AddHours(int.Parse(m.Value));
                        if (type == "m") newDate = newDate.AddMinutes(int.Parse(m.Value));
                    }
                }
            }
            return newDate;
        }
        #endregion

        #region 流程业务规则处理
        /// <summary>
        /// 流程完成，设置流程的状态为完成
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">数据上下文</param>
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
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">运行者</param>
        internal void Suspend(int processInstanceID,
            WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                Suspend(processInstanceID, runner, session);
                session.SaveChanges();
            }
        }

        /// <summary>
        /// 挂起流程实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
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
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="dbContext">数据上下文</param>
        /// <returns>标识</returns>
        internal Boolean Resume(int processInstanceID,
            WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
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
                    session.SaveChanges();
                    return true;
                }
                else
                {
                    throw new ProcessInstanceException("流程不在挂起状态，不能被完成！");
                }
            }
        }
        /// <summary>
        /// 恢复子流程
        /// </summary>
        /// <param name="invokedActivityInstanceID">激活活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">数据上下文</param>
        internal void RecallSubProcess(int invokedActivityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            //var sql = @"SELECT * FROM WfProcessInstance
            //                    WHERE InvokedActivityInstanceID=@invokedActivityInstanceID 
            //                        AND ProcessState=5
            //                    ORDER BY CreatedDateTime DESC",
            var list = session.GetRepository<ProcessInstanceEntity>().Query(e => e.InvokedActivityInstanceID == invokedActivityInstanceID
                && e.ProcessState == 5)
                .OrderByDescending(e => e.CreatedDateTime)
                .ToList();

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
        /// <param name="runner">运行者</param>
        /// <param name="session">数据上下文</param>
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
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">执行者</param>
        /// <returns>设置成功标识</returns>
        internal bool Cancel(int processInstanceID, WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessInstanceEntity>();
                var entity = repository.GetByID(processInstanceID);

                if (entity.ProcessState == (int)ProcessStateEnum.Running
                    || entity.ProcessState == (int)ProcessStateEnum.Ready
                    || entity.ProcessState == (int)ProcessStateEnum.Suspended)
                {
                    entity.ProcessState = (short)ProcessStateEnum.Canceled;
                    entity.LastUpdatedByUserID = runner.UserID;
                    entity.LastUpdatedByUserName = runner.UserName;
                    entity.LastUpdatedDateTime = DateTime.Now;

                    repository.Update(entity);
                    session.SaveChanges();

                    return true;
                }
                else
                {
                    throw new ProcessInstanceException("流程已经结束，或者已经被取消！");
                }
            }
        }

        /// <summary>
        /// 废弃单据下所有流程的信息
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">执行者</param>
        /// <returns>设置成功标识</returns>
        internal bool Discard(int processInstanceID, WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessInstanceEntity>();
                var entity = repository.GetByID(processInstanceID);

                if (entity.ProcessState == (int)ProcessStateEnum.Running
                    || entity.ProcessState == (int)ProcessStateEnum.Ready
                    || entity.ProcessState == (int)ProcessStateEnum.Suspended)
                {
                    entity.ProcessState = (short)ProcessStateEnum.Discarded;
                    entity.RecordStatusInvalid = 1;
                    entity.LastUpdatedByUserID = runner.UserID;
                    entity.LastUpdatedByUserName = runner.UserName;
                    entity.LastUpdatedDateTime = DateTime.Now;

                    repository.Update(entity);
                    session.SaveChanges();

                    return true;
                }
                else
                {
                    throw new ProcessInstanceException("流程已经结束，或者已经被取消！");
                }
            }
        }

        /// <summary>
        /// 流程终止操作
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="runner">执行者</param>
        /// <returns>设置成功标识</returns>
        internal bool Terminate(int processInstanceID, WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessInstanceEntity>();
                var entity = repository.GetByID(processInstanceID);

                if (entity.ProcessState == (int)ProcessStateEnum.Running
                    || entity.ProcessState == (int)ProcessStateEnum.Ready
                    || entity.ProcessState == (int)ProcessStateEnum.Suspended)
                {
                    entity.ProcessState = (short)ProcessStateEnum.Terminated;
                    entity.EndedByUserID = runner.UserID;
                    entity.EndedByUserName = runner.UserName;
                    entity.EndedDateTime = DateTime.Now;

                    repository.Update(entity);
                    session.SaveChanges();

                    return true;
                }
                else
                {
                    throw new ProcessInstanceException("流程已经结束，或者已经被取消！");
                }
            }
        }

        /// <summary>
        /// 设置流程过期时间
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="overdueDateTime">过期时间</param>
        /// <param name="runner">当前运行用户</param>
        /// <returns>设置成功标识</returns>
        internal bool SetOverdue(int processInstanceID, 
            DateTime overdueDateTime, 
            WfAppRunner runner)
        {
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessInstanceEntity>();
                var entity = repository.GetByID(processInstanceID);
                if (entity.ProcessState == (int)ProcessStateEnum.Running
                    || entity.ProcessState == (int)ProcessStateEnum.Ready
                    || entity.ProcessState == (int)ProcessStateEnum.Suspended)
                {
                    entity.OverdueDateTime = overdueDateTime;
                    entity.LastUpdatedByUserID = runner.UserID;
                    entity.LastUpdatedByUserName = runner.UserName;
                    entity.LastUpdatedDateTime = DateTime.Now;

                    repository.Update(entity);
                    session.SaveChanges();

                    return true;
                }
                else
                {
                    throw new ProcessInstanceException("流程已经结束，或者已经被取消！");
                }
            }
        }

        /// <summary>
        /// 删除不正常的流程实例（流程在取消状态，才可以进行删除！）
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        internal void Delete(int processInstanceID)
        {
            using (var session = DbFactory.CreateSession())
            {
                var repository = session.GetRepository<ProcessInstanceEntity>();
                var entity = repository.GetByID(processInstanceID);
                if (entity.ProcessState == (int)ProcessStateEnum.Canceled)
                {
                    repository.Delete(entity);
                    session.SaveChanges();
                }
                else
                {
                    throw new ProcessInstanceException("流程只有先取消，才可以删除！");
                }
            }
        }
        #endregion

    }
}
