using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern.Event
{
    /// <summary>
    /// 任务节点执行器
    /// </summary>
    internal class NodeMediatorScript : NodeMediator, ICompleteAutomaticlly
    {
        internal NodeMediatorScript(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorScript(IDbSession session)
            : base(session)
        {

        }

        /// <summary>
        /// 执行自动服务节点
        /// 1. 当设置任务完成时，同时设置活动完成
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                //完成节点上绑定的外部程序逻辑或服务
                OnExecutingServiceItem();
            }
            catch (System.Exception ex)
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
            var gatewayActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, runner);
            base.InsertActivityInstance(gatewayActivityInstance,
                session);

            base.CompleteActivityInstance(gatewayActivityInstance.ID,
                runner,
                session);

            gatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            base.LinkContext.ToActivityInstance = gatewayActivityInstance;

            //写节点转移实例数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                gatewayActivityInstance,
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
