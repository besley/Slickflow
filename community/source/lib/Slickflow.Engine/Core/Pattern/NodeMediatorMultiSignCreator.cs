/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

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

        /// <summary>
        /// 执行普通任务节点
        /// 1. 当设置任务完成时，同时设置活动完成
        /// 2. 当实例化活动数据时，产生新的任务数据
        /// </summary>
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
        /// 创建活动任务转移实例数据
        /// </summary>
        /// <param name="toActivity">活动</param>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">开始活动实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">会话</param>
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
            if (toActivity.ActivityType == ActivityTypeEnum.MultiSignNode)
            {
                if (toActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignForward)           //加签节点生成，跟普通任务节点生成一样
                {
                    //实例化Activity
                    var toActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

                    //进入运行状态
                    toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                    toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

                    //插入活动实例数据
                    base.ActivityInstanceManager.Insert(toActivityInstance, session);

                    //插入任务数据
                    base.CreateNewTask(toActivityInstance, activityResource, session);

                    //插入转移数据
                    InsertTransitionInstance(processInstance,
                        transitionGUID,
                        fromActivityInstance,
                        toActivityInstance,
                        transitionType,
                        flyingType,
                        activityResource.AppRunner,
                        session);
                }
                else if (toActivity.MultiSignDetail.ComplexType == ComplexTypeEnum.SignTogether)             //会签节点会生成多个实例子节点
                {
                    //创建会签节点的主节点，以及会签主节点下的实例子节点记录
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
