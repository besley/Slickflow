/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private ActivityForwardContext(IProcessModel processModel,
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
            IProcessModel processModel,
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
        internal static ActivityForwardContext CreateStartupContext(IProcessModel processModel,
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
            IProcessModel processModel,
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
            IProcessModel processModel,
            ProcessInstanceEntity processInstance,
            ActivityResource activityResource)
        {
            return new ActivityForwardContext(processModel, processInstance, jumpforwardActivity, activityResource);
        }
        
        #endregion
    }
}
