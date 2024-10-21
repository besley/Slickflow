
using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 返送回的处理
    /// </summary>
    internal class WfRuntimeManagerResend : WfRuntimeManager
    {
        /// <summary>
        /// 退回操作的处理逻辑
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            try
            {
                var runningExecutionContext = ActivityForwardContext.CreateRunningContextByTask(base.TaskView,
                    base.ProcessModel,
                    base.ActivityResource,
                    true,
                    session);

                //执行节点
                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(runningExecutionContext, session);
                mediator.LinkContext.FromActivityInstance = RunningActivityInstance;
                var toActivityGUID = runningExecutionContext.ActivityResource.NextActivityPerformers.First().Key;
                mediator.LinkContext.ToActivity = ProcessModel.GetActivity(toActivityGUID);
                mediator.ExecuteWorkItem();

                //构造回调函数需要的数据
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Success;
                result.Message = mediator.GetNodeMediatedMessage();
            }
            catch (WfRuntimeException rx)
            {
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = rx.Message;
                throw;
            }
            catch (System.Exception ex)
            {
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.RunApp_RuntimeError;
                result.Message = ex.Message;
                throw;
            }
        }
    }
}
