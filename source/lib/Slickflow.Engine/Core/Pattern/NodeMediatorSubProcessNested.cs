using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.Runtime;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Mediator Sub Process Nested
    /// 子流程节点执行器
    /// </summary>
    internal class NodeMediatorSubProcessNested : NodeMediator
    {
        internal NodeMediatorSubProcessNested(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorSubProcessNested(IDbSession session)
            : base(session)
        {
        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                if (base.LinkContext.FromActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    //检查子流程是否结束
                    //Check if the sub process has ended
                    var pim = new ProcessInstanceManager();
                    bool isCompleted = pim.CheckSubProcessInstanceCompleted(Session.Connection,
                        base.LinkContext.FromActivityInstance.ID,
                        base.LinkContext.FromActivityInstance.ActivityGUID,
                        Session.Transaction);
                    if (isCompleted == false)
                    {
                        throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorsubprocess.ExecuteWorkItem.notcompleted.warn",
                            string.Format("Activity:{0}", base.LinkContext.FromActivity.ActivityName)));
                    }
                }

                //完成当前的任务节点
                //Complete current node
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskID,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                //获取下一步节点列表：并继续执行
                //Get the next node list: and continue execution
                if (canContinueForwardCurrentNode)
                {
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, this.Session);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Complete work item
        /// </summary>
        internal bool CompleteWorkItem(int? taskID,
            ActivityResource activityResource,
            IDbSession session)
        {
            bool canContinueForwardCurrentNode = true;

            if (taskID != null)
            {
                //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
                //Complete this task, return that the task has been transferred to the next co signing task,
                //and do not continue to execute other nodes
                base.TaskManager.Complete(taskID.Value, activityResource.AppRunner, session);
            }

            //设置活动节点的状态为完成状态
            //Set the status of the activity node to complete status
            base.ActivityInstanceManager.Complete(base.LinkContext.FromActivityInstance.ID,
                activityResource.AppRunner,
                session);
            base.LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;

            //先判断是否是多实例类型的任务
            //First, determine whether it is a multi instance type task
            var miDetail = base.LinkContext.FromActivity.MultiSignDetail;
            if (miDetail != null && miDetail.ComplexType == ComplexTypeEnum.SignTogether)
            {
                //取出主节点信息
                //Retrieve main node information
                var mainNodeIndex = base.LinkContext.FromActivityInstance.MIHostActivityInstanceID.Value;
                var mainActivityInstance = base.ActivityInstanceManager.GetById(mainNodeIndex);

                //取出多实例节点列表
                //Retrieve a list of multiple instance nodes
                var sqList = base.ActivityInstanceManager.GetActivityMulitipleInstanceWithState(
                    mainNodeIndex,
                    base.LinkContext.FromActivityInstance.ProcessInstanceID,
                    (short)ActivityStateEnum.Suspended,
                    session).ToList<ActivityInstanceEntity>();

                if (base.LinkContext.FromActivity.MultiSignDetail.MergeType == MergeTypeEnum.Sequence)
                {
                    //取出最大执行节点
                    //Retrieve the maximum execution node 
                    short maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder).Value;

                    if (base.LinkContext.FromActivityInstance.CompleteOrder < maxOrder)
                    {
                        //设置下一个任务进入准备状态
                        //Set the next task to enter preparation mode
                        var currentNodeIndex = (short)base.LinkContext.FromActivityInstance.CompleteOrder.Value;
                        var nextActivityInstance = sqList[currentNodeIndex];
                        nextActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                        base.ActivityInstanceManager.Update(nextActivityInstance, session);

                        //设置下一个任务对应的子流程进入运行状态
                        //Set the sub process corresponding to the next task to enter the running state
                        base.ProcessInstanceManager.RecallSubProcess(nextActivityInstance.ID,
                            activityResource.AppRunner,
                            Session);

                        canContinueForwardCurrentNode = false;
                        base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.ForwardToNextSequenceTask;
                    }
                    else if (base.LinkContext.FromActivityInstance.CompleteOrder == maxOrder)
                    {
                        //完成最后一个会签任务，会签主节点状态由挂起设置为准备状态
                        //Complete the last countersignature task,
                        //and set the status of the countersignature master node from suspended to ready
                        mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                        base.ActivityInstanceManager.Update(mainActivityInstance, session);
                        //将执行权责交由会签主节点
                        //Transfer the execution responsibilities to the main signing node
                        base.LinkContext.FromActivityInstance = mainActivityInstance;
                    }
                }
                else if (base.LinkContext.FromActivity.MultiSignDetail.MergeType == MergeTypeEnum.Parallel)
                {
                    var allCount = sqList.Count();
                    var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed)
                        .ToList<ActivityInstanceEntity>()
                        .Count();

                    if (completedCount / allCount >= mainActivityInstance.CompleteOrder)
                    {
                        //如果超过约定的比例数，则执行下一步节点
                        //If the agreed proportion is exceeded, proceed to the next node
                        mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                        base.ActivityInstanceManager.Update(mainActivityInstance, session);
                        //将执行权责交由会签主节点
                        //Transfer the execution responsibilities to the main signing node
                        base.LinkContext.FromActivityInstance = mainActivityInstance;
                    }
                    else
                    {
                        canContinueForwardCurrentNode = false;
                        base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.WaitingForCompletedMore;
                    }
                }
            }
            return canContinueForwardCurrentNode;
        }

        /// <summary>
        /// Create activity task transition instance
        /// 创建活动任务转移数据
        /// </summary>
        internal override void CreateActivityTaskTransitionInstance(Activity toActivity,
           ProcessInstanceEntity processInstance,
           ActivityInstanceEntity fromActivityInstance,
           string transitionGUID,
           TransitionTypeEnum transitionType,
           TransitionFlyingTypeEnum flyingType,
           ActivityResource activityResource,
           IDbSession session)
        {
            Boolean isParallel = false;
            if (fromActivityInstance.ActivityType == (short)ActivityTypeEnum.GatewayNode)
            {
                var processModel = ProcessModelFactory.CreateByProcessInstance(session.Connection, processInstance, session.Transaction);
                var activityNode = processModel.GetActivity(fromActivityInstance.ActivityGUID);
                isParallel = processModel.IsAndSplitMI(activityNode);
            }

            if (isParallel)
            {
                var entity = new ActivityInstanceEntity();
                var plist = activityResource.NextActivityPerformers[toActivity.ActivityGUID];
                //创建并行多实例分支
                //Create parallel multi instance branches
                for (var i = 0; i < plist.Count; i++)
                {
                    var performer = plist[i];
                    CreateSubProcessNode(toActivity, processInstance, fromActivityInstance, transitionGUID, transitionType,
                       flyingType, activityResource, performer, session);
                }
            }
            else
            {
                if (toActivity.MultiSignDetail != null
                    && toActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignTogether)
                {
                    CreateMultipleInstance(toActivity, processInstance, fromActivityInstance,
                        transitionGUID, transitionType, flyingType, activityResource, session);
                }
                else
                {
                    CreateSubProcessNode(toActivity, processInstance, fromActivityInstance, transitionGUID, transitionType,
                        flyingType, activityResource, null, session);
                }
            }
        }

        /// <summary>
        /// Create sub process 
        /// 创建子流程节点数据以及子流程记录
        /// </summary>
        private void CreateSubProcessNode(Activity toActivity,
           ProcessInstanceEntity processInstance,
           ActivityInstanceEntity fromActivityInstance,
           string transitionGUID,
           TransitionTypeEnum transitionType,
           TransitionFlyingTypeEnum flyingType,
           ActivityResource activityResource,
           Performer performer,
           IDbSession session)
        {
            WfExecutedResult startedResult = WfExecutedResult.Default();

            var toActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
            toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            if (performer != null)
            {
                //并行容器中的子流程节点，每个人发起一个子流程
                //Sub process nodes in parallel containers, where each person initiates a sub process
                toActivityInstance.AssignedToUserIDs = performer.UserID;
                toActivityInstance.AssignedToUserNames = performer.UserName;

                base.ActivityInstanceManager.Insert(toActivityInstance, session);

                this.TaskManager.Insert(toActivityInstance, performer, activityResource.AppRunner, session);
            }
            else
            {
                toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

                base.ActivityInstanceManager.Insert(toActivityInstance, session);

                base.CreateNewTask(toActivityInstance, activityResource, session);
            }

            var newTransitionInstanceID = InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);

            //启动子流程
            //Startup sub process
            var subProcessNode = (SubProcessNode)toActivity.Node;
            subProcessNode.ActivityInstance = toActivityInstance;

            //复制子流程启动用户信息
            //Copy sub process to start user information
            WfAppRunner subRunner = null;
            var performerList = new PerformerList();
            if (performer != null)
            {
                subRunner = CreateSubProcessRunner(activityResource.AppRunner,
                    new Performer(performer.UserID, performer.UserName),
                    session);
                performerList.Add(performer);
            }
            else
            {
                subRunner = CreateSubProcessRunner(activityResource.AppRunner,
                   new Performer(activityResource.AppRunner.UserID, activityResource.AppRunner.UserName),
                   session);
                performerList = activityResource.NextActivityPerformers[toActivity.ActivityGUID];
            }

            var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartupSub(subRunner,
                processInstance,
                subProcessNode,
                performerList,
                session,
                ref startedResult);

            runtimeInstance.OnWfProcessExecuted += runtimeInstance_OnWfProcessStarted;
            runtimeInstance.Execute(session);

            void runtimeInstance_OnWfProcessStarted(object sender, WfEventArgs args)
            {
                startedResult = args.WfExecutedResult;
            }
        }

        /// <summary>
        /// Create multiple instance
        /// 会签类型的主节点, 多实例节点处理
        /// </summary>
        internal new void CreateMultipleInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            String transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //实例化主节点Activity
            //Instantiate the main node Activity
            var toActivityInstance = CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            toActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
            toActivityInstance.ComplexType = (short)ComplexTypeEnum.SignTogether;
            if (toActivity.MultiSignDetail.MergeType == MergeTypeEnum.Parallel)
            {
                toActivityInstance.CompleteOrder = toActivity.MultiSignDetail.CompleteOrder;
            }
            toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

            base.ActivityInstanceManager.Insert(toActivityInstance, session);

            InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);

            //插入会签子节点实例数据
            //Insert signature sub node instance data
            var plist = activityResource.NextActivityPerformers[toActivity.ActivityGUID];
            ActivityInstanceEntity entity = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                entity = base.ActivityInstanceManager.CreateActivityInstanceObject(toActivityInstance);
                entity.AssignedToUserIDs = plist[i].UserID;
                entity.AssignedToUserNames = plist[i].UserName;
                entity.MIHostActivityInstanceID = toActivityInstance.ID;
                entity.CompleteOrder = (short)(i + 1);

                //只有第一个节点处于运行状态，其它节点挂起
                //Only the first node is running, while the other nodes are suspended
                if ((i > 0) && (toActivity.MultiSignDetail.MergeType == MergeTypeEnum.Sequence))
                {
                    entity.ActivityState = (short)ActivityStateEnum.Suspended;
                }

                entity.ID = base.ActivityInstanceManager.Insert(entity, session);

                base.TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);

                //启动子流程
                //Startup sub process
                IDbSession subSession = SessionFactory.CreateSession();
                var subProcessNode = (SubProcessNode)toActivity.Node;
                subProcessNode.ActivityInstance = entity;   
                WfAppRunner subRunner = CreateSubProcessRunner(activityResource.AppRunner, 
                    plist[i],
                    session);

                WfExecutedResult startedResult = WfExecutedResult.Default();
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartupSub(subRunner,
                    processInstance,
                    subProcessNode,
                    plist,
                    session,
                    ref startedResult);

                if (runtimeInstance.WfExecutedResult.Status == WfExecutedStatus.Exception)
                {
                    throw new WfRuntimeException(runtimeInstance.WfExecutedResult.Message);
                }
                runtimeInstance.Execute(subSession);

                //如果是串行会签，只有第一个子流程可以运行，其它子流程处于挂起状态
                //If it is a serial countersignature,
                //only the first subprocess can run, and the other subprocesses are in a suspended state
                if ((i > 0) && (toActivity.MultiSignDetail.MergeType == MergeTypeEnum.Sequence))
                {
                    base.ProcessInstanceManager.Suspend(startedResult.ProcessInstanceIDStarted, subRunner, subSession);
                }
            }
        }

        /// <summary>
        /// When creating a subprocess, regenerate the runner information
        /// 创建子流程时，重新生成runner信息
        /// </summary>
        /// <returns></returns>
        private WfAppRunner CreateSubProcessRunner(WfAppRunner runner,
            Performer performer,
            IDbSession session)
        {
            WfAppRunner subRunner = new WfAppRunner();
            subRunner.ProcessGUID = runner.ProcessGUID;
            subRunner.AppInstanceCode = runner.AppInstanceCode;
            subRunner.AppInstanceID = runner.AppInstanceID;
            subRunner.AppName = runner.AppName;
            subRunner.Version = runner.Version;
            subRunner.UserID = performer.UserID;
            subRunner.UserName = performer.UserName;

            #region Dynamic sub process called
            //如果是动态调用子流程，则需要获取具体子流程实体对象
            //var isSubProcessNotExisted = false;
            //if (subProcessNode.SubProcessType == SubProcessTypeEnum.Dynamic)
            //{
            //    var subProcessId = runner.DynamicVariables[subProcessNode.SubVarName];
            //    if (!string.IsNullOrEmpty(subProcessId))
            //    {
            //        int processID = 0;
            //        int.TryParse(subProcessId, out processID);
            //        if (processID > 0)
            //        {
            //            var pm = new ProcessManager();
            //            var process = pm.GetByID(session.Connection, processID, session.Transaction);
            //            if (process != null)
            //            {
            //                subProcessNode.SubProcessGUID = process.ProcessGUID;
            //            }
            //            else
            //            {
            //                isSubProcessNotExisted = true;
            //            }
            //        }
            //        else
            //        {
            //            isSubProcessNotExisted = true;
            //        }
            //    }
            //    else
            //    {
            //        isSubProcessNotExisted = true;
            //    }
            //}

            //if (isSubProcessNotExisted == true)
            //{
            //    throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorsubprocess.CreateSubProcessRunner.nonevariableparamsvalue.warn"));
            //}
            #endregion

            return subRunner;
        }
    }
}
