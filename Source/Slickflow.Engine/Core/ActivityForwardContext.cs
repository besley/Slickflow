using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Core
{
    /// <summary>
    /// 活动节点执行上下文对象
    /// </summary>
    internal class ActivityForwardContext
    {
        #region ActivityForwardContext 属性列表

        internal ProcessModel ProcessModel { get; set; }
        internal ProcessInstanceEntity ProcessInstance { get; set; }
        internal ActivityEntity Activity { get; set; }
        internal ActivityResource ActivityResource { get; set; }
        internal ActivityInstanceEntity FromActivityInstance { get; set; }
        internal TaskViewEntity TaskView { get; set; }

        #endregion

        #region ActivityForwardContext 构造函数
        /// <summary>
        /// 开始节点的构造执行上下文对象
        /// </summary>
        /// <param name="processModel"></param>
        /// <param name="processInstance"></param>
        /// <param name="activity"></param>
        /// <param name="activityResource"></param>
        private ActivityForwardContext(ProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityEntity activity,
            ActivityResource activityResource)
        {
            ProcessModel = processModel;
            ProcessInstance = processInstance;
            Activity = activity;
            ActivityResource = activityResource;
        }

        /// <summary>
        /// 任务执行的上下文对象
        /// </summary>
        /// <param name="task"></param>
        /// <param name="processModel"></param>
        /// <param name="activityResource"></param>
        private ActivityForwardContext(TaskViewEntity task,
            ProcessModel processModel,
            ActivityResource activityResource)
        {
            this.TaskView = task;

            //check task condition has load activity instance
            this.FromActivityInstance = (new ActivityInstanceManager()).GetById(task.ActivityInstanceID);
            this.ProcessInstance = (new ProcessInstanceManager()).GetById(task.ProcessInstanceID);
            this.Activity = processModel.GetActivity(task.ActivityGUID);
            this.ProcessModel = processModel;
            this.ActivityResource = activityResource;
        }

        /// <summary>
        /// 启动流程的上下文对象
        /// </summary>
        /// <param name="processModel"></param>
        /// <param name="processInstance"></param>
        /// <param name="activity"></param>
        /// <param name="activityResource"></param>
        /// <returns></returns>
        internal static ActivityForwardContext CreateStartupContext(ProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityEntity activity,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(processModel, processInstance, activity, activityResource);
        }

        /// <summary>
        /// 创建任务执行上下文对象
        /// </summary>
        /// <param name="task"></param>
        /// <param name="processModel"></param>
        /// <param name="activityResource"></param>
        /// <returns></returns>
        internal static ActivityForwardContext CreateRunningContext(TaskViewEntity task,
            ProcessModel processModel,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(task, processModel, activityResource);
        }

        /// <summary>
        /// 创建流程跳转上下文对象
        /// </summary>
        /// <param name="jumpforwardActivity"></param>
        /// <param name="processModel"></param>
        /// <param name="processInstance"></param>
        /// <param name="activityResource"></param>
        /// <returns></returns>
        internal static ActivityForwardContext CreateJumpforwardContext(ActivityEntity jumpforwardActivity,
            ProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(processModel, processInstance, jumpforwardActivity, activityResource);
        }
        
        #endregion
    }
}
