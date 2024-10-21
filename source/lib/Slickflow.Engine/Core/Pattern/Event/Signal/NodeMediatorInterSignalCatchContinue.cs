
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

namespace Slickflow.Engine.Core.Pattern.Event.Signal
{
    /// <summary>
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterSignalCatchContinue : NodeMediator
    {
        internal NodeMediatorInterSignalCatchContinue(ActivityForwardContext forwardContext,
            IDbSession session)
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

                //完成当前的任务节点
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.ActivityResource,
                    Session);

                OnAfterExecuteWorkItem();

                //获取下一步节点列表：并继续执行
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
        /// 完成节点实例
        /// </summary>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">会话</param>        
        internal bool CompleteWorkItem(ActivityResource activityResource,
            IDbSession session)
        {
            WfAppRunner runner = new WfAppRunner
            {
                UserID = activityResource.AppRunner.UserID,         //避免taskview为空
                UserName = activityResource.AppRunner.UserName
            };

            //设置活动节点的状态为完成状态
            ActivityInstanceManager.Complete(LinkContext.FromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            ActivityForwardContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            bool canContinueForwardCurrentNode = LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

            return canContinueForwardCurrentNode;
        }
    }
}
