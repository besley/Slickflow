
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
    internal class NodeMediatorInterMsgCatch : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorInterMsgCatch(ActivityForwardContext forwardContext,
            IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Created automatically
        /// </summary>
        public ActivityInstanceEntity CreatedAutomaticlly(Activity toActivity, ProcessInstanceEntity processInstance, WfAppRunner runner, IDbSession session)
        {
            var msgCatchActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);

            //写入默认第一次的预选步骤用户列表
            //Write the default user list for the first pre selection step
            msgCatchActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);
            base.InsertActivityInstance(msgCatchActivityInstance, session);

            return msgCatchActivityInstance;
        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
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
            string transitionId,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity toActivity,
            ActivityInstanceEntity toActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            base.ActivityInstanceManager.Complete(toActivityInstance, runner, session);

            LinkContext.CurrentActivity = toActivity;
            LinkContext.CurrentActivityInstance = toActivityInstance;

            base.CreateActivityTaskTransitionInstance(toActivity,
                processInstance,
                fromActivityInstance,
                transitionId,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                ActivityForwardContext.ActivityResource,
                session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
