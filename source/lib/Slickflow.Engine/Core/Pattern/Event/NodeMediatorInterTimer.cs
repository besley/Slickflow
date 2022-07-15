/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

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
using Slickflow.Engine.Essential.Job;

namespace Slickflow.Engine.Core.Pattern.Event
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
            var timerActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, runner);
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
            base.LinkContext.ToActivityInstance = timerActivityInstance;

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
                    var overdueDateTime = System.DateTime.Now.Add(timeSpan);
                    return overdueDateTime;
                }
                catch (System.Exception ex)
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
