/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Schedule;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Runtime;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 节点执行器的抽象类
    /// </summary>
    internal abstract class NodeMediator
    {
        #region 属性列表
        private ActivityForwardContext _activityForwardContext;
        protected ActivityForwardContext ActivityForwardContext
        {
            get
            {
                return _activityForwardContext;
            }
        }

        private BackwardContext _backwardContext;
        protected BackwardContext BackwardContext
        {
            get
            {
                return _backwardContext;
            }
        }

        private IDbSession _session;
        protected IDbSession Session
        {
            get
            {
                return _session;
            }
        }

        private Linker _linker;
        internal Linker Linker
        {
            get
            {
                if (_linker == null)
                    _linker = new Linker();

                return _linker;
            }
        }

        /// <summary>
        /// 活动节点实例管理对象
        /// </summary>
        private ActivityInstanceManager activityInstanceManager;
        internal ActivityInstanceManager ActivityInstanceManager
        {
            get
            {
                if (activityInstanceManager == null)
                {
                    activityInstanceManager = new ActivityInstanceManager();
                }
                return activityInstanceManager;
            }
        }

        private TaskManager taskManager;
        internal TaskManager TaskManager
        {
            get
            {
                if (taskManager == null)
                {
                    taskManager = new TaskManager();
                }
                return taskManager;
            }
        }

        private ProcessInstanceManager processInstanceManager;
        internal ProcessInstanceManager ProcessInstanceManager
        {
            get
            {
                if (processInstanceManager == null)
                    processInstanceManager = new ProcessInstanceManager();
                return processInstanceManager;
            }
        }

        private WfNodeMediatedResult _wfNodeMediatedResult;
        internal WfNodeMediatedResult WfNodeMediatedResult
        {
            get
            {
                if (_wfNodeMediatedResult == null)
                {
                    _wfNodeMediatedResult = new WfNodeMediatedResult();
                }
                return _wfNodeMediatedResult;
            }
        }
        //回退时参数对象
        private ReturnDataContext _returnDataContext;

        public ReturnDataContext ReturnDataContext
        {
            get
            {
                if (_returnDataContext == null)
                {
                    _returnDataContext = new ReturnDataContext();
                }
                return _returnDataContext;
            }
        }
        #endregion

        #region 抽象方法列表
        /// <summary>
        /// 执行节点方法
        /// </summary>
        internal abstract void ExecuteWorkItem();
        #endregion

        #region 构造函数
        /// <summary>
        /// 向前流转时的NodeMediator的构造函数
        /// </summary>
        /// <param name="forwardContext">前进上下文</param>
        /// <param name="session">Session</param>
        internal NodeMediator(ActivityForwardContext forwardContext, IDbSession session)
        {
            _activityForwardContext = forwardContext;
            _session = session;
            Linker.FromActivity = forwardContext.Activity;
        }

        /// <summary>
        /// 退回处理时的NodeMediator的构造函数
        /// </summary>
        /// <param name="backwardContext">退回上下文</param>
        /// <param name="session">Session</param>
        internal NodeMediator(BackwardContext backwardContext, IDbSession session)
        {
            _session = session;
            _backwardContext = backwardContext;
            Linker.FromActivity = backwardContext.BackwardFromActivity;
        }

        internal NodeMediator(IDbSession session)
        {
            _session = session;
        }
        #endregion

        #region 执行外部事件的方法
        /// <summary>
        /// 执行外部操作的方法
        /// </summary>
        /// <param name="actionList">操作列表</param>
        /// <param name="actionMethodParameters">方法参数</param>
        internal void ExecteActionList(IList<ActionEntity> actionList,
            IDictionary<string, ActionParameterInternal> actionMethodParameters)
        {
            if (actionList != null && actionList.Count > 0)
            {
                var actionExecutor = new ActionExecutor();
                actionExecutor.ExecteActionList(actionList, actionMethodParameters);
            }
        }
        #endregion

        #region 流程执行逻辑
        /// <summary>
        /// 遍历执行当前节点后面的节点
        /// </summary>
        /// <param name="isJumpforward">是否跳转</param>
        internal virtual void ContinueForwardCurrentNode(bool isJumpforward)
        {
            var activityResource = ActivityForwardContext.ActivityResource;

            if (isJumpforward == false)
            {
                //非跳转模式的正常运行
                var nextActivityMatchedResult = this.ActivityForwardContext.ProcessModel.GetNextActivityList(
                    this.Linker.FromActivityInstance.ActivityGUID,
                    activityResource.ConditionKeyValuePair,
                    activityResource,
                    (a, b) => a.NextActivityPerformers.ContainsKey(b.ActivityGUID));

                if (nextActivityMatchedResult.MatchedType != NextActivityMatchedType.Successed
                    || nextActivityMatchedResult.Root.HasChildren == false)
                {
                    throw new WfRuntimeException("没有匹配的后续流转节点，流程虽然能处理当前节点，但是无法流转到下一步！");
                }

                ContinueForwardCurrentNodeRecurisivly(this.Linker.FromActivity,
                    this.Linker.FromActivityInstance,
                    nextActivityMatchedResult.Root,
                    activityResource.ConditionKeyValuePair,
                    isJumpforward);
            }
            else
            {
                //跳转模式运行
                var root = NextActivityComponentFactory.CreateNextActivityComponent();
                var nextActivityComponent = NextActivityComponentFactory.CreateNextActivityComponent(
                    this.Linker.FromActivity,
                    this.Linker.ToActivity);
                
                root.Add(nextActivityComponent);

                ContinueForwardCurrentNodeRecurisivly(this.Linker.FromActivity,
                    this.Linker.FromActivityInstance,
                    root,
                    activityResource.ConditionKeyValuePair,
                    isJumpforward);
            }
        }

        /// <summary>
        /// 递归执行节点
        /// 1)创建普通节点的任务
        /// 2)创建会签节点的任务
        /// </summary>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="isJumpforward">是否跳跃</param>
        /// <param name="root">根节点</param>
        /// <param name="conditionKeyValuePair">条件key-value</param>
        protected void ContinueForwardCurrentNodeRecurisivly(ActivityEntity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            NextActivityComponent root,
            IDictionary<string, string> conditionKeyValuePair,
            Boolean isJumpforward)
        {
            foreach (NextActivityComponent comp in root)
            {
                if (comp.HasChildren)
                {
                    //此节点类型为分支或合并节点类型：首先需要实例化当前节点(自动完成)
                    NodeMediatorGateway gatewayNodeMediator = NodeMediatorGatewayFactory.CreateGatewayNodeMediator(comp.Activity, 
                        this.ActivityForwardContext.ProcessModel, 
                        Session);

                    ICompleteAutomaticlly autoGateway = (ICompleteAutomaticlly)gatewayNodeMediator;
                    GatewayExecutedResult gatewayResult = autoGateway.CompleteAutomaticlly(
                        ActivityForwardContext.ProcessInstance,
                        comp.Transition.TransitionGUID,
                        fromActivityInstance,
                        ActivityForwardContext.ActivityResource,
                        Session);

                    if (gatewayResult.Status == GatewayExecutedStatus.Successed)
                    {
                        //遍历后续子节点
                        ContinueForwardCurrentNodeRecurisivly(fromActivity,
                            gatewayNodeMediator.GatewayActivityInstance,
                            comp,
                            conditionKeyValuePair,
                            isJumpforward);
                    }
                    else
                    {
                        WfRuntimeException e = new WfRuntimeException("第一个满足条件的节点已经被成功执行，此后的节点被阻止在XOrJoin节点!");
                        LogManager.RecordLog("递归执行节点方法异常", LogEventType.Exception, LogPriority.Normal, null, e);
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.TaskNode)                   //普通任务节点
                {
                    //此节点类型为任务节点：根据fromActivityInstance的类型判断是否可以创建任务
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //创建新任务节点
                        NodeMediator taskNodeMediator = new NodeMediatorTask(Session);
                        taskNodeMediator.CreateActivityTaskTransitionInstance(comp.Activity,
                            ActivityForwardContext.ProcessInstance,
                            fromActivityInstance,
                            comp.Transition.TransitionGUID,
                            comp.Transition.DirectionType == TransitionDirectionTypeEnum.Loop ?
                                TransitionTypeEnum.Loop : TransitionTypeEnum.Forward, //根据Direction方向确定是否是自身循环
                            isJumpforward == true ? 
                                TransitionFlyingTypeEnum.ForwardFlying : TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                    else
                    {
                        //下一步的任务节点没有创建，需给出提示信息
                        WfRuntimeException e = new WfRuntimeException("等待其它需要合并的分支!");
                        LogManager.RecordLog("递归执行节点方法异常", LogEventType.Exception, LogPriority.Normal, null, e);
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.SubProcessNode)         //子流程节点
                {
                    //节点类型为subprocessnode
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //实例化subprocess节点数据
                        NodeMediator subNodeMediator = new NodeMediatorSubProcess(Session);
                        subNodeMediator.CreateActivityTaskTransitionInstance(comp.Activity,
                            ActivityForwardContext.ProcessInstance,
                            fromActivityInstance,
                            comp.Transition.TransitionGUID,
                            comp.Transition.DirectionType == TransitionDirectionTypeEnum.Loop ?
                                TransitionTypeEnum.Loop : TransitionTypeEnum.Forward,
                            TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.EndNode)        //结束节点
                {
                    //此节点为完成结束节点，结束流程
                    NodeMediator endMediator = new NodeMediatorEnd(ActivityForwardContext, Session);
                    endMediator.Linker.ToActivity = comp.Activity;

                    //创建结束节点实例及转移数据
                    endMediator.CreateActivityTaskTransitionInstance(comp.Activity, ActivityForwardContext.ProcessInstance,
                        fromActivityInstance, comp.Transition.TransitionGUID, TransitionTypeEnum.Forward,
                        TransitionFlyingTypeEnum.NotFlying,
                        ActivityForwardContext.ActivityResource,
                        Session);

                    //执行结束节点中的业务逻辑
                    endMediator.ExecuteWorkItem();
                }
                else
                {
                    WfRuntimeException e = new WfRuntimeException(string.Format("XML文件定义了未知的节点类型，执行失败，节点类型信息：{0}",
                        comp.Activity.ActivityType.ToString()));
                    LogManager.RecordLog("递归执行节点方法异常", LogEventType.Exception, LogPriority.Normal, null, e);
                }
            }
        }

        /// <summary>
        /// 创建工作项及转移数据
        /// </summary>
        /// <param name="toActivity">活动</param>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">Session</param>
        internal virtual void CreateActivityTaskTransitionInstance(ActivityEntity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            String transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session){ }

        /// <summary>
        /// 创建任务的虚方法
        /// 1. 对于自动执行的工作项，无需重写该方法
        /// 2. 对于人工执行的工作项，需要重写该方法，插入待办的任务数据
        /// </summary>
        /// <param name="activityResource">活动资源</param>
        /// <param name="toActivityInstance">活动实例</param>
        /// <param name="session">Session</param>
        internal virtual void CreateNewTask(ActivityInstanceEntity toActivityInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            if (activityResource.NextActivityPerformers == null)
            {
                throw new WorkflowException("无法创建任务，流程流转下一步的办理人员不能为空！");
            }

            TaskManager.Insert(toActivityInstance,
                activityResource.NextActivityPerformers[toActivityInstance.ActivityGUID],
                activityResource.AppRunner,
                session);
        }

        /// <summary>
        /// 创建节点对象
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="activity">活动</param>
        /// <param name="runner">执行者</param>
        protected ActivityInstanceEntity CreateActivityInstanceObject(ActivityEntity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceEntity entity = ActivityInstanceManager.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.ID,
                activity,
                runner);

            return entity;
        }

        /// <summary>
        /// 创建退回类型的活动实例对象
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="backSrcActivityInstanceID">退回的活动实例ID</param>
        /// <param name="runner">登录用户</param>
        /// <returns></returns>
        protected ActivityInstanceEntity CreateBackwardToActivityInstanceObject(ProcessInstanceEntity processInstance,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            WfAppRunner runner)
        {
            ActivityInstanceEntity entity = ActivityInstanceManager.CreateBackwardActivityInstanceObject(
                processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.ID,
                this.BackwardContext.BackwardToTaskActivity,
                backwardType,
                backSrcActivityInstanceID,
                runner);

            return entity;
        }

        /// <summary>
        /// 插入连线实例的方法
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移ID</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="toActivityInstance">到达活动实例</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="runner">执行者</param>
        /// <param name="session">Session</param>
        internal virtual void InsertTransitionInstance(ProcessInstanceEntity processInstance,
            String transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity toActivityInstance,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            WfAppRunner runner,
            IDbSession session)
        {
            var tim = new TransitionInstanceManager();
            var transitionInstanceObject = tim.CreateTransitionInstanceObject(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                runner,
                (byte)ConditionParseResultEnum.Passed);

            tim.Insert(session.Connection, transitionInstanceObject, session.Transaction);
        }

        /// <summary>
        /// 生成任务办理人ID字符串列表
        /// </summary>
        /// <param name="performerList">操作者列表</param>
        /// <returns>ID字符串列表</returns>
        protected string GenerateActivityAssignedUserIDs(PerformerList performerList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var performer in performerList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(performer.UserID);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 生成办理人名称的字符串列表
        /// </summary>
        /// <param name="performerList">操作者列表</param>
        /// <returns>ID字符串列表</returns>
        protected string GenerateActivityAssignedUserNames(PerformerList performerList)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            foreach (var performer in performerList)
            {
                if (strBuilder.ToString() != "")
                    strBuilder.Append(",");
                strBuilder.Append(performer.UserName);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 由节点分配的人员信息生成PerformerList数据结构
        /// </summary>
        /// <param name="activityInstance">活动实例</param>
        /// <returns>人员列表</returns>
        protected PerformerList AntiGenerateActivityPerformerList(ActivityInstanceEntity activityInstance)
        {
            var performerList = new PerformerList();

            if (!string.IsNullOrEmpty(activityInstance.AssignedToUserIDs)
                && !string.IsNullOrEmpty(activityInstance.AssignedToUserNames))
            {
                var assignedToUserIDs = activityInstance.AssignedToUserIDs.Split(',');
                var assignedToUserNames = activityInstance.AssignedToUserNames.Split(',');

                for (var i = 0; i < assignedToUserIDs.Count(); i++)
                {
                    performerList.Add(new Performer(assignedToUserIDs[i], assignedToUserNames[i]));
                }
            }
            return performerList;
        }
        #endregion

        /// <summary>
        /// 根据节点执行结果类型，生成消息
        /// </summary>
        /// <returns>消息内容</returns>
        internal string GetNodeMediatedMessage()
        {
            var message = string.Empty;
            if (WfNodeMediatedResult.Feedback == WfNodeMediatedFeedback.ForwardToNextSequenceTask)
            {
                message = "串行会(加)签，设置下一个执行节点的任务进入运行状态！";
            }
            else if (WfNodeMediatedResult.Feedback == WfNodeMediatedFeedback.WaitingForCompletedMore)
            {
                message = "并行会(加)签，等待节点到达足够多的完成比例！";
            }

            return message;
        }
    }
}
