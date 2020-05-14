using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.SendBack
{
    /// <summary>
    /// 任务类型的退回处理器
    /// </summary>
    internal class NodeSendBackTask : NodeSendBack
    {
        internal NodeSendBackTask(SendBackOperation sendbackOperation, IDbSession session) 
            : base(sendbackOperation, session)
        {

        }

        /// <summary>
        /// 执行退回操作
        /// </summary>
        internal override void Execute()
        {
            if (base.SendBackOperation.IsCancellingBrothersNode == true)
            {
                //取消相邻并行分支上的其它并行节点
                CancelBrothersNode(base.SendBackOperation.BackwardToTaskActivity, 
                    base.SendBackOperation.BackwardFromActivityInstance,
                    base.SendBackOperation.ActivityResource.AppRunner,
                    base.Session);
            }

            base.CreateBackwardActivityTaskTransitionInstance(base.SendBackOperation.ProcessInstance,
                base.SendBackOperation.BackwardFromActivityInstance,
                base.SendBackOperation.BackwardToTaskActivity,
                base.SendBackOperation.BackwardType,
                TransitionTypeEnum.Sendback,
                TransitionFlyingTypeEnum.NotFlying,
                base.SendBackOperation.ActivityResource,
                base.Session);
        }

        /// <summary>
        /// 取消相邻分支节点
        /// </summary>
        /// <param name="toActivity">退回到的节点</param>
        /// <param name="fromActivityInstance">退回前的运行节点</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">数据会话</param>
        private void CancelBrothersNode(ActivityEntity toActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            var processModel = this.SendBackOperation.ProcessModel;
            var activityList = processModel.GetNextActivityListWithoutCondition(toActivity.ActivityGUID);
            var brothersList = activityList.Where(a => a.ActivityGUID != fromActivityInstance.ActivityGUID).ToList();

            foreach (var activity in brothersList)
            {
                var activityInstance = this.ActivityInstanceManager.GetActivityInstanceLatest(fromActivityInstance.ProcessInstanceID, 
                    activity.ActivityGUID,
                    session);
                if (activityInstance != null 
                    && this.ActivityInstanceManager.IncludeRunningState(activityInstance) == true)
                {
                    this.ActivityInstanceManager.Cancel(activityInstance.ID, runner, session);
                }
            }
        }
    }
}
