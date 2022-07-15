using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程模型帮助类
    /// </summary>
    public class ProcessModelHelper
    {
        /// <summary>
        /// 获取开始节点
        /// </summary>
        /// <param name="process">流程</param>
        /// <returns>开始节点</returns>
        public static Activity GetStartActivity(Process process)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityType == ActivityTypeEnum.StartNode);
            return activity;
        }

        /// <summary>
        /// 获取开始节点
        /// </summary>
        /// <param name="process">流程</param>
        /// <returns>开始节点</returns>
        public static Activity GetEndActivity(Process process)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityType == ActivityTypeEnum.EndNode);
            return activity;
        }

        /// <summary>
        /// 获取活动节点
        /// </summary>
        /// <param name="process">流程</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>活动节点</returns>
        public static Activity GetActivity(Process process, string activityGUID)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityGUID == activityGUID);
            return activity;
        }

        /// <summary>
        /// 获取第一个办理节点
        /// </summary>
        /// <param name="process">流程</param>
        /// <returns>开始节点</returns>
        public static Activity GetFirstActivity(Process process)
        {
            var activityList = process.ActivityList.ToList();
            var activity = activityList.SingleOrDefault<Activity>(a => a.ActivityType == ActivityTypeEnum.StartNode);

            var transitionList = process.TransitionList.ToList();
            var transition = transitionList.SingleOrDefault<Transition>(t=>t.FromActivityGUID == activity.ActivityGUID);
            var firstActivity = transition.ToActivity;

            return firstActivity;
        }

        /// <summary>
        /// 获取第一个办理节点
        /// </summary>
        /// <param name="process">流程</param>
        /// <param name="fromActivityGUID">起始活动节点</param>
        /// <returns>下一步节点</returns>
        public static Activity GetNextActivity(Process process, string fromActivityGUID)
        {
            var transitionList = process.TransitionList.ToList();
            var transition = transitionList.SingleOrDefault<Transition>(t => t.FromActivityGUID == fromActivityGUID);
            var nextActivity = transition.ToActivity;

            return nextActivity;
        }

        /// <summary>
        /// 获取连线列表
        /// </summary>
        /// <param name="process">流程</param>
        /// <param name="fromActivityGUID">活动GUID</param>
        /// <returns>连线列表</returns>
        public static IList<Transition> GetForwardTransitionList(Process process, string fromActivityGUID)
        {
            var transitionList = process.TransitionList.Where<Transition>(t=> t.FromActivityGUID == fromActivityGUID).ToList();
            return transitionList;
        }

        /// <summary>
        /// 获取连线实体
        /// </summary>
        /// <param name="process">流程</param>
        /// <param name="fromActivityGUID">起始节点GUID</param>
        /// <param name="toActivityGUID">到达节点GUID</param>
        /// <returns></returns>
        public static Transition GetForwardTransition(Process process, string fromActivityGUID, string toActivityGUID)
        {
            var transiton = process.TransitionList.SingleOrDefault<Transition>(t => t.FromActivityGUID == fromActivityGUID
                && t.ToActivityGUID == toActivityGUID);
            return transiton;
        }

        /// <summary>
        /// 获取连线列表
        /// </summary>
        /// <param name="process">流程</param>
        /// <param name="toActivityGUID">活动GUID</param>
        /// <returns>连线列表</returns>
        public static IList<Transition> GetBackwardTransitionList(Process process, string toActivityGUID)
        {
            var transitionList = process.TransitionList.Where<Transition>(t => t.ToActivityGUID == toActivityGUID).ToList();
            return transitionList;
        }

        #region 活动视图转换
        /// <summary>
        /// 从活动节点转换为活动视图
        /// </summary>
        /// <param name="entity">活动实体</param>
        /// <returns>活动视图</returns>
        public static ActivityView ConvertFromActivityEntity(Activity entity)
        {
            var view = new ActivityView();
            view.ActivityGUID = entity.ActivityGUID;
            view.ActivityName = entity.ActivityName;
            view.ActivityCode = entity.ActivityCode;
            view.ActivityType = entity.ActivityType.ToString();
            if (entity.TriggerDetail != null)
            {
                view.TriggerType = entity.TriggerDetail.TriggerType.ToString();
                view.MessageDirection = entity.TriggerDetail.MessageDirection.ToString();
                view.Expression = entity.TriggerDetail.Expression;
            }
            return view;
        }
        #endregion
    }
}
