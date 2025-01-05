
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
using static IronPython.Modules._ast;

namespace Slickflow.Engine.Business.Manager
{
    /// <summary>
    /// Activity Instance Manager
    /// 活动实例管理类
    /// </summary>
    internal class ActivityInstanceManager : ManagerBase
    {
        #region Constructor 构造函数
        internal ActivityInstanceManager()
        {
        }
        #endregion

        #region Activity instance data acquisition 活动实例数据获取
        /// <summary>
        /// Retrieve activity instances based on ID
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
        /// Retrieve activity instances based on ID
        /// 根据ID获取活动实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="activityInstanceID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetById(IDbConnection conn, int activityInstanceID, IDbTransaction trans)
        {
            return Repository.GetById<ActivityInstanceEntity>(conn, activityInstanceID, trans);
        }

        /// <summary>
        /// Obtain the current running node information of the process
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetRunningNode(WfAppRunner runner)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                //如果流程在运行状态，则返回运行时信息
                //If the process is in running state, return runtime information
                TaskViewEntity task = null;
                var entity = GetRunningNode(runner, session, out task);

                return entity;
            }
        }

        /// <summary>
        /// Obtain the current running node information of the process
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="taskView"></param>
        /// <returns></returns>
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
        /// Obtain the current running node information of the process
        /// 获取流程当前运行节点信息
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        /// <param name="taskView"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetRunningNode(WfAppRunner runner,
            IDbSession session,
            out TaskViewEntity taskView)
        {
            var taskID = runner.TaskID;
            taskView = null;    //default value;

            //根据TaskID获取
            //Obtain based on TaskID
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
            //No TaskID parameter passed for query
            var activityInstanceList = aim.GetRunningActivityInstanceList(runner.AppInstanceID, runner.ProcessGUID, runner.Version, session).ToList();
            if (activityInstanceList == null || activityInstanceList.Count == 0)
            {
                //当前没有运行状态的节点存在，流程不存在，或者已经结束或取消
                //There are currently no running nodes, processes that do not exist, or have already ended or been cancelled
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
                //Parallel mode processing
                //Retrieve the activity instance to be processed based on the current executor's identity (there are multiple nodes in pending or running status in parallel mode)
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
                    //Get TaskView
                    taskView = tm.GetTaskOfMine(session.Connection, runningNode.ID, runner.UserID, session.Transaction);
                }
                else
                {
                    //当前用户的待办任务不唯一，抛出异常，需要TaskID唯一界定
                    //The current user's pending tasks are not unique, and an exception is thrown. TaskID must be uniquely defined
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
        /// Get the instance of the return source activity
        /// 获取退回源活动实例
        /// </summary>
        /// <param name="currentActivityInstanceID"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetBackSrcActivityInstance(int currentActivityInstanceID)
        {
            var currentActivityInstance = GetById(currentActivityInstanceID);
            var backSrcActivityInstance = GetById(currentActivityInstance.BackSrcActivityInstanceID.Value);

            return backSrcActivityInstance;
        }

        /// <summary>
        /// Determine whether it is a processing task for a certain user
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
        /// Determine whether the node is in a running state
        /// 判断节点是否处于运行状态
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <returns></returns>
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
        /// Get Activity Instance
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
        /// Get Activity Instance Latest
        /// 获取活动节点实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
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
        /// Get Activity Instance Latest
        /// 获取最近的节点实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="activityGUID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Get Activity Instance Latest
        /// 获取最近的节点实例
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
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
        /// Get Activity Instance List Latest
        /// 获取最近的节点实例
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        internal List<ActivityInstanceEntity> GetActivityInstanceList(string processGUID,
            string version,
            string activityGUID)
        {
            ActivityInstanceEntity activityInstance = null;
            var sql = @"SELECT 
                                AI.* 
                            FROM WfActivityInstance AI
                            INNER JOIN WfProcessInstance PI 
                                ON AI.ProcessInstanceID = PI.ID
                            WHERE PI.ProcessState = 2 
                                AND PI.ProcessGUID = @processGUID
                                AND PI.Version=@version
                                AND AI.ActivityGUID = @activityGUID
                                AND (AI.ActivityState=1 OR AI.ActivityState=2)";
            var list = Repository.Query<ActivityInstanceEntity>(
                sql,
                new
                {
                    processGUID = processGUID,
                    version = version,
                    activityGUID = activityGUID
                }).ToList();

            return list;
        }

        /// <summary>
        /// Get activity instances with running status
        /// 获取运行状态的活动实例
        /// </summary>
        /// <param name="activityGUID"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetActivityRunning(int processInstanceID,
            string activityGUID,
            IDbSession session)
        {
            return GetActivityByState(processInstanceID, activityGUID, ActivityStateEnum.Running, session);
        }

        /// <summary>
        /// Obtain activity instances based on states
        /// 根据状态获取活动实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="activityGUID"></param>
        /// <param name="activityState"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Retrieve nodes that have already completed running
        /// 获取已经运行完成的节点
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <returns></returns>
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
        /// Obtain instances of activities that have already been completed in the previous step
        /// 获取上一步已经完成活动的实例
        /// </summary>
        /// <param name="runningNode"></param>
        /// <param name="previousActivityGUID"></param>
        /// <returns></returns>
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
        /// Obtain instances of activities that have already been completed in the previous step
        /// 获取上一步已经完成活动的实例
        /// </summary>
        /// <param name="runningNode"></param>
        /// <param name="previousActivityGUID">previous activity guid</param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetPreviousActivityInstanceSimple(ActivityInstanceEntity runningNode,
            string previousActivityGUID,
            IDbSession session)
        {
            ActivityInstanceEntity originalRunningNode = null;
            var processInstanceID = runningNode.ProcessInstanceID;

            //确定当前运行节点的初始信息
            //Determine the initial information of the current running node
            if (runningNode.BackSrcActivityInstanceID != null)
            {
                //*****注意：当前节点的类型已经是退回后生成的节点
                //获取退回之前的初始节点创建人信息
                //Note: The current node type is already a node generated after being returned
                //Obtain the creator information of the initial node before returning it
                originalRunningNode = GetById(session.Connection, runningNode.BackOrgActivityInstanceID.Value, session.Transaction);
            }
            else
            {
                originalRunningNode = runningNode;
            }

            //获取上一步节点列表
            //Retrieve the previous node list
            var instanceList = GetActivityInstanceListCompletedSimple(processInstanceID, previousActivityGUID, session);

            //排除掉是包含已经退回过的非初始节点
            //Excluding non initial nodes that have already been returned
            var withoutBackSrcInfoList = instanceList.Where(a => a.BackSrcActivityInstanceID == null);

            //上一步节点的完成人与当前运行节点的创建人匹配
            //Match the person who completed the previous node with the person who created the current running node
            var previousList = withoutBackSrcInfoList.Where(a => a.EndedByUserID == originalRunningNode.CreatedByUserID).ToList();
            if (previousList.Count > 0)
            {
                return previousList[0];
            }
            else if (withoutBackSrcInfoList.Count() == 1)
            {
                //合并后的节点退回到某个分支，最后一个分支的完成人才是合并之后节点的创建人
                //所以此时按照EndedByUserID和CreatedByUserID的比对是不靠谱的。

                //The merged node is returned to a branch, and the person who completed the last branch is the creator of the merged node
                //So at this point, comparing EndByUserID with CreatidByUserID is not reliable.
                return withoutBackSrcInfoList.ToList()[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtain activity instances with completion status
        /// 获取完成状态的活动实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
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
        /// Obtain activity instances with completion status
        /// 获取完成状态的活动实例
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="activityGUID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Retrieve activity instance information from task ID
        /// 由任务ID获取活动实例信息
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Retrieve activity instance information from task ID
        /// 由任务ID获取活动实例信息
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetByTask(int taskID)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var entity = GetByTask(taskID, session);
                return entity;
            }
        }

        /// <summary>
        /// Get the activity nodes running in the process instance
        /// 获取流程实例中运行的活动节点
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
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
        /// Get the activity nodes running in the process instance
        /// 获取流程实例中运行的活动节点
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="version"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Retrieve the list of completed child nodes under the main node
        /// 获取主节点下已经完成的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <returns></returns>
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
        /// Retrieve the list of completed child nodes under the main node
        /// 获取主节点下已经完成的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Retrieve completed multi instance nodes
        /// 获取已经完成的多实例子节点
        /// </summary>
        /// <param name="runningNode"></param>
        /// <param name="previousActivityGUID"></param>
        /// <returns></returns>
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
        /// Retrieve completed multi instance nodes
        /// 获取已经完成的多实例子节点
        /// </summary>
        /// <param name="runningNode"></param>
        /// <param name="previousActivityGUID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal List<ActivityInstanceEntity> GetPreviousParallelMultipleInstanceListCompleted(ActivityInstanceEntity runningNode,
            string previousActivityGUID,
            IDbSession session)
        {
            ActivityInstanceEntity originalRunningNode = null;
            var processInstanceID = runningNode.ProcessInstanceID;
            //确定当前运行节点的初始信息
            //Determine the initial information of the current running node
            if (runningNode.BackSrcActivityInstanceID != null)
            {
                //*****注意：当前节点的类型已经是退回后生成的节点
                //获取退回之前的初始节点创建人信息
                //Note: The current node type is already a node generated after being returned
                //Obtain the creator information of the initial node before returning it
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
            //Excluding non initial nodes that have already been returned
            var withoutBackSrcInfoList = instanceList.Where(a => a.BackSrcActivityInstanceID == null).ToList();

            //上一步节点的完成人与当前运行节点的创建人匹配
            //Match the person who completed the previous node with the person who created the current running node
            var firstMIChild = withoutBackSrcInfoList.First(a => a.EndedByUserID == originalRunningNode.CreatedByUserID);

            //根据主节点信息查询所有对应的子节点列表
            //Retrieve a list of all corresponding child nodes based on the master node information
            var previousList = GetMultipleInstanceListCompleted(firstMIChild.MIHostActivityInstanceID.Value, session);

            return previousList;
        }

        /// <summary>
        /// Determine whether the user is the assigned task user
        /// 判断用户是否是分配下来任务的用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
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
        /// Determine whether it is a signing together instance node
        /// 判断是否为会签实例节点 
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        internal Boolean IsMultipleInstanceChildren(ActivityInstanceEntity child)
        {
            return child.MIHostActivityInstanceID != null;
        }

        /// <summary>
        /// Filter activity instance list
        /// 过滤活动实例列表
        /// </summary>
        /// <param name="instanceList"></param>
        /// <returns></returns>
        private List<ActivityInstanceEntity> FilteredActivityInstanceInTheSameBatch(List<ActivityInstanceEntity> instanceList)
        {
            //如果有退回记录，则认为是同一批的运行模式，保证会签的通过率依然按照原来的CompleteOrder数值
            //否则，需要调用Resend()返送接口，而不用受到原来会签通过率CompleteOrder的限制。
            //此处以BackSrcActivityInstanceID作为监测点数据，就可以知道是否有退回批次
            //最后一次的退回批次BackSrcActivityInstanceID为最大值，如果没有退回则BackSrcActivityInstanceID为空

            //If there is a return record, it is considered to be the same batch's operating mode, ensuring that the pass rate of the countersignature still follows the original CompleteOrder value
            //Otherwise, the Resend() return interface needs to be called without being limited by the original pass rate of CompleteOrder.
            //By using BackSrcActivityInstanceID as the monitoring point data here, we can determine whether there are any returned batches
            //The BackSrcActivityInstanceID of the last batch returned is the maximum value. If there is no return, the BackSrcActivityInstanceID is empty
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
        /// Obtain multi instance nodes of signing together nodes based on activity status
        /// 根据活动状态获取会签节点的多实例节点
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="activityState"></param>
        /// <returns></returns>
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
        /// Retrieve the list of child nodes under the same batch of master nodes
        /// Including: filtering of rollback identification types that need to be filtered back and forth
        /// 获取同一批的主节点下的子节点列表记录
        /// 包括：需要过滤回退后的回退标识类型的过滤
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="activityState"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal IList<ActivityInstanceEntity> GetActivityMulitipleInstanceWithStateBatch(int processInstanceID,
            int mainActivityInstanceID,
            short? activityState,
            IDbSession session)
        {
            //取出处于多实例节点列表
            //Retrieve the list of nodes in multiple instances
            var instanceList = GetActivityMulitipleInstanceWithState(mainActivityInstanceID,
                processInstanceID,
                activityState,
                session).ToList<ActivityInstanceEntity>();

            instanceList = FilteredActivityInstanceInTheSameBatch(instanceList);
            return instanceList;
        }

        /// <summary>
        /// Obtain multi instance nodes of signing together nodes based on activity status
        /// 根据活动状态获取会签节点的多实例节点
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
        /// Query the number of branch instances
        /// 查询分支实例的个数
        /// </summary>
        /// <param name="splitActivityGUID"></param>
        /// <param name="splitActivityInstanceID"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Obtain the number of valid branches
        ///  获取有效的分支数目
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="splitGatewayInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Obtain the number of valid branches
        /// 获取有效的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        internal List<ActivityInstanceEntity> GetValidActivityInstanceListOfMI(int mainActivityInstanceID,
            int processInstanceID)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetValidActivityInstanceListOfMI(mainActivityInstanceID, processInstanceID, session);
            }
        }

        /// <summary>
        /// Obtain the number of valid branches
        /// 获取有效的子节点列表
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
            //Remove instances that have been returned before
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
                //Return the list of filtered out return nodes
                return validList;
            }

            return list;
        }
        #endregion

        #region Create activity instance 创建活动实例
        /// <summary>
        /// Create activity instance
        /// 创建活动实例的对象
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="appInstanceCode"></param>
        /// <param name="processGUID"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="activity"></param>
        /// <param name="runner"></param>
        /// <returns></returns>
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
        /// Calculate the expiration time of activity nodes
        /// 计算活动节点过期时间
        /// XmlConvert.ToTimeSpan()
        /// https://stackoverflow.com/questions/12466188/how-do-i-convert-an-iso8601-timespan-to-a-c-sharp-timespan
        /// </summary>
        /// <param name="boundaryList"></param>
        /// <param name="createdDateTime"></param>
        /// <returns></returns>
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
        /// Copy child nodes based on the master node
        /// 根据主节点复制子节点
        /// </summary>
        /// <param name="main"></param>
        /// <returns></returns>
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
        /// Create an object for the activity instance
        /// 创建活动实例的对象
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="appInstanceCode"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="processGUID"></param>
        /// <param name="activity"></param>
        /// <param name="backwardType"></param>
        /// <param name="backSrcActivityInstanceID"></param>
        /// <param name="backOrgActivityInstanceID"></param>
        /// <param name="runner"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity CreateBackwardActivityInstanceObject(string appName,
            string appInstanceID,
            string appInstanceCode,
            int processInstanceID,
            string processGUID,
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
            instance.ProcessGUID = processGUID;
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
        /// Update the number of tokens for the activity node
        /// 更新活动节点的Token数目
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void IncreaseTokensHad(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            ActivityInstanceEntity activityInstance = GetById(activityInstanceID);
            activityInstance.TokensHad += 1;
            Update(activityInstance, session);
        }
        #endregion

        #region Activity instance status setting 活动实例状态设置
        /// <summary>
        /// Activity instance read
        /// 活动实例被读取
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Read(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Running, runner, session);
        }

        /// <summary>
        /// Set activity instance status
        /// 设置活动实例状态
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="activityState"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
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
        /// Withdraw activity instance
        /// 撤销活动实例
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Withdraw(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceID, ActivityStateEnum.Withdrawed, runner, session);
        }

        /// <summary>
        /// Sendback activity instance
        /// 退回活动实例
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void SendBack(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceID, ActivityStateEnum.Sendbacked, runner, session);
        }

        /// <summary>
        /// Set the end status of the activity
        /// 设置活动结束状态
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="activityState"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
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
        /// Complete activity instance
        /// 活动实例完成
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal void Complete(int activityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceID, ActivityStateEnum.Completed, runner, session);
        }

        /// <summary>
        /// Update approval status to agree
        /// 更新审批状态为同意
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="session"></param>
        internal void SetApprovalStatus(int activityInstanceID, 
            IDbSession session)
        {
            var activityInstance = GetById(session.Connection, activityInstanceID, session.Transaction);
            //表示当前没有被审核过，才可以认为是审批同意状态
            //It can only be considered as approved if it has not been reviewed before
            if (activityInstance.ApprovalStatus == (short)ApprovalStatusEnum.Null)
            {
                activityInstance.ApprovalStatus = (short)ApprovalStatusEnum.Agreed;
                Update(activityInstance, session);
            }
        }

        /// <summary>
        /// Update the running nodes between branches and merges to block status
        /// 更新分支和合并之间的运行节点为阻止状态
        /// </summary>
        /// <param name="gatewayActivity"></param>
        /// <param name="gatewayInstance"></param>
        /// <param name="processModel"></param>
        /// <param name="session"></param>
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
        /// Update the activities in the activity to block status
        /// 更新活动里面的活动为阻止状态
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="taskActivityList"></param>
        /// <param name="session"></param>
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

        #region Activity instance record maintenance 活动实例记录维护
        /// <summary>
        /// Insert activity instance
        /// 插入活动实例
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
        /// Update activity instance
        /// 更新活动实例
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="session"></param>
        internal void Update(ActivityInstanceEntity entity,
            IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// Update activity instance
        /// 更新活动实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        internal void Update(IDbConnection conn, ActivityInstanceEntity entity, IDbTransaction trans)
        {
            Repository.Update(conn, entity, trans);
        }

        /// <summary>
        /// Cancel activity instance
        /// 取消节点运行
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
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
        /// Update the status of unfinished nodes in the countersignature node
        /// 更新会签节点中未办理完成的节点状态
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="session"></param>
        /// <param name="runner"></param>
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
        /// Withdraw the master node and its multiple instance nodes below it
        /// 撤销主节点及其下面的多实例子节点
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="session"></param>
        /// <param name="runner"></param>
        internal void WithdrawMainIncludedChildNodes(int mainActivityInstanceID, IDbSession session, WfAppRunner runner)
        {
            //先更新主节点状态为撤销状态
            //Update the status of the main node to revoked first
            SetActivityState(mainActivityInstanceID, ActivityStateEnum.Withdrawed, runner, session);

            //再更新主节点下的多实例子节点状态为撤销状态
            //Update the status of multiple instance nodes under the main node to withdrawn state again
            WithdrawMultipleInstance(mainActivityInstanceID, session, runner);
        }

        /// <summary>
        /// Update the node status of the counter signature processing node
        /// Notes:Nodes in a ready or suspended state can be withdrawn 
        /// 更新会签办理节点的节点状态
        /// 备注:准备或挂起状态的节点可以撤销
        /// </summary>
        /// <param name="mainActivityInstanceID"></param>
        /// <param name="session"></param>
        /// <param name="runner"></param>
        private void WithdrawMultipleInstance(int mainActivityInstanceID, IDbSession session, WfAppRunner runner)
        {
            var sql = @"UPDATE WfActivityInstance 
                        SET ActivityState=@activityState, 
                        LastUpdatedByUserID=@lastUpdatedByUserID,
                        LastUpdatedByUserName=@lastUpdatedByUserName,
                        LastUpdatedDateTime=@lastUpdatedDateTime
                        WHERE MIHostActivityInstanceID=@mainActivityInstanceID
                            AND (ActivityState=1 OR ActivityState=5)";      

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
        /// Put the node back in a suspended state
        /// Notes: Used when revoking the last sub node of the co signature
        /// 重新使节点处于挂起状态
        /// 说明：会签最后一个子节点撤销时候用到
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="session"></param>
        /// <param name="runner"></param>
        internal void Resuspend(int activityInstanceID, IDbSession session, WfAppRunner runner)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Suspended, runner, session);
        }

        /// <summary>
        /// Put the node back in a suspended state
        /// Notes: Used when revoking the last sub node of the co signature
        /// 重新使节点处于挂起状态
        /// 说明：会签最后一个子节点撤销时候用到
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="session"></param>
        /// <param name="runner"></param>
        internal void Rerun(int activityInstanceID, IDbSession session, WfAppRunner runner)
        {
            SetActivityState(activityInstanceID, ActivityStateEnum.Running, runner, session);
        }

        /// <summary>
        /// Delete activity instance
        /// 删除活动实例
        /// </summary>
        /// <param name="activityInstanceID"></param>
        /// <param name="session"></param>
        internal void Delete(int activityInstanceID,
            IDbSession session = null)
        {
            Repository.Delete<ActivityInstanceEntity>(session.Connection,
                activityInstanceID,
                session.Transaction);
        }
        #endregion

        #region Activity Instance Approval Status 活动实例审批状态
        /// <summary>
        /// Agree
        /// 同意
        /// </summary>
        /// <param name="taskID"></param>
        internal void Agree(int taskID)
        {
            var activityInstance = GetByTask(taskID);
            activityInstance.ApprovalStatus = (short)ApprovalStatusEnum.Agreed;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Update(activityInstance, session);
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
        /// Refuse
        /// 拒绝
        /// </summary>
        /// <param name="taskID"></param>
        internal void Refuse(int taskID)
        {
            var activityInstance = GetByTask(taskID);
            activityInstance.ApprovalStatus = (short)ApprovalStatusEnum.Refused;
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();
                Update(activityInstance, session);
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
        /// Check the pass type of the current node
        ///  检查当前节点的通过类型
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
                    //Whether the judgment is passed in the case of multiple instances of co signing
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
        /// Check the satisfaction of the pass rate of the co signing nodes
        /// 检查会签节点通过率的满足情况
        /// </summary>
        /// <param name="currentActivityInstance"></param>
        /// <param name="mainActivityInstance"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        internal NodePassedTypeEnum CheckMIPassRateInfo(ActivityInstanceEntity currentActivityInstance,
            ActivityInstanceEntity mainActivityInstance,
            IDbSession session)
        {
            var passedType = NodePassedTypeEnum.Default;
            var childActivityInstanceList = GetValidActivityInstanceListOfMI(mainActivityInstance.ID, mainActivityInstance.ProcessInstanceID,
                session);

            //参与通过率类型计算的数目列举
            //List of numbers involved in the calculation of pass rate types
            var agreedCount = childActivityInstanceList.Where(a => a.ApprovalStatus == (short)ApprovalStatusEnum.Agreed).Count();
            var refusedCount = childActivityInstanceList.Where(a => a.ApprovalStatus == (short)ApprovalStatusEnum.Refused).Count();

            if (mainActivityInstance.MergeType == (short)MergeTypeEnum.Sequence)
            {
                if (mainActivityInstance.CompareType.Value == (short)CompareTypeEnum.Count)
                {
                    //通过率类型为:个数
                    //The pass rate type is: number
                    var thresholdCount = mainActivityInstance.CompleteOrder.Value;
                    var allCount = childActivityInstanceList.Max(a => a.CompleteOrder).Value;       //Total number of approvals 总共审批数目
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
                        //Waiting for approval from other co signatories
                        passedType = NodePassedTypeEnum.NeedToBeMoreApproved;
                    }
                    return passedType;
                }
                else if (mainActivityInstance.CompareType.Value == (short)CompareTypeEnum.Percentage)
                {
                    //按照百分比数目比较
                    //Compare by percentage number
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
                    //The pass rate type is: number
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
                        //Waiting for approval from other co signatories
                        passedType = NodePassedTypeEnum.NeedToBeMoreApproved;
                    }
                    return passedType;
                }
                else if (mainActivityInstance.CompareType.Value == (short)CompareTypeEnum.Percentage)
                {
                    //按照百分比数目比较
                    //Compare by percentage number
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
        /// Obtain the approval rate type of the countersignature node
        /// 获取会签节点的审批通过率类型
        /// </summary>
        /// <param name="currentActivityInstance"></param>
        /// <param name="mainActivityInstance"></param>
        /// <param name="session"></param>
        /// <returns></returns>
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
