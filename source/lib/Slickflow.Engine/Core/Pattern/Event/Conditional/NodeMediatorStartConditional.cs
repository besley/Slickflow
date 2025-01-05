using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Core.Runtime;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;


namespace Slickflow.Engine.Core.Pattern.Event.Conditional
{
    /// <summary>
    /// Node Mediator Start Conditional
    /// 开始节点执行器
    /// </summary>
    internal class NodeMediatorStartConditional : NodeMediator
    {
        internal NodeMediatorStartConditional(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute Work Item
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                //判断条件表达式是否满足
                //Determine whether the conditional expression satisfies
                var expression = ActivityForwardContext.Activity.TriggerDetail.Expression;
                var dvKeyValuePair = ActivityForwardContext.ActivityResource.AppRunner.Conditions;

                string expressionReplaced = ExpressionParser.ReplaceParameterToValue(expression, dvKeyValuePair);
                var result = ExpressionParser.Parse(expressionReplaced);

                if (result == true)
                {
                    ProcessInstanceManager pim = new ProcessInstanceManager();
                    var newID = pim.Insert(Session.Connection, ActivityForwardContext.ProcessInstance,
                        Session.Transaction);
                    ActivityForwardContext.ProcessInstance.ID = newID;

                    //执行前Action列表
                    //On before execute work item
                    OnBeforeExecuteWorkItem();

                    CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                        ActivityForwardContext.ActivityResource,
                        Session);

                    //执行后Action列表
                    //ON after execute work item
                    OnAfterExecuteWorkItem();

                    //执行开始节点之后的节点集合
                    //Collection of nodes after executing the start node
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, Session);
                }
                else
                {
                    throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorstartconditional.ExecuteWorkItem.exception"));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Set the start node to the completed state
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
