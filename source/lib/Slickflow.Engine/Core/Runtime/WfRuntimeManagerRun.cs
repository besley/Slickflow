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

using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Pattern;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 应用执行运行时
    /// </summary>
    internal class WfRuntimeManagerRun : WfRuntimeManager
    {
        /// <summary>
        /// 运行执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            var result = base.WfExecutedResult;
            result.ProcessInstanceIDStarted = RunningActivityInstance.ProcessInstanceID;

            try
            {
                //创建运行时上下文
                ActivityForwardContext runningExecutionContext = null;
                if (base.TaskView != null)
                {
                    //有TaskID的任务类型执行上下文
                    runningExecutionContext = ActivityForwardContext.CreateRunningContextByTask(base.TaskView,
                        base.ProcessModel,
                        base.ActivityResource,
                        false,
                        session);
                }
                else
                {
                    //Interrupt事件类型的活动执行上下文
                    runningExecutionContext = ActivityForwardContext.CreateRunningContextByActivity(base.RunningActivityInstance,
                        base.ProcessModel,
                        base.ActivityResource,
                        false,
                        session);
                }

                //执行流程运行事件
                OnProcessRunning(session);

                //执行节点流转过程
                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(runningExecutionContext, session);
                mediator.LinkContext.FromActivityInstance = RunningActivityInstance;
                mediator.ExecuteWorkItem();

                //执行流程完成事件
                OnProcessCompleted(session);
               
                //构造回调函数需要的数据
                result.Status = WfExecutedStatus.Success;
                result.Message = mediator.GetNodeMediatedMessage();
            }
            catch (WfRuntimeException rx)
            {
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = rx.Message;
                throw;
            }
            catch (System.Exception ex)
            {
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = ex.Message;
                throw;
            } 
        }

        /// <summary>
        /// 执行流程完成事件
        /// </summary>
        /// <param name="session">会话</param>
        private void OnProcessRunning(IDbSession session)
        {
            //调用流程结束事件
            DelegateExecutor.InvokeExternalDelegate(session,
                EventFireTypeEnum.OnProcessRunning,
                base.ActivityResource.AppRunner.DelegateEventList,
                RunningActivityInstance.ProcessInstanceID);
        }

        /// <summary>
        /// 执行流程完成事件
        /// </summary>
        /// <param name="session">会话</param>
        private void OnProcessCompleted(IDbSession session)
        {
            var pim = new ProcessInstanceManager();
            var entity = pim.GetById(session.Connection, RunningActivityInstance.ProcessInstanceID, session.Transaction);
            if (entity.ProcessState == (short)ProcessStateEnum.Completed)
            {
                //调用流程结束事件
                DelegateExecutor.InvokeExternalDelegate(session,
                    EventFireTypeEnum.OnProcessCompleted,
                    base.ActivityResource.AppRunner.DelegateEventList,
                    RunningActivityInstance.ProcessInstanceID);
            }
        }
    }
}
