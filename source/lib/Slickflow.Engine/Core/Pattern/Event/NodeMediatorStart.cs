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
using System.Diagnostics;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Pattern.Event
{
    /// <summary>
    /// 开始节点执行器
    /// </summary>
    internal class NodeMediatorStart : NodeMediator
    {
        internal NodeMediatorStart(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {
            
        }

        /// <summary>
        /// 执行开始节点
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                //写入流程实例
                ProcessInstanceManager pim = new ProcessInstanceManager();
                var newID = pim.Insert(this.Session.Connection, ActivityForwardContext.ProcessInstance,
                    this.Session.Transaction);
                ActivityForwardContext.ProcessInstance.ID = newID;

                //执行前Action列表
                OnBeforeExecuteWorkItem();

                CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                //执行后Action列表
                OnAfterExecuteWorkItem();

                //执行开始节点之后的节点集合
                ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, this.Session);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 置开始节点为结束状态
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            //开始节点没前驱信息
            var fromActivityInstance = base.CreateActivityInstanceObject(base.LinkContext.FromActivity, processInstance, activityResource.AppRunner);

            base.ActivityInstanceManager.Insert(fromActivityInstance, session);

            base.ActivityInstanceManager.Complete(fromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            fromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            base.LinkContext.FromActivityInstance = fromActivityInstance;

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
