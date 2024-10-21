using Slickflow.Data;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 加签执行运行时
    /// </summary>
    internal class WfRuntimeManagerSignForward : WfRuntimeManager
    {
        /// <summary>
        /// 加签执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            //根据加签类型选项，生成新的ActivityInstance记录
            //加签类型有前加签，后加签和并加签
            try
            {
                var signforwardExecutionContext = ActivityForwardContext.CreateRunningContextByTask(base.TaskView,
                    base.ProcessModel,
                    base.ActivityResource,
                    false,
                    session);

                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(signforwardExecutionContext, session);
                mediator.ExecuteWorkItem();

                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Success;
                result.Message = mediator.GetNodeMediatedMessage();
            }
            catch (WfRuntimeException rx)
            {
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.SignForward_RuntimeError;
                result.Message = rx.Message;
                throw;
            }
            catch (System.Exception ex)
            {
                var result = base.WfExecutedResult;
                result.Status = WfExecutedStatus.Failed;
                result.ExceptionType = WfExceptionType.SignForward_RuntimeError;
                result.Message = ex.Message;
                throw;
            }
        }
    }
}
