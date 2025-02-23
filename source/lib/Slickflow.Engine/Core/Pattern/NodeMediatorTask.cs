using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Mediator Task
    /// 任务节点执行器
    /// </summary>
    internal class NodeMediatorTask : NodeMediator
    {
        internal NodeMediatorTask(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorTask(IDbSession session)
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
                OnBeforeExecuteWorkItem();

                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskID,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                OnAfterExecuteWorkItem();

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
        /// 完成节点实例
        /// </summary>   
        internal bool CompleteWorkItem(int? taskID,
            ActivityResource activityResource,
            IDbSession session)
        {
            WfAppRunner runner = new WfAppRunner
            {
                UserID = activityResource.AppRunner.UserID,         
                UserName = activityResource.AppRunner.UserName
            };

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
            Boolean canContinueForwardCurrentNode = base.LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

            return canContinueForwardCurrentNode;
        }

        /// <summary>
        /// Create activity task transition instance
        /// 创建活动任务转移实例数据
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
                //并发多实例分支判断(AndSplit Multiple)
                //Concurrent multi instance branch judgment (AndSplit Multiple)
                var processModel = ProcessModelFactory.CreateByProcessInstance(session.Connection, processInstance, session.Transaction);
                var activityNode = processModel.GetActivity(fromActivityInstance.ActivityID);
                isParallel = processModel.IsAndSplitMI(activityNode);
            }

            if (isParallel)
            {
                //并行多实例容器
                //Parallel mutiple instance container
                ActivityInstanceEntity entity = null;
                var plist = activityResource.NextActivityPerformers[toActivity.ActivityID];

                //创建并行多实例分支
                //Create parallel multi instance branches
                for (var i = 0; i < plist.Count; i++)
                {
                    entity = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                    entity.AssignedToUserIDs = plist[i].UserID;
                    entity.AssignedToUserNames = plist[i].UserName;
                    entity.ActivityState = (short)ActivityStateEnum.Ready;

                    entity.ID = base.ActivityInstanceManager.Insert(entity, session);

                    base.TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);

                    InsertTransitionInstance(processInstance,
                        transitionGUID,
                        fromActivityInstance,
                        entity,
                        transitionType,
                        flyingType,
                        activityResource.AppRunner,
                        session);
                }
            }
            else
            {
                //普通任务节点
                //Normal Task Node
                var toActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

                //处理多次退回后的返送
                //Handling returns after multiple returns
                WriteBackSrcOrgInformation(toActivityInstance, fromActivityInstance, session);

                toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);
                
                base.ActivityInstanceManager.Insert(toActivityInstance, session);

                base.CreateNewTask(toActivityInstance, activityResource, session);

                InsertTransitionInstance(processInstance,
                    transitionGUID,
                    fromActivityInstance,
                    toActivityInstance,
                    transitionType,
                    flyingType,
                    activityResource.AppRunner,
                    session);

                //调用外部事件的注册方法
                //Call the registration method for external events
                var delegateContext = new DelegateContext
                {
                    AppInstanceID = processInstance.AppInstanceID,
                    ProcessID = processInstance.ProcessID,
                    ProcessInstanceID = processInstance.ID,
                    ActivityID = toActivity.ActivityID,
                    ActivityCode = toActivity.ActivityCode,
                    ActivityResource = activityResource
                };
                DelegateExecutor.InvokeExternalDelegate(session,
                    EventFireTypeEnum.OnActivityCreated,
                    activityResource.AppRunner.DelegateEventList,
                    delegateContext);
            }
        }

        /// <summary>
        /// Maintain source node information for multiple returns
        /// 维护多次退回时的源节点信息
        /// </summary>
        /// <param name="toActivityInstance"></param>
        /// <param name="fromActivityInstance"></param>
        /// <param name="session"></param>
        private void WriteBackSrcOrgInformation(ActivityInstanceEntity toActivityInstance, 
            ActivityInstanceEntity fromActivityInstance,
            IDbSession session)
        {
            if (fromActivityInstance.BackSrcActivityInstanceID != null)
            {
                var backSrcActivityInstance = base.ActivityInstanceManager.GetById(session.Connection,
                    fromActivityInstance.BackSrcActivityInstanceID.Value, session.Transaction);
                toActivityInstance.BackSrcActivityInstanceID = backSrcActivityInstance.BackSrcActivityInstanceID;
                toActivityInstance.BackOrgActivityInstanceID = backSrcActivityInstance.BackOrgActivityInstanceID;
            }
        }
    }
}
