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
using Slickflow.Engine.Core.Runtime;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Essential;

namespace Slickflow.Engine.Core.Pattern.Event.Message
{
    /// <summary>
    /// End node mediator message throw
    /// 结束节点处理类
    /// </summary>
    internal class NodeMediatorEndMsgThrow : NodeMediator
    {
        internal NodeMediatorEndMsgThrow(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            //执行前Action列表
            //On before execute work item
            OnBeforeExecuteWorkItem();

            //设置流程完成
            //Set process instance completed
            ProcessInstanceManager pim = new ProcessInstanceManager();
            var processInstance = pim.Complete(ActivityForwardContext.ProcessInstance.Id,
                ActivityForwardContext.ActivityResource.AppRunner,
                Session);

            //执行节点上的消息发布
            //Publish messages on the execution node
            var msgDelegateService = new MessageDelegateService();
            msgDelegateService.PublishMessage(processInstance, LinkContext.CurrentActivity, ActivityForwardContext.FromActivityInstance);

            //如果当前流程是子流程，则子流程完成，主流程流转到下一节点
            //If the current process is a subprocess,
            //then the subprocess is completed and the main process flows to the next node
            if (pim.IsSubProcess(processInstance) == true)
            {
                ContinueMainProcessRunning(processInstance, Session);
            }

            //执行后Action列表
            //On after execute work item
            OnAfterExecuteWorkItem();
        }

        /// <summary>
        /// Continue to execute the main process
        /// 继续执行主流程
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="session"></param>
        private void ContinueMainProcessRunning(ProcessInstanceEntity processInstance,
            IDbSession session)
        {
            //读取流程下一步办理人员列表信息
            //Read the personnel list information for the next step in the process
            var runner = FillNextActivityPerformersByRoleList(processInstance.InvokedActivityInstanceId,
                processInstance.InvokedActivityId,
                session);

            //开始执行下一步
            //Start executing the next step
            var runAppResult = WfExecutedResult.Default();
            var runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceAppRunning(runner, session, ref runAppResult);
            if (runAppResult.Status == WfExecutedStatus.Exception)
            {
                throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorend.ContinueMainProcessRunning.warn", runAppResult.Message));
            }

            //注册事件并运行
            //Register the event and run it
            WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                runtimeInstance_OnWfProcessRunning,
                runtimeInstance_OnWfProcessContinued);
            bool isRun = runtimeInstance.Execute(session);

            void runtimeInstance_OnWfProcessRunning(object sender, WfEventArgs args)
            {
                Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                    Delegate.EventFireTypeEnum.OnProcessRunning,
                    runner.DelegateEventList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
            {
                runAppResult = args.WfExecutedResult;
                if (runAppResult.Status == WfExecutedStatus.Success)
                {
                    Delegate.DelegateExecutor.InvokeExternalDelegate(session,
                        Delegate.EventFireTypeEnum.OnProcessContinued,
                        runner.DelegateEventList,
                        runtimeInstance.ProcessInstanceId);
                }
            }
        }

        /// <summary>
        /// Add role users using process defined resources
        /// 使用流程定义资源添加角色用户
        /// </summary>
        private WfAppRunner FillNextActivityPerformersByRoleList(int mainActivityInstanceId,
            string mainActivityId,
            IDbSession session)
        {
            var pm = new ProcessInstanceManager();
            var mainProcessInstance = pm.GetByActivity(session.Connection, mainActivityInstanceId, session.Transaction);
            var processModel = ProcessModelFactory.CreateByProcessInstance(session.Connection,
                mainProcessInstance,
                session.Transaction);
            var nextSteps = processModel.GetNextActivityTreeView(mainActivityId);

            //获取主流程的任务
            //The task of obtaining the mainstream process
            var task = new TaskManager().GetTaskByActivity(session.Connection, mainProcessInstance.Id, mainActivityInstanceId, session.Transaction);
            var runner = new WfAppRunner
            {
                AppName = mainProcessInstance.AppName,
                AppInstanceId = mainProcessInstance.AppInstanceId,
                AppInstanceCode = mainProcessInstance.AppInstanceCode,
                ProcessId = mainProcessInstance.ProcessId,
                Version = mainProcessInstance.Version,
                UserId = task.AssignedUserId,
                UserName = task.AssignedUserName
            };

            foreach (var node in nextSteps)
            {
                Dictionary<string, PerformerList> dict = new Dictionary<string, PerformerList>();
                var performerList = PerformerBuilder.CreatePerformerList(node.Roles);      
                if (node.ActivityType != ActivityTypeEnum.EndNode
                    && performerList.Count == 0)
                {
                    throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediatorend.FillNextActivityPerformersByRoleList.warn", node.ActivityName));
                }
                else
                {
                    dict.Add(node.ActivityId, performerList);
                }
                runner.NextActivityPerformers = dict;
            }
            return runner;
        }

        /// <summary>
        /// End node activity and transfer instantiation, no task data available
        /// 结束节点活动及转移实例化，没有任务数据
        /// </summary>
        internal override void CreateActivityTaskTransitionInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            string transitionId,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var toActivityInstance = CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            ActivityInstanceManager.Insert(toActivityInstance, session);

            ActivityInstanceManager.Complete(toActivityInstance.Id,
                activityResource.AppRunner,
                session);

            base.InsertTransitionInstance(processInstance,
                transitionId,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }
    }
}
