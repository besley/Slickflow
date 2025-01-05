using System;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Creator for multi instance node scenes
    /// 多实例节点场景的创建器
    /// </summary>
    internal class NodeMediatorMultiSignCreator : NodeMediator
    {
        internal NodeMediatorMultiSignCreator(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorMultiSignCreator(IDbSession session)
            : base(session)
        {
        }

        internal override void ExecuteWorkItem()
        {
            try
            {
                ;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Create activity task transition instance
        /// </summary>
        internal override void CreateActivityTaskTransitionInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //判断是否是会签节点，如果是创建会签节点
            //Determine whether it is a countersignature node, and if it is a creation countersignature node
            if (toActivity.ActivityType == ActivityTypeEnum.MultiSignNode)
            {
                if (toActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignForward)          
                {
                    //加签节点生成，跟普通任务节点生成一样
                    //Generate signature nodes, just like generating regular task nodes
                    var toActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                    toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                    toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

                    base.ActivityInstanceManager.Insert(toActivityInstance, session);

                    base.CreateNewTask(toActivityInstance, activityResource, session);

                    InsertTransitionInstance(processInstance,
                        transitionGUID,
                        fromActivityInstance,
                        toActivityInstance,
                        transitionType,
                        flyingType,
                        activityResource.AppRunner,
                        session);
                }
                else if (toActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignTogether)            
                {
                    //创建会签节点的主节点，以及会签主节点下的实例子节点记录
                    //Create the master node for the countersignature node and record the instance child nodes under the countersignature master node
                    CreateMultipleInstance(toActivity, processInstance, fromActivityInstance,
                        transitionGUID, transitionType, flyingType, activityResource, session);
                }
                else
                {
                    throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatormicreator.CreateActivityTaskTransitionInstance.warn"));
                }
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediatormicreator.CreateActivityTaskTransitionInstance.exception"));
            }
        }
    }
}
