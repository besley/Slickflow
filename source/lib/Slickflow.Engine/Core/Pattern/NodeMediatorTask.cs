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
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 任务节点执行器
    /// </summary>
    internal class NodeMediatorTask : NodeMediator
    {
        internal NodeMediatorTask(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorTask(IDbSession session)
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
                //执行前Action列表
                OnBeforeExecuteWorkItem();

                //完成当前的任务节点
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskID,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                //执行后Action列表
                OnAfterExecuteWorkItem();

                //获取下一步节点列表：并继续执行
                if (canContinueForwardCurrentNode)
                {
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, this.Session);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 完成节点实例
        /// </summary>
        /// <param name="taskID">任务视图</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">会话</param>        
        internal bool CompleteWorkItem(int? taskID,
            ActivityResource activityResource,
            IDbSession session)
        {
            WfAppRunner runner = new WfAppRunner
            {
                UserID = activityResource.AppRunner.UserID,         //避免taskview为空
                UserName = activityResource.AppRunner.UserName
            };

            //流程强制拉取向前跳转时，没有运行人的任务实例
            if (taskID != null)
            {
                //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
                base.TaskManager.Complete(taskID.Value, activityResource.AppRunner, session);
            }

            //设置活动节点的状态为完成状态
            base.ActivityInstanceManager.Complete(base.LinkContext.FromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            base.LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            Boolean canContinueForwardCurrentNode = base.LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

            return canContinueForwardCurrentNode;
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
            Boolean isParallel = false;
            if (fromActivityInstance.ActivityType == (short)ActivityTypeEnum.GatewayNode)
            {
                //并发多实例分支判断(AndSplit Multiple)
                var processModel = ProcessModelFactory.Create(processInstance.ProcessGUID, processInstance.Version);
                var activityNode = processModel.GetActivity(fromActivityInstance.ActivityGUID);
                isParallel = processModel.IsAndSplitMI(activityNode);
            }

            if (isParallel)
            {
                //并行多实例容器
                ActivityInstanceEntity entity = null;
                var plist = activityResource.NextActivityPerformers[toActivity.ActivityGUID];

                //创建并行多实例分支
                for (var i = 0; i < plist.Count; i++)
                {
                    entity = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                    entity.AssignedToUserIDs = plist[i].UserID;
                    entity.AssignedToUserNames = plist[i].UserName;
                    entity.ActivityState = (short)ActivityStateEnum.Ready;
                    //插入活动实例数据
                    entity.ID = base.ActivityInstanceManager.Insert(entity, session);
                    //插入任务
                    base.TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);
                    //插入转移数据
                    InsertTransitionInstance(processInstance,
                        transitionGUID,
                        fromActivityInstance,
                        entity,
                        transitionType,
                        flyingType,
                        activityResource.AppRunner,
                        session);
                }
            }
            else
            {
                //普通任务节点
                var toActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

                //处理多次退回后的返送
                WriteBackSrcOrgInformation(toActivityInstance, fromActivityInstance, session);

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

                //调用外部事件的注册方法
                var delegateContext = new DelegateContext
                {
                    AppInstanceID = processInstance.AppInstanceID,
                    ProcessGUID = processInstance.ProcessGUID,
                    ProcessInstanceID = processInstance.ID,
                    ActivityGUID = toActivity.ActivityGUID,
                    ActivityCode = toActivity.ActivityCode,
                    ActivityResource = activityResource
                };
                DelegateExecutor.InvokeExternalDelegate(session,
                    EventFireTypeEnum.OnActivityCreated,
                    activityResource.AppRunner.DelegateEventList,
                    delegateContext);
            }
        }

        /// <summary>
        /// 维护多次退回时的源节点信息
        /// </summary>
        /// <param name="toActivityInstance">目的节点</param>
        /// <param name="fromActivityInstance">开始节点</param>
        /// <param name="session">会话</param>
        private void WriteBackSrcOrgInformation(ActivityInstanceEntity toActivityInstance, 
            ActivityInstanceEntity fromActivityInstance,
            IDbSession session)
        {
            if (fromActivityInstance.BackSrcActivityInstanceID != null)
            {
                var backSrcActivityInstance = base.ActivityInstanceManager.GetById(session.Connection,
                    fromActivityInstance.BackSrcActivityInstanceID.Value, session.Transaction);
                toActivityInstance.BackSrcActivityInstanceID = backSrcActivityInstance.BackSrcActivityInstanceID;
                toActivityInstance.BackOrgActivityInstanceID = backSrcActivityInstance.BackOrgActivityInstanceID;
            }
        }
    }
}
