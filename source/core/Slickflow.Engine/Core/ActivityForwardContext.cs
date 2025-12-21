using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core
{
    /// <summary>
    /// Activity node execution context object
    /// 活动节点执行上下文对象
    /// </summary>
    internal class ActivityForwardContext
    {
        #region ActivityForwardContext Property

        internal IProcessModel ProcessModel { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        internal Activity CurrentActivity { get; set; }
        internal ActivityResource ActivityResource { get; set; }
        internal ActivityInstanceEntity FromActivityInstance { get; set; }
        internal Nullable<int> TaskId { get; set; }
        internal Boolean IsNotParsedByTransition { get; set; }

        #endregion

        #region ActivityForwardContext Constructor
        /// <summary>
        /// Context object on the starting node
        /// 开始节点上的上下文对象
        /// </summary>
        private ActivityForwardContext(IProcessModel processModel,
            ProcessInstanceEntity processInstance,
            Activity currentActivity,
            ActivityResource activityResource,
            Boolean isNotParsedByTransition = false)
        {
            ProcessModel = processModel;
            ProcessInstance = processInstance;
            CurrentActivity = currentActivity;
            ActivityResource = activityResource;
            IsNotParsedByTransition = isNotParsedByTransition;
        }

        /// <summary>
        /// Context object for task node execution
        /// 任务节点执行的上下文对象
        /// </summary>
        private ActivityForwardContext(TaskViewEntity taskView,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedByTransition,
            IDbSession session)
        {
            TaskId = taskView.Id;

            //check task condition has load activity instance
            FromActivityInstance = (new ActivityInstanceManager()).GetById(session.Connection, taskView.ActivityInstanceId, session.Transaction);
            ProcessInstance = (new ProcessInstanceManager()).GetById(session.Connection, taskView.ProcessInstanceId, session.Transaction);
            CurrentActivity = processModel.GetActivity(taskView.ActivityId);
            ProcessModel = processModel;
            ActivityResource = activityResource;
            IsNotParsedByTransition = isNotParsedByTransition;  
        }

        /// <summary>
        /// Interrupt event type activity instance execution context object
        /// Interrupt事件类型活动实例执行上下文对象
        /// </summary>
        private ActivityForwardContext(ActivityInstanceEntity activityInstance,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedByTransition,
            IDbSession session)
        {
            //check task condition has load activity instance
            FromActivityInstance = (new ActivityInstanceManager()).GetById(session.Connection, activityInstance.Id, session.Transaction);
            ProcessInstance = (new ProcessInstanceManager()).GetById(session.Connection, activityInstance.ProcessInstanceId, session.Transaction);
            CurrentActivity = processModel.GetActivity(activityInstance.ActivityId);
            ProcessModel = processModel;
            ActivityResource = activityResource;
            IsNotParsedByTransition = isNotParsedByTransition;
        }

        /// <summary>
        /// Context object for initiating the process
        /// 启动流程的上下文对象
        /// </summary>
        internal static ActivityForwardContext CreateStartupContext(IProcessModel processModel,
            ProcessInstanceEntity processInstance,
            Activity currentActivity,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(processModel, processInstance, currentActivity, activityResource, false);
        }

        /// <summary>
        /// Create task execution context object
        /// 创建任务执行上下文对象
        /// </summary>
        internal static ActivityForwardContext CreateRunningContextByTask(TaskViewEntity taskView,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedForward,
            IDbSession session)
        {
            return new ActivityForwardContext(taskView, processModel, activityResource, isNotParsedForward, session);
        }

        /// <summary>
        /// Interrupt event type creates activity execution context object
        /// Interrupt事件类型创建活动执行上下文对象
        /// </summary>
        internal static ActivityForwardContext CreateRunningContextByActivity(ActivityInstanceEntity activityInstance,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedForward,
            IDbSession session)
        {
            return new ActivityForwardContext(activityInstance, processModel, activityResource, isNotParsedForward, session);
        }

        /// <summary>
        /// Create process jump context object
        /// 创建流程跳转上下文对象
        /// </summary>
        internal static ActivityForwardContext CreateJumpforwardContext(Activity jumpforwardActivity,
            IProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(processModel, processInstance, jumpforwardActivity, activityResource, true);
        }
        #endregion
    }
}
