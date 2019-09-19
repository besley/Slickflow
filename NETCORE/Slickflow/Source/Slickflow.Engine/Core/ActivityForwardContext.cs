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
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Core
{
    /// <summary>
    /// 活动节点执行上下文对象
    /// </summary>
    internal class ActivityForwardContext
    {
        #region ActivityForwardContext 属性列表

        internal IProcessModel ProcessModel { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        internal ActivityEntity Activity { get; set; }
        internal ActivityResource ActivityResource { get; set; }
        internal ActivityInstanceEntity FromActivityInstance { get; set; }
        internal Nullable<int> TaskID { get; set; }
        internal Boolean IsNotParsedByTransition { get; set; }

        #endregion

        #region ActivityForwardContext 构造函数
        /// <summary>
        /// 开始节点的构造执行上下文对象
        /// </summary>
        /// <param name="processModel">流程模型</param>
        /// <param name="processInstance">流程实例</param>
        /// <param name="activity">活动</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="isNotParsedByTransition">非解析流转</param>
        private ActivityForwardContext(IProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityEntity activity,
            ActivityResource activityResource,
            Boolean isNotParsedByTransition = false)
        {
            ProcessModel = processModel;
            ProcessInstance = processInstance;
            Activity = activity;
            ActivityResource = activityResource;
            IsNotParsedByTransition = isNotParsedByTransition;
        }

        /// <summary>
        /// 任务执行的上下文对象
        /// </summary>
        /// <param name="taskView"></param>
        /// <param name="taskID">任务</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="isNotParsedByTransition">非解析流转</param>
        /// <param name="session">数据会话</param>
        private ActivityForwardContext(TaskViewEntity taskView,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedByTransition,
            IDbSession session)
        {
            TaskID = taskView.TaskID;

            //check task condition has load activity instance
            FromActivityInstance = (new ActivityInstanceManager()).GetById(session.Connection, taskView.ActivityInstanceID, session.Transaction);
            ProcessInstance = (new ProcessInstanceManager()).GetById(session.Connection, taskView.ProcessInstanceID, session.Transaction);
            Activity = processModel.GetActivity(taskView.ActivityGUID);
            ProcessModel = processModel;
            ActivityResource = activityResource;
            IsNotParsedByTransition = isNotParsedByTransition;  
        }

        /// <summary>
        /// 启动流程的上下文对象
        /// </summary>
        /// <param name="processModel"></param>
        /// <param name="processInstance"></param>
        /// <param name="activity"></param>
        /// <param name="activityResource"></param>
        /// <returns></returns>
        internal static ActivityForwardContext CreateStartupContext(IProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityEntity activity,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(processModel, processInstance, activity, activityResource, false);
        }

        /// <summary>
        /// 创建任务执行上下文对象
        /// </summary>
        /// <param name="taskView">任务</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="isNotParsedForward">不需要解析的流转</param>
        /// <param name="session">数据会话</param>
        /// <returns>活动上下文</returns>
        internal static ActivityForwardContext CreateRunningContext(TaskViewEntity taskView,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedForward,
            IDbSession session)
        {
            return new ActivityForwardContext(taskView, processModel, activityResource, isNotParsedForward, session);
        }

        /// <summary>
        /// 创建流程跳转上下文对象
        /// </summary>
        /// <param name="jumpforwardActivity">跳转节点</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="processInstance">活动实例</param>
        /// <param name="activityResource">活动资源</param>
        /// <returns></returns>
        internal static ActivityForwardContext CreateJumpforwardContext(ActivityEntity jumpforwardActivity,
            IProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(processModel, processInstance, jumpforwardActivity, activityResource, true);
        }
        
        #endregion
    }
}
