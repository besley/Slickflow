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
    /// 活动节点执行上下文对象
    /// </summary>
    internal class ActivityForwardContext
    {
        #region ActivityForwardContext 属性列表

        internal IProcessModel ProcessModel { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        internal Activity Activity { get; set; }
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
            Activity activity,
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
        /// Interrupt事件类型活动实例执行上下文对象
        /// </summary>
        /// <param name="activityInstance">活动实例</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="isNotParsedByTransition">非解析流转</param>
        /// <param name="session">数据会话</param>
        private ActivityForwardContext(ActivityInstanceEntity activityInstance,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedByTransition,
            IDbSession session)
        {
            //check task condition has load activity instance
            FromActivityInstance = (new ActivityInstanceManager()).GetById(session.Connection, activityInstance.ID, session.Transaction);
            ProcessInstance = (new ProcessInstanceManager()).GetById(session.Connection, activityInstance.ProcessInstanceID, session.Transaction);
            Activity = processModel.GetActivity(activityInstance.ActivityGUID);
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
            Activity activity,
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
        internal static ActivityForwardContext CreateRunningContextByTask(TaskViewEntity taskView,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedForward,
            IDbSession session)
        {
            return new ActivityForwardContext(taskView, processModel, activityResource, isNotParsedForward, session);
        }

        /// <summary>
        /// Interrupt事件类型创建活动执行上下文对象
        /// </summary>
        /// <param name="activityInstance">活动实例</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="isNotParsedForward">不需要解析的流转</param>
        /// <param name="session">数据会话</param>
        /// <returns>活动上下文</returns>
        internal static ActivityForwardContext CreateRunningContextByActivity(ActivityInstanceEntity activityInstance,
            IProcessModel processModel,
            ActivityResource activityResource,
            Boolean isNotParsedForward,
            IDbSession session)
        {
            return new ActivityForwardContext(activityInstance, processModel, activityResource, isNotParsedForward, session);
        }

        /// <summary>
        /// 创建流程跳转上下文对象
        /// </summary>
        /// <param name="jumpforwardActivity">跳转节点</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="processInstance">活动实例</param>
        /// <param name="activityResource">活动资源</param>
        /// <returns></returns>
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
