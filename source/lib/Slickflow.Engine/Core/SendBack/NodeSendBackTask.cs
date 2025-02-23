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
    /// Sendback processing for task types
    /// 任务类型的退回处理器
    /// </summary>
    internal class NodeSendBackTask : NodeSendBack
    {
        internal NodeSendBackTask(SendBackOperation sendbackOperation, IDbSession session) 
            : base(sendbackOperation, session)
        {

        }

        /// <summary>
        /// Execute method
        /// </summary>
        internal override void Execute()
        {
            if (base.SendBackOperation.IsCancellingBrothersNode == true)
            {
                //取消相邻并行分支上的其它并行节点
                //Cancel other parallel nodes on adjacent parallel branches
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
        /// Cancel adjacent branch nodes
        /// 取消相邻分支节点
        /// </summary>
        private void CancelBrothersNode(Activity toActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            var processModel = this.SendBackOperation.ProcessModel;
            var activityList = processModel.GetNextActivityListWithoutCondition(toActivity.ActivityID);
            var brothersList = activityList.Where(a => a.ActivityID != fromActivityInstance.ActivityID).ToList();

            foreach (var activity in brothersList)
            {
                var activityInstance = this.ActivityInstanceManager.GetActivityInstanceLatest(fromActivityInstance.ProcessInstanceID, 
                    activity.ActivityID,
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
