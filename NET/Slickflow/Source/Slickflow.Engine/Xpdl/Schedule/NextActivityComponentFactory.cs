using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 下一步节点的工厂类
    /// </summary>
    public class NextActivityComponentFactory
    {
        /// <summary>
        /// 创建下一步活动的节点
        /// </summary>
        /// <param name="transition">转移</param>
        /// <param name="activity">活动</param>
        /// <returns>下一步节点封装</returns>
        internal static NextActivityComponent CreateNextActivityComponent(TransitionEntity transition,
            ActivityEntity activity)
        {
            string name = string.Empty;
            NextActivityComponent component = null;
            if (XPDLHelper.IsSimpleComponentNode(activity.ActivityType) == true)           //可流转简单类型节点
            {
                name = "单一节点";
                component = new NextActivityItem(name, transition, activity);
            }
            else if (XPDLHelper.IsIntermediateEventComponentNode(activity.ActivityType) == true)
            {
                name = "跨事件节点";
                component = new NextActivityIntermediate(name, transition, activity);
            }
            else if (XPDLHelper.IsGatewayComponentNode(activity.ActivityType) == true)
            {
                if (activity.GatewayDirectionType == GatewayDirectionEnum.AndSplit
                    || activity.GatewayDirectionType == GatewayDirectionEnum.AndJoin)
                {
                    name = "必全选节点";                 
                }
                else if (activity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI
                    || activity.GatewayDirectionType == GatewayDirectionEnum.AndJoinMI)
                {
                    name = "并行多实例节点";
                }
                else if (activity.GatewayDirectionType == GatewayDirectionEnum.OrSplit
                    || activity.GatewayDirectionType == GatewayDirectionEnum.OrJoin)
                {
                    name = "或多选节点";
                }
                else if (activity.GatewayDirectionType == GatewayDirectionEnum.XOrSplit
                    || activity.GatewayDirectionType == GatewayDirectionEnum.XOrJoin)
                {
                    name = "异或节点";
                }
                else if (activity.GatewayDirectionType == GatewayDirectionEnum.EOrJoin)
                {
                    name = "增强合并多选节点";
                }
                else
                {
                    throw new WfXpdlException(string.Format("无法创建下一步节点列表，不明确的分支类型：{0}", 
                        activity.GatewayDirectionType.ToString()));
                }
                component = new NextActivityGateway(name, transition, activity);
            }
            else if (activity.ActivityType == ActivityTypeEnum.SubProcessNode)
            {
                name = "子流程节点";
                component = new NextActivityItem(name, transition, activity);
            }
            else
            {
                throw new WfXpdlException(string.Format("无法创建下一步节点列表，不明确的节点类型：{0}",
                    activity.ActivityType.ToString()));
            }

            return component;
        }

        /// <summary>
        /// 创建跳转节点(强制拉取跳转方式，后续节点状态可以强制拉取前置节点到当前节点[后续节点])
        /// </summary>
        /// <param name="fromActivity">要拉取的节点</param>
        /// <param name="toActivity">拉取到节点</param>
        /// <returns>下一步节点封装</returns>
        internal static NextActivityComponent CreateNextActivityComponent(ActivityEntity fromActivity,
            ActivityEntity toActivity)
        {
            NextActivityComponent component = null;
            if (XPDLHelper.IsSimpleComponentNode(fromActivity.ActivityType) == true)       //可流转简单类型节点
            {
                string name = "单一节点";
                var transition = CreateJumpforwardEmptyTransition(fromActivity, toActivity);

                component = new NextActivityItem(name, transition, toActivity);     //强制拉取跳转类型的transition 为空类型
            }
            else
            {
                throw new ApplicationException(string.Format("不能跳转到其它非任务类型的节点！当前节点:{0}", 
                    fromActivity.ActivityType));
            }
            return component;
        }

        /// <summary>
        /// 创建跳转Transition实体对象
        /// </summary>
        /// <param name="fromActivity">来源节点</param>
        /// <param name="toActivity">目标节点</param>
        /// <returns>转移实体</returns>
        internal static TransitionEntity CreateJumpforwardEmptyTransition(ActivityEntity fromActivity, 
            ActivityEntity toActivity)
        {
            TransitionEntity transition = new TransitionEntity();
            transition.TransitionGUID = "JUMP-TRANSITION";
            transition.FromActivity = fromActivity;
            transition.FromActivityGUID = fromActivity.ActivityGUID;
            transition.ToActivity = toActivity;
            transition.ToActivityGUID = toActivity.ActivityGUID;
            transition.DirectionType = TransitionDirectionTypeEnum.Forward;

            return transition;
        }

        /// <summary>
        /// 创建下一步根显示节点
        /// </summary>
        /// <returns>根节点</returns>
        internal static NextActivityComponent CreateNextActivityComponent()
        {
            NextActivityComponent root = new NextActivityGateway("下一步步骤列表", null,  null);
            return root;
        }

        /// <summary>
        /// 创建上一步根显示节点
        /// </summary>
        /// <returns>根节点</returns>
        internal static NextActivityComponent CreatePreviousActivityComponent()
        {
            NextActivityComponent root = new NextActivityGateway("上一步步骤列表", null, null);
            return root;
        }

        /// <summary>
        /// 根据现有下一步节点列表，创建新的下一步节点列表对象
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static NextActivityComponent CreateNextActivityComponent(NextActivityComponent c)
        {
            NextActivityComponent newComp = CreateNextActivityComponent(c.Transition, c.Activity);
            return newComp;
        }
    }
}
