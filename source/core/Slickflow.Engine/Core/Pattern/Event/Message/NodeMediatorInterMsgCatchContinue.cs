
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
using Slickflow.Module.Essential;

namespace Slickflow.Engine.Core.Pattern.Event.Message
{
    /// <summary>
    /// Intermediate Node Mediator Message Catch Continue
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterMsgCatchContinue : NodeMediator
    {
        internal NodeMediatorInterMsgCatchContinue(ActivityForwardContext forwardContext,
            IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute Work Item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                OnBeforeExecuteWorkItem();

                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.ActivityResource,
                    Session);

                OnAfterExecuteWorkItem();

                //获取下一步节点列表：并继续执行
                //Get the next node list: and continue execution
                if (canContinueForwardCurrentNode)
                {
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, Session);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Complete work item
        /// 完成节点实例
        /// </summary>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>        
        internal bool CompleteWorkItem(ActivityResource activityResource,
            IDbSession session)
        {
            WfAppRunner runner = new WfAppRunner
            {
                UserId = activityResource.AppRunner.UserId,        
                UserName = activityResource.AppRunner.UserName
            };

            //设置活动节点的状态为完成状态
            //Set the status of the activity node to complete status
            ActivityInstanceManager.Complete(LinkContext.FromActivityInstance.Id,
                activityResource.AppRunner,
                session);

            LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            ActivityForwardContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            bool canContinueForwardCurrentNode = LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

            return canContinueForwardCurrentNode;
        }
    }
}
