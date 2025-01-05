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
    /// Runtimer Manager Run
    /// 应用执行运行时
    /// </summary>
    internal class WfRuntimeManagerRun : WfRuntimeManager
    {
        /// <summary>
        /// Run execute method
        /// 运行执行方法
        /// </summary>
        /// <param name="session"></param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            var result = base.WfExecutedResult;
            result.ProcessInstanceIDStarted = RunningActivityInstance.ProcessInstanceID;

            try
            {
                ActivityForwardContext runningExecutionContext = null;
                if (base.TaskView != null)
                {
                    //有TaskID的任务类型执行上下文
                    //Task type execution context with TaskID
                    runningExecutionContext = ActivityForwardContext.CreateRunningContextByTask(base.TaskView,
                        base.ProcessModel,
                        base.ActivityResource,
                        false,
                        session);
                }
                else
                {
                    //Interrupt事件类型的活动执行上下文
                    //Interrupt event type activity execution context
                    runningExecutionContext = ActivityForwardContext.CreateRunningContextByActivity(base.RunningActivityInstance,
                        base.ProcessModel,
                        base.ActivityResource,
                        false,
                        session);
                }

                OnProcessRunning(session);

                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(runningExecutionContext, session);
                mediator.LinkContext.FromActivityInstance = RunningActivityInstance;
                mediator.ExecuteWorkItem();

                OnProcessCompleted(session);
               
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
        /// Process Running event
        /// 执行流程时的绑定事件
        /// </summary>
        /// <param name="session"></param>
        private void OnProcessRunning(IDbSession session)
        {
            DelegateExecutor.InvokeExternalDelegate(session,
                EventFireTypeEnum.OnProcessRunning,
                base.ActivityResource.AppRunner.DelegateEventList,
                RunningActivityInstance.ProcessInstanceID);
        }

        /// <summary>
        /// Process Complete event
        /// 执行流程完成事件
        /// </summary>
        /// <param name="session"></param>
        private void OnProcessCompleted(IDbSession session)
        {
            var pim = new ProcessInstanceManager();
            var entity = pim.GetById(session.Connection, RunningActivityInstance.ProcessInstanceID, session.Transaction);
            if (entity.ProcessState == (short)ProcessStateEnum.Completed)
            {
                DelegateExecutor.InvokeExternalDelegate(session,
                    EventFireTypeEnum.OnProcessCompleted,
                    base.ActivityResource.AppRunner.DelegateEventList,
                    RunningActivityInstance.ProcessInstanceID);
            }
        }
    }
}
