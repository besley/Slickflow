
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Core.Runtime;

namespace Slickflow.Engine.Core.Pattern.Event.Message
{
    /// <summary>
    /// Intermediate Message Go Node Mediator
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterMessageGo : NodeMediator
    {
        internal NodeMediatorInterMessageGo(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
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

                //完成当前的任务节点
                //Complete the current task node
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskID,
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
        /// Complete work item
        /// 完成任务实例
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>        
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
                TaskManager.Complete(taskID.Value, activityResource.AppRunner, session);
            }

            //设置活动节点的状态为完成状态
            //Set the status of the activity node to complete status
            ActivityInstanceManager.Complete(LinkContext.FromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            bool canContinueForwardCurrentNode = LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

            return canContinueForwardCurrentNode;
        }

        /// <summary>
        /// Create activity task transfer instance data
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
            int newActivityInstanceID = 0;
            bool isParallel = false;
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
                //Parallel multi instance container
                var entity = new ActivityInstanceEntity();
                var plist = activityResource.NextActivityPerformers[toActivity.ActivityID];

                //创建并行多实例分支
                //Create parallel multi instance branches
                for (var i = 0; i < plist.Count; i++)
                {
                    entity = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                    entity.AssignedToUserIDs = plist[i].UserID;
                    entity.AssignedToUserNames = plist[i].UserName;
                    entity.ActivityState = (short)ActivityStateEnum.Ready;

                    entity.ID = ActivityInstanceManager.Insert(entity, session);

                    TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);

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
                //Normal task
                var toActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

                newActivityInstanceID = ActivityInstanceManager.Insert(toActivityInstance, session);

                base.CreateNewTask(toActivityInstance, activityResource, session);

                InsertTransitionInstance(processInstance,
                    transitionGUID,
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
