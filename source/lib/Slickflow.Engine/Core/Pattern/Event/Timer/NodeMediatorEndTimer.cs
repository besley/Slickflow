using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Core.Runtime;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;

namespace Slickflow.Engine.Core.Pattern.Event.Timer
{
    /// <summary>
    /// 结束节点处理类
    /// </summary>
    internal class NodeMediatorEndTimer : NodeMediator
    {
        internal NodeMediatorEndTimer(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// 节点内部业务逻辑执行
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            //执行前Action列表
            OnBeforeExecuteWorkItem();

            //设置流程完成
            ProcessInstanceManager pim = new ProcessInstanceManager();
            var processInstance = pim.Complete(ActivityForwardContext.ProcessInstance.ID,
                ActivityForwardContext.ActivityResource.AppRunner,
                Session);

            //如果当前流程是子流程，则子流程完成，主流程流转到下一节点
            if (pim.IsSubProcess(processInstance) == true)
            {
                ContinueMainProcessRunning(processInstance, Session);
            }

            //执行后Action列表
            OnAfterExecuteWorkItem();
        }

        /// <summary>
        /// 继续执行主流程
        /// </summary>
        /// <param name="processInstance">子流程实例</param>
        /// <param name="session">数据库会话</param>
        private void ContinueMainProcessRunning(ProcessInstanceEntity processInstance,
            IDbSession session)
        {
            //读取流程下一步办理人员列表信息
            var runner = FillNextActivityPerformersByRoleList(processInstance.InvokedActivityInstanceID,
                processInstance.InvokedActivityGUID,
                session);

            //开始执行下一步
            var runAppResult = WfExecutedResult.Default();
            var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceAppRunning(runner, session, ref runAppResult);
            if (runAppResult.Status == WfExecutedStatus.Exception)
            {
                throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorend.ContinueMainProcessRunning.warn", runAppResult.Message));
            }

            //注册事件并运行
            WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                runtimeInstance_OnWfProcessRunning,
                runtimeInstance_OnWfProcessContinued);
            bool isRun = runtimeInstance.Execute(session);

            void runtimeInstance_OnWfProcessRunning(object sender, WfEventArgs args)
            {
                DelegateExecutor.InvokeExternalDelegate(session,
                    EventFireTypeEnum.OnProcessRunning,
                    runner.DelegateEventList,
                    runtimeInstance.ProcessInstanceID);
            }

            void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
            {
                runAppResult = args.WfExecutedResult;
                if (runAppResult.Status == WfExecutedStatus.Success)
                {
                    DelegateExecutor.InvokeExternalDelegate(session,
                        EventFireTypeEnum.OnProcessContinued,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceID);
                }
            }
        }

        /// <summary>
        /// 使用流程定义资源添加角色用户
        /// </summary>
        /// <param name="mainActivityInstanceID">主流程节点实例ID</param>
        /// <param name="mainActivityGUID">主流程节点GUID</param>
        /// <param name="session">数据库会话</param>
        /// <returns>执行用户</returns>
        private WfAppRunner FillNextActivityPerformersByRoleList(int mainActivityInstanceID,
            string mainActivityGUID,
            IDbSession session)
        {
            var pm = new ProcessInstanceManager();
            var mainProcessInstance = pm.GetByActivity(session.Connection, mainActivityInstanceID, session.Transaction);
            var processModel = ProcessModelFactory.CreateByProcessInstance(session.Connection,
                mainProcessInstance,
                session.Transaction);
            var nextSteps = processModel.GetNextActivityTree(mainActivityGUID);
            //获取主流程的任务
            var task = new TaskManager().GetTaskByActivity(session.Connection, mainProcessInstance.ID, mainActivityInstanceID, session.Transaction);
            var runner = new WfAppRunner
            {
                AppName = mainProcessInstance.AppName,
                AppInstanceID = mainProcessInstance.AppInstanceID,
                AppInstanceCode = mainProcessInstance.AppInstanceCode,
                ProcessGUID = mainProcessInstance.ProcessGUID,
                Version = mainProcessInstance.Version,
                UserID = task.AssignedToUserID,
                UserName = task.AssignedToUserName
            };

            foreach (var node in nextSteps)
            {
                Dictionary<string, PerformerList> dict = new Dictionary<string, PerformerList>();
                var performerList = PerformerBuilder.CreatePerformerList(node.Roles);      //根据节点角色定义，读取执行者列表
                if (node.ActivityType != ActivityTypeEnum.EndNode
                    && performerList.Count == 0)
                {
                    throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorend.FillNextActivityPerformersByRoleList.warn", node.ActivityName));
                }
                else
                {
                    dict.Add(node.ActivityGUID, performerList);
                }
                runner.NextActivityPerformers = dict;
            }
            return runner;
        }

        /// <summary>
        /// 结束节点活动及转移实例化，没有任务数据
        /// </summary>
        /// <param name="toActivity">当前Activity</param>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">Session</param>
        internal override void CreateActivityTaskTransitionInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var toActivityInstance = CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            ActivityInstanceManager.Insert(toActivityInstance, session);

            ActivityInstanceManager.Complete(toActivityInstance.ID,
                activityResource.AppRunner,
                session);

            //写节点转移实例数据
            base.InsertTransitionInstance(processInstance,
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
