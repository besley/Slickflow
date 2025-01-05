using System;
using System.Collections.Generic;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Mediator Revise
    /// 流程修订的具体执行类
    /// </summary>
    internal class NodeMediatorRevise : NodeMediator
    {
        internal NodeMediatorRevise(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorRevise(IDbSession session)
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

                //先取出原始退回节点信息
                //First, retrieve the original return node information
                var backSrcActivityInstanceID = base.ActivityForwardContext.FromActivityInstance.BackSrcActivityInstanceID;
                if (backSrcActivityInstanceID != null)
                {
                    //完成当前的任务节点
                    //Complete the current task node
                    bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskID,
                        ActivityForwardContext.ActivityResource,
                        this.Session);

                    if (canContinueForwardCurrentNode == true)
                    {
                        var aim = new ActivityInstanceManager();
                        var backSrcActivityInstance = aim.GetById(backSrcActivityInstanceID.Value);

                        //取出下一步办理人员信息
                        //Retrieve the information of the next processing personnel
                        var nextStep = base.ActivityForwardContext.ActivityResource.AppRunner.NextActivityPerformers;
                        var performerList = nextStep[backSrcActivityInstance.ActivityGUID];

                        //判断不同的修订模式
                        //Determine different revision modes
                        var mainActivityInstanceID = backSrcActivityInstance.MIHostActivityInstanceID;
                        if (mainActivityInstanceID != null)
                        {
                            //会签模式
                            //复制与会签子节点相同的活动实例和任务记录 
                            //Signing mode
                            //Copy activity instances and task records with the same attendance child nodes
                            var mainActivityInstance = aim.GetById(mainActivityInstanceID.Value);
                            CloneChildNodeOfMI(performerList, mainActivityInstance, base.Session);
                        }
                        else
                        {
                            //并行分支（多实例）的情况
                            ////In the case of parallel branches (multiple instances)
                            var transitionList = ActivityForwardContext.ProcessModel.GetBackwardTransitionList(backSrcActivityInstance.ActivityGUID);
                            if (transitionList != null && transitionList.Count == 1)
                            {
                                var transition = transitionList[0];
                                var gatewayNode = ActivityForwardContext.ProcessModel.GetActivity(transition.FromActivityGUID);
                                if (gatewayNode.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
                                {
                                    //复制并行分支多实例
                                    //Copy parallel branches with multiple instances
                                    var gatewayActivityInstance = base.ActivityInstanceManager.GetActivityInstanceLatest(
                                        backSrcActivityInstance.ProcessInstanceID,
                                        gatewayNode.ActivityGUID, base.Session);
                                    CloneChildNodeOfAndSplitMI(performerList, transition.TransitionGUID, gatewayActivityInstance, backSrcActivityInstance, base.Session);
                                }
                                else
                                {
                                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodemediatorrevise.ExecuteWorkItem.warn"));
                                }
                            }
                            else
                            {
                                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodemediatorrevise.ExecuteWorkItem.warn"));
                            }
                        }
                    }
                    else
                    {
                        throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodemediatorrevise.ExecuteWorkItem.exception"));
                    } 
                }
                else
                {
                    throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodemediatorrevise.ExecuteWorkItem.error"));
                }
                OnAfterExecuteWorkItem();
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

                //设置活动节点的状态为完成状态
                //Set the status of the activity node to complete status
                var activityInstanceID = base.ActivityInstanceManager.GetByTask(taskID.Value).ID;
                base.ActivityInstanceManager.Complete(activityInstanceID,
                    activityResource.AppRunner,
                    session);
            }
            return canContinueForwardCurrentNode;
        }

        /// <summary>
        /// Copy multi instance node data
        /// 复制多实例子节点数据
        /// </summary>
        /// <param name="plist"></param>
        /// <param name="mainActivityInstance"></param>
        /// <param name="session"></param>
        private void CloneChildNodeOfMI(PerformerList plist,
            ActivityInstanceEntity mainActivityInstance, 
            IDbSession session)
        {
            var childNodeListOfMI = base.ActivityInstanceManager.GetValidActivityInstanceListOfMI(mainActivityInstance.ID,
                mainActivityInstance.ProcessInstanceID, session);

            ActivityInstanceEntity entity = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                var userID = plist[i].UserID;
                var userName = plist[i].UserName;
                var isTaskExisted = IsTaskExisted(childNodeListOfMI, userID);
                if (isTaskExisted == true)
                {
                    //如果活动或者任务已经存在，则不用创建新活动和任务
                    //If an activity or task already exists, there is no need to create a new one
                    continue;
                }

                //根据主节点来复制子节点数据
                //Copy child node data based on the master node
                entity = ActivityInstanceManager.CreateActivityInstanceObject(mainActivityInstance);
                entity.AssignedToUserIDs = userID;
                entity.AssignedToUserNames = userName;
                entity.MIHostActivityInstanceID = mainActivityInstance.ID;

                //并行串行下，多实例子节点的执行顺序设置
                //Setting the Execution Order of Multiple Real Example Nodes in Parallel Serial
                if (mainActivityInstance.MergeType == (short)MergeTypeEnum.Sequence)
                {
                    entity.CompleteOrder = (short)(i + 1);
                }
                else if (mainActivityInstance.MergeType == (short)MergeTypeEnum.Parallel)
                {
                    //并行模式下CompleteOrder的优先级一样，所以置为 -1
                    //In parallel mode, the priority of Completed Order is the same, so it is set to -1
                    entity.CompleteOrder = -1;       
                }

                //如果是串行会签，只有第一个节点处于运行状态，其它节点挂起
                //If it is a serial countersignature, only the first node is running, and the other nodes are suspended
                if ((i > 0) && (mainActivityInstance.MergeType.Value == (short)MergeTypeEnum.Sequence))
                {
                    entity.ActivityState = (short)ActivityStateEnum.Suspended;
                }

                entity.ID = ActivityInstanceManager.Insert(entity, session);

                base.TaskManager.Insert(entity, 
                    plist[i], 
                    base.ActivityForwardContext.ActivityResource.AppRunner, 
                    session);
            }
        }

        /// <summary>
        /// Determine whether the activity instance already exists
        /// 判断活动实例是否已经存在
        /// </summary>
        /// <param name="childNodeOfMI"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private Boolean IsTaskExisted(List<ActivityInstanceEntity> childNodeOfMI, 
            string userID)
        {
            var isExisted = false;
            var child = childNodeOfMI.Find(a => a.AssignedToUserIDs.Contains(userID));

            if (child != null)
                isExisted = true;
           
            return isExisted;
        }

        /// <summary>
        /// Copy parallel branch multi instance node data
        /// 复制并行分支多实例节点数据
        /// </summary>
        private void CloneChildNodeOfAndSplitMI(PerformerList plist,
            string transitionGUID,
            ActivityInstanceEntity gatewayActivityInstance,
            ActivityInstanceEntity toActivityInstance, 
            IDbSession session)
        {
            var childNodeListOfAndSplitMI = base.ActivityInstanceManager.GetValidSplitedActivityInstanceList(gatewayActivityInstance.ProcessInstanceID,
                gatewayActivityInstance.ID, session);

            ActivityInstanceEntity newChildInstance = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                var userID = plist[i].UserID;
                var userName = plist[i].UserName;
                var isTaskExisted = IsTaskExisted(childNodeListOfAndSplitMI, userID);
                if (isTaskExisted == true)
                {
                    //如果活动或者任务已经存在，则不用创建新活动和任务
                    //If an activity or task already exists, there is no need to create a new one
                    continue;
                }

                newChildInstance = ActivityInstanceManager.CreateActivityInstanceObject(toActivityInstance);
                newChildInstance.AssignedToUserIDs = userID;
                newChildInstance.AssignedToUserNames = userName;

                newChildInstance.ID = ActivityInstanceManager.Insert(newChildInstance, session);

                base.InsertTransitionInstance(base.ActivityForwardContext.ProcessInstance,
                    transitionGUID,
                    gatewayActivityInstance,
                    newChildInstance,
                    TransitionTypeEnum.Forward,
                    TransitionFlyingTypeEnum.NotFlying,
                    base.ActivityForwardContext.ActivityResource.AppRunner,
                    session);

                base.TaskManager.Insert(newChildInstance,
                    plist[i],
                    base.ActivityForwardContext.ActivityResource.AppRunner,
                    session);
            }
        }
    }
}
