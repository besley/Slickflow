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
    /// Start Node Mediator
    /// 开始节点执行器
    /// </summary>
    internal class NodeMediatorStart : NodeMediator
    {
        internal NodeMediatorStart(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {
            
        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                ProcessInstanceManager pim = new ProcessInstanceManager();
                var newId = pim.Insert(this.Session.Connection, ActivityForwardContext.ProcessInstance,
                    this.Session.Transaction);
                ActivityForwardContext.ProcessInstance.Id = newId;

                OnBeforeExecuteWorkItem();

                CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                OnAfterExecuteWorkItem();

                //执行开始节点之后的节点集合
                //Collection of nodes after executing the start node
                ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, this.Session);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Complete automatically
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            var fromActivityInstance = base.CreateActivityInstanceObject(base.LinkContext.FromActivity, processInstance, activityResource.AppRunner);

            base.ActivityInstanceManager.Insert(fromActivityInstance, session);

            base.ActivityInstanceManager.Complete(fromActivityInstance.Id,
                activityResource.AppRunner,
                session);

            fromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            base.LinkContext.FromActivityInstance = fromActivityInstance;

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
