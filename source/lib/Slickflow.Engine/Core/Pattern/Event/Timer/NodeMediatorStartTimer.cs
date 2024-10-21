
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

namespace Slickflow.Engine.Core.Pattern.Event.Timer
{
    /// <summary>
    /// 开始节点执行器
    /// </summary>
    internal class NodeMediatorStartTimer : NodeMediator
    {
        internal NodeMediatorStartTimer(ActivityForwardContext forwardContext, IDbSession session)
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
                var newID = pim.Insert(Session.Connection, ActivityForwardContext.ProcessInstance,
                    Session.Transaction);
                ActivityForwardContext.ProcessInstance.ID = newID;

                //执行前Action列表
                OnBeforeExecuteWorkItem();

                CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                    ActivityForwardContext.ActivityResource,
                    Session);

                //执行后Action列表
                OnAfterExecuteWorkItem();

                //执行开始节点之后的节点集合
                ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, Session);
            }
            catch (Exception ex)
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
            var fromActivityInstance = CreateActivityInstanceObject(LinkContext.FromActivity, processInstance, activityResource.AppRunner);

            ActivityInstanceManager.Insert(fromActivityInstance, session);

            ActivityInstanceManager.Complete(fromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            fromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            LinkContext.FromActivityInstance = fromActivityInstance;

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
