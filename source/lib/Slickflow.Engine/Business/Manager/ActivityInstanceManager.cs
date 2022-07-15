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
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// 活动实例管理类
    /// </summary>
    internal class ActivityInstanceManager : ManagerBase
    {
        #region 构造函数
        internal ActivityInstanceManager()
        {
        }
        #endregion

        #region 活动实例数据获取
        /// <summary>
        /// 根据ID获取活动实例
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetById(int activityInstanceID)
        {
            try
            {
                return Repository.GetById<ActivityInstanceEntity>(activityInstanceID);
            }
            catch (System.Exception e)
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("activityinstancemanager.getbyid.error", e.Message),
                    e);
            }
        }

        /// <summary>
        /// 根据ID获取活动实例
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="trans">事务</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetById(IDbConnection conn, int activityInstanceID, IDbTransaction trans)
        {
            return Repository.GetById<ActivityInstanceEntity>(conn, activityInstanceID, trans);
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                //如果流程在运行状态，则返回运行时信息
                TaskViewEntity task = null;
                var entity = GetRunningNode(runner, session, out task);

                return entity;
            }
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="taskView">任务视图</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetRunningNode(WfAppRunner runner,
            out TaskViewEntity taskView)
        {
            taskView = null;
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var entity = GetRunningNode(runner, session, out taskView);
                return entity;
            }
        }

        /// <summary>
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner">执行者</param>
        /// <param name="session">数据库会话</param>
        /// <param name="taskView">任务视图</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetRunningNode(WfAppRunner runner,
            IDbSession session,
            out TaskViewEntity taskView)
        {
            var taskID = runner.TaskID;
            taskView = null;    //default value;

            //根据TaskID获取
            var aim = new ActivityInstanceManager();
            var tm = new TaskManager();

            ActivityInstanceEntity runningNode = null;
            if (taskID != null && taskID.Value > 0)
            {
                taskView = tm.GetTaskView(session.Connection, taskID.Value, session.Transaction);
                runningNode = aim.GetById(session.Connection, taskView.ActivityInstanceID, session.Transaction);
                return runningNode;
            }

            //没有传递TaskID参数，进行查询
            var activityInstanceList = aim.GetRunningActivityInstanceList(runner.AppInstanceID, runner.ProcessGUID, runner.Version, session).ToList();
            if (activityInstanceList == null || activityInstanceList.Count == 0)
            {
                //当前没有运行状态的节点存在，流程不存在，或者已经结束或取消
                var message = LocalizeHelper.GetEngineMessage("activityinstancemanager.getrunningnode.error");
                var e = new WorkflowException(message);
                LogManager.RecordLog(message, LogEventType.Exception, LogPriority.Normal, null, e);
                throw e;
            }

            if (activityInstanceList.Count == 1)
            {
                runningNode = activityInstanceList[0];
                taskView = tm.GetTaskOfMine(session.Connection, runningNode.ID, runner.UserID, session.Transaction);
            }
            else if (activityInstanceList.Count > 1)
            {
                //并行模式处理
                //根据当前执行者身份取出(他或她)要办理的活动实例（并行模式下有多个处于待办或运行状态的节点）
                foreach (var ai in activityInstanceList)
                {
                    if (ai.AssignedToUserIDs == runner.UserID)
                    {
                        runningNode = ai;
                        break;
                    }
                }

                if (runningNode != null)
                {
                    //获取taskview
                    taskView = tm.GetTaskOfMine(session.Connection, runningNode.ID, runner.UserID, session.Transaction);
                }
                else
                {
                    //当前用户的待办任务不唯一，抛出异常，需要TaskID唯一界定
                    var msgException = LocalizeHelper.GetEngineMessage("activityinstancemanager.getrunningnode.unique.error");
                    var e = new WorkflowException(msgException);
                    LogManager.RecordLog(LocalizeHelper.GetEngineMessage("activityinstancemanager.getrunningnode.error"),
                        LogEventType.Exception, LogPriority.Normal, null, e);
                    throw e;
                }
            }
            return runningNode;
        }

        /// <summary>
        /// 获取退回源活动实例
        /// </summary>
        /// <param name="currentActivityInstanceID">当前活动实例</param>
        /// <returns>退回源活动实例</returns>
        internal ActivityInstanceEntity GetBackSrcActivityInstance(int currentActivityInstanceID)
        {
            var currentActivityInstance = GetById(currentActivityInstanceID);
            var backSrcActivityInstance = GetById(currentActivityInstance.BackSrcActivityInstanceID.Value);

            return backSrcActivityInstance;
        }

        /// <summary>
        /// 判断是否是某个用户的办理任务
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        internal bool IsMineTask(ActivityInstanceEntity entity,
            string userID)
        {
            bool isMine = entity.AssignedToUserIDs.Contains(userID.ToString());
            return isMine;
        }

        /// <summary>
        /// 判断节点是否处于运行状态
        /// </summary>
        /// <param name="activityInstance">活动实例</param>
        /// <returns>是否标志</returns>
        internal Boolean IncludeRunningState(ActivityInstanceEntity activityInstance)
        {
            var isIncluded = false;
            if (activityInstance.ActivityState == (short)ActivityStateEnum.Ready
                || activityInstance.ActivityState == (short)ActivityStateEnum.Running
                || activityInstance.ActivityState == (short)ActivityStateEnum.Suspended)
            {
                isIncluded = true;
            }
            return isIncluded;
        }

        /// <summary>
        /// 获取活动节点实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal IList<ActivityInstanceEntity> GetActivityInstances(int processInstanceID,
            IDbSession session)
        {
            var sql = @"SELECT * FROM WfActivityInstance 
                        WHERE ProcessInstanceID = @processInstanceID 
                            ORDER BY ID";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceID = processInstanceID
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// 获取活动节点实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetActivityInstanceLatest(int processInstanceID,
            string activityGUID)
        {
            ActivityInstanceEntity activityInstance = null;
            var session = SessionFactory.CreateSession();
            try
            {
                activityInstance = GetActivityInstanceLatest(processInstanceID, activityGUID, session);
            }
            catch
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
            return activityInstance;
        }

        /// <summary>
        /// 获取最近的节点实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="session">数据会话</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetActivityInstanceLatest(int processInstanceID,
            string activityGUID,
            IDbSession session)
        {
            ActivityInstanceEntity activityInstance = null;
            var sql = @"SELECT * FROM WfActivityInstance 
                        WHERE ProcessInstanceID = @processInstanceID
                            AND ActivityGUID = @activityGUID
                            ORDER BY ID DESC";
            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceID = processInstanceID,
                    activityGUID = activityGUID
                },
                session.Transaction).ToList();

            if (list.Count > 0)
                activityInstance = list[0];

            return activityInstance;
        }

        /// <summary>
        /// 获取最近的节点实例
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetActivityInstanceLatest(string appInstanceID,
            string processGUID,
            string activityGUID)
        {
            ActivityInstanceEntity activityInstance = null;
            var sql = @"SELECT 
                                AI.* 
                            FROM WfActivityInstance AI
                            INNER JOIN WfProcessInstance PI 
                                ON AI.ProcessInstanceID = PI.ID
                            WHERE PI.ProcessState = 2 
                                AND AI.AppInstanceID = @appInstanceID 
                                AND AI.ProcessGUID = @processGUID
                                AND AI.ActivityGUID = @activityGUID";
            var list = Repository.Query<ActivityInstanceEntity>(
                sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID,
                    activityGUID = activityGUID
                }).ToList();
            if (list.Count > 0) activityInstance = list[0];

            return activityInstance;
        }

        /// <summary>
        /// 获取运行状态的活动实例
        /// </summary>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity GetActivityRunning(int processInstanceID,
            string activityGUID,
            IDbSession session)
        {
            return GetActivityByState(processInstanceID, activityGUID, ActivityStateEnum.Running, session);
        }

        /// <summary>
        /// 根据状态获取活动实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="activityState">活动状态</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity GetActivityByState(int processInstanceID,
            string activityGUID,
            ActivityStateEnum activityState,
            IDbSession session)
        {
            ActivityInstanceEntity entity = null;
            var sql = @"SELECT * FROM WfActivityInstance 
                        WHERE ProcessInstanceID = @processInstanceID 
                            AND ActivityGUID = @activityGUID 
                            AND ActivityState = @state
                        ORDER BY ID DESC";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceID = processInstanceID,
                    activityGUID = activityGUID.ToString(),
                    state = (short)activityState
                },
                session.Transaction).ToList();
            if (list.Count == 1)
            {
                entity = list[0];
            }
            return entity;
        }

        /// <summary>
        /// 获取已经运行完成的节点
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <returns>活动列表</returns>
        internal List<ActivityInstanceEntity> GetActivityInstanceListCompleted(string appInstanceID,
            string processGUID)
        {
            //activityState: 4-completed（完成）
            var sql = @"SELECT 
                                AI.* 
                            FROM WfActivityInstance AI
                            INNER JOIN WfProcessInstance PI 
                                ON AI.ProcessInstanceID = PI.ID
                            WHERE PI.ProcessState = 2 
                                AND AI.AppInstanceID = @appInstanceID 
                                AND AI.ProcessGUID = @processGUID
                                AND AI.ActivityState = 4";

            var list = Repository.Query<ActivityInstanceEntity>(
                sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID.ToString()
                }).ToList();
            return list;
        }

        /// <summary>
        /// 获取上一步已经完成活动的实例
        /// </summary>
        /// <param name="runningNode">运行节点</param>
        /// <param name="previousActivityGUID">活动GUID</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity GetPreviousActivityInstanceSimple(ActivityInstanceEntity runningNode,
            string previousActivityGUID)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                return GetPreviousActivityInstanceSimple(runningNode, previousActivityGUID, session);
            }
            catch
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 获取上一步已经完成活动的实例
        /// </summary>
        /// <param name="runningNode">运行节点</param>
        /// <param name="previousActivityGUID">活动GUID</param>
        /// <param name="session">数据会话</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity GetPreviousActivityInstanceSimple(ActivityInstanceEntity runningNode,
            string previousActivityGUID,
            IDbSession session)
        {
            ActivityInstanceEntity originalRunningNode = null;
            var processInstanceID = runningNode.ProcessInstanceID;

            //确定当前运行节点的初始信息
            if (runningNode.BackSrcActivityInstanceID != null)
            {
                //*****注意：当前节点的类型已经是退回后生成的节点
                //获取退回之前的初始节点创建人信息
                originalRunningNode = GetById(session.Connection, runningNode.BackOrgActivityInstanceID.Value, session.Transaction);
            }
            else
            {
                originalRunningNode = runningNode;
            }

            //获取上一步节点列表
            var instanceList = GetActivityInstanceListCompletedSimple(processInstanceID, previousActivityGUID, session);
            //排除掉是包含已经退回过的非初始节点
            var withoutBackSrcInfoList = instanceList.Where(a => a.BackSrcActivityInstanceID == null);

            //上一步节点的完成人与当前运行节点的创建人匹配
            var previousList = withoutBackSrcInfoList.Where(a => a.EndedByUserID == originalRunningNode.CreatedByUserID).ToList();
            if (previousList.Count > 0)
            {
                return previousList[0];
            }
            else if (withoutBackSrcInfoList.Count() == 1)
            {
                //合并后的节点退回到某个分支，最后一个分支的完成人才是合并之后节点的创建人
                //所以此时按照EndedByUserID和CreatedByUserID的比对是不靠谱的。
                return withoutBackSrcInfoList.ToList()[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取完成状态的活动实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动实例实体</returns>
        internal IList<ActivityInstanceEntity> GetActivityInstanceListCompletedSimple(int processInstanceID,
            string activityGUID)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var list = GetActivityInstanceListCompletedSimple(processInstanceID, activityGUID, session);
                return list;
            }
        }

        /// <summary>
        /// 获取完成状态的活动实例
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <param name="session">数据会话</param>
        /// <returns>活动实例实体</returns>
        internal IList<ActivityInstanceEntity> GetActivityInstanceListCompletedSimple(int processInstanceID,
            string activityGUID,
            IDbSession session)
        {
            //activityState: 4-completed（完成）
            var sql = @"SELECT * FROM WfActivityInstance 
                        WHERE ProcessInstanceID = @processInstanceID 
                            AND ActivityGUID = @activityGUID 
                            AND ActivityState = @state 
                        ORDER BY ID DESC";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceID = processInstanceID,
                    activityGUID = activityGUID.ToString(),
                    state = (short)ActivityStateEnum.Completed
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// 由任务ID获取活动实例信息
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity GetByTask(int taskID,
            IDbSession session)
        {
            ActivityInstanceEntity entity = null;
            var sql = @"SELECT 
                            AI.* 
                        FROM WfActivityInstance AI
                        INNER JOIN WfTasks T ON AI.ID = T.ActivityInstanceID
                        WHERE T.ID = @taskID";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    taskID = taskID
                },
                session.Transaction).ToList();
            if (list.Count == 1)
            {
                entity = list[0];
            }
            return entity;
        }

        /// <summary>
        /// 由任务ID获取活动实例信息
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity GetByTask(int taskID)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var entity = GetByTask(taskID, session);
                return entity;
            }
        }

        /// <summary>
        /// 获取流程实例中运行的活动节点
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>活动实例列表</returns>
        internal IEnumerable<ActivityInstanceEntity> GetRunningActivityInstanceList(string appInstanceID,
            string processGUID,
            string version = null)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetRunningActivityInstanceList(appInstanceID, processGUID, version, session);
            }
        }

        /// <summary>
        /// 获取流程实例中运行的活动节点
        /// </summary>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="session">数据库会话</param>
        /// <returns>活动实例列表</returns>
        internal IEnumerable<ActivityInstanceEntity> GetRunningActivityInstanceList(string appInstanceID,
            string processGUID,
            string version,
            IDbSession session)
        {
            //activityState: 1-ready（准备）, 2-running（）运行；
            if (string.IsNullOrEmpty(version)) version = "1";
            var sql = @"SELECT 
                                AI.* 
                            FROM WfActivityInstance AI 
                            INNER JOIN WfProcessInstance PI 
                                ON AI.ProcessInstanceID = PI.ID 
                            WHERE (AI.ActivityState=1 OR AI.ActivityState=2) 
                                AND PI.ProcessState = 2 
                                AND PI.Version = @version 
                                AND AI.AppInstanceID = @appInstanceID 
                                AND AI.ProcessGUID = @processGUID";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    appInstanceID = appInstanceID,
                    processGUID = processGUID,
                    version = version
                },
                session.Transaction);

            return list;
        }

        /// <summary>
        /// 获取主节点下已经完成的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID">主活动实例ID</param>
        /// <returns>活动列表</returns>
        internal List<ActivityInstanceEntity> GetMultipleInstanceListCompleted(int mainActivityInstanceID)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                return GetMultipleInstanceListCompleted(mainActivityInstanceID, session);
            }
            catch(System.Exception ex)
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 获取主节点下已经完成的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID">主活动实例ID</param>
        /// <param name="session">数据会话</param>
        /// <returns>活动列表</returns>
        internal List<ActivityInstanceEntity> GetMultipleInstanceListCompleted(int mainActivityInstanceID,
            IDbSession session)
        {
            //activityState: 4-completed（完成）
            var sql = @"SELECT 
                                AI.* 
                            FROM WfActivityInstance AI
                            INNER JOIN WfProcessInstance PI 
                                ON AI.ProcessInstanceID = PI.ID
                            WHERE PI.ProcessState = 2 
                                AND MIHostActivityInstanceID = @mainActivityInstanceID
                                AND AI.ActivityState = 4";
            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    mainActivityInstanceID = mainActivityInstanceID
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// 获取已经完成的多实例子节点
        /// </summary>
        /// <param name="runningNode">当前运行节点</param>
        /// <param name="previousActivityGUID">前置活动GUID</param>
        /// <returns>子节点列表</returns>
        internal List<ActivityInstanceEntity> GetPreviousParallelMultipleInstanceListCompleted(ActivityInstanceEntity runningNode,
            string previousActivityGUID)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var previousList = GetPreviousParallelMultipleInstanceListCompleted(runningNode, previousActivityGUID, session);
                return previousList;
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 获取已经完成的多实例子节点
        /// </summary>
        /// <param name="runningNode">当前运行节点</param>
        /// <param name="previousActivityGUID">前置活动GUID</param>
        /// <param name="session">会话</param>
        /// <returns>子节点列表</returns>
        internal List<ActivityInstanceEntity> GetPreviousParallelMultipleInstanceListCompleted(ActivityInstanceEntity runningNode,
            string previousActivityGUID,
            IDbSession session)
        {
            ActivityInstanceEntity originalRunningNode = null;
            var processInstanceID = runningNode.ProcessInstanceID;
            //确定当前运行节点的初始信息
            if (runningNode.BackSrcActivityInstanceID != null)
            {
                //*****注意：当前节点的类型已经是退回后生成的节点
                //获取退回之前的初始节点创建人信息
                originalRunningNode = GetById(session.Connection, runningNode.BackOrgActivityInstanceID.Value, session.Transaction);
            }
            else
            {
                originalRunningNode = runningNode;
            }

            var sql = @"SELECT * FROM WfActivityInstance 
                        WHERE ProcessInstanceID = @processInstanceID 
                            AND ActivityGUID = @activityGUID 
                            AND ActivityState = 4
                            AND MIHostActivityInstanceID IS NOT NULL 
                        ORDER BY ID DESC";
            var instanceList = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    processInstanceID = runningNode.ProcessInstanceID,
                    activityGUID = previousActivityGUID
                },
                session.Transaction).ToList();

            //排除掉是包含已经退回过的非初始节点
            var withoutBackSrcInfoList = instanceList.Where(a => a.BackSrcActivityInstanceID == null).ToList();

            //上一步节点的完成人与当前运行节点的创建人匹配
            var firstMIChild = withoutBackSrcInfoList.First(a => a.EndedByUserID == originalRunningNode.CreatedByUserID);
            //根据主节点信息查询所有对应的子节点列表
            var previousList = GetMultipleInstanceListCompleted(firstMIChild.MIHostActivityInstanceID.Value, session);

            return previousList;
        }

        /// <summary>
        /// 判断用户是否是分配下来任务的用户
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userID">用户ID</param>
        /// <returns>是否</returns>
        private bool IsAssignedUserInActivityInstance(ActivityInstanceEntity entity,
            int userID)
        {
            var assignedToUsers = entity.AssignedToUserIDs;
            var userList = assignedToUsers.Split(',');
            var single = userList.FirstOrDefault<string>(a => a == userID.ToString());
            if (!string.IsNullOrEmpty(single))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断是否为会签实例节点
        /// </summary>
        /// <param name="child">活动实例</param>
        /// <returns>是否结果</returns>
        internal Boolean IsMultipleInstanceChildren(ActivityInstanceEntity child)
        {
            return child.MIHostActivityInstanceID != null;
        }

        /// <summary>
        /// 过滤活动实例列表
        /// </summary>
        /// <param name="instanceList">实例列表</param>
        /// <returns>同一批次的实例列表</returns>
        private List<ActivityInstanceEntity> FilteredActivityInstanceInTheSameBatch(List<ActivityInstanceEntity> instanceList)
        {
            //如果有退回记录，则认为是同一批的运行模式，保证会签的通过率依然按照原来的CompleteOrder数值
            //否则，需要调用Resend()返送接口，而不用受到原来会签通过率CompleteOrder的限制。
            //此处以BackSrcActivityInstanceID作为监测点数据，就可以知道是否有退回批次
            //最后一次的退回批次BackSrcActivityInstanceID为最大值，如果没有退回则BackSrcActivityInstanceID为空
            var maxBackSrcActivityInstanceID = instanceList.Max<ActivityInstanceEntity>(a => a.BackSrcActivityInstanceID);
            if (maxBackSrcActivityInstanceID != null)
            {
                var childrenList = instanceList.Where<ActivityInstanceEntity>(a => a.BackSrcActivityInstanceID == maxBackSrcActivityInstanceID.Value).ToList();
                return childrenList;
            }
            else
            {
                return instanceList;
            }
        }

        /// <summary>
        /// 获取会签节点的多实例节点
        /// </summary>
        /// <param name="mainActivityInstanceID">会签节点</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityState">活动状态</param>
        /// <returns>活动实例列表</returns>
        internal List<ActivityInstanceEntity> GetActivityMulitipleInstanceWithState(int mainActivityInstanceID,
            int processInstanceID,
            short? activityState = null)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var childActivityInstance =  GetActivityMulitipleInstanceWithState(mainActivityInstanceID, 
                    processInstanceID, activityState, session);

                return childActivityInstance;
            }
            catch
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 获取同一批的主节点下的子节点列表记录
        /// 包括：需要过滤回退后的回退标识类型的过滤
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="activityState">活动类型</param>
        /// <param name="session">数据会话</param>
        /// <returns>节点列表</returns>
        internal IList<ActivityInstanceEntity> GetActivityMulitipleInstanceWithStateBatch(int processInstanceID,
            int mainActivityInstanceID,
            short? activityState,
            IDbSession session)
        {
            //取出处于多实例节点列表
            var instanceList = GetActivityMulitipleInstanceWithState(mainActivityInstanceID,
                processInstanceID,
                activityState,
                session).ToList<ActivityInstanceEntity>();

            instanceList = FilteredActivityInstanceInTheSameBatch(instanceList);
            return instanceList;
        }

        /// <summary>
        /// 获取会签节点的多实例节点
        /// </summary>
        /// <param name="mainActivityInstanceID">会签节点</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activityState">活动状态</param>
        /// <param name="session">session会话</param>
        /// <returns></returns>
        internal List<ActivityInstanceEntity> GetActivityMulitipleInstanceWithState(int mainActivityInstanceID,
            int processInstanceID,
            short? activityState,
            IDbSession session)
        {
            //activityState: 1-ready（准备）, 2-running（）运行；
            var sql = @"SELECT * FROM WfActivityInstance 
                            WHERE MIHostActivityInstanceID = @activityInstanceID 
                                AND processInstanceID = @processInstanceID
                            ";
            if (activityState.HasValue)
            {
                sql += " AND ActivityState = @activityState ";
            }
            sql += " ORDER BY CompleteOrder";

            var list = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    activityInstanceID = mainActivityInstanceID,
                    processInstanceID = processInstanceID,
                    activityState = activityState
                },
                session.Transaction).ToList();

            return list;
        }


        /// <summary>
        /// 查询分支实例的个数
        /// </summary>
        /// <param name="splitActivityGUID">分支节点GUID</param>
        /// <param name="splitActivityInstanceID">分支节点活动实例ID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="session">会话</param>
        /// <returns>有效分支转移个数</returns>
        internal int GetGatewayInstanceCountByTransition(string splitActivityGUID,
            int splitActivityInstanceID,
            int processInstanceID,
            IDbSession session)
        {
            var sql = @"SELECT * FROM wftransitioninstance
                            WHERE processinstanceid=@processinstanceID 
                                AND fromactivityguid=@fromActivityGUID
                                AND fromactivityinstanceid=@fromActivityInstanceID";
            var list = Repository.Query<TransitionInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    fromActivityGUID = splitActivityGUID,
                    fromActivityInstanceID = splitActivityInstanceID,
                    processinstanceID = processInstanceID
                },
                session.Transaction).ToList();
            return list.Count();
        }

        /// <summary>
        ///  获取有效的分支数目
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="splitGatewayInstanceID">Split网关实例ID</param>
        /// <param name="session">会话</param>
        /// <returns>活动实例列表</returns>
        internal List<ActivityInstanceEntity> GetValidSplitedActivityInstanceList(int processInstanceID, 
            int splitGatewayInstanceID, 
            IDbSession session)
        {
            var sql = @"SELECT 
                         A.*
                        FROM WfActivityInstance A 
                        INNER JOIN WfTransitionInstance T ON
                         A.ID = T.ToActivityInstanceID
                        WHERE (A.ActivityState=1
                                  OR A.ActivityState=2
                                  OR A.ActivityState=4
                                  OR A.ActivityState=5) 
                         AND T.FromActivityInstanceID = @fromActivityInstanceID
                        ";
            var list = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    fromActivityInstanceID = splitGatewayInstanceID
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// 获取有效的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <returns>子节点列表</returns>
        internal List<ActivityInstanceEntity> GetValidActivityInstanceListOfMI(int mainActivityInstanceID,
            int processInstanceID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetValidActivityInstanceListOfMI(mainActivityInstanceID, processInstanceID, session);
            }
        }

        /// <summary>
        /// 获取有效的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="session">数据会话</param>
        /// <returns>子节点列表</returns>
        internal List<ActivityInstanceEntity> GetValidActivityInstanceListOfMI(int mainActivityInstanceID,
            int processInstanceID,
            IDbSession session)
        {
            var sql = @"SELECT 
                         *
                        FROM WfActivityInstance
                        WHERE (ActivityState=1
                                  OR ActivityState = 2
                                  OR ActivityState = 4
                                  OR ActivityState = 5) 
	                        AND MIHostActivityInstanceID = @mainActivityInstanceID
                            AND ProcessInstanceID = @processInstanceID
                        ";
            var list = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    mainActivityInstanceID = mainActivityInstanceID,
                    processInstanceID = processInstanceID
                },
                session.Transaction).ToList();

            //去除掉有退回过的实例
            var backList = new List<int>();
            foreach (var child in list)
            {
                if (child.BackSrcActivityInstanceID != null)
                {
                    if (backList.Any(a=>a == child.BackSrcActivityInstanceID.Value) == false)
                        backList.Add(child.BackSrcActivityInstanceID.Value);
                }

                if (child.BackOrgActivityInstanceID != null)
                {
                    if (backList.Any(a=>a==child.BackOrgActivityInstanceID.Value) == false)
                        backList.Add(child.BackOrgActivityInstanceID.Value);
                }
            }

            if (backList.Count > 0)
            {
                int[] backArray = backList.ToArray();
                var validList = list.Where(a => !backArray.Contains(a.ID)).ToList();
                //返回过滤掉退回节点的列表
                return validList;
            }

            return list;
        }
        #endregion

        #region 创建活动实例
        /// <summary>
        /// 创建活动实例的对象
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="appInstanceCode">应用实例代码</param>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activity">活动</param>
        /// <param name="runner">运行者</param>
        /// <returns>活动实例实体</returns>
        internal ActivityInstanceEntity CreateActivityInstanceObject(string appName,
            string appInstanceID,
            string appInstanceCode,
            string processGUID,
            int processInstanceID,
            Activity activity,
            WfAppRunner runner)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityGUID = activity.ActivityGUID;
            instance.ActivityName = activity.ActivityName;
            instance.ActivityCode = activity.ActivityCode;
            instance.ActivityType = (short)activity.ActivityType;
            instance.WorkItemType = (short)activity.WorkItemType;
            instance.GatewayDirectionTypeID = activity.GatewayDetail != null ? (short)activity.GatewayDetail.DirectionType : null;
            instance.ProcessGUID = processGUID;
            instance.AppName = appName;
            instance.AppInstanceID = appInstanceID;
            instance.AppInstanceCode = appInstanceCode;
            instance.ProcessInstanceID = processInstanceID;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedByUserID = runner.UserID;
            instance.CreatedByUserName = runner.UserName;
            instance.CreatedDateTime = System.DateTime.Now;
            instance.OverdueDateTime = CalculateActivityOverdueDateTime(activity.BoundaryList, instance.CreatedDateTime);
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanNotRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// 计算活动节点过期时间
        /// XmlConvert.ToTimeSpan()
        /// https://stackoverflow.com/questions/12466188/how-do-i-convert-an-iso8601-timespan-to-a-c-sharp-timespan
        /// </summary>
        /// <param name="boundaryList">边界列表</param>
        /// <param name="createdDateTime">活动创建时间</param>
        /// <returns>过期时间</returns>
        private Nullable<DateTime> CalculateActivityOverdueDateTime(IList<Boundary> boundaryList, 
            DateTime createdDateTime)
        {
            Nullable<DateTime> overdueDateTime = null;
            if (boundaryList != null && boundaryList.Count() > 0)
            {
                foreach (var boundary in boundaryList)
                {
                    if (boundary.EventTriggerType == EventTriggerEnum.Timer
                        && !string.IsNullOrEmpty(boundary.Expression))
                    {
                        var timeSpan = System.Xml.XmlConvert.ToTimeSpan(boundary.Expression);
                        overdueDateTime = createdDateTime.Add(timeSpan);
                        break;
                    }
                }
            }
            return overdueDateTime;
        }


        /// <summary>
        /// 根据主节点复制子节点
        /// </summary>
        /// <param name="main">活动实例</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity CreateActivityInstanceObject(ActivityInstanceEntity main)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityGUID = main.ActivityGUID;
            instance.ActivityName = main.ActivityName;
            instance.ActivityCode = main.ActivityCode;
            instance.ActivityType = main.ActivityType;
            instance.WorkItemType = main.WorkItemType;
            instance.GatewayDirectionTypeID = main.GatewayDirectionTypeID;
            instance.ProcessGUID = main.ProcessGUID;
            instance.AppName = main.AppName;
            instance.AppInstanceID = main.AppInstanceID;
            instance.AppInstanceCode = main.AppInstanceCode;
            instance.ProcessInstanceID = main.ProcessInstanceID;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedByUserID = main.CreatedByUserID;
            instance.CreatedByUserName = main.CreatedByUserName;
            instance.CreatedDateTime = System.DateTime.Now;
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanNotRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// 创建活动实例的对象
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="appInstanceID">应用实例ID</param>
        /// <param name="appInstanceCode">应用实例代码</param>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="activity">活动</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="backSrcActivityInstanceID">退回源活动实例ID</param>
        /// <param name="backOrgActivityInstanceID">最初活动实例ID</param>
        /// <param name="runner">执行者</param>
        /// <returns>活动实例</returns>
        internal ActivityInstanceEntity CreateBackwardActivityInstanceObject(string appName,
            string appInstanceID,
            string appInstanceCode,
            int processInstanceID,
            Activity activity,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            int backOrgActivityInstanceID,
            WfAppRunner runner)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityGUID = activity.ActivityGUID;
            instance.ActivityName = activity.ActivityName;
            instance.ActivityCode = activity.ActivityCode;
            instance.ActivityType = (short)activity.ActivityType;
            instance.WorkItemType = (short)activity.WorkItemType;
            instance.GatewayDirectionTypeID = activity.GatewayDetail != null ? (short)activity.GatewayDetail.DirectionType : null;
            instance.ProcessGUID = activity.ProcessGUID;
            instance.AppName = appName;
            instance.AppInstanceID = appInstanceID;
            instance.AppInstanceCode = appInstanceCode;
            instance.ProcessInstanceID = processInstanceID;
            instance.BackwardType = (short)backwardType;
            instance.BackSrcActivityInstanceID = backSrcActivityInstanceID;
            instance.BackOrgActivityInstanceID = backOrgActivityInstanceID;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedByUserID = runner.UserID;
            instance.CreatedByUserName = runner.UserName;
            instance.CreatedDateTime = System.DateTime.Now;
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanNotRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// 更新活动节点的Token数目
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void IncreaseTokensHad(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            ActivityInstanceEntity activityInstance = GetById(activityInstanceID);
            activityInstance.TokensHad += 1;
            Update(activityInstance, session);
        }
        #endregion

        #region 活动实例状态设置
        /// <summary>
        /// 活动实例被读取
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">执行者</param>
        /// <param name="session">会话</param>
        internal void Read(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Running, runner, session);
        }

        /// <summary>
        /// 设置活动实例状态
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="activityState">活动状态</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        private void SetActivityState(int activityInstanceID,
            ActivityStateEnum activityState,
            WfAppRunner runner,
            IDbSession session)
        {
            var activityInstance = GetById(session.Connection, activityInstanceID, session.Transaction);
            activityInstance.ActivityState = (short)activityState;
            activityInstance.LastUpdatedByUserID = runner.UserID;
            activityInstance.LastUpdatedByUserName = runner.UserName;
            activityInstance.LastUpdatedDateTime = System.DateTime.Now;
            Update(activityInstance, session);
        }

        /// <summary>
        /// 撤销活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Withdraw(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceID, ActivityStateEnum.Withdrawed, runner, session);
        }

        /// <summary>
        /// 退回活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void SendBack(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceID, ActivityStateEnum.Sendbacked, runner, session);
        }

        /// <summary>
        /// 设置活动结束状态
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="activityState">活动状态</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        private void EndActivityState(int activityInstanceID,
            ActivityStateEnum activityState,
            WfAppRunner runner,
            IDbSession session)
        {
            var activityInstance = GetById(session.Connection, activityInstanceID, session.Transaction);
            activityInstance.ActivityState = (short)activityState;
            activityInstance.EndedByUserID = runner.UserID;
            activityInstance.EndedByUserName = runner.UserName;
            activityInstance.EndedDateTime = System.DateTime.Now;

            Update(activityInstance, session);
        }

        /// <summary>
        /// 活动实例完成
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Complete(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceID, ActivityStateEnum.Completed, runner, session);
        }

        /// <summary>
        /// 更新审批状态为同意
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="session"></param>
        internal void SetApprovalStatus(int activityInstanceID, 
            IDbSession session)
        {
            var activityInstance = GetById(session.Connection, activityInstanceID, session.Transaction);
            //表示当前没有被审核过，才可以认为是审批同意状态
            if (activityInstance.ApprovalStatus == (short)ApprovalStatusEnum.Null)
            {
                activityInstance.ApprovalStatus = (short)ApprovalStatusEnum.Agreed;
                Update(activityInstance, session);
            }
        }

        /// <summary>
        /// 更新分支和合并之间的运行节点为阻止状态
        /// </summary>
        /// <param name="gatewayActivity">网关(合并)节点</param>
        /// <param name="gatewayInstance">网关(合并)实例</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="session">数据会话</param>
        internal void UpdateActivityInstanceBlockedBetweenSplitJoin(Activity gatewayActivity, 
            ActivityInstanceEntity gatewayInstance,
            IProcessModel processModel,
            IDbSession session)
        {
            var joinCount = 0;
            var splitCount = 0;
            var splitActivity = processModel.GetBackwardGatewayActivity(gatewayActivity, ref joinCount, ref splitCount);
            var taskActivityList = processModel.GetAllTaskActivityList(splitActivity, gatewayActivity);

            UpdateActivityInstanceBlockedBetweenSplitJoin(gatewayInstance.ProcessInstanceID, taskActivityList, session);
        }

        /// <summary>
        /// 更新活动里面的活动为阻止状态
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="taskActivityList">任务节点列表</param>
        /// <param name="session">数据会话</param>
        private void UpdateActivityInstanceBlockedBetweenSplitJoin(int processInstanceID, 
            IList<Activity> taskActivityList,
            IDbSession session)
        {
            var idsin = taskActivityList.Select(a => a.ActivityGUID).ToList();
            var updSql = @"UPDATE WfActivityInstance 
                        SET CanNotRenewInstance=1 
                        WHERE ProcessInstanceID=@processInstanceID 
                            AND ActivityState in (1, 2, 5) 
                            AND ActivityGUID in @ids";

            var rows = Repository.Execute(session.Connection, updSql,
                new
                {
                    processInstanceID = processInstanceID,
                    ids = idsin
                },
                session.Transaction);
        }
        #endregion

        #region 活动实例记录维护
        /// <summary>
        /// 插入活动实例
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="session">会话</param>
        /// <returns>新的自增长ID键值</returns>
        internal int Insert(ActivityInstanceEntity entity,
            IDbSession session)
        {
            //SET ActivityName When It Is NULL
            if (entity.ActivityType == (short)ActivityTypeEnum.GatewayNode
                && string.IsNullOrEmpty(entity.ActivityName))
            {
                entity.ActivityName = "GATEWAY";
            }

            int newID = Repository.Insert(session.Connection, entity, session.Transaction);
            entity.ID = newID;

            return newID;
        }

        /// <summary>
        /// 更新活动实例
        /// </summary>
        /// <param name="entity">活动实例实体</param>
        /// <param name="session">会话</param>
        internal void Update(ActivityInstanceEntity entity,
            IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// 更新活动实例
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="entity">实体</param>
        /// <param name="trans">事务</param>
        internal void Update(IDbConnection conn, ActivityInstanceEntity entity, IDbTransaction trans)
        {
            Repository.Update(conn, entity, trans);
        }

        /// <summary>
        /// 取消节点运行
        /// </summary>
        /// <param name="activityInstanceID">活动实例</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal void Cancel(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            var sql = @"UPDATE WfActivityInstance 
                        SET ActivityState=@activityState,
                        LastUpdatedByUserID=@lastUpdatedByUserID,
                        LastUpdatedByUserName=@lastUpdatedByUserName,
                        LastUpdatedDateTime=@lastUpdatedDateTime 
                        WHERE ID=@activityInstanceID 
                            AND (ActivityState=1 OR ActivityState=2 OR ActivityState=5)";
            Repository.Execute(session.Connection, sql,
                new
                {
                    activityState = (short)ActivityStateEnum.Cancelled,
                    lastUpdatedByUserID = runner.UserID,
                    lastUpdatedByUserName = runner.UserName,
                    LastUpdatedDateTime = System.DateTime.Now,
                    activityInstanceID = activityInstanceID
                }, session.Transaction);
        }

        /// <summary>
        /// 更新会签节点中未办理完成的节点状态
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="session">会话</param>
        /// <param name="runner">执行者</param>
        internal void CancelUnCompletedMultipleInstance(int mainActivityInstanceID, IDbSession session, WfAppRunner runner)
        {
            var sql = @"UPDATE WfActivityInstance 
                        SET ActivityState=@activityState,
                        LastUpdatedByUserID=@lastUpdatedByUserID,
                        LastUpdatedByUserName=@lastUpdatedByUserName,
                        LastUpdatedDateTime=@lastUpdatedDateTime 
                        WHERE MIHostActivityInstanceID=@mainActivityInstanceID 
                            AND (ActivityState=1 OR ActivityState=2 OR ActivityState=5)";

            Repository.Execute(session.Connection, sql,
                new
                {
                    activityState = (short)ActivityStateEnum.Cancelled,
                    lastUpdatedByUserID = runner.UserID,
                    lastUpdatedByUserName = runner.UserName,
                    LastUpdatedDateTime = System.DateTime.Now,
                    mainActivityInstanceID = mainActivityInstanceID
                }, session.Transaction);
        }

        /// <summary>
        /// 撤销主节点及其下面的多实例子节点
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="session">会话</param>
        /// <param name="runner">执行者</param>
        internal void WithdrawMainIncludedChildNodes(int mainActivityInstanceID, IDbSession session, WfAppRunner runner)
        {
            //先更新主节点状态为撤销状态
            SetActivityState(mainActivityInstanceID, ActivityStateEnum.Withdrawed, runner, session);

            //再更新主节点下的多实例子节点状态为撤销状态
            WithdrawMultipleInstance(mainActivityInstanceID, session, runner);
        }

        /// <summary>
        /// 更新会签办理节点的节点状态
        /// </summary>
        /// <param name="mainActivityInstanceID">主节点ID</param>
        /// <param name="session">会话</param>
        /// <param name="runner">执行者</param>
        private void WithdrawMultipleInstance(int mainActivityInstanceID, IDbSession session, WfAppRunner runner)
        {
            var sql = @"UPDATE WfActivityInstance 
                        SET ActivityState=@activityState, 
                        LastUpdatedByUserID=@lastUpdatedByUserID,
                        LastUpdatedByUserName=@lastUpdatedByUserName,
                        LastUpdatedDateTime=@lastUpdatedDateTime
                        WHERE MIHostActivityInstanceID=@mainActivityInstanceID
                            AND (ActivityState=1 OR ActivityState=5)";      //准备或挂起状态的节点可以撤销

            Repository.Execute(session.Connection, sql,
                new
                {
                    activityState = (short)ActivityStateEnum.Withdrawed,
                    lastUpdatedByUserID = runner.UserID,
                    lastUpdatedByUserName = runner.UserName,
                    LastUpdatedDateTime = System.DateTime.Now,
                    mainActivityInstanceID = mainActivityInstanceID
                }, session.Transaction);
        }

        /// <summary>
        /// 重新使节点处于挂起状态
        /// 说明：会签最后一个子节点撤销时候用到
        /// </summary>
        /// <param name="activityInstanceID">活动实例节点ID</param>
        /// <param name="session">会话</param>
        /// <param name="runner">执行者</param>
        internal void Resuspend(int activityInstanceID, IDbSession session, WfAppRunner runner)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Suspended, runner, session);
        }

        /// <summary>
        /// 删除活动实例
        /// </summary>
        /// <param name="activityInstanceID">活动实例ID</param>
        /// <param name="session">会话</param>
        internal void Delete(int activityInstanceID,
            IDbSession session = null)
        {
            Repository.Delete<ActivityInstanceEntity>(session.Connection,
                activityInstanceID,
                session.Transaction);
        }
        #endregion

        #region 活动实例审批状态
        /// <summary>
        /// 同意
        /// </summary>
        /// <param name="taskID">任务ID</param>
        internal void Agree(int taskID)
        {
            var activityInstance = GetByTask(taskID);
            activityInstance.ApprovalStatus = (short)ApprovalStatusEnum.Agreed;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Update(activityInstance, session);
                //更新提交
                session.Commit();
            }
            catch(System.Exception ex)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="taskID">任务ID</param>
        internal void Refuse(int taskID)
        {
            var activityInstance = GetByTask(taskID);
            activityInstance.ApprovalStatus = (short)ApprovalStatusEnum.Refused;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Update(activityInstance, session);
                //更新提交
                session.Commit();
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }
       
        /// <summary>
        ///  检查当前节点的通过类型
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="session">会话</param>
        /// <returns>通过结果对象</returns>
        internal NodePassedResult CheckActivityInstancePassedResult(Nullable<int> taskID,
            IDbSession session)
        {
            var result = NodePassedResult.Create(NodePassedTypeEnum.Default);
            if (taskID != null)
            {
                var activityInstance = GetByTask(taskID.Value, session);
                if (IsMultipleInstanceChildren(activityInstance) == true)
                {
                    //会签多实例情况下的是否通过判断
                    var mainActivityInstance = GetById(session.Connection, activityInstance.MIHostActivityInstanceID.Value, session.Transaction);
                    var nodePassedType = CheckMIPassRateInfo(activityInstance, mainActivityInstance, session);
                    result = NodePassedResult.Create(nodePassedType);
                }
                else
                {
                    result = NodePassedResult.CreateByApprovalStatus(activityInstance.ApprovalStatus);
                }
            }
            return result;
        }

        /// <summary>
        /// 检查会签节点通过率的满足情况
        /// </summary>
        /// <param name="currentActivityInstance">当前审批节点</param>
        /// <param name="mainActivityInstance">主节点ID</param>
        /// <param name="session">会话</param>
        /// <returns>检验结果</returns>
        internal NodePassedTypeEnum CheckMIPassRateInfo(ActivityInstanceEntity currentActivityInstance,
            ActivityInstanceEntity mainActivityInstance,
            IDbSession session)
        {
            var passedType = NodePassedTypeEnum.Default;
            var childActivityInstanceList = GetValidActivityInstanceListOfMI(mainActivityInstance.ID, mainActivityInstance.ProcessInstanceID,
                session);

            //参与通过率类型计算的数目列举
            var agreedCount = childActivityInstanceList.Where(a => a.ApprovalStatus == (short)ApprovalStatusEnum.Agreed).Count();
            var refusedCount = childActivityInstanceList.Where(a => a.ApprovalStatus == (short)ApprovalStatusEnum.Refused).Count();

            if (mainActivityInstance.MergeType == (short)MergeTypeEnum.Sequence)
            {
                if (mainActivityInstance.CompareType.Value == (short)CompareTypeEnum.Count)
                {
                    //通过率类型为:个数
                    var thresholdCount = mainActivityInstance.CompleteOrder.Value;
                    var allCount = childActivityInstanceList.Max(a => a.CompleteOrder).Value;       //总共审批数目
                    var negativeCount = allCount - thresholdCount;

                    if (refusedCount > negativeCount)
                    {
                        passedType = NodePassedTypeEnum.NotPassed;
                    }
                    else if (agreedCount >= thresholdCount)
                    {
                        passedType = NodePassedTypeEnum.Passed;
                    }
                    else
                    {
                        //等待其他会签人员审批
                        passedType = NodePassedTypeEnum.NeedToBeMoreApproved;
                    }
                    return passedType;
                }
                else if (mainActivityInstance.CompareType.Value == (short)CompareTypeEnum.Percentage)
                {
                    //按照百分比数目比较
                    var thresholdPercentage = mainActivityInstance.CompleteOrder.Value;
                    var allCount = childActivityInstanceList.Count;
                    var negativePercentage = 1 - thresholdPercentage;

                    if ((refusedCount * 0.01) / (allCount * 0.01) > negativePercentage)
                    {
                        passedType = NodePassedTypeEnum.NotPassed;
                    }
                    else if ((agreedCount * 0.01) / (allCount * 0.01) >= thresholdPercentage)
                    {
                        passedType = NodePassedTypeEnum.Passed;
                    }
                    else
                    {
                        passedType = NodePassedTypeEnum.NeedToBeMoreApproved;
                    }
                    return passedType;
                }
                else
                {
                    var msgException = LocalizeHelper.GetEngineMessage("activityinstancemanager.CheckPassRateMatchedInfo.UnknownCompareTypeValue");
                    throw new WorkflowException(msgException);
                }
            }
            else if (mainActivityInstance.MergeType == (short)MergeTypeEnum.Parallel)
            {
                if (mainActivityInstance.CompareType.Value == (short)CompareTypeEnum.Count)
                {
                    //通过率类型为:个数
                    var thresholdCount = mainActivityInstance.CompleteOrder.Value;
                    var allCount = childActivityInstanceList.Count;
                    var negativeCount = allCount - thresholdCount;

                    if (refusedCount > negativeCount)
                    {
                        passedType = NodePassedTypeEnum.NotPassed;
                    }
                    else if (agreedCount >= thresholdCount)
                    {
                        passedType = NodePassedTypeEnum.Passed;
                    }
                    else
                    {
                        //等待其他会签人员审批
                        passedType = NodePassedTypeEnum.NeedToBeMoreApproved;
                    }
                    return passedType;
                }
                else if (mainActivityInstance.CompareType.Value == (short)CompareTypeEnum.Percentage)
                {
                    //按照百分比数目比较
                    var thresholdPercentage = mainActivityInstance.CompleteOrder.Value;
                    var allCount = childActivityInstanceList.Count;
                    var negativePercentage = 1 - thresholdPercentage;

                    if ((refusedCount * 0.01) / (allCount * 0.01) > negativePercentage)
                    {
                        passedType = NodePassedTypeEnum.NotPassed;
                    }
                    else if ((agreedCount * 0.01) / (allCount * 0.01) >= thresholdPercentage)
                    {
                        passedType = NodePassedTypeEnum.Passed;
                    }
                    else
                    {
                        passedType = NodePassedTypeEnum.NeedToBeMoreApproved;
                    }
                    return passedType;
                }
                else
                {
                    var msgException = LocalizeHelper.GetEngineMessage("activityinstancemanager.CheckPassRateMatchedInfo.UnknownCompareTypeValue");
                    throw new WorkflowException(msgException);
                }
            }
            else
            {
                var msgException = LocalizeHelper.GetEngineMessage("activityinstancemanager.CheckPassRateMatchedInfo.UnknownMergeTypeValue");
                throw new WorkflowException(msgException);
            }
        }

        /// <summary>
        /// 获取会签节点的审批通过率类型
        /// </summary>
        /// <param name="currentActivityInstance">当前活动实例</param>
        /// <param name="mainActivityInstance">主节点</param>
        /// <param name="session">会话</param>
        /// <returns>是否可以通过</returns>
        internal Boolean GetMiApprovalThresholdStatus(ActivityInstanceEntity currentActivityInstance,
            ActivityInstanceEntity mainActivityInstance, 
            IDbSession session)
        {
            var finalPassed = false;
            var passedType = CheckMIPassRateInfo(currentActivityInstance, mainActivityInstance, session);
            if (passedType == NodePassedTypeEnum.Passed || passedType == NodePassedTypeEnum.NotPassed)
            {
                finalPassed = true;
            }
            return finalPassed;
        }
        #endregion
    }
}
