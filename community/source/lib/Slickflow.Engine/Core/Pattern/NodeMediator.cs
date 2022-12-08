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
using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Schedule;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Runtime;
using Slickflow.Engine.Core.Pattern.Event;
using Slickflow.Engine.Core.Pattern.Gateway;
using Slickflow.Engine.Delegate;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 节点执行器的抽象类
    /// </summary>
    internal abstract class NodeMediator
    {
        #region 属性列表
        /// <summary>
        /// 活动上下文
        /// </summary>
        private ActivityForwardContext _activityForwardContext;
        /// <summary>
        /// 活动上下文
        /// </summary>
        protected ActivityForwardContext ActivityForwardContext
        {
            get
            {
                return _activityForwardContext;
            }
        }

        /// <summary>
        /// 退回上下文
        /// </summary>
        private BackwardContext _backwardContext;
        /// <summary>
        /// 退回上下文
        /// </summary>
        protected BackwardContext BackwardContext
        {
            get
            {
                return _backwardContext;
            }
        }

        /// <summary>
        /// 数据库会话
        /// </summary>
        private IDbSession _session;
        /// <summary>
        /// 数据库会话
        /// </summary>
        protected IDbSession Session
        {
            get
            {
                return _session;
            }
        }

        /// <summary>
        /// 节点间连线
        /// </summary>
        private LinkContext _linkContext;
        /// <summary>
        /// 节点间连线
        /// </summary>
        internal LinkContext LinkContext
        {
            get
            {
                if (_linkContext == null)
                    _linkContext = new LinkContext();

                return _linkContext;
            }
        }

        /// <summary>
        /// 活动节点实例管理对象
        /// </summary>
        private ActivityInstanceManager activityInstanceManager;
        /// <summary>
        /// 活动实例管理
        /// </summary>
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

        /// <summary>
        /// 任务管理
        /// </summary>
        private TaskManager taskManager;
        /// <summary>
        /// 任务管理
        /// </summary>
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

        /// <summary>
        /// 流程实例管理
        /// </summary>
        private ProcessInstanceManager processInstanceManager;
        /// <summary>
        /// 流程实例管理
        /// </summary>
        internal ProcessInstanceManager ProcessInstanceManager
        {
            get
            {
                if (processInstanceManager == null)
                    processInstanceManager = new ProcessInstanceManager();
                return processInstanceManager;
            }
        }

        /// <summary>
        /// 节点执行结果
        /// </summary>
        private WfNodeMediatedResult _wfNodeMediatedResult;
        /// <summary>
        /// 节点执行结果
        /// </summary>
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
            set
            {
                _wfNodeMediatedResult = value;
            }
        }

        /// <summary>
        /// 返回上下文
        /// </summary>
        private ReturnDataContext _returnDataContext;
        /// <summary>
        /// 返回上下文
        /// </summary>
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

        /// <summary>
        /// 委托服务
        /// </summary>
        private DelegateServiceBase _delegateService;
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
            LinkContext.FromActivity = forwardContext.Activity;
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
            LinkContext.FromActivity = backwardContext.BackwardFromActivity;
        }

        internal NodeMediator(IDbSession session)
        {
            _session = session;
        }
        #endregion

        #region 执行外部事件的方法
        /// <summary>
        /// 获取委托服务
        /// </summary>
        /// <returns>委托服务实例</returns>
        private DelegateServiceBase GetDelegateService()
        {
            if (_delegateService == null)
            {
                int? fromActivityInstanceID = null;
                string fromActivityGUID = string.Empty;
                string fromActivityName = string.Empty;
                if (IsNotNodeMediatorStart(this) == true)
                {
                    fromActivityInstanceID = ActivityForwardContext.FromActivityInstance.ID;
                    fromActivityGUID = ActivityForwardContext.FromActivityInstance.ActivityGUID;
                    fromActivityName = ActivityForwardContext.FromActivityInstance.ActivityName;
                }

                var delegateContext = new DelegateContext
                {
                    AppInstanceID = ActivityForwardContext.ProcessInstance.AppInstanceID,
                    ProcessGUID = ActivityForwardContext.ProcessInstance.ProcessGUID,
                    ProcessInstanceID = ActivityForwardContext.ProcessInstance.ID,
                    ActivityGUID = fromActivityGUID,
                    ActivityName = fromActivityName
                };
                _delegateService = DelegateServiceFactory.CreateDelegateService(DelegateScopeTypeEnum.Activity,
                   this.Session,
                   delegateContext);
            }
            return _delegateService;
        }

        /// <summary>
        /// 判断是否是起始类型的节点
        /// </summary>
        /// <param name="nodeMediator">节点监督器</param>
        /// <returns>判断结果</returns>
        private bool IsNotNodeMediatorStart(NodeMediator nodeMediator)
        {
            if (!(nodeMediator is NodeMediatorStart))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 触发前执行
        /// </summary>
        protected void OnBeforeExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = LinkContext.FromActivity.ActionList;
            ActionExecutor.ExecteActionListBefore(actionList, delegateService as IDelegateService);

            //----> 节点流转前，调用活动执行的委托事件
            var currentActivity = LinkContext.ToActivity != null ? LinkContext.ToActivity : LinkContext.FromActivity;
            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuting,
                currentActivity,
                ActivityForwardContext);
        }

        /// <summary>
        /// 触发后执行
        /// </summary>
        protected void OnAfterExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = ((this is NodeMediatorEnd)) ? LinkContext.ToActivity.ActionList : LinkContext.FromActivity.ActionList;
            ActionExecutor.ExecteActionListAfter(actionList, delegateService as IDelegateService);

            //----> 节点流转完成后，调用活动完成执行的委托事件
            var currentActivity = LinkContext.ToActivity != null ? LinkContext.ToActivity : LinkContext.FromActivity;
            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuted,
                currentActivity,
                ActivityForwardContext);
        }

        /// <summary>
        /// 执行代码自动服务内容
        /// </summary>
        protected void OnExecutingServiceItem()
        {
            var delegateService = GetDelegateService();
            var serviceList = LinkContext.ToActivity.ServiceList;
            ServiceExecutor.ExecuteServiceList(serviceList, delegateService as IDelegateService);
        }

        protected void OnExecutingScriptItem()
        {
            var delegateService = GetDelegateService();
            var scriptList = LinkContext.ToActivity.ScriptList;
            ScriptExecutor.ExecuteScriptList(scriptList, delegateService as IDelegateService);
        }
        #endregion

        #region 流程执行逻辑
        /// <summary>
        /// 遍历执行当前节点后面的节点
        /// 正常运行：需要走Transition的解析流转
        /// 其它方式为直接指定：比如跳转，返送等，不需要解析，不能等同于正常流转解析
        /// </summary>
        /// <param name="isNotParsedByTransition">不需要走解析流转</param>
        /// <param name="session">数据会话</param>
        internal void ContinueForwardCurrentNode(bool isNotParsedByTransition, IDbSession session)
        {
            try
            {
                var mediatorResult = new List<WfNodeMediatedResult>();
                var activityResource = ActivityForwardContext.ActivityResource;

                if (isNotParsedByTransition == true)
                {
                    //跳转模式时，直接流转运行
                    var root = NextActivityComponentFactory.CreateNextActivityComponent();
                    var nextActivityComponent = NextActivityComponentFactory.CreateNextActivityComponent(
                        this.LinkContext.FromActivity,
                        this.LinkContext.ToActivity);

                    root.Add(nextActivityComponent);

                    ContinueForwardCurrentNodeRecurisivly(this.LinkContext.FromActivity,
                        this.LinkContext.FromActivityInstance,
                        root,
                        activityResource.ConditionKeyValuePair,
                        isNotParsedByTransition,
                        ref mediatorResult);
                }
                else
                {
                    //普通正常运行
                    var nextActivityMatchedResult = this.ActivityForwardContext.ProcessModel.GetNextActivityList(
                        this.LinkContext.FromActivityInstance.ActivityGUID,
                        ActivityForwardContext.TaskID,
                        activityResource.ConditionKeyValuePair,
                        activityResource,
                        (a, b) => a.NextActivityPerformers.ContainsKey(b.ActivityGUID),
                        session);

                    if (nextActivityMatchedResult.MatchedType != NextActivityMatchedType.Successed
                        || nextActivityMatchedResult.Root.HasChildren == false)
                    {
                        throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNode.warn"));
                    }
                    
                    ContinueForwardCurrentNodeRecurisivly(this.LinkContext.FromActivity,
                        this.LinkContext.FromActivityInstance,
                        nextActivityMatchedResult.Root,
                        activityResource.ConditionKeyValuePair,
                        isNotParsedByTransition,
                        ref mediatorResult);
                }

                if (mediatorResult.Count() > 0)
                {
                    this.WfNodeMediatedResult = mediatorResult[0];
                }
            }
            catch (System.Exception ex)
            {
                throw new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNode.error", ex.Message),
                    ex.InnerException != null? ex.InnerException : ex);
            }
        }

        /// <summary>
        /// 递归执行节点
        /// 1)创建普通节点的任务
        /// 2)创建会签节点的任务
        /// </summary>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="isNotParsedForward">是否跳跃</param>
        /// <param name="root">根节点</param>
        /// <param name="conditionKeyValuePair">条件key-value</param>
        /// <param name="mediatorResult">执行结果的返回列表</param>
        protected void ContinueForwardCurrentNodeRecurisivly(Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            NextActivityComponent root,
            IDictionary<string, string> conditionKeyValuePair,
            Boolean isNotParsedForward,
            ref List<WfNodeMediatedResult> mediatorResult)
        {
            foreach (NextActivityComponent comp in root)
            {
                if (comp.HasChildren)
                {
                    NodeAutoExecutedResult nodeExecutedResult = null;
                    if (XPDLHelper.IsGatewayComponentNode(comp.Activity.ActivityType) == true)
                    {
                        //此节点类型为任务节点：根据fromActivityInstance的类型判断是否可以创建任务
                        if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                        {
                            //此节点类型为分支或合并节点类型：首先需要实例化当前节点(自动完成)
                            NodeMediatorGateway gatewayNodeMediator = NodeMediatorFactory.CreateNodeMediatorGateway(comp.Activity,
                            this.ActivityForwardContext.ProcessModel,
                            Session);

                            ICompleteGatewayAutomaticlly autoGateway = (ICompleteGatewayAutomaticlly)gatewayNodeMediator;
                            nodeExecutedResult = autoGateway.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                                comp.Transition.TransitionGUID,
                                fromActivity,
                                fromActivityInstance,
                                ActivityForwardContext.ActivityResource.AppRunner,
                                Session);

                            if (nodeExecutedResult.Status == NodeAutoExecutedStatus.Successed)
                            {
                                //遍历后续子节点
                                ContinueForwardCurrentNodeRecurisivly(gatewayNodeMediator.GatewayActivity,
                                    gatewayNodeMediator.GatewayActivityInstance,
                                    comp,
                                    conditionKeyValuePair,
                                    isNotParsedForward,
                                    ref mediatorResult);
                            }
                            else
                            {
                                mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                    WfNodeMediatedFeedback.OrJoinOneBranchHasBeenFinishedWaittingOthers));
                                LogManager.RecordLog("ContinueForwardCurrentNodeRecurisivlyExeception",
                                    LogEventType.Exception,
                                    LogPriority.Normal,
                                    null,
                                    new WfRuntimeException(
                                        LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.warn"))
                                    );
                            }
                        }
                        else
                        {
                            //下一步的任务节点没有创建，需给出提示信息
                            if (IsWaitingOneOfJoin(fromActivity.GatewayDetail) == true)
                            {
                                mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                    WfNodeMediatedFeedback.NeedOtherGatewayBranchesToJoin));
                                LogManager.RecordLog("NodeMediator.ContinueForwardCurrentNodeRecurisivly.IsWaintingOneOfJoin.Exception",
                                    LogEventType.Exception,
                                    LogPriority.Normal,
                                    null,
                                    new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.waitingothers.warn"))
                                    );
                            }
                            else
                            {
                                mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                    WfNodeMediatedFeedback.OtherUnknownReasonToDebug));
                                LogManager.RecordLog("NodeMediator.ContinueForwardCurrentNodeRecurisivly.IsWaintingOneOfJoin.OtherUnknownReason.Exception",
                                    LogEventType.Exception,
                                    LogPriority.Normal,
                                    null,
                                    new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.otherreason.warn"))
                                    );
                            }
                        }
                    }
                    else if (XPDLHelper.IsCrossOverComponentNode(comp.Activity.ActivityType) == true)
                    {
                        //中间事件类型节点，调用外部业务逻辑，然后流程继续向下流转
                        NodeMediator eventNodeMediator = NodeMediatorFactory.CreateNodeMediatorEvent(ActivityForwardContext,
                            comp.Activity,
                            ActivityForwardContext.ProcessModel,
                            Session);
                        eventNodeMediator.LinkContext.FromActivity = fromActivity;
                        eventNodeMediator.LinkContext.ToActivity = comp.Activity;
                        eventNodeMediator.ExecuteWorkItem();

                        ICompleteAutomaticlly autoEvent = (ICompleteAutomaticlly)eventNodeMediator;
                        nodeExecutedResult = autoEvent.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                            comp.Transition.TransitionGUID,
                            fromActivity,
                            fromActivityInstance,
                            comp.Activity,
                            ActivityForwardContext.ActivityResource.AppRunner,
                            Session);

                        if (nodeExecutedResult.Status == NodeAutoExecutedStatus.Successed)
                        {
                            //遍历后续子节点
                            ContinueForwardCurrentNodeRecurisivly(eventNodeMediator.LinkContext.ToActivity,
                                eventNodeMediator.LinkContext.ToActivityInstance,
                                comp,
                                conditionKeyValuePair,
                                isNotParsedForward,
                                ref mediatorResult);
                        }
                        else
                        {
                            mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                WfNodeMediatedFeedback.IntermediateEventFailed));
                            LogManager.RecordLog("IntermediateNodeExecuteException",
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfRuntimeException(nodeExecutedResult.Status.ToString()));
                        }
                    }
                    else
                    {
                        mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                            WfNodeMediatedFeedback.UnknownNodeTypeToWatch));
                        LogManager.RecordLog("NodeMediator.UnknownType.Exception",
                            LogEventType.Exception,
                            LogPriority.Normal,
                            null,
                            new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.unknowntype.warn",
                                comp.Activity.ActivityType.ToString()))
                            );
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
                            isNotParsedForward == true ?
                                TransitionFlyingTypeEnum.ForwardFlying : TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                    else
                    {
                        //下一步的任务节点没有创建，需给出提示信息
                        if (IsWaitingOneOfJoin(fromActivity.GatewayDetail) == true)
                        {
                            mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                WfNodeMediatedFeedback.NeedOtherGatewayBranchesToJoin));
                            LogManager.RecordLog("NodeMediator.TaskNode.NeedOtherGatewayBranchesToJoin.Exception",
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.waitingothers.warn"))
                                );
                        }
                        else
                        {
                            mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                WfNodeMediatedFeedback.OtherUnknownReasonToDebug));
                            LogManager.RecordLog("NodeMediator.TaskNode.NeedOtherGatewayBranchesToJoin.OtherUnkownReaseon.Exception",
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.otherreason.warn"))
                                );
                        }
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.ServiceNode)
                {
                    //服务类型节点，调用外部业务逻辑，然后流程继续向下流转
                    NodeAutoExecutedResult serviceExecutedResult = null;
                    NodeMediator serviceNodeMediator = NodeMediatorFactory.CreateNodeMediatorEvent(ActivityForwardContext,
                        comp.Activity,
                        ActivityForwardContext.ProcessModel,
                        Session);
                    serviceNodeMediator.ExecuteWorkItem();

                    ICompleteAutomaticlly autoEvent = (ICompleteAutomaticlly)serviceNodeMediator;
                    serviceExecutedResult = autoEvent.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                        comp.Transition.TransitionGUID,
                        fromActivity,
                        fromActivityInstance,
                        comp.Activity,
                        ActivityForwardContext.ActivityResource.AppRunner,
                        Session);

                    if (serviceExecutedResult.Status == NodeAutoExecutedStatus.Successed)
                    {
                        //遍历后续子节点
                        ContinueForwardCurrentNodeRecurisivly(serviceNodeMediator.LinkContext.ToActivity,
                            serviceNodeMediator.LinkContext.ToActivityInstance,
                            comp,
                            conditionKeyValuePair,
                            isNotParsedForward,
                            ref mediatorResult);
                    }
                    else
                    {
                        mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                            WfNodeMediatedFeedback.IntermediateEventFailed));
                        LogManager.RecordLog("ServiceNodeExecuteException",
                            LogEventType.Exception,
                            LogPriority.Normal,
                            null,
                            new WfRuntimeException(serviceExecutedResult.Status.ToString()));
                    }
                }
                else if(comp.Activity.ActivityType == ActivityTypeEnum.ScriptNode)
                {
                    //脚本类型节点，调用脚本程序，然后流程继续向下流转
                    NodeAutoExecutedResult scriptExecutedResult = null;
                    NodeMediator scriptNodeMediator = NodeMediatorFactory.CreateNodeMediatorEvent(ActivityForwardContext,
                        comp.Activity,
                        ActivityForwardContext.ProcessModel,
                        Session);
                    scriptNodeMediator.ExecuteWorkItem();

                    ICompleteAutomaticlly autoEvent = (ICompleteAutomaticlly)scriptNodeMediator;
                    scriptExecutedResult = autoEvent.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                        comp.Transition.TransitionGUID,
                        fromActivity,
                        fromActivityInstance,
                        comp.Activity,
                        ActivityForwardContext.ActivityResource.AppRunner,
                        Session);
                    if(scriptExecutedResult.Status == NodeAutoExecutedStatus.Successed)
                    {
                        ContinueForwardCurrentNodeRecurisivly(scriptNodeMediator.LinkContext.ToActivity,
                            scriptNodeMediator.LinkContext.ToActivityInstance,
                            comp,
                            conditionKeyValuePair,
                            isNotParsedForward,
                            ref mediatorResult);
                    }
                    else
                    {
                        mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(WfNodeMediatedFeedback.IntermediateEventFailed));
                        LogManager.RecordLog("ScriptNodeExecuteException",
                            LogEventType.Exception,
                            LogPriority.Normal,
                            null,
                            new WfRuntimeException(scriptExecutedResult.Status.ToString()));
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.EndNode)        //结束节点
                {
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //此节点为完成结束节点，结束流程
                        var endMediator = NodeMediatorFactory.CreateNodeMediatorEnd(ActivityForwardContext, comp.Activity, Session);
                        endMediator.LinkContext.ToActivity = comp.Activity;

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
                        //结束节点没有创建，需给出提示信息
                        if (IsWaitingOneOfJoin(fromActivity.GatewayDetail) == true)
                        {
                            mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                WfNodeMediatedFeedback.NeedOtherGatewayBranchesToJoin));
                            LogManager.RecordLog("NodeMediator.EndNode.NeedOtherGatewayBranchesToJoin.Exception",
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.waitingothers.warn"))
                                );
                        }
                        else
                        {
                            mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                WfNodeMediatedFeedback.OtherUnknownReasonToDebug));
                            LogManager.RecordLog("NodeMediator.EndNode.NeedOtherGatewayBranchesToJoin.OtherUnkownReaseon.Exception",
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.otherreason.warn"))
                                );
                        }
                    }
                }
                else
                {
                    mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                        WfNodeMediatedFeedback.UnknownNodeTypeToWatch));
                    LogManager.RecordLog("NodeMediator.UnkonwNodeType.Exception", 
                        LogEventType.Exception, 
                        LogPriority.Normal,
                        null,
                        new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.unknowntype.warn",
                            comp.Activity.ActivityType.ToString()))
                        );
                }
            }
        }

        /// <summary>
        /// 判断是否是合并情况
        /// </summary>
        /// <param name="gatewayDetail"></param>
        /// <returns></returns>
        private Boolean IsWaitingOneOfJoin(GatewayDetail gatewayDetail)
        {
            Boolean isWaiting = false;
            if (gatewayDetail != null)
            {
                var directionEnum = gatewayDetail.DirectionType;
                if (directionEnum == GatewayDirectionEnum.OrJoin
                    || directionEnum == GatewayDirectionEnum.XOrJoin
                    || directionEnum == GatewayDirectionEnum.AndJoin
                    || directionEnum == GatewayDirectionEnum.AndJoinMI)
                {
                    isWaiting = true;
                }
            }
            return isWaiting;
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
        internal virtual void CreateActivityTaskTransitionInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            String transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        { }

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
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodemediator.createnewtask.warn"));
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
        protected ActivityInstanceEntity CreateActivityInstanceObject(Activity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceEntity entity = ActivityInstanceManager.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.AppInstanceCode,
                processInstance.ProcessGUID,
                processInstance.ID,
                activity,
                runner);

            return entity;
        }

        /// <summary>
        /// 插入实例数据
        /// </summary>
        /// <param name="activityInstance">活动资源</param>
        /// <param name="session">会话</param>
        protected virtual int InsertActivityInstance(ActivityInstanceEntity activityInstance,
            IDbSession session)
        {
            return ActivityInstanceManager.Insert(activityInstance, session);
        }


        /// <summary>
        /// 节点对象的完成方法
        /// </summary>
        /// <param name="ActivityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        protected virtual void CompleteActivityInstance(int ActivityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            //设置完成状态
            ActivityInstanceManager.Complete(ActivityInstanceID,
                runner,
                session);
        }

        /// <summary>
        /// 会签类型的主节点, 多实例节点处理
        /// 创建会签节点的主节点，以及会签主节点下的实例子节点记录
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="toActivity">活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="activityResource">资源</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="session">会话</param>
        internal void CreateMultipleInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            String transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //实例化主节点Activity
            var toActivityInstance = CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            //更新主节点实例数据
            toActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
            toActivityInstance.ComplexType = (short)ComplexTypeEnum.SignTogether;

            //串行或并行类型
            toActivityInstance.MergeType = (short)toActivity.MultiSignDetail.MergeType;
            //通过率类型：个数或者百分比
            toActivityInstance.CompareType = (short)toActivity.MultiSignDetail.CompareType;
            //主节点设置通过率
            toActivityInstance.CompleteOrder = toActivity.MultiSignDetail.CompleteOrder;

            toActivityInstance.AssignedToUserIDs = PerformerBuilder.GenerateActivityAssignedUserIDs(
                activityResource.NextActivityPerformers[toActivity.ActivityGUID]);
            toActivityInstance.AssignedToUserNames = PerformerBuilder.GenerateActivityAssignedUserNames(
                activityResource.NextActivityPerformers[toActivity.ActivityGUID]);

            //插入主节点实例数据
            ActivityInstanceManager.Insert(toActivityInstance, session);

            //插入主节点转移数据
            InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);

            //插入会签子节点实例数据
            var plist = activityResource.NextActivityPerformers[toActivity.ActivityGUID];
            ActivityInstanceEntity entity = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                entity = ActivityInstanceManager.CreateActivityInstanceObject(toActivityInstance);
                entity.AssignedToUserIDs = plist[i].UserID;
                entity.AssignedToUserNames = plist[i].UserName;
                entity.MIHostActivityInstanceID = toActivityInstance.ID;

                //并行串行下，多实例子节点的执行顺序设置
                if (toActivityInstance.MergeType == (short)MergeTypeEnum.Sequence)
                {
                    entity.CompleteOrder = (short)(i + 1);
                }
                else if (toActivityInstance.MergeType == (short)MergeTypeEnum.Parallel)
                {
                    entity.CompleteOrder = -1;       //并行模式下CompleteOrder的优先级一样，所以置为 -1
                }

                //如果是串行会签，只有第一个节点处于运行状态，其它节点挂起
                if ((i > 0) && (toActivity.MultiSignDetail.MergeType == MergeTypeEnum.Sequence))
                {
                    entity.ActivityState = (short)ActivityStateEnum.Suspended;
                }

                //插入活动实例数据，并返回活动实例ID
                entity.ID = ActivityInstanceManager.Insert(entity, session);

                //插入任务数据
                TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);
            }
        }

        /// <summary>
        /// 创建退回类型的活动实例对象
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="backSrcActivityInstanceID">退回的活动实例ID</param>
        /// <param name="backOrgActivityInstanceID">初始办理活动实例ID</param>
        /// <param name="runner">登录用户</param>
        /// <returns></returns>
        protected ActivityInstanceEntity CreateBackwardToActivityInstanceObject(ProcessInstanceEntity processInstance,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            int backOrgActivityInstanceID,
            WfAppRunner runner)
        {
            ActivityInstanceEntity entity = ActivityInstanceManager.CreateBackwardActivityInstanceObject(
                processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.AppInstanceCode,
                processInstance.ID,
                this.BackwardContext.BackwardToTaskActivity,
                backwardType,
                backSrcActivityInstanceID,
                backOrgActivityInstanceID,
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
        /// <returns>新转移实例ID</returns>
        internal virtual int InsertTransitionInstance(ProcessInstanceEntity processInstance,
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
            var newID = tim.Insert(session.Connection, transitionInstanceObject, session.Transaction);

            return newID;
        }

        /// <summary>
        /// 生成活动用户分配信息
        /// </summary>
        /// <param name="toActivityInstance">下一步活动实例</param>
        /// <param name="activityResource">活动资源</param>
        /// <returns>下一步活动实例</returns>
        protected ActivityInstanceEntity GenerateActivityAssignedUserInfo(ActivityInstanceEntity toActivityInstance,
            ActivityResource activityResource)
        {
            if (activityResource.AppRunner.NextPerformerType == NextPerformerIntTypeEnum.Specific
                && activityResource.NextActivityPerformers != null)
            {
                //前端显式指定下一步骤的执行用户列表
                toActivityInstance.AssignedToUserIDs = PerformerBuilder.GenerateActivityAssignedUserIDs(
                    activityResource.NextActivityPerformers[toActivityInstance.ActivityGUID]);
                toActivityInstance.AssignedToUserNames = PerformerBuilder.GenerateActivityAssignedUserNames(
                    activityResource.NextActivityPerformers[toActivityInstance.ActivityGUID]);
            }
            else if (activityResource.AppRunner.NextPerformerType ==  NextPerformerIntTypeEnum.Definition)
            {
                //根据节点上的角色定义获取下一步骤的执行用户列表
                var performers = ActivityForwardContext.ProcessModel.GetActivityPerformers(toActivityInstance.ActivityGUID);
                activityResource.NextActivityPerformers = performers;

                toActivityInstance.AssignedToUserIDs = PerformerBuilder.GenerateActivityAssignedUserIDs(
                    performers[toActivityInstance.ActivityGUID]);
                toActivityInstance.AssignedToUserNames = PerformerBuilder.GenerateActivityAssignedUserNames(
                    performers[toActivityInstance.ActivityGUID]);
            }
            else if (activityResource.AppRunner.NextPerformerType == NextPerformerIntTypeEnum.Single)
            {
                //用于测试使用的单一用户列表
                activityResource.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(toActivityInstance.ActivityGUID,
                    activityResource.AppRunner.UserID,
                    activityResource.AppRunner.UserName);

                toActivityInstance.AssignedToUserIDs = activityResource.AppRunner.UserID;
                toActivityInstance.AssignedToUserNames = activityResource.AppRunner.UserName;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("nodemediator.generateactivityassigneduserinfo.warn"));
            }
            return toActivityInstance;
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
                message = LocalizeHelper.GetEngineMessage("nodemediator.getnodemediatedmessage.next.warn");
            }
            else if (WfNodeMediatedResult.Feedback == WfNodeMediatedFeedback.WaitingForCompletedMore)
            {
                message = LocalizeHelper.GetEngineMessage("nodemediator.GetNodeMediatedMessage.waiting.warn");
            }
            else if (WfNodeMediatedResult.Feedback == WfNodeMediatedFeedback.NotEnoughApprovalBranchesCount)
            {
                message = LocalizeHelper.GetEngineMessage("nodemediator.GetNodeMediatedMessage.notenoughapproval.warn");
            }
            return message;
        }
    }
}
