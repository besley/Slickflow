/*
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
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
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
        /// 执行普通任务节点
        /// 1. 当设置任务完成时，同时设置活动完成
        /// 2. 当实例化活动数据时，产生新的任务数据
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                //完成当前的任务节点
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskView,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                if (canContinueForwardCurrentNode)
                {
                    bool isJumpforward = ActivityForwardContext.TaskView == null ? true : false;
                    //获取下一步节点列表：并继续执行
                    ContinueForwardCurrentNode(isJumpforward);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 完成任务实例
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>
        internal bool CompleteWorkItem(TaskViewEntity taskView,
            ActivityResource activityResource,
            IDbSession session)
        {
            bool canContinueForwardCurrentNode = true;

            //流程强制拉取向前跳转时，没有运行人的任务实例
            if (taskView != null)
            {
                //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
                base.TaskManager.Complete(taskView.TaskID, activityResource.AppRunner, session);
            }

            //设置活动节点的状态为完成状态
            base.ActivityInstanceManager.Complete(base.Linker.FromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            base.Linker.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;

            //先判断是否是多实例类型的任务
            if (base.Linker.FromActivity.ActivityTypeDetail.ComplexType == ComplexTypeEnum.MultipleInstance)
            {
                //取出主节点信息
                var mainNodeIndex = base.Linker.FromActivityInstance.MIHostActivityInstanceID.Value;
                var mainActivityInstance = base.ActivityInstanceManager.GetById(mainNodeIndex);

                //取出多实例节点列表
                var sqList = base.ActivityInstanceManager.GetActivityMulitipleInstance(
                    mainNodeIndex,
                    base.Linker.FromActivityInstance.ProcessInstanceID,
                    session).ToList<ActivityInstanceEntity>();

                if (sqList == null || sqList.Count == 0)
                {
                    throw new WfRuntimeException("会签主节点下没有多实例子节点，流程运行数据异常！");
                }

                if (base.Linker.FromActivity.ActivityTypeDetail.MergeType == MergeTypeEnum.Sequence)
                {
                    //取出最大执行节点
                    short maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder.Value);

                    if (base.Linker.FromActivityInstance.CompleteOrder < maxOrder)
                    {
                        //设置下一个任务进入准备状态
                        var currentNodeIndex = (short)base.Linker.FromActivityInstance.CompleteOrder.Value;
                        var nextActivityInstance = sqList[currentNodeIndex];
                        nextActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                        base.ActivityInstanceManager.Update(nextActivityInstance, session);

                        canContinueForwardCurrentNode = false;
                        base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.ForwardToNextSequenceTask;
                    }
                    else if (base.Linker.FromActivityInstance.CompleteOrder == maxOrder)
                    {
                        //完成最后一个会签任务，会签主节点状态由挂起设置为准备状态
                        mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                        base.ActivityInstanceManager.Update(mainActivityInstance, session);
                    }
                } 
                else if (base.Linker.FromActivity.ActivityTypeDetail.MergeType == MergeTypeEnum.Parallel)
                {
                    var allCount = sqList.Count();
                    var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed)
                        .ToList<ActivityInstanceEntity>()
                        .Count();
                    if (completedCount / allCount >= mainActivityInstance.CompleteOrder)
                    {
                        //如果超过约定的比例数，则执行下一步节点
                        mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                        base.ActivityInstanceManager.Update(mainActivityInstance, session);
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

        internal override void CreateActivityTaskTransitionInstance(ActivityEntity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            if (toActivity.ActivityTypeDetail != null
                && toActivity.ActivityTypeDetail.ComplexType == ComplexTypeEnum.MultipleInstance)
            {
                CreateMultipleInstance(toActivity, processInstance, fromActivityInstance,
                    transitionGUID, transitionType, flyingType, activityResource, session);
            }
            else
            {
                //实例化Activity
                var toActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

                //进入运行状态
                toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                toActivityInstance.AssignedToUsers = GenerateActivityAssignedUsers(
                    activityResource.NextActivityPerformers[toActivity.ActivityGUID]);

                //插入活动实例数据
                base.ActivityInstanceManager.Insert(toActivityInstance, session);

                //插入任务数据
                base.CreateNewTask(toActivityInstance, activityResource, session);

                //插入转移数据
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

        /// <summary>
        /// 会签类型的主节点, 多实例节点处理
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="fromToTransition"></param>
        /// <param name="fromActivityInstance"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>
        internal void CreateMultipleInstance(ActivityEntity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            String transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //实例化主节点Activity
            var toActivityInstance = base.CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            //主节点实例数据
            toActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
            toActivityInstance.ComplexType = (short)ComplexTypeEnum.MultipleInstance;
            if (toActivity.ActivityTypeDetail.MergeType == MergeTypeEnum.Parallel)
            {
                toActivityInstance.CompleteOrder = toActivity.ActivityTypeDetail.CompleteOrder;
            }
            toActivityInstance.AssignedToUsers = GenerateActivityAssignedUsers(
                activityResource.NextActivityPerformers[toActivity.ActivityGUID]);

            //插入主节点实例数据
            base.ActivityInstanceManager.Insert(toActivityInstance, session);

            //插入主节点转移数据
            InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);

            //插入会签子节点实例数据
            var plist = activityResource.NextActivityPerformers[toActivity.ActivityGUID];
            ActivityInstanceEntity entity = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                entity = base.ActivityInstanceManager.CreateActivityInstanceObject(toActivityInstance);
                entity.AssignedToUsers = plist[i].UserID.ToString();
                entity.MIHostActivityInstanceID = toActivityInstance.ID;
                entity.CompleteOrder = (short)(i + 1);

                //如果是串行会签，只有第一个节点处于运行状态，其它节点挂起
                if ((i > 0) && (toActivity.ActivityTypeDetail.MergeType == MergeTypeEnum.Sequence))
                {
                    entity.ActivityState = (short)ActivityStateEnum.Suspended;
                }

                //插入活动实例数据，并返回活动实例ID
                entity.ID = base.ActivityInstanceManager.Insert(entity, session);
                
                //插入任务数据
                base.TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);
            }
        }
    }
}
