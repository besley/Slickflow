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
using Slickflow.Engine.Essential;

namespace Slickflow.Engine.Core.Pattern.Event.Timer
{
    /// <summary>
    /// Intermediate timer node mediator
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterTimer : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorInterTimer(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Created automatically
        /// </summary>
        public ActivityInstanceEntity CreatedAutomaticlly(Activity toActivity, ProcessInstanceEntity processInstance, WfAppRunner runner, IDbSession session)
        {
            var timerActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);

            //存储下一步步骤人员信息
            //Store personnel information for the next step
            timerActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);

            //延时时间信息
            //Overdue datetime
            timerActivityInstance.OverdueDateTime = CalcDataTimeFromVariable(toActivity);
            timerActivityInstance.AssignedUserIds = WfDefine.SYSTEM_JOBTIMER_USER_ID;
            timerActivityInstance.AssignedUserNames = WfDefine.SYSTEM_JOBTIMER_USER_NAME;

            //定时作业信息
            //job timer
            timerActivityInstance.JobTimerType = (short)JobTimerTypeEnum.Timer;
            timerActivityInstance.JobTimerStatus = (short)JobTimerStatusEnum.Ready;

            base.InsertActivityInstance(timerActivityInstance,
                session);

            return timerActivityInstance;
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

            LinkContext.CurrentActivityInstance = toActivityInstance;

            base.InsertTransitionInstance(processInstance,
                transitionId,
                fromActivityInstance,
                toActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            var tm = new TaskManager();
            var newTaskId = tm.Insert(toActivityInstance,
                new Performer(WfDefine.SYSTEM_JOBTIMER_USER_ID, WfDefine.SYSTEM_JOBTIMER_USER_NAME),
                runner,
                session);
            tm.SetTaskType(newTaskId, TaskTypeEnum.Automantic, session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }

        /// <summary>
        /// Calculate delay time
        /// 计算延迟时间
        /// </summary>
        /// <param name="timerActivity"></param>
        /// <returns></returns>
        private DateTime CalcDataTimeFromVariable(Activity timerActivity)
        {
            if (!string.IsNullOrEmpty(timerActivity.TriggerDetail.Expression))
            {
                var expression = timerActivity.TriggerDetail.Expression;
                try
                {
                    var timeSpan = System.Xml.XmlConvert.ToTimeSpan(expression);
                    var overdueDateTime = DateTime.UtcNow.Add(timeSpan);
                    return overdueDateTime;
                }
                catch (Exception ex)
                {
                    throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorintertimer.CalcDataTimeFromVariable.error", expression), ex);
                }
            }
            else
            {
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("nodemediatorintertimer.CalcDataTimeFromVariable.error"));
            }
        }
    }
}
