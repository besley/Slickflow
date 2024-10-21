
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

namespace Slickflow.Engine.Core.Pattern.Event.Signal
{
    /// <summary>
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterSignalThrow : NodeMediator, ICompleteAutomaticlly
    {
        internal NodeMediatorInterSignalThrow(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// 执行方法
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

        #region ICompleteAutomaticlly 成员
        /// <summary>
        /// 自动完成
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="toActivity">目标活动</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        /// <returns>网关执行结果</returns>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity toActivity,
            WfAppRunner runner,
            IDbSession session)
        {
            var messageActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            LinkContext.ToActivity = toActivity;
            LinkContext.ToActivityInstance = messageActivityInstance;

            base.InsertActivityInstance(messageActivityInstance,
                session);

            base.CompleteActivityInstance(messageActivityInstance.ID,
                runner,
                session);
            messageActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;


            //执行节点上的信号发布
            var signalDelegateService = new SignalDelegateService();
            signalDelegateService.PublishSignal(processInstance, LinkContext.ToActivity, messageActivityInstance);

            //写节点转移实例数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                messageActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
        #endregion
    }
}
