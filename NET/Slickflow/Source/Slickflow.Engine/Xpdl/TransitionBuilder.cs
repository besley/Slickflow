using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 转移构造类
    /// </summary>
    public class TransitionBuilder
    {
        #region 静态方法
        /// <summary>
        /// 创建跳转Transition实体对象
        /// </summary>
        /// <param name="fromActivity">来源节点</param>
        /// <param name="toActivity">目标节点</param>
        /// <returns>转移实体</returns>
        public static TransitionEntity CreateJumpforwardEmptyTransition(ActivityEntity fromActivity, ActivityEntity toActivity)
        {
            TransitionEntity transition = new TransitionEntity();
            transition.TransitionGUID = string.Empty;
            transition.FromActivity = fromActivity;
            transition.FromActivityGUID = fromActivity.ActivityGUID;
            transition.ToActivity = toActivity;
            transition.ToActivityGUID = toActivity.ActivityGUID;
            transition.DirectionType = TransitionDirectionTypeEnum.Forward;

            return transition;
        }
        #endregion
    }
}
