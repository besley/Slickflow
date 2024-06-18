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
    /// 流程运行时管理
    /// </summary>
    internal abstract class WfRuntimeManager
    {
        #region 抽象方法
        internal abstract void ExecuteInstanceImp(IDbSession session);
        #endregion

        #region 流转属性和基础方法
        internal WfAppRunner AppRunner { get; set; }
        internal IProcessModel ProcessModel { get; set; }
        internal ProcessInstanceEntity ParentProcessInstance { get; set; }
        internal NodeBase InvokedSubProcessNode { get; set; }
        internal ActivityResource ActivityResource { get; set; }
        internal TaskViewEntity TaskView { get; set; }
        internal ActivityInstanceEntity RunningActivityInstance { get; set; }
        internal int ProcessInstanceID { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        
        //流程返签或退回时的属性
        internal BackwardContext BackwardContext { get; set; }
        internal Boolean IsBackward { get; set; }
        internal SendBackOperation SendBackOperation { get; set; }

        /// <summary>
        /// 流程执行结果对象
        /// </summary>
        internal WfExecutedResult WfExecutedResult { get; set; }
        #endregion

        #region 构造方法
        internal WfRuntimeManager()
        {
            AppRunner = new WfAppRunner();
            BackwardContext = new BackwardContext();
        }
        #endregion

        #region 执行方法
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <returns>执行状态</returns>
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
        /// 事件回调
        /// <param name="result">执行结果</param>
        internal void Callback(WfExecutedResult result)
        {
            WfEventArgs args = new WfEventArgs(result);
            _onWfProcessExecuted.Invoke(this, args);
        }
        #endregion

        #region 流程事件定义及绑定
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
        /// 绑定事件
        /// </summary>
        /// <param name="executing">执行事件</param>
        /// <param name="executed">执行完成事件</param>
        internal WfRuntimeManager RegisterEvent(EventHandler<WfEventArgs> executing, EventHandler<WfEventArgs> executed)
        {
            if (executing != null)
                this.OnWfProcessExecuting += executing;

            if (executed != null)
                this._onWfProcessExecuted += executed;

            return this;
        }

        /// <summary>
        /// 解除绑定事件
        /// </summary>
        /// <param name="executing">执行事件</param>
        /// <param name="executed">执行完成事件</param>
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

            