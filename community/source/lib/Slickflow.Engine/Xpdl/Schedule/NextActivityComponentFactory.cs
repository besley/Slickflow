using System;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;

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
        internal static NextActivityComponent CreateNextActivityComponent(Transition transition,
            Activity activity)
        {
            string name = string.Empty;
            NextActivityComponent component = null;
            if (XPDLHelper.IsSimpleComponentNode(activity.ActivityType) == true)           //可流转简单类型节点
            {
                //单一节点
                name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.singlenode");
                component = new NextActivityItem(name, transition, activity);
            }
            else if (XPDLHelper.IsCrossOverComponentNode(activity.ActivityType) == true)
            {
                //跨事件节点，包括服务节点
                name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.crossovereventnode");
                component = new NextActivityIntermediate(name, transition, activity);
            }
            else if (XPDLHelper.IsGatewayComponentNode(activity.ActivityType) == true)
            {
                if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplit
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoin)
                {
                    //必全选节点
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.mandatorycheckall");                 
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoinMI)
                {
                    //并行多实例节点
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.parallelmultipleinstance");
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrSplit
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrJoin
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.ApprovalOrSplit)
                {
                    //或多选节点
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.orsplitorjoin");
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrSplit
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrJoin)
                {
                    //异或节点
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.xor");
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.EOrJoin)
                {
                    //增强合并多选节点
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.eorjoin");
                }
                else
                {
                    throw new WfXpdlException(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.CreateNextActivityComponent.gateway.error", 
                        activity.GatewayDetail.DirectionType.ToString()));
                }
                component = new NextActivityRouter(name, transition, activity);
            }
            else if (activity.ActivityType == ActivityTypeEnum.SubProcessNode)
            {
                //子流程节点
                name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.subprocess");
                component = new NextActivityItem(name, transition, activity);
            }
            else
            {
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.CreateNextActivityComponent.error",
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
        internal static NextActivityComponent CreateNextActivityComponent(Activity fromActivity,
            Activity toActivity)
        {
            NextActivityComponent component = null;
            if (XPDLHelper.IsSimpleComponentNode(fromActivity.ActivityType) == true)       //可流转简单类型节点
            {
                //单一节点
                string name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.singlenode"); ;
                var transition = CreateJumpforwardEmptyTransition(fromActivity, toActivity);

                component = new NextActivityItem(name, transition, toActivity);     //强制拉取跳转类型的transition 为空类型
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.CreateNextActivityComponent.jump.error", 
                    fromActivity.ActivityType.ToString()));
            }
            return component;
        }

        /// <summary>
        /// 创建跳转Transition实体对象
        /// </summary>
        /// <param name="fromActivity">来源节点</param>
        /// <param name="toActivity">目标节点</param>
        /// <returns>转移实体</returns>
        internal static Transition CreateJumpforwardEmptyTransition(Activity fromActivity, 
            Activity toActivity)
        {
            Transition transition = new Transition();
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
            //下一步步骤列表
            NextActivityComponent root = new NextActivityRouter(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.nextsteplist"), 
                null,  null);
            return root;
        }

        /// <summary>
        /// 创建上一步根显示节点
        /// </summary>
        /// <returns>根节点</returns>
        internal static NextActivityComponent CreatePreviousActivityComponent()
        {
            //上一步步骤列表
            NextActivityComponent root = new NextActivityRouter(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.previoussteplist"),
                null, null);
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
