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
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 退回处理时的节点调节器
    /// </summary>
    internal class NodeMediatorBackward : NodeMediator
    {
        internal NodeMediatorBackward(BackwardContext backwardContext, IDbSession session)
            : base(backwardContext, session)
        {
            
        }

        internal override void ExecuteWorkItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建退回时的流转节点对象、任务和转移数据
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">运行节点实例</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
        internal void CreateBackwardActivityTaskTransitionInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            BackwardTypeEnum backwardType,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //实例化Activity
            var toActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                fromActivityInstance.ID,
                base.BackwardContext.BackwardToTaskActivityInstance.ID,
                activityResource.AppRunner);

            //进入准备运行状态
            toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

            //插入活动实例数据
            base.ActivityInstanceManager.Insert(toActivityInstance,
                session);

            base.ReturnDataContext.ActivityInstanceID = toActivityInstance.ID;
            base.ReturnDataContext.ProcessInstanceID = toActivityInstance.ProcessInstanceID;

            //插入任务数据
            base.CreateNewTask(toActivityInstance, activityResource, session);

            //插入转移数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }

         /// <summary>
        /// 退回是会签情况下的处理：
        /// 要退回的节点是会签节点
        /// 1) 全部实例化会签节点下的多实例节点
        /// 2) 只取得办理完成的节点，而且保证CompleteOrder的唯一性
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="backwardToTaskActvity">退回到的活动节点</param>
        /// <param name="fromActivityInstance">运行节点</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="previousMainInstance">前主节点实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
        internal void CreateBackwardActivityTaskRepeatedSignTogetherMultipleInstance(ProcessInstanceEntity processInstance,
            Activity backwardToTaskActvity,
            ActivityInstanceEntity fromActivityInstance,
            BackwardTypeEnum backwardType,
            ActivityInstanceEntity previousMainInstance,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //上一步节点是会签节点的退回处理
            //需要重新实例化会签节点上的所有办理人的任务
            //重新封装任务办理人为AssignedToUsers, AssignedToUsernames
            var performerList = AntiGenerateActivityPerformerList(previousMainInstance);

            activityResource.NextActivityPerformers.Clear();
            activityResource.NextActivityPerformers = new Dictionary<string, PerformerList>();
            activityResource.NextActivityPerformers.Add(backwardToTaskActvity.ActivityGUID, performerList);

            //重新生成会签节点的多实例数据
            CreateMultipleInstance(backwardToTaskActvity, processInstance, fromActivityInstance,
                transitionGUID, transitionType, flyingType, activityResource, session);
        }

        /// <summary>
        /// 创建多实例节点之间回滚时的活动实例，任务数据
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="originalBackwardToActivityInstance">原始退回到的节点实例</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="backSrcActivityInstanceID">源退回节点实例ID</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
        internal void CreateBackwardActivityTaskOfInnerMultipleInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity originalBackwardToActivityInstance,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            ActivityResource activityResource,
            IDbSession session)
        {
            //创建回滚到的节点信息
            var rollbackPreviousActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                backSrcActivityInstanceID,
                originalBackwardToActivityInstance.ID,
                activityResource.AppRunner);

            rollbackPreviousActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            rollbackPreviousActivityInstance.MIHostActivityInstanceID = originalBackwardToActivityInstance.MIHostActivityInstanceID;
            rollbackPreviousActivityInstance.CompleteOrder = originalBackwardToActivityInstance.CompleteOrder;
            rollbackPreviousActivityInstance.ComplexType = originalBackwardToActivityInstance.ComplexType;
            rollbackPreviousActivityInstance.SignForwardType = originalBackwardToActivityInstance.SignForwardType;
            rollbackPreviousActivityInstance.AssignedToUserIDs = originalBackwardToActivityInstance.AssignedToUserIDs;      //多实例节点为单一用户任务
            rollbackPreviousActivityInstance.AssignedToUserNames = originalBackwardToActivityInstance.AssignedToUserNames;

            //插入新活动实例数据
            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);

            base.ReturnDataContext.ActivityInstanceID = rollbackPreviousActivityInstance.ID;
            base.ReturnDataContext.ProcessInstanceID = rollbackPreviousActivityInstance.ProcessInstanceID;

            //创建新任务数据
            base.CreateNewTask(rollbackPreviousActivityInstance, activityResource, session);
        }

        /// <summary>
        /// 最后一个会签多实例子节点的撤销操作
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">运行节点</param>
        /// <param name="originalBackwardToActivityInstance">初始退回到的节点实例</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
        internal void CreateBackwardActivityTaskTransitionOfLastMultipleInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity originalBackwardToActivityInstance,
            BackwardTypeEnum backwardType,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //创建回滚到的节点信息
            var rollbackPreviousActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                fromActivityInstance.ID,
                originalBackwardToActivityInstance.ID,
                activityResource.AppRunner);

            rollbackPreviousActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            rollbackPreviousActivityInstance.MIHostActivityInstanceID = originalBackwardToActivityInstance.MIHostActivityInstanceID;
            rollbackPreviousActivityInstance.CompleteOrder = originalBackwardToActivityInstance.CompleteOrder;
            rollbackPreviousActivityInstance.ComplexType = originalBackwardToActivityInstance.ComplexType;
            rollbackPreviousActivityInstance.SignForwardType = originalBackwardToActivityInstance.SignForwardType;
            rollbackPreviousActivityInstance.AssignedToUserIDs = originalBackwardToActivityInstance.AssignedToUserIDs;      //多实例节点为单一用户任务
            rollbackPreviousActivityInstance.AssignedToUserNames = originalBackwardToActivityInstance.AssignedToUserNames;

            //插入新活动实例数据
            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);
            base.ReturnDataContext.ActivityInstanceID = rollbackPreviousActivityInstance.ID;
            base.ReturnDataContext.ProcessInstanceID = rollbackPreviousActivityInstance.ProcessInstanceID;
            //创建新任务数据
            base.CreateNewTask(rollbackPreviousActivityInstance, activityResource, session);

            //插入转移数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                rollbackPreviousActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }
    }
}
