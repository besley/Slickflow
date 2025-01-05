using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Gateway
{
    /// <summary>
    /// Gatewayt Node Mediator
    /// 网关控制节点执行器
    /// </summary>
    internal class NodeMediatorGateway
    {
        #region Property and constructor
        private Activity _gatewayActivity;
        internal Activity GatewayActivity
        {
            get { return _gatewayActivity; }
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

        internal ActivityInstanceEntity GatewayActivityInstance
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
        
        internal NodeMediatorGateway(Activity gActivity, IProcessModel processModel, IDbSession session)
        {
            _gatewayActivity = gActivity;
            _processModel = processModel;
            _session = session;
        }
        #endregion

        /// <summary>
        /// Create activity instance
        /// </summary>
        protected ActivityInstanceEntity CreateActivityInstanceObject(Activity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceManager aim = new ActivityInstanceManager();
            this.GatewayActivityInstance = aim.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.AppInstanceCode,
                processInstance.ProcessGUID,
                processInstance.ID,
                activity,
                runner);

            return this.GatewayActivityInstance;
        }

        /// <summary>
        /// Insert activity instance
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
        /// Compelete activity instance
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
