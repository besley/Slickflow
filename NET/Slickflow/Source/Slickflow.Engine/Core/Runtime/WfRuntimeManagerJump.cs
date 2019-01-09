/*
* Slickflow 开源项目遵循LGPL协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
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
using System.Threading;
using System.Data.Linq;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 跳转方式的处理
    /// </summary>
    internal class WfRuntimeManagerJump : WfRuntimeManager
    {
        /// <summary>
        /// 跳转执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            //回跳类型的处理
            if (base.IsBackward == true)
            {
                //创建新任务节点
                var backMostPreviouslyActivityInstanceID = GetBackwardMostPreviouslyActivityInstanceID();
                var nodeMediatorBackward = new NodeMediatorBackward(base.BackwardContext, session);

                nodeMediatorBackward.CreateBackwardActivityTaskTransitionInstance(base.BackwardContext.ProcessInstance,
                    base.BackwardContext.BackwardFromActivityInstance,
                    BackwardTypeEnum.Sendback,
                    backMostPreviouslyActivityInstanceID,
                    base.BackwardContext.BackwardToTargetTransitionGUID,
                    TransitionTypeEnum.Sendback,
                    TransitionFlyingTypeEnum.NotFlying,
                    base.ActivityResource,
                    session);

                //更新当前办理节点的状态（从准备或运行状态更新为退回状态）
                var aim = new ActivityInstanceManager();
                aim.SendBack(base.BackwardContext.BackwardFromActivityInstance.ID,
                    base.ActivityResource.AppRunner,
                    session);

                //构造回调函数需要的数据
                result.BackwardTaskReceiver = base.BackwardContext.BackwardTaskReceiver;
                result.Status = WfExecutedStatus.Success;
            }
            else
            {
                var jumpActivityGUID = base.AppRunner.NextActivityPerformers.First().Key;
                var jumpforwardActivity = base.ProcessModel.GetActivity(jumpActivityGUID);
                var proecessInstance = (new ProcessInstanceManager()).GetById(base.RunningActivityInstance.ProcessInstanceID);
                var jumpforwardExecutionContext = ActivityForwardContext.CreateJumpforwardContext(jumpforwardActivity,
                    base.ProcessModel, proecessInstance, base.ActivityResource);

                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(jumpforwardExecutionContext, session);
                mediator.Linker.FromActivityInstance = base.RunningActivityInstance;
                mediator.Linker.ToActivity = jumpforwardActivity;
                mediator.ExecuteWorkItem();

                result.Status = WfExecutedStatus.Success;
                result.Message = mediator.GetNodeMediatedMessage();
            }
        }
    }
}
