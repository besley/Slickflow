
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
using Slickflow.Engine.Core.Pattern.Event.Message;
using Slickflow.Engine.Core.Pattern.Event.Timer;
using Slickflow.Engine.Core.Pattern.Event.Conditional;
using Slickflow.Engine.Core.Pattern.Event.Signal;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Mediator Abstract Class
    /// 节点执行器的抽象类
    /// </summary>
    internal abstract class NodeMediator
    {
        #region Property

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

        private LinkContext _linkContext;
        internal LinkContext LinkContext
        {
            get
            {
                if (_linkContext == null)
                    _linkContext = new LinkContext();

                return _linkContext;
            }
        }

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
            set
            {
                _wfNodeMediatedResult = value;
            }
        }

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

        private DelegateServiceBase _delegateService;
        #endregion

        #region Abstract Method
        internal abstract void ExecuteWorkItem();
        #endregion

        #region Constructor Function
        internal NodeMediator(ActivityForwardContext forwardContext, IDbSession session)
        {
            _activityForwardContext = forwardContext;
            _session = session;
            LinkContext.FromActivity = forwardContext.Activity;
        }

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

        #region Execute Method
        /// <summary>
        /// Get Delegate Servide
        /// </summary>
        /// <returns></returns>
        private DelegateServiceBase GetDelegateService()
        {
            if (_delegateService == null)
            {
                int? fromActivityInstanceID = null;
                string fromActivityID = string.Empty;
                string fromActivityName = string.Empty;
                if (IsNotNodeMediatorStart(this) == true)
                {
                    fromActivityInstanceID = ActivityForwardContext.FromActivityInstance.ID;
                    fromActivityID = ActivityForwardContext.FromActivityInstance.ActivityID;
                    fromActivityName = ActivityForwardContext.FromActivityInstance.ActivityName;
                }

                var delegateContext = new DelegateContext
                {
                    AppInstanceID = ActivityForwardContext.ProcessInstance.AppInstanceID,
                    ProcessID = ActivityForwardContext.ProcessInstance.ProcessID,
                    ProcessInstanceID = ActivityForwardContext.ProcessInstance.ID,
                    ActivityID = fromActivityID,
                    ActivityName = fromActivityName
                };
                _delegateService = DelegateServiceFactory.CreateDelegateService(DelegateScopeTypeEnum.Activity,
                   this.Session,
                   delegateContext);
            }
            return _delegateService;
        }

        /// <summary>
        /// Determine whether it is a starting type node
        /// 判断是否是起始类型的节点
        /// </summary>
        /// <param name="nodeMediator"></param>
        /// <returns></returns>
        private bool IsNotNodeMediatorStart(NodeMediator nodeMediator)
        {
            if (!(nodeMediator is NodeMediatorStart)
                && !(nodeMediator is NodeMediatorStartMsgCatch)
                && !(nodeMediator is NodeMediatorStartMsgThrow)
                && !(nodeMediator is NodeMediatorStartSignalCatch)
                && !(nodeMediator is NodeMediatorStartSignalThrow)
                && !(nodeMediator is NodeMediatorStartTimer)
                && !(nodeMediator is NodeMediatorStartConditional))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// On before execute work item
        /// 触发前执行
        /// </summary>
        protected void OnBeforeExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = LinkContext.FromActivity.ActionList;
            ActionExecutor.ExecuteActionListBefore(actionList, delegateService as IDelegateService);

            //----> 节点流转前，调用活动执行的委托事件
            //Before node flow, invoke the delegation event of activity execution
            var currentActivity = LinkContext.ToActivity != null ? LinkContext.ToActivity : LinkContext.FromActivity;
            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuting,
                currentActivity,
                ActivityForwardContext);
        }

        /// <summary>
        /// On after execute work item
        /// 触发后执行
        /// </summary>
        protected void OnAfterExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = ((this is NodeMediatorEnd) || (this is NodeMediatorIntermediate)) ? LinkContext.ToActivity.ActionList : LinkContext.FromActivity.ActionList;
            ActionExecutor.ExecuteActionListAfter(actionList, delegateService as IDelegateService);

            //----> 节点流转完成后，调用活动完成执行的委托事件
            //After the node flow is completed, invoke the delegation event that the activity completes execution
            var currentActivity = LinkContext.ToActivity != null ? LinkContext.ToActivity : LinkContext.FromActivity;
            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuted,
                currentActivity,
                ActivityForwardContext);
        }

        /// <summary>
        /// Execute code to automatically serve content
        /// 执行代码自动服务内容
        /// </summary>
        protected void OnExecutingServiceItem()
        {
            var delegateService = GetDelegateService();
            var serviceList = LinkContext.ToActivity.ServiceList;
            ServiceExecutor.ExecuteServiceList(serviceList, delegateService as IDelegateService);
        }

        /// <summary>
        /// Executing script item
        /// </summary>
        protected void OnExecutingScriptItem()
        {
            var delegateService = GetDelegateService();
            var scriptList = LinkContext.ToActivity.ScriptList;
            ScriptExecutor.ExecuteScriptList(scriptList, delegateService as IDelegateService);
        }
        #endregion

        #region Process Running Logical
        /// <summary>
        /// Traverse and execute the nodes following the current node
        /// Normal operation: Transition parsing flow is required
        /// Other methods are directly specified, such as jump, return, etc., 
        /// which do not require parsing and cannot be equated with normal flow parsing
        /// 遍历执行当前节点后面的节点
        /// 正常运行：需要走Transition的解析流转
        /// 其它方式为直接指定：比如跳转，返送等，不需要解析，不能等同于正常流转解析
        /// </summary>
        /// <param name="isNotParsedByTransition"></param>
        /// <param name="session"></param>
        internal void ContinueForwardCurrentNode(bool isNotParsedByTransition, IDbSession session)
        {
            try
            {
                var mediatorResult = new List<WfNodeMediatedResult>();
                var activityResource = ActivityForwardContext.ActivityResource;

                if (isNotParsedByTransition == true)
                {
                    //跳转模式时，直接流转运行
                    //Jump style
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
                    //Normal running
                    var nextActivityMatchedResult = this.ActivityForwardContext.ProcessModel.GetNextActivityList(
                        this.LinkContext.FromActivityInstance.ActivityID,
                        ActivityForwardContext.TaskID,
                        activityResource.ConditionKeyValuePair,
                        activityResource,
                        (a, b) => a.NextActivityPerformers.ContainsKey(b.ActivityID),
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
        /// Recursive execution node
        /// 1) Task of creating regular nodes
        /// 2) Task of creating a countersignature node
        /// 递归执行节点
        /// 1)创建普通节点的任务
        /// 2)创建会签节点的任务
        /// </summary>
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
                        //This node type is a task node: Determine whether a task can be created based on the type of from Activity Instance
                        if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                        {
                            //此节点类型为分支或合并节点类型：首先需要实例化当前节点(自动完成)
                            //This node type is a branch or merge node type: First, the current node needs to be instantiated (auto complete)
                            NodeMediatorGateway gatewayNodeMediator = NodeMediatorFactory.CreateNodeMediatorGateway(comp.Activity,
                            this.ActivityForwardContext.ProcessModel,
                            Session);

                            ICompleteGatewayAutomaticlly autoGateway = (ICompleteGatewayAutomaticlly)gatewayNodeMediator;
                            nodeExecutedResult = autoGateway.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                                comp.Transition.TransitionID,
                                fromActivity,
                                fromActivityInstance,
                                ActivityForwardContext.ActivityResource.AppRunner,
                                Session);

                            if (nodeExecutedResult.Status == NodeAutoExecutedStatus.Successed)
                            {
                                //遍历后续子节点
                                //Traverse subsequent child nodes
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
                            //The next task node has not been created, and a prompt message is required
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
                        //Intermediate event type node, calling external business logic,
                        //and then the process continues to flow downwards
                        NodeMediator eventNodeMediator = NodeMediatorFactory.CreateNodeMediatorEvent(ActivityForwardContext,
                            comp.Activity,
                            ActivityForwardContext.ProcessModel,
                            Session);
                        eventNodeMediator.LinkContext.FromActivity = fromActivity;
                        eventNodeMediator.LinkContext.ToActivity = comp.Activity;
                        eventNodeMediator.ExecuteWorkItem();

                        ICompleteAutomaticlly autoEvent = (ICompleteAutomaticlly)eventNodeMediator;
                        nodeExecutedResult = autoEvent.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                            comp.Transition.TransitionID,
                            fromActivity,
                            fromActivityInstance,
                            comp.Activity,
                            ActivityForwardContext.ActivityResource.AppRunner,
                            Session);

                        if (nodeExecutedResult.Status == NodeAutoExecutedStatus.Successed)
                        {
                            //遍历后续子节点
                            //Traverse subsequent child nodes
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
                else if (comp.Activity.ActivityType == ActivityTypeEnum.TaskNode)                  
                {
                    //此节点类型为任务节点：根据fromActivityInstance的类型判断是否可以创建任务
                    //This node type is a task node: Determine whether a task can be created based on the type of from Activity Instance
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //创建新任务节点
                        //Create new task node
                        NodeMediator taskNodeMediator = new NodeMediatorTask(Session);
                        taskNodeMediator.CreateActivityTaskTransitionInstance(comp.Activity,
                            ActivityForwardContext.ProcessInstance,
                            fromActivityInstance,
                            comp.Transition.TransitionID,
                            comp.Transition.DirectionType == TransitionDirectionTypeEnum.Loop ?
                                TransitionTypeEnum.Loop : TransitionTypeEnum.Forward, 
                            isNotParsedForward == true ?
                                TransitionFlyingTypeEnum.ForwardFlying : TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                    else
                    {
                        //下一步的任务节点没有创建，需给出提示信息
                        //The next task node has not been created, and a prompt message is required
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
                    //Service type node, calling external business logic, and then the process continues to flow downwards
                    NodeAutoExecutedResult serviceExecutedResult = null;
                    NodeMediator serviceNodeMediator = NodeMediatorFactory.CreateNodeMediatorEvent(ActivityForwardContext,
                        comp.Activity,
                        ActivityForwardContext.ProcessModel,
                        Session);
                    serviceNodeMediator.ExecuteWorkItem();

                    ICompleteAutomaticlly autoEvent = (ICompleteAutomaticlly)serviceNodeMediator;
                    serviceExecutedResult = autoEvent.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                        comp.Transition.TransitionID,
                        fromActivity,
                        fromActivityInstance,
                        comp.Activity,
                        ActivityForwardContext.ActivityResource.AppRunner,
                        Session);

                    if (serviceExecutedResult.Status == NodeAutoExecutedStatus.Successed)
                    {
                        //遍历后续子节点
                        //Traverse subsequent child nodes
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
                    //Script type node, calling script program, and then the process continues to flow downwards
                    NodeAutoExecutedResult scriptExecutedResult = null;
                    NodeMediator scriptNodeMediator = NodeMediatorFactory.CreateNodeMediatorEvent(ActivityForwardContext,
                        comp.Activity,
                        ActivityForwardContext.ProcessModel,
                        Session);
                    scriptNodeMediator.ExecuteWorkItem();

                    ICompleteAutomaticlly autoEvent = (ICompleteAutomaticlly)scriptNodeMediator;
                    scriptExecutedResult = autoEvent.CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                        comp.Transition.TransitionID,
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
                else if (comp.Activity.ActivityType == ActivityTypeEnum.MultiSignNode)      
                {
                    //此节点类型为任务节点：根据fromActivityInstance的类型判断是否可以创建任务
                    //This node type is a task node: Determine whether a task can be created based on the type of from Activity Instance
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //创建新多实例节点
                        NodeMediator mediatorMICreator = new NodeMediatorMultiSignCreator(Session);
                        mediatorMICreator.CreateActivityTaskTransitionInstance(comp.Activity,
                            ActivityForwardContext.ProcessInstance,
                            fromActivityInstance,
                            comp.Transition.TransitionID,
                            comp.Transition.DirectionType == TransitionDirectionTypeEnum.Loop ?
                                TransitionTypeEnum.Loop : TransitionTypeEnum.Forward, 
                            isNotParsedForward == true ?
                                TransitionFlyingTypeEnum.ForwardFlying : TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                    else
                    {
                        //下一步的任务节点没有创建，需给出提示信息
                        ////The next task node has not been created, and a prompt message is required
                        if (IsWaitingOneOfJoin(fromActivity.GatewayDetail) == true)
                        {
                            mediatorResult.Add(WfNodeMediatedResult.CreateNodeMediatedResultWithException(
                                WfNodeMediatedFeedback.NeedOtherGatewayBranchesToJoin));
                            LogManager.RecordLog("ContinueForwardCurrentNodeRecurisivlyExeception",
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
                            LogManager.RecordLog("ContinueForwardCurrentNodeRecurisivlyExeception",
                                LogEventType.Exception,
                                LogPriority.Normal,
                                null,
                                new WfRuntimeException(LocalizeHelper.GetEngineMessage("nodemediator.ContinueForwardCurrentNodeRecurisivly.otherreason.warn"))
                                );
                        }
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.SubProcessNode)         
                {
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        var subNodeMediator = NodeMediatorFactory.CreateNodeMediatorSubProcess(comp.Activity, this.Session);
                        subNodeMediator.CreateActivityTaskTransitionInstance(comp.Activity,
                            ActivityForwardContext.ProcessInstance,
                            fromActivityInstance,
                            comp.Transition.TransitionID,
                            comp.Transition.DirectionType == TransitionDirectionTypeEnum.Loop ?
                                TransitionTypeEnum.Loop : TransitionTypeEnum.Forward,
                            TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);
                    }
                }
                else if (comp.Activity.ActivityType == ActivityTypeEnum.EndNode)        
                {
                    if (fromActivityInstance.ActivityState == (short)ActivityStateEnum.Completed)
                    {
                        //此节点为完成结束节点，结束流程
                        //This node is the completion end node, ending the process
                        var endMediator = NodeMediatorFactory.CreateNodeMediatorEnd(ActivityForwardContext, comp.Activity, Session);
                        endMediator.LinkContext.ToActivity = comp.Activity;

                        //创建结束节点实例及转移数据
                        //Create end node instance and transitioni data
                        endMediator.CreateActivityTaskTransitionInstance(comp.Activity, ActivityForwardContext.ProcessInstance,
                            fromActivityInstance, comp.Transition.TransitionID, TransitionTypeEnum.Forward,
                            TransitionFlyingTypeEnum.NotFlying,
                            ActivityForwardContext.ActivityResource,
                            Session);

                        //执行结束节点中的业务逻辑
                        //Create end node instance and transfer data
                        endMediator.ExecuteWorkItem();
                    }
                    else
                    {
                        //结束节点没有创建，需给出提示信息
                        //End node not created, prompt message is required
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
        /// Determine whether it is a merger situation
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
        /// Create activity task transtion instance
        /// 创建工作项及转移数据
        /// </summary>
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
        /// Virtual method for creating tasks
        /// 1.  For automatically executed work items, there is no need to rewrite this method
        /// 2.  For manually executed work items, the method needs to be rewritten to insert pending task data
        /// 创建任务的虚方法
        /// 1. 对于自动执行的工作项，无需重写该方法
        /// 2. 对于人工执行的工作项，需要重写该方法，插入待办的任务数据
        /// </summary>
        /// <param name="activityResource"></param>
        /// <param name="toActivityInstance"></param>
        /// <param name="session"></param>
        internal virtual void CreateNewTask(ActivityInstanceEntity toActivityInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            if (activityResource.NextActivityPerformers == null)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodemediator.createnewtask.warn"));
            }

            TaskManager.Insert(toActivityInstance,
                activityResource.NextActivityPerformers[toActivityInstance.ActivityID],
                activityResource.AppRunner,
                session);
        }

        /// <summary>
        /// Create activity instance object
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="activity"></param>
        /// <param name="runner"></param>
        protected ActivityInstanceEntity CreateActivityInstanceObject(Activity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceEntity entity = ActivityInstanceManager.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.AppInstanceCode,
                processInstance.ProcessID,
                processInstance.ID,
                activity,
                runner);

            return entity;
        }

        /// <summary>
        /// Insert activity instance
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="session"></param>
        protected virtual int InsertActivityInstance(ActivityInstanceEntity activityInstance,
            IDbSession session)
        {
            return ActivityInstanceManager.Insert(activityInstance, session);
        }


        /// <summary>
        /// Complete activity instance
        /// </summary>
        /// <param name="ActivityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        protected virtual void CompleteActivityInstance(int ActivityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            ActivityInstanceManager.Complete(ActivityInstanceID,
                runner,
                session);
        }

        /// <summary>
        /// Master node of countersignature type, handling multiple instance nodes
        /// Create the master node for the countersignature node and record the instance child nodes under the countersignature master node
        /// 会签类型的主节点, 多实例节点处理
        /// 创建会签节点的主节点，以及会签主节点下的实例子节点记录
        /// </summary>
        internal void CreateMultipleInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            String transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var toActivityInstance = CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            toActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
            toActivityInstance.ComplexType = (short)ComplexTypeEnum.SignTogether;

            toActivityInstance.MergeType = (short)toActivity.MultiSignDetail.MergeType;
            toActivityInstance.CompareType = (short)toActivity.MultiSignDetail.CompareType;
            toActivityInstance.CompleteOrder = toActivity.MultiSignDetail.CompleteOrder;

            toActivityInstance.AssignedToUserIDs = PerformerBuilder.GenerateActivityAssignedUserIDs(
                activityResource.NextActivityPerformers[toActivity.ActivityID]);
            toActivityInstance.AssignedToUserNames = PerformerBuilder.GenerateActivityAssignedUserNames(
                activityResource.NextActivityPerformers[toActivity.ActivityID]);

            ActivityInstanceManager.Insert(toActivityInstance, session);
            InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);

            //插入会签子节点实例数据
            //Insert signature sub node instance data
            var plist = activityResource.NextActivityPerformers[toActivity.ActivityID];
            ActivityInstanceEntity entity = new ActivityInstanceEntity();
            for (short i = 0; i < plist.Count; i++)
            {
                entity = ActivityInstanceManager.CreateActivityInstanceObject(toActivityInstance);
                entity.AssignedToUserIDs = plist[i].UserID;
                entity.AssignedToUserNames = plist[i].UserName;
                entity.MIHostActivityInstanceID = toActivityInstance.ID;

                //并行串行下，多实例子节点的执行顺序设置
                //Setting the Execution Order of Multiple Real Example Nodes in Parallel Serial
                if (toActivityInstance.MergeType == (short)MergeTypeEnum.Sequence)
                {
                    entity.CompleteOrder = (short)(i + 1);
                }
                else if (toActivityInstance.MergeType == (short)MergeTypeEnum.Parallel)
                {
                    //并行模式下CompleteOrder的优先级一样，所以置为 -1
                    //In parallel mode, the priority of Completed Order is the same, so it is set to -1
                    entity.CompleteOrder = -1;       
                }

                //如果是串行会签，只有第一个节点处于运行状态，其它节点挂起
                //If it is a serial countersignature, only the first node is running, and the other nodes are suspended
                if ((i > 0) && (toActivity.MultiSignDetail.MergeType == MergeTypeEnum.Sequence))
                {
                    entity.ActivityState = (short)ActivityStateEnum.Suspended;
                }

                entity.ID = ActivityInstanceManager.Insert(entity, session);
                TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);
            }
        }

        /// <summary>
        /// Create backward activity instance object
        /// 创建退回类型的活动实例对象
        /// </summary>
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
                processInstance.ProcessID,
                this.BackwardContext.BackwardToTaskActivity,
                backwardType,
                backSrcActivityInstanceID,
                backOrgActivityInstanceID,
                runner);

            return entity;
        }

        /// <summary>
        /// Insert transition instance
        /// </summary>
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
        /// Generate activity user allocation information
        /// 生成活动用户分配信息
        /// </summary>
        /// <param name="toActivityInstance"></param>
        /// <param name="activityResource"></param>
        /// <returns></returns>
        protected ActivityInstanceEntity GenerateActivityAssignedUserInfo(ActivityInstanceEntity toActivityInstance,
            ActivityResource activityResource)
        {
            if (activityResource.AppRunner.NextPerformerType == NextPerformerIntTypeEnum.Specific
                && activityResource.NextActivityPerformers != null)
            {
                //前端显式指定下一步骤的执行用户列表
                //The front-end explicitly specifies the list of executing users for the next step
                toActivityInstance.AssignedToUserIDs = PerformerBuilder.GenerateActivityAssignedUserIDs(
                    activityResource.NextActivityPerformers[toActivityInstance.ActivityID]);
                toActivityInstance.AssignedToUserNames = PerformerBuilder.GenerateActivityAssignedUserNames(
                    activityResource.NextActivityPerformers[toActivityInstance.ActivityID]);
            }
            else if (activityResource.AppRunner.NextPerformerType ==  NextPerformerIntTypeEnum.Definition)
            {
                //根据节点上的角色定义获取下一步骤的执行用户列表
                //Obtain the list of executing users for the next step based on the role definition on the node
                var performers = ActivityForwardContext.ProcessModel.GetActivityPerformers(toActivityInstance.ActivityID);
                activityResource.NextActivityPerformers = performers;

                toActivityInstance.AssignedToUserIDs = PerformerBuilder.GenerateActivityAssignedUserIDs(
                    performers[toActivityInstance.ActivityID]);
                toActivityInstance.AssignedToUserNames = PerformerBuilder.GenerateActivityAssignedUserNames(
                    performers[toActivityInstance.ActivityID]);
            }
            else if (activityResource.AppRunner.NextPerformerType == NextPerformerIntTypeEnum.Single)
            {
                //用于测试使用的单一用户列表
                //Single user list for testing purposes
                activityResource.NextActivityPerformers = ActivityResource.CreateNextActivityPerformers(toActivityInstance.ActivityID,
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
        /// Generate PerformerList data structure based on personnel information assigned by nodes
        /// 由节点分配的人员信息生成PerformerList数据结构
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <returns></returns>
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
        /// Generate messages based on the type of node execution result
        /// 根据节点执行结果类型，生成消息
        /// </summary>
        /// <returns></returns>
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
