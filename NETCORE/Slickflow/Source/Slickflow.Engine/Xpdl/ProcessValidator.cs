using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程校验器
    /// </summary>
    internal class ProcessValidator
    {
        /// <summary>
        /// 校验方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>校验结果</returns>
        internal static ProcessValidateResult Validate(ProcessValidateEntity entity)
        {
            var startActivityGUID = entity.StartActivityGUID;
            var activityList = entity.ActivityList;
            var transitionList = entity.TransitionList;
            var result = new ProcessValidateResult();
            activityList = activityList.Where(a => a.ActivityGUID != startActivityGUID).ToList();
            result.ActivityList = TranverseTransitonList(activityList, transitionList, startActivityGUID);

            return result;
        }

        /// <summary>
        /// 迭代验证方法
        /// </summary>
        /// <param name="activityList">活动列表</param>
        /// <param name="transitionList">转移列表</param>
        /// <param name="activityGUID">活动GUID</param>
        /// <returns>孤立活动列表</returns>
        internal static IList<ActivityEntity> TranverseTransitonList(IList<ActivityEntity> activityList,
            IList<TransitionEntity> transitionList,
            string activityGUID)
        {
            var list = transitionList.Where(t => t.FromActivityGUID == activityGUID).ToList();
            foreach (var t in list)
            {
                var toActivityGUID = t.ToActivityGUID;
                activityList = activityList.Where(a => a.ActivityGUID != toActivityGUID).ToList();
                activityList = TranverseTransitonList(activityList, transitionList, toActivityGUID);
            }
            return activityList;
        }
    }
}
