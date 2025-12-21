
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Core.Runtime;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Event.Timer
{
    /// <summary>
    /// Intermediate timer go node mediator
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterTimerGo : NodeMediator
    {
        internal NodeMediatorInterTimerGo(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                //检查Timer节点运行条件
                //Check if the operating conditions of the node are met
                CheckBeingExecutedInfo();

                OnBeforeExecuteWorkItem();

                //完成当前的任务节点
                //Complete current task node
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskId,
                    ActivityForwardContext.ActivityResource,
                    Session);

                OnAfterExecuteWorkItem();

                //获取下一步节点列表：并继续执行
                //Get the next node list: and continue execution
                if (canContinueForwardCurrentNode)
                {
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, Session);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if the operating conditions of the node are met
        /// 检查节点运行条件是否满足
        /// </summary>
        private void CheckBeingExecutedInfo()
        {
            if (LinkContext.FromActivityInstance.OverdueDateTime < DateTime.UtcNow)
            {
                throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorintertimergo.CheckBeingExecutedInfo.exception"));
            }
        }

        /// <summary>
        /// Complete work item
        /// 完成任务实例
        /// </summary>     
        internal bool CompleteWorkItem(int? taskId,
            ActivityResource activityResource,
            IDbSession session)
        {
            WfAppRunner runner = new WfAppRunner
            {
                UserId = activityResource.AppRunner.UserId,        
                UserName = activityResource.AppRunner.UserName
            };

            if (taskId != null)
            {
                //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
                //Complete this task, return that the task has been transferred to the next co signing task,
                //and do not continue to execute other nodes
                TaskManager.Complete(taskId.Value, activityResource.AppRunner, session);
            }

            ActivityInstanceManager.Complete(LinkContext.FromActivityInstance.Id,
                activityResource.AppRunner,
                session);

            LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            bool canContinueForwardCurrentNode = LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

            return canContinueForwardCurrentNode;
        }

        /// <summary>
        /// Create activity task transition instance
        /// 创建活动任务转移实例数据
        /// </summary>
        internal override void CreateActivityTaskTransitionInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            string transitionId,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            int newActivityInstanceId = 0;
            bool isParallel = false;
            if (fromActivityInstance.ActivityType == (short)ActivityTypeEnum.GatewayNode)
            {
                //并发多实例分支判断(AndSplit Multiple)
                //Concurrent multi instance branch judgment (AndSplit Multiple)
                var processModel = ProcessModelFactory.CreateByProcessInstance(session.Connection,
                    processInstance,
                    session.Transaction);
                var activityNode = processModel.GetActivity(fromActivityInstance.ActivityId);
                isParallel = processModel.IsAndSplitMI(activityNode);
            }

            if (isParallel)
            {
                //并行多实例容器
                //Parallel multi instance container
                var entity = new ActivityInstanceEntity();
                var plist = activityResource.NextActivityPerformers[toActivity.ActivityId];

                //创建并行多实例分支
                //Create parallel multi instance branches
                for (var i = 0; i < plist.Count; i++)
                {
                    entity = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                    entity.AssignedUserIds = plist[i].UserId;
                    entity.AssignedUserNames = plist[i].UserName;
                    entity.ActivityState = (short)ActivityStateEnum.Ready;

                    entity.Id = ActivityInstanceManager.Insert(entity, session);

                    TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);

                    InsertTransitionInstance(processInstance,
                        transitionId,
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
                //Normal task node
                var toActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

                toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

                newActivityInstanceId = ActivityInstanceManager.Insert(toActivityInstance, session);
                base.CreateNewTask(toActivityInstance, activityResource, session);

                InsertTransitionInstance(processInstance,
                    transitionId,
                    fromActivityInstance,
                    toActivityInstance,
                    transitionType,
                    flyingType,
                    activityResource.AppRunner,
                    session);
            }
        }
    }
}
