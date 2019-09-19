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

using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 跳转方式的处理
    /// </summary>
    internal class WfRuntimeManagerClose : WfRuntimeManager
    {
        /// <summary>
        /// 跳转执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            var jumpActivityGUID = base.AppRunner.NextActivityPerformers.First().Key;
            var jumpforwardActivity = base.ProcessModel.GetActivity(jumpActivityGUID);
            var processInstance = (new ProcessInstanceManager()).GetById(base.RunningActivityInstance.ProcessInstanceID);
            var jumpforwardExecutionContext = ActivityForwardContext.CreateJumpforwardContext(jumpforwardActivity,
                base.ProcessModel, processInstance, base.ActivityResource);
            jumpforwardExecutionContext.FromActivityInstance = base.RunningActivityInstance;

            NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(jumpforwardExecutionContext, session);
            mediator.Linker.FromActivityInstance = base.RunningActivityInstance;
            mediator.Linker.ToActivity = jumpforwardActivity;

            if (mediator is NodeMediatorEnd)
            {
                //结束节点的连线转移
                mediator.CreateActivityTaskTransitionInstance(jumpforwardActivity,
                    processInstance,
                    base.RunningActivityInstance,
                    WfDefine.WF_XPDL_JUMP_BYPASS_GUID,
                    TransitionTypeEnum.Forward,
                    TransitionFlyingTypeEnum.ForwardFlying,
                    base.ActivityResource,
                    session);
            }
            mediator.ExecuteWorkItem();

            result.Status = WfExecutedStatus.Success;
            result.Message = mediator.GetNodeMediatedMessage();
        }
    }
}
