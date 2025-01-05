
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Essential;

namespace Slickflow.Engine.Core.Pattern.Event.Message
{
    /// <summary>
    /// Intermediate Node Mediator message catch
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterMsgCatch : NodeMediator, ICompleteAutomaticlly
    {
        internal NodeMediatorInterMsgCatch(ActivityForwardContext forwardContext,
            IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                OnBeforeExecuteWorkItem();

                OnAfterExecuteWorkItem();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Complete automatically
        /// </summary>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity toActivity,
            WfAppRunner runner,
            IDbSession session)
        {
            var nextActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);

            //写入默认第一次的预选步骤用户列表
            //Write the default user list for the first pre selection step
            nextActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);
            base.InsertActivityInstance(nextActivityInstance, session);

            LinkContext.ToActivity = toActivity;
            LinkContext.ToActivityInstance = nextActivityInstance;

            base.CreateActivityTaskTransitionInstance(toActivity,
                processInstance,
                fromActivityInstance,
                transitionGUID,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                ActivityForwardContext.ActivityResource,
                session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
