using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Essential;

namespace Slickflow.Engine.Core.Pattern.Event
{
    /// <summary>
    /// Event node mediator
    /// </summary>
    internal class NodeMediatorEvent
    {
        #region Property and constructor

        private ActivityForwardContext _activityFowardContext;
        internal ActivityForwardContext ActivityForwardContext
        {
            get { return _activityFowardContext; }
           
        }

        private Activity _eventActivity;
        internal Activity EventActivity
        {
            get { return _eventActivity; }
        }

        private IProcessModel _processModel;
        internal IProcessModel ProcessModel
        {
            get { return _processModel; }
        }

        private IDbSession _session;
        internal IDbSession Session
        {
            get { return _session; }
        }

        internal ActivityInstanceEntity EventActivityInstance
        {
            get;
            set;
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

        /// <summary>
        /// Constructor function
        /// </summary>
        internal NodeMediatorEvent(ActivityForwardContext forwardContext,
            Activity eActivity, 
            IProcessModel processModel, 
            IDbSession session)
        {
            _activityFowardContext = forwardContext;
            _eventActivity = eActivity;
            _processModel = processModel;
            _session = session;
        }
        #endregion

        internal virtual void ExecuteWorkItem() { }

        /// <summary>
        /// Execute action list
        /// </summary>
        /// <param name="actionList"></param>
        /// <param name="delegateService"></param>
        protected void ExecteActionList(IList<Xpdl.Entity.Action> actionList,
            IDelegateService delegateService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                ActionExecutor.ExecuteActionList(actionList, delegateService);
            }
        }

        /// <summary>
        /// Get delegate sevice
        /// </summary>
        /// <returns></returns>
        private DelegateServiceBase GetDelegateService()
        {
            //执行Action列表
            var delegateContext = new DelegateContext
            {
                AppInstanceID = ActivityForwardContext.ProcessInstance.AppInstanceID,
                ProcessID = ActivityForwardContext.ProcessInstance.ProcessID,
                ProcessInstanceID = ActivityForwardContext.ProcessInstance.ID,
                ActivityID = ActivityForwardContext.FromActivityInstance.ActivityID,
                ActivityName = ActivityForwardContext.FromActivityInstance.ActivityName
            };

            var delegateService = DelegateServiceFactory.CreateDelegateService(DelegateScopeTypeEnum.Activity,
                this.Session,
                delegateContext);
            return delegateService;
        }

        /// <summary>
        /// On before execute work item
        /// </summary>
        protected void OnBeforeExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = this.EventActivity.ActionList;
            ActionExecutor.ExecuteActionListBefore(actionList, delegateService as IDelegateService);

            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuting,
                this.EventActivity,
                ActivityForwardContext);
        }

        /// <summary>
        /// On executing service item
        /// </summary>
        protected void OnExecutingServiceItem()
        {
            var delegateService = GetDelegateService();
            var serviceList = this.EventActivity.ServiceList;
            ServiceExecutor.ExecuteServiceList(serviceList, delegateService as IDelegateService);
        }

        /// <summary>
        /// On after execute work item
        /// </summary>
        protected void OnAfterExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = this.EventActivity.ActionList;
            ActionExecutor.ExecuteActionListAfter(actionList, delegateService as IDelegateService);

            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuted,
                this.EventActivity,
                ActivityForwardContext);
        }

        /// <summary>
        /// Create activity instance object
        /// 创建节点对象
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="processInstance"></param>
        /// <param name="runner"></param>
        protected ActivityInstanceEntity CreateActivityInstanceObject(Activity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceManager aim = new ActivityInstanceManager();
            this.EventActivityInstance = aim.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.AppInstanceCode,
                processInstance.ProcessID,
                processInstance.ID,
                activity,
                runner);

            return this.EventActivityInstance;
        }

        /// <summary>
        /// Insert activity instance
        /// 插入实例数据
        /// </summary>
        /// <param name="activityInstance"></param>
        /// <param name="session"></param>
        internal virtual void InsertActivityInstance(ActivityInstanceEntity activityInstance,
            IDbSession session)
        {
            ActivityInstanceManager.Insert(activityInstance, session);
        }

        /// <summary>
        /// Insert transition instance
        /// 插入连线实例的方法
        /// </summary>
        internal virtual int InsertTransitionInstance(ProcessInstanceEntity processInstance,
            string transitionGUID,
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
        /// Complete activity instance
        /// 节点对象的完成方法
        /// </summary>
        /// <param name="ActivityInstanceID"></param>
        /// <param name="runner"></param>
        /// <param name="session"></param>
        internal virtual void CompleteActivityInstance(int ActivityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            ActivityInstanceManager.Complete(ActivityInstanceID,
                runner,
                session);
        }
    }
}
