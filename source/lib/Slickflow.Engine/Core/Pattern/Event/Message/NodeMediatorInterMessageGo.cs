﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Core.Runtime;

namespace Slickflow.Engine.Core.Pattern.Event.Message
{
    /// <summary>
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterMessageGo : NodeMediator
    {
        internal NodeMediatorInterMessageGo(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// 执行方法
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
                    Session);

                //执行后Action列表
                OnAfterExecuteWorkItem();

                //获取下一步节点列表：并继续执行
                if (canContinueForwardCurrentNode)
                {
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, Session);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 完成任务实例
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
                TaskManager.Complete(taskID.Value, activityResource.AppRunner, session);
            }

            //设置活动节点的状态为完成状态
            ActivityInstanceManager.Complete(LinkContext.FromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            bool canContinueForwardCurrentNode = LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

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
            int newActivityInstanceID = 0;
            bool isParallel = false;
            if (fromActivityInstance.ActivityType == (short)ActivityTypeEnum.GatewayNode)
            {
                //并发多实例分支判断(AndSplit Multiple)
                var processModel = ProcessModelFactory.CreateByProcessInstance(session.Connection, processInstance, session.Transaction);
                var activityNode = processModel.GetActivity(fromActivityInstance.ActivityGUID);
                isParallel = processModel.IsAndSplitMI(activityNode);
            }

            if (isParallel)
            {
                //并行多实例容器
                var entity = new ActivityInstanceEntity();
                var plist = activityResource.NextActivityPerformers[toActivity.ActivityGUID];

                //创建并行多实例分支
                for (var i = 0; i < plist.Count; i++)
                {
                    entity = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                    entity.AssignedToUserIDs = plist[i].UserID;
                    entity.AssignedToUserNames = plist[i].UserName;
                    entity.ActivityState = (short)ActivityStateEnum.Ready;
                    //插入活动实例数据
                    entity.ID = ActivityInstanceManager.Insert(entity, session);
                    //插入任务
                    TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);
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
                var toActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

                //进入运行状态
                toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

                //插入活动实例数据
                newActivityInstanceID = ActivityInstanceManager.Insert(toActivityInstance, session);

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
        }
    }
}
