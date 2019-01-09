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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
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
    /// 结束节点处理类
    /// </summary>
    internal class NodeMediatorEnd : NodeMediator
    {
        internal NodeMediatorEnd(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {
            
        }

        /// <summary>
        /// 节点内部业务逻辑执行
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            //设置流程完成
            ProcessInstanceManager pim = new ProcessInstanceManager();
            pim.Complete(ActivityForwardContext.ProcessInstance.ID, 
                ActivityForwardContext.ActivityResource.AppRunner, 
                Session);
        }

        /// <summary>
        /// 结束节点活动及转移实例化，没有任务数据
        /// </summary>
        /// <param name="toActivity">当前Activity</param>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">Session</param>
        internal override void CreateActivityTaskTransitionInstance(ActivityEntity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var toActivityInstance = base.CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            base.ActivityInstanceManager.Insert(toActivityInstance, session);

            base.ActivityInstanceManager.Complete(toActivityInstance.ID,
                activityResource.AppRunner,
                session);

            //写节点转移实例数据
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
