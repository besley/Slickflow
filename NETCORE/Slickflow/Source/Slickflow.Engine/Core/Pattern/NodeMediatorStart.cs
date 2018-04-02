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
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Pattern
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
                var newID = pim.Insert(ActivityForwardContext.ProcessInstance,
                    this.Session);

                ActivityForwardContext.ProcessInstance.ID = newID;
                
                CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                //执行开始节点之后的节点集合
                ContinueForwardCurrentNode(false);

                //执行Action列表
                ExecteActionList(ActivityForwardContext.Activity.ActionList,
                    ActivityForwardContext.ActivityResource.AppRunner.ActionMethodParameters);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行外部操作的方法
        /// </summary>
        /// <param name="actionList"></param>
        /// <param name="actionMethodParameters"></param>
        public new void ExecteActionList(IList<ActionEntity> actionList, IDictionary<string, ActionParameterInternal> actionMethodParameters)
        {
            if (actionList != null && actionList.Count > 0)
            {
                var actionExecutor = new ActionExecutor();
                actionExecutor.ExecteActionList(actionList, actionMethodParameters);
            }
        }

        /// <summary>
        /// 置开始节点为结束状态
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="activityResource">活动实例</param>
        /// <param name="session">会话</param>
        /// <returns>网关执行结果</returns>
        private GatewayExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            //开始节点没前驱信息
            var fromActivityInstance = base.CreateActivityInstanceObject(base.Linker.FromActivity, processInstance, activityResource.AppRunner);

            base.ActivityInstanceManager.Insert(fromActivityInstance, session);

            base.ActivityInstanceManager.Complete(fromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            fromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            base.Linker.FromActivityInstance = fromActivityInstance;

            GatewayExecutedResult result = GatewayExecutedResult.CreateGatewayExecutedResult(GatewayExecutedStatus.Successed);
            return result;
        }
    }
}
