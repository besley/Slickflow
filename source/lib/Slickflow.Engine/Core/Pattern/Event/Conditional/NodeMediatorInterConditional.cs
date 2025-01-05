
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
    internal class NodeMediatorInterConditional : NodeMediator, ICompleteAutomaticlly
    {
        internal NodeMediatorInterConditional(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

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

        #region ICompleteAutomaticlly Member
        /// <summary>
        /// Complete Automatically
        /// 自动完成
        /// </summary>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity toActivity,
            WfAppRunner runner,
            IDbSession session)
        {
            var condActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            //存储下一步步骤人员信息
            //Store performer information for the next step
            condActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);
            condActivityInstance.AssignedToUserIDs = WfDefine.SYSTEM_JOBTIMER_USER_ID;
            condActivityInstance.AssignedToUserNames = WfDefine.SYSTEM_JOBTIMER_USER_NAME;

            //定时作业信息
            //Job timer info
            condActivityInstance.JobTimerType = (short)JobTimerTypeEnum.Conditional;
            condActivityInstance.JobTimerStatus = (short)JobTimerStatusEnum.Ready;
            condActivityInstance.TriggerExpression = toActivity.TriggerDetail.Expression;

            base.InsertActivityInstance(condActivityInstance,
                session);
            LinkContext.ToActivityInstance = condActivityInstance;

            //写节点转移实例数据
            //Write transition instance data
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                condActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            //插入任务数据
            //Insert task data
            var tm = new TaskManager();
            var newTaskID = tm.Insert(condActivityInstance,
                new Performer(WfDefine.SYSTEM_JOBTIMER_USER_ID, WfDefine.SYSTEM_JOBTIMER_USER_NAME),
                runner,
                session);
            tm.SetTaskType(newTaskID, TaskTypeEnum.Automantic, session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
        #endregion
    }
}
