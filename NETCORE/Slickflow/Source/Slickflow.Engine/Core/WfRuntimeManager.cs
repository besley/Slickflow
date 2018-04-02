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
using System.Threading;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core
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
        
        //流程返签或退回时的属性
        internal BackwardContext BackwardContext { get; set; }
        internal Boolean IsBackward { get; set; }

        /// <summary>
        /// 流程执行结果对象
        /// </summary>
        internal WfExecutedResult WfExecutedResult { get; set; }

        /// <summary>
        /// 获取退回时最早节点实例ID，支持连续退回
        /// </summary>
        /// <returns></returns>
        protected int GetBackwardMostPreviouslyActivityInstanceID()
        {
            //获取退回节点实例ID
            int backMostPreviouslyActivityInstanceID;
            if (BackwardContext.BackwardToTaskActivityInstance.MIHostActivityInstanceID != null)
            {
                //多实例子节点模式，不考虑连续模式
                backMostPreviouslyActivityInstanceID = BackwardContext.BackwardToTaskActivityInstance.ID;
            }
            else
            {
                if (BackwardContext.BackwardToTaskActivityInstance.BackSrcActivityInstanceID != null)
                    backMostPreviouslyActivityInstanceID = BackwardContext.BackwardToTaskActivityInstance.BackSrcActivityInstanceID.Value;
                else
                    backMostPreviouslyActivityInstanceID = BackwardContext.BackwardToTaskActivityInstance.ID;
            }

            return backMostPreviouslyActivityInstanceID;
        }
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
        /// <returns></returns>
        internal bool Execute(IDbSession session)
        {
            try
            {
                ExecuteInstanceImp(session);
            }
            catch (WfRuntimeException rx)
            {
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, AppRunner, rx);
                throw;
            }
            catch (System.Exception e)
            {
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
        /// </summary>
        /// <param name="runtimeType"></param>
        /// <param name="result"></param>
        internal void Callback(WfExecutedResult result)
        {
            WfEventArgs args = new WfEventArgs(result);
            if (_onWfProcessExecuted != null)
            {
                _onWfProcessExecuted(this, args);
            }
        }
        #endregion

        #region 流程事件定义
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
        #endregion
    }
}

            