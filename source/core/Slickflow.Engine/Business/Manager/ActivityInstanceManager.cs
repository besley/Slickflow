
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
        /// Retrieve activity instances based on Id
        /// 根据ID获取活动实例
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        internal ActivityInstanceEntity GetById(int activityInstanceId)
        {
            try
            {
                return Repository.GetById<ActivityInstanceEntity>(activityInstanceId);
            }
            catch (System.Exception e)
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("activityinstancemanager.getbyid.error", e.Message),
                    e);
            }
        }

        /// <summary>
        /// Retrieve activity instances based on Id
        /// 根据ID获取活动实例
        /// </summary>
        internal ActivityInstanceEntity GetById(IDbConnection conn, int activityInstanceId, IDbTransaction trans)
        {
            return Repository.GetById<ActivityInstanceEntity>(conn, activityInstanceId, trans);
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
            var taskId = runner.TaskId;
            taskView = null;    //default value;

            //根据TaskId获取
            //Obtain based on TaskId
            var aim = new ActivityInstanceManager();
            var tm = new TaskManager();

            ActivityInstanceEntity runningNode = null;
            if (taskId != null && taskId.Value > 0)
            {
                taskView = tm.GetTaskView(session.Connection, taskId.Value, session.Transaction);
                runningNode = aim.GetById(session.Connection, taskView.ActivityInstanceId, session.Transaction);
                return runningNode;
            }

            //没有传递TaskID参数，进行查询
            //No TaskId parameter passed for query
            var activityInstanceList = aim.GetRunningActivityInstanceList(runner.AppInstanceId, runner.ProcessId, runner.Version, session).ToList();
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
                taskView = tm.GetTaskOfMineByActivityInstance(session.Connection, runningNode.Id, runner.UserId, session.Transaction);
            }
            else if (activityInstanceList.Count > 1)
            {
                //并行模式处理
                //根据当前执行者身份取出(他或她)要办理的活动实例（并行模式下有多个处于待办或运行状态的节点）
                //Parallel mode processing
                //Retrieve the activity instance to be processed based on the current executor's identity (there are multiple nodes in pending or running status in parallel mode)
                foreach (var ai in activityInstanceList)
                {
                    if (ai.AssignedUserIds == runner.UserId)
                    {
                        runningNode = ai;
                        break;
                    }
                }

                if (runningNode != null)
                {
                    //获取taskview
                    //Get TaskView
                    taskView = tm.GetTaskOfMineByActivityInstance(session.Connection, runningNode.Id, runner.UserId, session.Transaction);
                }
                else
                {
                    //当前用户的待办任务不唯一，抛出异常，需要TaskId唯一界定
                    //The current user's pending tasks are not unique, and an exception is thrown. TaskId must be uniquely defined
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
        internal ActivityInstanceEntity GetBackSrcActivityInstance(int currentActivityInstanceId)
        {
            var currentActivityInstance = GetById(currentActivityInstanceId);
            var backSrcActivityInstance = GetById(currentActivityInstance.BackSrcActivityInstanceId.Value);

            return backSrcActivityInstance;
        }

        /// <summary>
        /// Determine whether it is a processing task for a certain user
        /// 判断是否是某个用户的办理任务
        /// </summary>
        internal bool IsMineTask(ActivityInstanceEntity entity,
            string userId)
        {
            bool isMine = entity.AssignedUserIds.Contains(userId.ToString());
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
        internal IList<ActivityInstanceEntity> GetActivityInstanceList(int processInstanceId,
            IDbSession session)
        {
            //var list = Repository.GetAll<ActivityInstanceEntity>(session.Connection, session.Transaction)
            //            .Where<ActivityInstanceEntity>(a => a.ProcessInstanceId == processInstanceId)
            //            .ToList();
            var sql = @"SELECT 
                            * 
                        FROM wf_activity_instance 
                        WHERE process_instance_id = @processInstanceId 
                            ORDER BY id";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceId = processInstanceId
                },
                session.Transaction).ToList();

            return list;
        }

        /// <summary>
        /// Get Activity Instance Latest
        /// 获取活动节点实例
        /// </summary>
        internal ActivityInstanceEntity GetActivityInstanceLatest(int processInstanceId,
            string activityId)
        {
            ActivityInstanceEntity activityInstance = null;
            var session = SessionFactory.CreateSession();
            try
            {
                activityInstance = GetActivityInstanceLatest(processInstanceId, activityId, session);
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
        internal ActivityInstanceEntity GetActivityInstanceLatest(int processInstanceId,
            string activityId,
            IDbSession session)
        {
            ActivityInstanceEntity activityInstance = null;
            //var list = Repository.GetAll<ActivityInstanceEntity>(session.Connection, session.Transaction)
            //    .Where<ActivityInstanceEntity>(
            //        a => a.ProcessInstanceId == processInstanceId &&
            //            a.ActivityId == activityId
            //        )
            //    .OrderByDescending(a => a.Id)
            //    .ToList();

            var sql = @"SELECT 
                            * 
                        FROM wf_activity_instance 
                        WHERE process_instance_id = @processInstanceId
                            AND activity_id = @activityId
                            ORDER BY id DESC";
            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceId = processInstanceId,
                    activityId = activityId
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
        internal ActivityInstanceEntity GetActivityInstanceLatest(string appInstanceId,
            string processId,
            string activityId)
        {
            var sql = @"SELECT 
                            AI.*
                        FROM wf_activity_instance AI
                        INNER JOIN wf_process_instance PI
                            ON AI.process_instance_id = PI.id
                        WHERE PI.process_state = 2 
                            AND AI.app_instance_id = @appInstanceId 
                            AND AI.process_id = @processId
                            AND AI.activity_id = @activityId";
            var list = Repository.Query<ActivityInstanceEntity>(
                sql,
                new
                {
                    appInstanceId = appInstanceId,
                    processId = processId,
                    activityId = activityId
                }).ToList();

            if (list.Count > 0)
            {
                var matchedActivityInstance = list[0];
                var fullActivityInstance = Repository.GetById<ActivityInstanceEntity>(matchedActivityInstance.Id);
                return fullActivityInstance;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Activity Instance List Latest
        /// 获取最近的节点实例
        /// </summary>
        internal List<ActivityInstanceEntity> GetActivityInstanceList(string processId,
            string version,
            string activityId)
        {
            ActivityInstanceEntity activityInstance = null;
            var sql = @"SELECT 
                            AI.*
                        FROM wf_activity_instance AI
                        INNER JOIN wf_process_instance PI 
                            ON AI.process_instance_id = PI.id
                        WHERE PI.process_state = 2 
                            AND PI.process_id = @processId
                            AND PI.version=@version
                            AND AI.activity_id = @activityId
                            AND (AI.activity_state=1 OR AI.activity_state=2)";
            var list = Repository.Query<ActivityInstanceEntity>(
                sql,
                new
                {
                    processId = processId,
                    version = version,
                    activityId = activityId
                }).ToList();

            return list;
        }

        /// <summary>
        /// Get activity instances with running status
        /// 获取运行状态的活动实例
        /// </summary>
        internal ActivityInstanceEntity GetActivityRunning(int processInstanceId,
            string activityId,
            IDbSession session)
        {
            return GetActivityByState(processInstanceId, activityId, ActivityStateEnum.Running, session);
        }

        /// <summary>
        /// Obtain activity instances based on states
        /// 根据状态获取活动实例
        /// </summary>
        internal ActivityInstanceEntity GetActivityByState(int processInstanceId,
            string activityId,
            ActivityStateEnum activityState,
            IDbSession session)
        {
            ActivityInstanceEntity entity = null;
            var sql = @"SELECT 
                            * 
                        FROM wf_activity_instance 
                        WHERE process_instance_id = @processInstanceId 
                            AND activity_id = @activityId 
                            AND activity_state = @state
                        ORDER BY id DESC";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceId = processInstanceId,
                    activityId = activityId,
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
        internal List<ActivityInstanceEntity> GetActivityInstanceListCompleted(string appInstanceId,
            string processId)
        {
            //activityState: 4-completed（完成）
            var sql = @"SELECT 
                            AI.* 
                        FROM wf_activity_instance AI
                        INNER JOIN wf_process_instance PI 
                            ON AI.process_instance_id = PI.id
                        WHERE PI.process_state = 2 
                            AND AI.app_instance_id = @appInstanceId 
                            AND AI.process_id = @processId
                            AND AI.activity_state = 4";

            var list = Repository.Query<ActivityInstanceEntity>(
                sql,
                new
                {
                    appInstanceId = appInstanceId,
                    processId = processId
                }).ToList();
            return list;
        }

        /// <summary>
        /// Obtain instances of activities that have already been completed in the previous step
        /// 获取上一步已经完成活动的实例
        /// </summary>
        internal ActivityInstanceEntity GetPreviousActivityInstanceSimple(ActivityInstanceEntity runningNode,
            string previousActivityId)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                return GetPreviousActivityInstanceSimple(runningNode, previousActivityId, session);
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
        internal ActivityInstanceEntity GetPreviousActivityInstanceSimple(ActivityInstanceEntity runningNode,
            string previousActivityId,
            IDbSession session)
        {
            ActivityInstanceEntity originalRunningNode = null;
            var processInstanceId = runningNode.ProcessInstanceId;

            //确定当前运行节点的初始信息
            //Determine the initial information of the current running node
            if (runningNode.BackSrcActivityInstanceId != null)
            {
                //*****注意：当前节点的类型已经是退回后生成的节点
                //获取退回之前的初始节点创建人信息
                //Note: The current node type is already a node generated after being returned
                //Obtain the creator information of the initial node before returning it
                originalRunningNode = GetById(session.Connection, runningNode.BackOrgActivityInstanceId.Value, session.Transaction);
            }
            else
            {
                originalRunningNode = runningNode;
            }

            //获取上一步节点列表
            //Retrieve the previous node list
            var instanceList = GetActivityInstanceListCompletedSimple(processInstanceId, previousActivityId, session);

            //排除掉是包含已经退回过的非初始节点
            //Excluding non initial nodes that have already been returned
            var withoutBackSrcInfoList = instanceList.Where(a => a.BackSrcActivityInstanceId == null);

            //上一步节点的完成人与当前运行节点的创建人匹配
            //Match the person who completed the previous node with the person who created the current running node
            var previousList = withoutBackSrcInfoList.Where(a => a.EndedUserId == originalRunningNode.CreatedUserId).ToList();
            if (previousList.Count > 0)
            {
                return previousList[0];
            }
            else if (withoutBackSrcInfoList.Count() == 1)
            {
                //合并后的节点退回到某个分支，最后一个分支的完成人才是合并之后节点的创建人
                //所以此时按照EndedUserID和CreatedUserID的比对是不靠谱的。

                //The merged node is returned to a branch, and the person who completed the last branch is the creator of the merged node
                //So at this point, comparing EndByUserId with CreatidByUserId is not reliable.
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
        internal IList<ActivityInstanceEntity> GetActivityInstanceListCompletedSimple(int processInstanceId,
            string activityId)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var list = GetActivityInstanceListCompletedSimple(processInstanceId, activityId, session);
                return list;
            }
        }

        /// <summary>
        /// Obtain activity instances with completion status
        /// 获取完成状态的活动实例
        /// </summary>
        internal IList<ActivityInstanceEntity> GetActivityInstanceListCompletedSimple(int processInstanceId,
            string activityId,
            IDbSession session)
        {
            //activityState: 4-completed（完成）
            var sql = @"SELECT 
                            * 
                        FROM wf_activity_instance 
                        WHERE process_instance_id = @processInstanceId 
                            AND activity_id = @activityId 
                            AND activity_state = @state 
                        ORDER BY id DESC";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    processInstanceId = processInstanceId,
                    activityId = activityId,
                    state = (short)ActivityStateEnum.Completed
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// Retrieve activity instance information from task Id
        /// 由任务ID获取活动实例信息
        /// </summary>
        internal ActivityInstanceEntity GetByTask(int taskId,
            IDbSession session)
        {
            ActivityInstanceEntity entity = null;
            var sql = @"SELECT 
                            AI.* 
                        FROM wf_activity_instance AI
                        INNER JOIN wf_task T 
                            ON AI.id = T.activity_instance_id
                        WHERE T.id = @taskId";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    taskId = taskId
                },
                session.Transaction).ToList();
            if (list.Count == 1)
            {
                entity = list[0];
            }
            return entity;
        }

        /// <summary>
        /// Retrieve activity instance information from task Id
        /// 由任务ID获取活动实例信息
        /// </summary>
        internal ActivityInstanceEntity GetByTask(int taskId)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var entity = GetByTask(taskId, session);
                return entity;
            }
        }

        /// <summary>
        /// Get the activity nodes running in the process instance
        /// 获取流程实例中运行的活动节点
        /// </summary>
        internal IEnumerable<ActivityInstanceEntity> GetRunningActivityInstanceList(string appInstanceId,
            string processId,
            string version = null)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetRunningActivityInstanceList(appInstanceId, processId, version, session);
            }
        }

        /// <summary>
        /// Get the activity nodes running in the process instance
        /// 获取流程实例中运行的活动节点
        /// </summary>
        internal IEnumerable<ActivityInstanceEntity> GetRunningActivityInstanceList(string appInstanceId,
            string processId,
            string version,
            IDbSession session)
        {
            //activityState: 1-ready（准备）, 2-running（）运行；
            if (string.IsNullOrEmpty(version)) version = "1";
            var sql = @"SELECT 
                            AI.* 
                        FROM wf_activity_instance AI 
                        INNER JOIN wf_process_instance PI 
                            ON AI.process_instance_id = PI.id
                        WHERE (AI.activity_state=1 OR AI.activity_state=2) 
                            AND PI.process_state = 2 
                            AND PI.version = @version 
                            AND AI.app_instance_id = @appInstanceId 
                            AND AI.process_id = @processId";

            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    appInstanceId = appInstanceId,
                    processId = processId,
                    version = version
                },
                session.Transaction);

            return list;
        }

        /// <summary>
        /// Retrieve the list of completed child nodes under the main node
        /// 获取主节点下已经完成的子节点列表
        /// </summary>
        internal List<ActivityInstanceEntity> GetMultipleInstanceListCompleted(int mainActivityInstanceId)
        {
            var session = SessionFactory.CreateSession();
            try
            {
                return GetMultipleInstanceListCompleted(mainActivityInstanceId, session);
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
        internal List<ActivityInstanceEntity> GetMultipleInstanceListCompleted(int mainActivityInstanceId,
            IDbSession session)
        {
            //activityState: 4-completed（完成）
            var sql = @"SELECT 
                            AI.* 
                        FROM wf_activity_instance AI
                        INNER JOIN wf_process_instance PI 
                            ON AI.process_instance_id = PI.id
                        WHERE PI.process_state = 2 
                            AND AI.main_activity_instance_id = @mainActivityInstanceId
                            AND AI.activity_state = 4";
            var list = Repository.Query<ActivityInstanceEntity>(session.Connection,
                sql,
                new
                {
                    mainActivityInstanceId = mainActivityInstanceId
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// Retrieve completed multi instance nodes
        /// 获取已经完成的多实例子节点
        /// </summary>
        internal List<ActivityInstanceEntity> GetPreviousParallelMultipleInstanceListCompleted(ActivityInstanceEntity runningNode,
            string previousActivityId)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var previousList = GetPreviousParallelMultipleInstanceListCompleted(runningNode, previousActivityId, session);
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
        internal List<ActivityInstanceEntity> GetPreviousParallelMultipleInstanceListCompleted(ActivityInstanceEntity runningNode,
            string previousActivityId,
            IDbSession session)
        {
            ActivityInstanceEntity originalRunningNode = null;
            var processInstanceId = runningNode.ProcessInstanceId;
            //确定当前运行节点的初始信息
            //Determine the initial information of the current running node
            if (runningNode.BackSrcActivityInstanceId != null)
            {
                //*****注意：当前节点的类型已经是退回后生成的节点
                //获取退回之前的初始节点创建人信息
                //Note: The current node type is already a node generated after being returned
                //Obtain the creator information of the initial node before returning it
                originalRunningNode = GetById(session.Connection, runningNode.BackOrgActivityInstanceId.Value, session.Transaction);
            }
            else
            {
                originalRunningNode = runningNode;
            }

            //加签情况下，如果没有加签给别人，此时节点还是普通任务节点
            //只有加签给别人，才会真正生成加签多实例节点，此处移除对 MIHostActivityInstanceId IS NOT NULL 的判断过滤
            //In the case of signing, if no one else has signed, the node is still a regular task node
            //Only by adding signatures to others can multi instance nodes with signatures be truly generated. Here, the filtering of MIHostActiveInstanceId IS NOT NULL judgment is removed
            var sql = @"SELECT 
                            * 
                        FROM wf_activity_instance 
                        WHERE process_instance_id = @processInstanceId 
                            AND activity_id = @activityId 
                            AND activity_state = 4
                        ORDER BY id DESC";
            //var sql = @"SELECT 
            //                * 
            //            FROM wf_activity_instance 
            //            WHERE process_instance_id = @processInstanceId 
            //                AND activity_id = @activityId 
            //                AND activity_state = 4
            //                AND main_activity_instance_id IS NOT NULL 
            //            ORDER BY id DESC";
            var instanceList = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    processInstanceId = runningNode.ProcessInstanceId,
                    activityId = previousActivityId
                },
                session.Transaction).ToList();

            //排除掉是包含已经退回过的非初始节点
            //Excluding non initial nodes that have already been returned
            var withoutBackSrcInfoList = instanceList.Where(a => a.BackSrcActivityInstanceId == null).ToList();

            //上一步节点的完成人与当前运行节点的创建人匹配
            //Match the person who completed the previous node with the person who created the current running node
            var firstMIChild = withoutBackSrcInfoList.First(a => a.EndedUserId == originalRunningNode.CreatedUserId);

            //根据主节点信息查询所有对应的子节点列表
            //Retrieve a list of all corresponding child nodes based on the master node information
            var previousList = GetMultipleInstanceListCompleted(firstMIChild.MainActivityInstanceId.Value, session);

            return previousList;
        }

        /// <summary>
        /// Determine whether the user is the assigned task user
        /// 判断用户是否是分配下来任务的用户
        /// </summary>
        private bool IsAssignedUserInActivityInstance(ActivityInstanceEntity entity,
            int userId)
        {
            var assignedUserIDs = entity.AssignedUserIds;
            var userList = assignedUserIDs.Split(',');
            var single = userList.FirstOrDefault<string>(a => a == userId.ToString());
            if (!string.IsNullOrEmpty(single))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determine whether it is a signing together instance node
        /// 判断是否为会签实例节点 
        /// </summary>
        internal Boolean IsMultipleInstanceChildren(ActivityInstanceEntity child)
        {
            return child.MainActivityInstanceId != null;
        }

        /// <summary>
        /// Filter activity instance list
        /// 过滤活动实例列表
        /// </summary>
        private List<ActivityInstanceEntity> FilteredActivityInstanceInTheSameBatch(List<ActivityInstanceEntity> instanceList)
        {
            //如果有退回记录，则认为是同一批的运行模式，保证会签的通过率依然按照原来的CompleteOrder数值
            //否则，需要调用Resend()返送接口，而不用受到原来会签通过率CompleteOrder的限制。
            //此处以BackSrcActivityInstanceID作为监测点数据，就可以知道是否有退回批次
            //最后一次的退回批次BackSrcActivityInstanceID为最大值，如果没有退回则BackSrcActivityInstanceID为空

            //If there is a return record, it is considered to be the same batch's operating mode, ensuring that the pass rate of the countersignature still follows the original CompleteOrder value
            //Otherwise, the Resend() return interface needs to be called without being limited by the original pass rate of CompleteOrder.
            //By using BackSrcActivityInstanceId as the monitoring point data here, we can determine whether there are any returned batches
            //The BackSrcActivityInstanceId of the last batch returned is the maximum value. If there is no return, the BackSrcActivityInstanceId is empty
            var maxBackSrcActivityInstanceId = instanceList.Max<ActivityInstanceEntity>(a => a.BackSrcActivityInstanceId);
            if (maxBackSrcActivityInstanceId != null)
            {
                var childrenList = instanceList.Where<ActivityInstanceEntity>(a => a.BackSrcActivityInstanceId == maxBackSrcActivityInstanceId.Value).ToList();
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
        internal List<ActivityInstanceEntity> GetActivityMulitipleInstanceWithState(int mainActivityInstanceId,
            int processInstanceId,
            short? activityState = null)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var childActivityInstance =  GetActivityMulitipleInstanceWithState(mainActivityInstanceId, 
                    processInstanceId, activityState, session);

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
        internal IList<ActivityInstanceEntity> GetActivityMulitipleInstanceWithStateBatch(int processInstanceId,
            int mainActivityInstanceId,
            short? activityState,
            IDbSession session)
        {
            //取出处于多实例节点列表
            //Retrieve the list of nodes in multiple instances
            var instanceList = GetActivityMulitipleInstanceWithState(mainActivityInstanceId,
                processInstanceId,
                activityState,
                session).ToList<ActivityInstanceEntity>();

            instanceList = FilteredActivityInstanceInTheSameBatch(instanceList);
            return instanceList;
        }

        /// <summary>
        /// Obtain multi instance nodes of signing together nodes based on activity status
        /// 根据活动状态获取会签节点的多实例节点
        /// </summary>
        internal List<ActivityInstanceEntity> GetActivityMulitipleInstanceWithState(int mainActivityInstanceId,
            int processInstanceId,
            short? activityState,
            IDbSession session)
        {
            //activityState: 1-ready（准备）, 2-running（）运行；
            var sql = @"SELECT 
                            * 
                        FROM wf_activity_instance 
                        WHERE main_activity_instance_id = @activityInstanceId 
                            AND process_instance_id = @processInstanceId
                            ";
            if (activityState.HasValue)
            {
                sql += " AND activity_state = @activityState ";
            }
            sql += " ORDER BY complete_order";

            var list = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    activityInstanceId = mainActivityInstanceId,
                    processInstanceId = processInstanceId,
                    activityState = activityState
                },
                session.Transaction).ToList();

            return list;
        }


        /// <summary>
        /// Query the number of branch instances
        /// 查询分支实例的个数
        /// </summary>
        internal int GetGatewayInstanceCountByTransition(string splitActivityId,
            int splitActivityInstanceId,
            int processInstanceId,
            IDbSession session)
        {
            var sql = @"SELECT 
                            * 
                        FROM wf_transition_instance
                        WHERE process_instance_id=@processInstanceId 
                            AND from_activity_id=@fromActivityId
                            AND from_activity_instance_id=@fromActivityInstanceId";
            var list = Repository.Query<TransitionInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    fromActivityId = splitActivityId,
                    fromActivityInstanceId = splitActivityInstanceId,
                    processInstanceId = processInstanceId
                },
                session.Transaction).ToList();
            return list.Count();
        }

        /// <summary>
        /// Obtain the number of valid branches
        ///  获取有效的分支数目
        /// </summary>
        internal List<ActivityInstanceEntity> GetValidSplitedActivityInstanceList(int processInstanceId, 
            int splitGatewayInstanceId, 
            IDbSession session)
        {
            var sql = @"SELECT 
                            A.*
                        FROM wf_activity_instance A 
                        INNER JOIN wf_transition_instance T 
                            ON A.id = T.to_activity_instance_id
                        WHERE (A.activity_state=1
                                  OR A.activity_state=2
                                  OR A.activity_state=4
                                  OR A.activity_state=5) 
                            AND T.from_activity_instance_id = @fromActivityInstanceId
                        ";
            var list = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    fromActivityInstanceId = splitGatewayInstanceId
                },
                session.Transaction).ToList();
            return list;
        }

        /// <summary>
        /// Obtain the number of valid branches
        /// 获取有效的子节点列表
        /// </summary>
        internal List<ActivityInstanceEntity> GetValidActivityInstanceListOfMI(int mainActivityInstanceId,
            int processInstanceId)
        {
            using (var session = SessionFactory.CreateSession())
            {
                return GetValidActivityInstanceListOfMI(mainActivityInstanceId, processInstanceId, session);
            }
        }

        /// <summary>
        /// Obtain the number of valid branches
        /// 获取有效的子节点列表
        /// </summary>
        internal List<ActivityInstanceEntity> GetValidActivityInstanceListOfMI(int mainActivityInstanceId,
            int processInstanceId,
            IDbSession session)
        {
            var sql = @"SELECT 
                         *
                        FROM wf_activity_instance
                        WHERE (activity_state=1
                                  OR activity_state = 2
                                  OR activity_state = 4
                                  OR activity_state = 5) 
	                        AND main_activity_instance_id = @mainActivityInstanceId
                            AND process_instance_id = @processInstanceId
                        ";
            var list = Repository.Query<ActivityInstanceEntity>(
                session.Connection,
                sql,
                new
                {
                    mainActivityInstanceId = mainActivityInstanceId,
                    processInstanceId = processInstanceId
                },
                session.Transaction).ToList();

            //去除掉有退回过的实例
            //Remove instances that have been returned before
            var backList = new List<int>();
            foreach (var child in list)
            {
                if (child.BackSrcActivityInstanceId != null)
                {
                    if (backList.Any(a=>a == child.BackSrcActivityInstanceId.Value) == false)
                        backList.Add(child.BackSrcActivityInstanceId.Value);
                }

                if (child.BackOrgActivityInstanceId != null)
                {
                    if (backList.Any(a=>a==child.BackOrgActivityInstanceId.Value) == false)
                        backList.Add(child.BackOrgActivityInstanceId.Value);
                }
            }

            if (backList.Count > 0)
            {
                int[] backArray = backList.ToArray();
                var validList = list.Where(a => !backArray.Contains(a.Id)).ToList();
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
        internal ActivityInstanceEntity CreateActivityInstanceObject(string appName,
            string appInstanceId,
            string appInstanceCode,
            string processId,
            int processInstanceId,
            Activity activity,
            WfAppRunner runner)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityId = activity.ActivityId;
            instance.ActivityName = activity.ActivityName;
            instance.ActivityCode = activity.ActivityCode;
            instance.ActivityType = (short)activity.ActivityType;
            instance.WorkItemType = (short)activity.WorkItemType;
            instance.GatewayDirectionTypeId = activity.GatewayDetail != null ? (short)activity.GatewayDetail.DirectionType : null;
            instance.ProcessId = processId;
            instance.AppName = appName;
            instance.AppInstanceId = appInstanceId;
            instance.AppInstanceCode = appInstanceCode;
            instance.ProcessInstanceId = processInstanceId;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedUserId = runner.UserId;
            instance.CreatedUserName = runner.UserName;
            instance.CreatedDateTime = System.DateTime.UtcNow;
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
            instance.ActivityId = main.ActivityId;
            instance.ActivityName = main.ActivityName;
            instance.ActivityCode = main.ActivityCode;
            instance.ActivityType = main.ActivityType;
            instance.WorkItemType = main.WorkItemType;
            instance.GatewayDirectionTypeId = main.GatewayDirectionTypeId;
            instance.ProcessId = main.ProcessId;
            instance.AppName = main.AppName;
            instance.AppInstanceId = main.AppInstanceId;
            instance.AppInstanceCode = main.AppInstanceCode;
            instance.ProcessInstanceId = main.ProcessInstanceId;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedUserId = main.CreatedUserId;
            instance.CreatedUserName = main.CreatedUserName;
            instance.CreatedDateTime = System.DateTime.UtcNow;
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanNotRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// Create an object for the activity instance
        /// 创建活动实例的对象
        /// </summary>
        internal ActivityInstanceEntity CreateBackwardActivityInstanceObject(string appName,
            string appInstanceId,
            string appInstanceCode,
            int processInstanceId,
            string processId,
            Activity activity,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceId,
            int backOrgActivityInstanceId,
            WfAppRunner runner)
        {
            ActivityInstanceEntity instance = new ActivityInstanceEntity();
            instance.ActivityId = activity.ActivityId;
            instance.ActivityName = activity.ActivityName;
            instance.ActivityCode = activity.ActivityCode;
            instance.ActivityType = (short)activity.ActivityType;
            instance.WorkItemType = (short)activity.WorkItemType;
            instance.GatewayDirectionTypeId = activity.GatewayDetail != null ? (short)activity.GatewayDetail.DirectionType : null;
            instance.ProcessId = processId;
            instance.AppName = appName;
            instance.AppInstanceId = appInstanceId;
            instance.AppInstanceCode = appInstanceCode;
            instance.ProcessInstanceId = processInstanceId;
            instance.BackwardType = (short)backwardType;
            instance.BackSrcActivityInstanceId = backSrcActivityInstanceId;
            instance.BackOrgActivityInstanceId = backOrgActivityInstanceId;
            instance.TokensRequired = 1;
            instance.TokensHad = 1;
            instance.CreatedUserId = runner.UserId;
            instance.CreatedUserName = runner.UserName;
            instance.CreatedDateTime = System.DateTime.UtcNow;
            instance.ActivityState = (short)ActivityStateEnum.Ready;
            instance.CanNotRenewInstance = 0;

            return instance;
        }

        /// <summary>
        /// Update the number of tokens for the activity node
        /// 更新活动节点的Token数目
        /// </summary>
        internal void IncreaseTokensHad(int activityInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            ActivityInstanceEntity activityInstance = GetById(activityInstanceId);
            activityInstance.TokensHad += 1;
            Update(activityInstance, session);
        }
        #endregion

        #region Activity instance status setting 活动实例状态设置
        /// <summary>
        /// Activity instance read
        /// 活动实例被读取
        /// </summary>
        internal void Read(int activityInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            SetActivityState(activityInstanceId, ActivityStateEnum.Running, runner, session);
        }

        /// <summary>
        /// Set activity instance status
        /// 设置活动实例状态
        /// </summary>
        private void SetActivityState(int activityInstanceId,
            ActivityStateEnum activityState,
            WfAppRunner runner,
            IDbSession session)
        {
            var activityInstance = GetById(session.Connection, activityInstanceId, session.Transaction);
            activityInstance.ActivityState = (short)activityState;
            activityInstance.UpdatedUserId = runner.UserId;
            activityInstance.UpdatedUserName = runner.UserName;
            activityInstance.UpdatedDateTime = System.DateTime.UtcNow;
            Update(activityInstance, session);
        }

        /// <summary>
        /// Withdraw activity instance
        /// 撤销活动实例
        /// </summary>
        internal void Withdraw(int activityInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceId, ActivityStateEnum.Withdrawed, runner, session);
        }

        /// <summary>
        /// Sendback activity instance
        /// 退回活动实例
        /// </summary>
        internal void SendBack(int activityInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceId, ActivityStateEnum.Sendbacked, runner, session);
        }

        /// <summary>
        /// Set the end status of the activity
        /// 设置活动结束状态
        /// </summary>
        private void EndActivityState(int activityInstanceId,
            ActivityStateEnum activityState,
            WfAppRunner runner,
            IDbSession session)
        {
            var activityInstance = GetById(session.Connection, activityInstanceId, session.Transaction);
            activityInstance.ActivityState = (short)activityState;
            activityInstance.EndedUserId = runner.UserId;
            activityInstance.EndedUserName = runner.UserName;
            activityInstance.EndedDateTime = System.DateTime.UtcNow;

            Update(activityInstance, session);
        }

        /// <summary>
        /// Complete activity instance
        /// 活动实例完成
        /// </summary>
        internal void Complete(int activityInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            EndActivityState(activityInstanceId, ActivityStateEnum.Completed, runner, session);
        }

        internal void Complete(ActivityInstanceEntity activityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            activityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            activityInstance.EndedUserId = runner.UserId;
            activityInstance.EndedUserName = runner.UserName;
            activityInstance.EndedDateTime = System.DateTime.UtcNow;

            Update(activityInstance, session);
        }

        /// <summary>
        /// Update approval status to agree
        /// 更新审批状态为同意
        /// </summary>
        internal void SetApprovalStatus(int activityInstanceId, 
            IDbSession session)
        {
            var activityInstance = GetById(session.Connection, activityInstanceId, session.Transaction);
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
        internal void UpdateActivityInstanceBlockedBetweenSplitJoin(Activity gatewayActivity, 
            ActivityInstanceEntity gatewayInstance,
            IProcessModel processModel,
            IDbSession session)
        {
            var joinCount = 0;
            var splitCount = 0;
            var splitActivity = processModel.GetBackwardGatewayActivity(gatewayActivity, ref joinCount, ref splitCount);
            var taskActivityList = processModel.GetAllTaskActivityList(splitActivity, gatewayActivity);

            UpdateActivityInstanceBlockedBetweenSplitJoin(gatewayInstance.ProcessInstanceId, taskActivityList, session);
        }

        /// <summary>
        /// Update the activities in the activity to block status
        /// 更新活动里面的活动为阻止状态
        /// </summary>
        private void UpdateActivityInstanceBlockedBetweenSplitJoin(int processInstanceId, 
            IList<Activity> taskActivityList,
            IDbSession session)
        {
            var idsin = taskActivityList.Select(a => a.ActivityId).ToList();
            var updSql = @"UPDATE wf_activity_instance 
                        SET cannot_renew_instance=1 
                        WHERE process_instance_id=@processInstanceId 
                            AND activity_state in (1, 2, 5) 
                            AND activity_id in @ids";

            var rows = Repository.Execute(session.Connection, updSql,
                new
                {
                    processInstanceId = processInstanceId,
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
        internal int Insert(ActivityInstanceEntity entity,
            IDbSession session)
        {
            //SET ActivityName When It Is NULL
            if (entity.ActivityType == (short)ActivityTypeEnum.GatewayNode
                && string.IsNullOrEmpty(entity.ActivityName))
            {
                entity.ActivityName = "GATEWAY";
            }

            int newId = Repository.Insert(session.Connection, entity, session.Transaction);
            entity.Id = newId;

            return newId;
        }

        /// <summary>
        /// Update activity instance
        /// 更新活动实例
        /// </summary>
        internal void Update(ActivityInstanceEntity entity,
            IDbSession session)
        {
            Repository.Update(session.Connection, entity, session.Transaction);
        }

        /// <summary>
        /// Update activity instance
        /// 更新活动实例
        /// </summary>
        internal void Update(IDbConnection conn, ActivityInstanceEntity entity, IDbTransaction trans)
        {
            Repository.Update(conn, entity, trans);
        }

        /// <summary>
        /// Cancel activity instance
        /// 取消节点运行
        /// </summary>
        internal void Cancel(int activityInstanceId,
            WfAppRunner runner,
            IDbSession session)
        {
            var sql = @"UPDATE wf_activity_instance 
                        SET activity_state=@activityState,
                            updated_user_id=@updatedUserId,
                            updated_User_name=@updatedUserName,
                            updated_datetime=@updatedDateTime 
                        WHERE id=@activityInstanceId 
                            AND (activity_state=1 OR activity_state=2 OR activity_state=5)";
            Repository.Execute(session.Connection, sql,
                new
                {
                    activityState = (short)ActivityStateEnum.Cancelled,
                    updatedUserId = runner.UserId,
                    updatedUserName = runner.UserName,
                    updatedDateTime = System.DateTime.UtcNow,
                    activityInstanceId = activityInstanceId
                }, session.Transaction);
        }

        /// <summary>
        /// Update the status of unfinished nodes in the countersignature node
        /// 更新会签节点中未办理完成的节点状态
        /// </summary>
        internal void CancelUnCompletedMultipleInstance(int mainActivityInstanceId, IDbSession session, WfAppRunner runner)
        {
            var sql = @"UPDATE wf_activity_instance 
                        SET activity_state=@activityState,
                            updated_user_id=@updatedUserId,
                            updated_User_name=@updatedUserName,
                            updated_datetime=@updatedDateTime 
                        WHERE main_activity_instance_id=@mainActivityInstanceId 
                            AND (activity_state=1 OR activity_state=2 OR activity_state=5)";

            Repository.Execute(session.Connection, sql,
                new
                {
                    activityState = (short)ActivityStateEnum.Cancelled,
                    updatedUserId = runner.UserId,
                    updatedUserName = runner.UserName,
                    updatedDateTime = System.DateTime.UtcNow,
                    mainActivityInstanceId = mainActivityInstanceId
                }, session.Transaction);
        }

        /// <summary>
        /// Withdraw the master node and its multiple instance nodes below it
        /// 撤销主节点及其下面的多实例子节点
        /// </summary>
        internal void WithdrawMainIncludedChildNodes(int mainActivityInstanceId, IDbSession session, WfAppRunner runner)
        {
            //先更新主节点状态为撤销状态
            //Update the status of the main node to revoked first
            SetActivityState(mainActivityInstanceId, ActivityStateEnum.Withdrawed, runner, session);

            //再更新主节点下的多实例子节点状态为撤销状态
            //Update the status of multiple instance nodes under the main node to withdrawn state again
            WithdrawMultipleInstance(mainActivityInstanceId, session, runner);
        }

        /// <summary>
        /// Update the node status of the counter signature processing node
        /// Notes:Nodes in a ready or suspended state can be withdrawn 
        /// 更新会签办理节点的节点状态
        /// 备注:准备或挂起状态的节点可以撤销
        /// </summary>
        private void WithdrawMultipleInstance(int mainActivityInstanceId, IDbSession session, WfAppRunner runner)
        {
            var sql = @"UPDATE wf_activity_instance 
                        SET activity_state=@activityState, 
                            updated_user_id=@updatedUserId,
                            updated_user_name=@updatedUserName,
                            updated_datetime=@updatedDateTime
                        WHERE main_activity_instance_id=@mainActivityInstanceId
                            AND (activity_state=1 OR activity_state=5)";      

            Repository.Execute(session.Connection, sql,
                new
                {
                    activityState = (short)ActivityStateEnum.Withdrawed,
                    updatedUserId = runner.UserId,
                    updatedUserName = runner.UserName,
                    updatedDateTime = System.DateTime.UtcNow,
                    mainActivityInstanceId = mainActivityInstanceId
                }, session.Transaction);
        }

        /// <summary>
        /// Put the node back in a suspended state
        /// Notes: Used when revoking the last sub node of the co signature
        /// 重新使节点处于挂起状态
        /// 说明：会签最后一个子节点撤销时候用到
        /// </summary>
        internal void Resuspend(int activityInstanceId, IDbSession session, WfAppRunner runner)
        {
            SetActivityState(activityInstanceId, ActivityStateEnum.Suspended, runner, session);
        }

        /// <summary>
        /// Put the node back in a suspended state
        /// Notes: Used when revoking the last sub node of the co signature
        /// 重新使节点处于挂起状态
        /// 说明：会签最后一个子节点撤销时候用到
        /// </summary>
        internal void Rerun(int activityInstanceId, IDbSession session, WfAppRunner runner)
        {
            SetActivityState(activityInstanceId, ActivityStateEnum.Running, runner, session);
        }

        /// <summary>
        /// Delete activity instance
        /// 删除活动实例
        /// </summary>
        internal void Delete(int activityInstanceId,
            IDbSession session = null)
        {
            Repository.Delete<ActivityInstanceEntity>(session.Connection,
                activityInstanceId,
                session.Transaction);
        }
        #endregion

        #region Activity Instance Approval Status 活动实例审批状态
        /// <summary>
        /// Agree
        /// 同意
        /// </summary>
        internal void Agree(int taskId)
        {
            var activityInstance = GetByTask(taskId);
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
        internal void Refuse(int taskId)
        {
            var activityInstance = GetByTask(taskId);
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
        internal NodePassedResult CheckActivityInstancePassedResult(Nullable<int> activityInstanceId,
            IDbSession session)
        {
            var result = NodePassedResult.Create(NodePassedTypeEnum.Default);
            if (activityInstanceId != null)
            {
                var activityInstance = GetById(session.Connection, activityInstanceId.Value, session.Transaction);
                if (IsMultipleInstanceChildren(activityInstance) == true)
                {
                    //会签多实例情况下的是否通过判断
                    //Whether the judgment is passed in the case of multiple instances of co signing
                    var mainActivityInstance = GetById(session.Connection, activityInstance.MainActivityInstanceId.Value, session.Transaction);
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
        internal NodePassedTypeEnum CheckMIPassRateInfo(ActivityInstanceEntity currentActivityInstance,
            ActivityInstanceEntity mainActivityInstance,
            IDbSession session)
        {
            var passedType = NodePassedTypeEnum.Default;
            var childActivityInstanceList = GetValidActivityInstanceListOfMI(mainActivityInstance.Id, mainActivityInstance.ProcessInstanceId,
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
