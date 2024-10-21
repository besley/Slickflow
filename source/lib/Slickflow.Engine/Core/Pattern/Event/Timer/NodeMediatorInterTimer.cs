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
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterTimer : NodeMediator, ICompleteAutomaticlly
    {
        internal NodeMediatorInterTimer(ActivityForwardContext forwardContext, IDbSession session)
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
            var timerActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            //存储下一步步骤人员信息
            timerActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);
            //延时时间信息
            timerActivityInstance.OverdueDateTime = CalcDataTimeFromVariable(toActivity);
            timerActivityInstance.AssignedToUserIDs = WfDefine.SYSTEM_JOBTIMER_USER_ID;
            timerActivityInstance.AssignedToUserNames = WfDefine.SYSTEM_JOBTIMER_USER_NAME;
            //定时作业信息
            timerActivityInstance.JobTimerType = (short)JobTimerTypeEnum.Timer;
            timerActivityInstance.JobTimerStatus = (short)JobTimerStatusEnum.Ready;

            base.InsertActivityInstance(timerActivityInstance,
                session);
            LinkContext.ToActivityInstance = timerActivityInstance;

            //写节点转移实例数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                timerActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            //插入任务数据
            var tm = new TaskManager();
            var newTaskID = tm.Insert(timerActivityInstance,
                new Performer(WfDefine.SYSTEM_JOBTIMER_USER_ID, WfDefine.SYSTEM_JOBTIMER_USER_NAME),
                runner,
                session);
            tm.SetTaskType(newTaskID, TaskTypeEnum.Automantic, session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }

        /// <summary>
        /// 计算延迟时间
        /// </summary>
        /// <param name="timerActivity">活动</param>
        /// <returns>延迟时间</returns>
        private DateTime CalcDataTimeFromVariable(Activity timerActivity)
        {
            if (!string.IsNullOrEmpty(timerActivity.TriggerDetail.Expression))
            {
                var expression = timerActivity.TriggerDetail.Expression;
                try
                {
                    var timeSpan = System.Xml.XmlConvert.ToTimeSpan(expression);
                    var overdueDateTime = DateTime.Now.Add(timeSpan);
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
        #endregion
    }
}
