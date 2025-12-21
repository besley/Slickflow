
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Core.Runtime;
using Microsoft.VisualBasic;
using static IronPython.Modules._ast;

namespace Slickflow.Engine.Core.Pattern.Event.Conditional
{
    /// <summary>
    /// Intermediate event node (condition) processing class
    /// 中间事件节点(条件)处理类
    /// </summary>
    internal class NodeMediatorInterConditional : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorInterConditional(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Created automatically
        /// </summary>
        public ActivityInstanceEntity CreatedAutomaticlly(Activity toActivity, ProcessInstanceEntity processInstance, WfAppRunner runner, IDbSession session)
        {
            var condActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            //存储下一步步骤人员信息
            //Store performer information for the next step
            condActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);
            condActivityInstance.AssignedUserIds = WfDefine.SYSTEM_JOBTIMER_USER_ID;
            condActivityInstance.AssignedUserNames = WfDefine.SYSTEM_JOBTIMER_USER_NAME;

            //定时作业信息
            //Job timer info
            condActivityInstance.JobTimerType = (short)JobTimerTypeEnum.Conditional;
            condActivityInstance.JobTimerStatus = (short)JobTimerStatusEnum.Ready;
            condActivityInstance.TriggerExpression = toActivity.TriggerDetail.Expression;

            base.InsertActivityInstance(condActivityInstance,
                session);

            return condActivityInstance;
        }

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

        #region ICompleteAutomaticlly Member
        /// <summary>
        /// Complete Automatically
        /// 自动完成
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
            LinkContext.CurrentActivityInstance = toActivityInstance;

            //写节点转移实例数据
            //Write transition instance data
            base.InsertTransitionInstance(processInstance,
                transitionId,
                fromActivityInstance,
                toActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            //插入任务数据
            //Insert task data
            var tm = new TaskManager();
            var newTaskId = tm.Insert(toActivityInstance,
                new Performer(WfDefine.SYSTEM_JOBTIMER_USER_ID, WfDefine.SYSTEM_JOBTIMER_USER_NAME),
                runner,
                session);
            tm.SetTaskType(newTaskId, TaskTypeEnum.Automantic, session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
        #endregion
    }
}
