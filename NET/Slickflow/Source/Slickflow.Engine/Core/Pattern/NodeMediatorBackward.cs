/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
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
        /// <param name="backMostPreviouslyActivityInstanceID">退回节点实例ID</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
        internal void CreateBackwardActivityTaskTransitionInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            BackwardTypeEnum backwardType,
            int backMostPreviouslyActivityInstanceID,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //实例化Activity
            var toActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                backMostPreviouslyActivityInstanceID,
                activityResource.AppRunner);

            //进入准备运行状态
            toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            toActivityInstance.AssignedToUserIDs = base.GenerateActivityAssignedUserIDs(
                activityResource.NextActivityPerformers[base.BackwardContext.BackwardToTaskActivity.ActivityGUID]);
            toActivityInstance.AssignedToUserNames = base.GenerateActivityAssignedUserNames(
                activityResource.NextActivityPerformers[base.BackwardContext.BackwardToTaskActivity.ActivityGUID]);

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
    }
}
