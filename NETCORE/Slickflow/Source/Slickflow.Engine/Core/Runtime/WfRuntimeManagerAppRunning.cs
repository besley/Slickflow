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
using Slickflow.Engine.Core.Pattern;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Delegate;


namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 应用执行运行时
    /// </summary>
    internal class WfRuntimeManagerAppRunning : WfRuntimeManager
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
                var runningExecutionContext = ActivityForwardContext.CreateRunningContext(base.TaskView,
                    base.ProcessModel,
                    base.ActivityResource,
                    false,
                    session);

                //----> 节点开始流转，调用活动开始执行的委托事件
                DelegateExecutor.InvokeExternalDelegate(session,
                    EventFireTypeEnum.OnActivityExecuting,
                    runningExecutionContext);

                //执行节点流转过程
                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(runningExecutionContext, session);
                mediator.Linker.FromActivityInstance = RunningActivityInstance;
                mediator.ExecuteWorkItem();

                //----> 节点流转完成后，调用活动完成执行的委托事件
                DelegateExecutor.InvokeExternalDelegate(session,
                    EventFireTypeEnum.OnActivityExecuted,
                    runningExecutionContext);

                //构造回调函数需要的数据
                result.Status = WfExecutedStatus.Success;
                result.Message = mediator.GetNodeMediatedMessage();
            }
            catch (WfRuntimeException rx)
            {
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = rx.Message;
                throw rx;
            }
            catch (System.Exception ex)
            {
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = ex.Message;
                throw ex;
            } 
        }
    }
}
