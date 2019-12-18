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
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 流程修订的具体执行类
    /// </summary>
    internal class NodeMediatorRevise : NodeMediator
    {
        #region 构造函数
        internal NodeMediatorRevise(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorRevise(IDbSession session)
            : base(session)
        {

        }
        #endregion

        /// <summary>
        /// 执行普通任务节点
        /// 1. 当设置任务完成时，同时设置活动完成
        /// 2. 当实例化活动数据时，产生新的任务数据
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                //执行前Action列表
                OnBeforeExecuteWorkItem();

                //先取出原始退回节点信息
                var backSrcActivityInstanceID = base.ActivityForwardContext.FromActivityInstance.BackSrcActivityInstanceID;
                if (backSrcActivityInstanceID != null)
                {
                    //完成当前的任务节点
                    bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskID,
                        ActivityForwardContext.ActivityResource,
                        this.Session);

                    if (canContinueForwardCurrentNode == true)
                    {
                        var aim = new ActivityInstanceManager();
                        var backSrcActivityInstance = aim.GetById(backSrcActivityInstanceID.Value);

                        //取出下一步办理人员信息
                        var nextStep = base.ActivityForwardContext.ActivityResource.AppRunner.NextActivityPerformers;
                        var performerList = nextStep[backSrcActivityInstance.ActivityGUID];

                        //判断不同的修订模式
                        var mainActivityInstanceID = backSrcActivityInstance.MIHostActivityInstanceID;
                        if (mainActivityInstanceID != null)
                        {
                            //会签模式
                            //复制与会签子节点相同的活动实例和任务记录 
                            var mainActivityInstance = aim.GetById(mainActivityInstanceID.Value);
                            CloneChildNodeOfMI(performerList, mainActivityInstance, base.Session);
                        }
                        else
                        {
                            //并行分支（多实例）的情况
                            var transitionList = ActivityForwardContext.ProcessModel.GetBackwardTransitionList(backSrcActivityInstance.ActivityGUID);
                            if (transitionList != null && transitionList.Count == 1)
                            {
                                var transition = transitionList[0];
                                var gatewayNode = ActivityForwardContext.ProcessModel.GetActivity(transition.FromActivityGUID);
                                if (gatewayNode.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
                                {
                                    //复制并行分支多实例
                                    var gatewayActivityInstance = base.ActivityInstanceManager.GetActivityInstanceLatest(
                                        backSrcActivityInstance.ProcessInstanceID,
                                        gatewayNode.ActivityGUID, base.Session);
                                    CloneChildNodeOfAndSplitMI(performerList, transition.TransitionGUID, gatewayActivityInstance, backSrcActivityInstance, base.Session);
                                }
                                else
                                {
                                    throw new WorkflowException("当前节点不满足并行分支多实例(AndSplitMI)的流程修订，暂不支持！请联系技术人员！");
                                }
                            }
                            else
                            {
                                throw new WorkflowException("当前不是会签或者并行分支多实例(AndSplitMI)的流程修订，暂不支持！请联系技术人员！");
                            }
                        }
                    }
                    else
                    {
                        throw new WorkflowException("虽然能够完成当前任务节点，但是不能继续向下流转，请联系技术人员！");
                    } 
                }
                else
                {
                    throw new WorkflowException("流程无法修订，因为没有退回来源节点数据！");
                }
                //执行后Action列表
                OnAfterExecuteWorkItem();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 完成任务实例
        /// </summary>
        /// <param name="taskID">任务视图</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">会话</param>        
        internal bool CompleteWorkItem(int? taskID,
            ActivityResource activityResource,
            IDbSession session)
        {
            bool canContinueForwardCurrentNode = true;

            WfAppRunner runner = new WfAppRunner
            {
                UserID = activityResource.AppRunner.UserID,         //避免taskview为空
                UserName = activityResource.AppRunner.UserName
            };

            //流程强制拉取向前跳转时，没有运行人的任务实例
            if (taskID != null)
            {
                //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
                base.TaskManager.Complete(taskID.Value, activityResource.AppRunner, session);

                //设置活动节点的状态为完成状态
                var activityInstanceID = base.ActivityInstanceManager.GetByTask(taskID.Value).ID;
                base.ActivityInstanceManager.Complete(activityInstanceID,
                    activityResource.AppRunner,
                    session);
            }
            return canContinueForwardCurrentNode;
        }

        /// <summary>
        /// 复制多实例子节点数据
        /// </summary>
        /// <param name="plist">执行人员列表</param>
        /// <param name="mainActivityInstance">主节点</param>
        /// <param name="session">数据会话</param>
        private void CloneChildNodeOfMI(PerformerList plist,
            ActivityInstanceEntity mainActivityInstance, 
            IDbSession session)
        {
            //查询已有的子节点列表
            var childNodeListOfMI = base.ActivityInstanceManager.GetValidActivityInstanceListOfMI(mainActivityInstance.ID, session);

            //创建活动实例
            ActivityInstanceEntity entity = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                var userID = plist[i].UserID;
                var userName = plist[i].UserName;
                var isTaskExisted = IsTaskExisted(childNodeListOfMI, userID);
                if (isTaskExisted == true)
                {
                    //如果活动或者任务已经存在，则不用创建新活动和任务
                    continue;
                }

                //根据主节点来复制子节点数据
                entity = ActivityInstanceManager.CreateActivityInstanceObject(mainActivityInstance);
                entity.AssignedToUserIDs = userID;
                entity.AssignedToUserNames = userName;
                entity.MIHostActivityInstanceID = mainActivityInstance.ID;

                //并行串行下，多实例子节点的执行顺序设置
                if (mainActivityInstance.MergeType == (short)MergeTypeEnum.Sequence)
                {
                    entity.CompleteOrder = (short)(i + 1);
                }
                else if (mainActivityInstance.MergeType == (short)MergeTypeEnum.Parallel)
                {
                    entity.CompleteOrder = -1;       //并行模式下CompleteOrder的优先级一样，所以置为 -1
                }

                //如果是串行会签，只有第一个节点处于运行状态，其它节点挂起
                if ((i > 0) && (mainActivityInstance.MergeType.Value == (short)MergeTypeEnum.Sequence))
                {
                    entity.ActivityState = (short)ActivityStateEnum.Suspended;
                }

                //插入活动实例数据，并返回活动实例ID
                entity.ID = ActivityInstanceManager.Insert(entity, session);

                //插入任务数据
                base.TaskManager.Insert(entity, 
                    plist[i], 
                    base.ActivityForwardContext.ActivityResource.AppRunner, 
                    session);
            }
        }

        /// <summary>
        /// 判断活动实例是否已经存在
        /// </summary>
        /// <param name="childNodeOfMI">活动实例列表</param>
        /// <param name="userID">用户ID</param>
        /// <returns>存在标记</returns>
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
        /// 复制并行分支多实例节点数据
        /// </summary>
        /// <param name="plist">执行人员列表</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="gatewayActivityInstance">网关活动实例</param>
        /// <param name="toActivityInstance">多实例分支节点</param>
        /// <param name="session">数据会话</param>
        private void CloneChildNodeOfAndSplitMI(PerformerList plist,
            string transitionGUID,
            ActivityInstanceEntity gatewayActivityInstance,
            ActivityInstanceEntity toActivityInstance, 
            IDbSession session)
        {
            //查询已有的子节点列表
            var childNodeListOfAndSplitMI = base.ActivityInstanceManager.GetValidSplitedActivityInstanceList(gatewayActivityInstance.ProcessInstanceID,
                gatewayActivityInstance.ID, session);

            //创建活动实例
            ActivityInstanceEntity newChildInstance = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                var userID = plist[i].UserID;
                var userName = plist[i].UserName;
                var isTaskExisted = IsTaskExisted(childNodeListOfAndSplitMI, userID);
                if (isTaskExisted == true)
                {
                    //如果活动或者任务已经存在，则不用创建新活动和任务
                    continue;
                }

                newChildInstance = ActivityInstanceManager.CreateActivityInstanceObject(toActivityInstance);
                newChildInstance.AssignedToUserIDs = userID;
                newChildInstance.AssignedToUserNames = userName;

                //插入活动实例数据，并返回活动实例ID
                newChildInstance.ID = ActivityInstanceManager.Insert(newChildInstance, session);

                //插入转移数据
                base.InsertTransitionInstance(base.ActivityForwardContext.ProcessInstance,
                    transitionGUID,
                    gatewayActivityInstance,
                    newChildInstance,
                    TransitionTypeEnum.Forward,
                    TransitionFlyingTypeEnum.NotFlying,
                    base.ActivityForwardContext.ActivityResource.AppRunner,
                    session);

                //插入任务数据
                base.TaskManager.Insert(newChildInstance,
                    plist[i],
                    base.ActivityForwardContext.ActivityResource.AppRunner,
                    session);
            }
        }
    }
}
