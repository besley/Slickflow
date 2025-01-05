using System;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next Activity Component Factory
    /// 下一步节点的工厂类
    /// </summary>
    public class NextActivityComponentFactory
    {
        /// <summary>
        /// Create Next Activity Component
        /// 创建下一步活动的节点
        /// </summary>
        internal static NextActivityComponent CreateNextActivityComponent(Transition transition,
            Activity activity)
        {
            string name = string.Empty;
            NextActivityComponent component = null;
            if (XPDLHelper.IsSimpleComponentNode(activity.ActivityType) == true)           
            {
                //单一节点
                //Simple node
                name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.singlenode");
                component = new NextActivityItem(name, transition, activity);
            }
            else if (XPDLHelper.IsCrossOverComponentNode(activity.ActivityType) == true)
            {
                //跨事件节点，包括服务节点
                //CrossOver node
                name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.crossovereventnode");
                component = new NextActivityIntermediate(name, transition, activity);
            }
            else if (XPDLHelper.IsGatewayComponentNode(activity.ActivityType) == true)
            {
                if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplit
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoin)
                {
                    //必全选节点
                    //All nodes must be selected
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.mandatorycheckall");                 
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndJoinMI)
                {
                    //并行多实例节点
                    //AndSplitMI
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.parallelmultipleinstance");
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrSplit
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrJoin
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.ApprovalOrSplit)
                {
                    //或多选节点
                    //Or select multiple nodes
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.orsplitorjoin");
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrSplit
                    || activity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrJoin)
                {
                    //异或节点
                    //XOr Node
                    name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.xor");
                }
                else if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.EOrJoin)
                {
                    //增强合并多选节点
                    //EOrJoin Node
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
                //Sub Process Node
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
        /// Create jump nodes (forced pull jump method, 
        /// subsequent node status can be forcibly pulled from the previous node to the current node [subsequent node])
        /// 创建跳转节点(强制拉取跳转方式，后续节点状态可以强制拉取前置节点到当前节点[后续节点])
        /// </summary>
        /// <param name="fromActivity"></param>
        /// <param name="toActivity"></param>
        /// <returns></returns>
        internal static NextActivityComponent CreateNextActivityComponent(Activity fromActivity,
            Activity toActivity)
        {
            NextActivityComponent component = null;
            if (XPDLHelper.IsSimpleComponentNode(fromActivity.ActivityType) == true)       
            {
                //单一节点
                //Simple node
                string name = LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.singlenode"); ;
                var transition = CreateJumpforwardEmptyTransition(fromActivity, toActivity);

                component = new NextActivityItem(name, transition, toActivity);    
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.CreateNextActivityComponent.jump.error", 
                    fromActivity.ActivityType.ToString()));
            }
            return component;
        }

        /// <summary>
        /// Create jump forward empty transition
        /// 创建跳转Transition实体对象
        /// </summary>
        /// <param name="fromActivity"></param>
        /// <param name="toActivity"></param>
        /// <returns></returns>
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
        /// Create next activity component
        /// 创建下一步根显示节点
        /// </summary>
        /// <returns>根节点</returns>
        internal static NextActivityComponent CreateNextActivityComponent()
        {
            NextActivityComponent root = new NextActivityRouter(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.nextsteplist"), 
                null,  null);
            return root;
        }

        /// <summary>
        /// Create previous activity component
        /// 创建上一步根显示节点
        /// </summary>
        /// <returns>根节点</returns>
        internal static NextActivityComponent CreatePreviousActivityComponent()
        {
            NextActivityComponent root = new NextActivityRouter(LocalizeHelper.GetEngineMessage("nextactivitycomponentfactory.previoussteplist"),
                null, null);
            return root;
        }

        /// <summary>
        /// Create next activity component
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
