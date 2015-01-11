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
    /// 子流程节点执行器
    /// </summary>
    internal class NodeMediatorSubProcess : NodeMediator
    {
        internal NodeMediatorSubProcess(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorSubProcess(IDbSession session)
            : base(session)
        {
        }

        /// <summary>
        /// 执行子流程节点
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                if (base.Linker.FromActivity.ActivityType == ActivityTypeEnum.SubProcessNode)
                {
                    //检查子流程是否结束
                    var pim = new ProcessInstanceManager();
                    bool isCompleted = pim.CheckSubProcessInstanceCompleted(Session.Connection,
                        base.Linker.FromActivityInstance.ID,
                        base.Linker.FromActivityInstance.ActivityGUID,
                        Session.Transaction);
                    if (isCompleted == false)
                    {
                        throw new WfRuntimeException(string.Format("当前子流程:[{0}]并没有到达结束状态，主流程无法向下执行。",
                            base.Linker.FromActivity.ActivityName));
                    }
                }

                //完成当前的任务节点
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskView.TaskID,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                if (canContinueForwardCurrentNode)
                {
                    //获取下一步节点列表：并继续执行
                    ContinueForwardCurrentNode(false);
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
        internal bool CompleteWorkItem(long taskID,
            ActivityResource activityResource,
            IDbSession session)
        {
            bool canContinueForwardCurrentNode = true;

            //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
            base.TaskManager.Complete(taskID, activityResource.AppRunner, session);

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

                if (base.Linker.FromActivity.ActivityTypeDetail.MergeType == MergeTypeEnum.Sequence)
                {
                    //取出最大执行节点
                    short maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder).Value;

                    if (base.Linker.FromActivityInstance.CompleteOrder < maxOrder)
                    {
                        //设置下一个任务进入准备状态
                        var currentNodeIndex = (short)base.Linker.FromActivityInstance.CompleteOrder.Value;
                        var nextActivityInstance = sqList[currentNodeIndex];
                        nextActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                        base.ActivityInstanceManager.Update(nextActivityInstance, session);

                        //设置下一个任务对应的子流程进入运行状态
                        base.ProcessInstanceManager.Resume(nextActivityInstance.ID,
                            activityResource.AppRunner,
                            Session);

                        canContinueForwardCurrentNode = false;
                        base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.ForwardToNextSequenceTask;
                    }
                    else if (base.Linker.FromActivityInstance.CompleteOrder == maxOrder)
                    {
                        //完成最后一个会签任务，会签主节点状态由挂起设置为准备状态
                        mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                        base.ActivityInstanceManager.Update(mainActivityInstance, session);
                        //将执行权责交由会签主节点
                        base.Linker.FromActivityInstance = mainActivityInstance;
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
                        //将执行权责交由会签主节点
                        base.Linker.FromActivityInstance = mainActivityInstance;
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
            if (toActivity.ActivityTypeDetail.ComplexType == ComplexTypeEnum.MultipleInstance)
            {
                CreateMultipleInstance(toActivity, processInstance, fromActivityInstance,
                    transitionGUID, transitionType, flyingType, activityResource, session);
            }
            else
            {
                //实例化Activity
                var toActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

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

                //启动子流程
                WfExecutedResult startedResult = null;
                var subProcessNode = (SubProcessNode)toActivity.Node;
                subProcessNode.ActivityInstance = fromActivityInstance;
                WfAppRunner subRunner = CopyActivityForwardRunner(activityResource.AppRunner, 
                    new Performer(activityResource.AppRunner.UserID, 
                        activityResource.AppRunner.UserName),
                    subProcessNode);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(subRunner,
                    ActivityForwardContext.ProcessInstance,
                    subProcessNode,
                    ref startedResult);
                runtimeInstance.Execute(Session);
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
            var toActivityInstance = CreateActivityInstanceObject(toActivity,
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

                //只有第一个节点处于运行状态，其它节点挂起
                if ((i > 0) && (toActivity.ActivityTypeDetail.MergeType == MergeTypeEnum.Sequence))
                {
                    entity.ActivityState = (short)ActivityStateEnum.Suspended;
                }

                //插入活动实例数据，并返回活动实例ID
                entity.ID = base.ActivityInstanceManager.Insert(entity, session);

                //插入任务数据
                base.TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);

                //启动子流程
                IDbSession subSession = SessionFactory.CreateSession();
                WfExecutedResult startedResult = null;
                var subProcessNode = (SubProcessNode)toActivity.Node;
                subProcessNode.ActivityInstance = entity;   //在流程实例表中记录激活子流程的活动节点ID
                WfAppRunner subRunner = CopyActivityForwardRunner(activityResource.AppRunner, 
                    plist[i],
                    subProcessNode);
                var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(subRunner,
                    processInstance,
                    subProcessNode,
                    ref startedResult);

                if (runtimeInstance.WfExecutedResult.Status == WfExecutedStatus.Exception)
                {
                    throw new WfRuntimeException(runtimeInstance.WfExecutedResult.Message);
                }
                runtimeInstance.Execute(subSession);

                //如果是串行会签，只有第一个子流程可以运行，其它子流程处于挂起状态
                if ((i > 0) && (toActivity.ActivityTypeDetail.MergeType == MergeTypeEnum.Sequence))
                {
                    var startResult = (WfExecutedResult)runtimeInstance.WfExecutedResult;
                    base.ProcessInstanceManager.Suspend(startedResult.ProcessInstanceIDStarted, subRunner, subSession);
                }
            }
        }

        /// <summary>
        /// 创建子流程时，重新生成runner信息
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="subProcessNode"></param>
        /// <returns></returns>
        private WfAppRunner CopyActivityForwardRunner(WfAppRunner runner, 
            Performer performer,
            SubProcessNode subProcessNode)
        {
            WfAppRunner subRunner = new WfAppRunner();
            subRunner.AppInstanceCode = runner.AppInstanceCode;
            subRunner.AppInstanceID = runner.AppInstanceID;
            subRunner.AppName = runner.AppName;
            subRunner.UserID = performer.UserID;
            subRunner.UserName = performer.UserName;
            subRunner.ProcessGUID = subProcessNode.SubProcessGUID;
            return subRunner;
        }
    }
}
