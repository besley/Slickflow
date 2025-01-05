using System;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.SendBack;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// Workflow Runtime Manager
    /// 流程运行时管理
    /// </summary>
    internal abstract class WfRuntimeManager
    {
        #region Abstract Method
        internal abstract void ExecuteInstanceImp(IDbSession session);
        #endregion

        #region Property and Basic Method
        internal WfAppRunner AppRunner { get; set; }
        internal IProcessModel ProcessModel { get; set; }
        internal ProcessInstanceEntity ParentProcessInstance { get; set; }
        internal NodeBase InvokedSubProcessNode { get; set; }
        internal ActivityResource ActivityResource { get; set; }
        internal TaskViewEntity TaskView { get; set; }
        internal ActivityInstanceEntity RunningActivityInstance { get; set; }
        internal int ProcessInstanceID { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        
        internal BackwardContext BackwardContext { get; set; }
        internal Boolean IsBackward { get; set; }
        internal SendBackOperation SendBackOperation { get; set; }

        internal WfExecutedResult WfExecutedResult { get; set; }

        internal WfRuntimeManager()
        {
            AppRunner = new WfAppRunner();
            BackwardContext = new BackwardContext();
        }
        #endregion

        #region Execute Method
        /// <summary>
        /// Execute Method
        /// 执行方法
        /// </summary>
        /// <returns></returns>
        internal bool Execute(IDbSession session)
        {
            try
            {
                ExecuteInstanceImp(session);
            }
            catch (WfRuntimeException rx)
            {
                WfExecutedResult.Status = WfExecutedStatus.Exception;
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, AppRunner, rx);
                throw;
            }
            catch (System.Exception e)
            {
                WfExecutedResult.Status = WfExecutedStatus.Failed;
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, AppRunner, e);
                throw;
            }
            finally
            {
                Callback(WfExecutedResult);
            }

            return true;
        }

        /// <summary>
        /// Callback
        /// 事件回调
        /// <param name="result"></param>
        internal void Callback(WfExecutedResult result)
        {
            WfEventArgs args = new WfEventArgs(result);
            _onWfProcessExecuted.Invoke(this, args);
        }
        #endregion

        #region Register Event
        private event EventHandler<WfEventArgs> _onWfProcessExecuting;
        internal event EventHandler<WfEventArgs> OnWfProcessExecuting
        {
            add
            {
                _onWfProcessExecuting += value;
            }
            remove
            {
                _onWfProcessExecuting -= value;
            }
        }

        private event EventHandler<WfEventArgs> _onWfProcessExecuted;
        internal event EventHandler<WfEventArgs> OnWfProcessExecuted
        {
            add
            {
                _onWfProcessExecuted += value;
            }
            remove
            {
                _onWfProcessExecuted -= value;
            }
        }

        /// <summary>
        /// Register Event
        /// 注册事件
        /// </summary>
        /// <param name="executing"></param>
        /// <param name="executed"></param>
        internal WfRuntimeManager RegisterEvent(EventHandler<WfEventArgs> executing, EventHandler<WfEventArgs> executed)
        {
            if (executing != null)
                this.OnWfProcessExecuting += executing;

            if (executed != null)
                this._onWfProcessExecuted += executed;

            return this;
        }

        /// <summary>
        /// Unregister Event
        /// 解除注册事件
        /// </summary>
        /// <param name="executing"></param>
        /// <param name="executed"></param>
        internal WfRuntimeManager UnRegiesterEvent(EventHandler<WfEventArgs> executing, EventHandler<WfEventArgs> executed)
        {
            if (executing != null)
                this.OnWfProcessExecuting -= executing;

            if (executed != null)
                this._onWfProcessExecuted -= executed;

            return this;
        }
        #endregion
    }
}

            