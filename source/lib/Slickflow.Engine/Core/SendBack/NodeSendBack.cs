using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.SendBack
{
    /// <summary>
    /// Node Send Back
    /// 节点退回器
    /// </summary>
    internal abstract class NodeSendBack
    {
        #region Property and Constructor
        internal NodeSendBack(SendBackOperation sendbackOperation, IDbSession session)
        {
            _sendBackOperation = sendbackOperation;
            _session = session;
            _activityInstanceManager = new ActivityInstanceManager();
            _taskManager = new TaskManager();
            _transitionInstanceManager = new TransitionInstanceManager();
        }

        private IDbSession _session;
        protected IDbSession Session {  get { return _session; } }

        private SendBackOperation _sendBackOperation;
        protected SendBackOperation SendBackOperation { get { return _sendBackOperation; } }

        private ActivityInstanceManager _activityInstanceManager;
        protected ActivityInstanceManager ActivityInstanceManager { get { return _activityInstanceManager; } }

        private TaskManager _taskManager;
        protected TaskManager TaskManager { get { return _taskManager; } }

        private TransitionInstanceManager _transitionInstanceManager;
        protected TransitionInstanceManager TransitionInstanceManager {  get { return _transitionInstanceManager; } }
        #endregion

        #region Abstract Method
        internal abstract void Execute();
        #endregion

        /// <summary>
        /// Create Backward activity instance
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
                processInstance.ProcessGUID,
                this.SendBackOperation.BackwardToTaskActivity,
                backwardType,
                backSrcActivityInstanceID,
                backOrgActivityInstanceID,
                runner);

            return entity;
        }

        /// <summary>
        /// Virtual method for creating tasks
        /// 1.  For automatically executed work items, there is no need to rewrite this method
        /// 2.  For manually executed work items, the method needs to be rewritten to insert pending task data
        /// 创建任务的虚方法
        /// 1. 对于自动执行的工作项，无需重写该方法
        /// 2. 对于人工执行的工作项，需要重写该方法，插入待办的任务数据
        /// </summary>
        internal virtual void CreateNewTask(ActivityInstanceEntity toActivityInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            if (activityResource.NextActivityPerformers == null)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodesendback.ExecuteInstanceImp.error"));
            }

            TaskManager.Insert(toActivityInstance,
                activityResource.NextActivityPerformers[toActivityInstance.ActivityGUID],
                activityResource.AppRunner,
                session);
        }

        /// <summary>
        /// Insert transition instance
        /// 插入连线实例的方法
        /// </summary>
        internal virtual int InsertTransitionInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity toActivityInstance,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            WfAppRunner runner,
            IDbSession session)
        {
            var transition = SendBackOperation.ProcessModel.GetForwardTransition(toActivityInstance.ActivityGUID,
                fromActivityInstance.ActivityGUID);
            var transitionGUID = transition != null ? transition.TransitionGUID : WfDefine.WF_XPDL_SEND_BACK_UNKNOWN_GUID;
            var transitionInstanceObject = this.TransitionInstanceManager.CreateTransitionInstanceObject(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                runner,
                (byte)ConditionParseResultEnum.Passed);
            var newID = this.TransitionInstanceManager.Insert(session.Connection, transitionInstanceObject, session.Transaction);

            return newID;
        }

        /// <summary>
        /// Create backward activity task transition instance
        /// 创建退回时的流转节点对象、任务和转移数据
        /// </summary>
        internal void CreateBackwardActivityTaskTransitionInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            Activity backwardToTaskActivity,
            BackwardTypeEnum backwardType,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var previousActivityInstance = this.ActivityInstanceManager.GetPreviousActivityInstanceSimple(fromActivityInstance, 
                backwardToTaskActivity.ActivityGUID, 
                session);

            if (previousActivityInstance == null)
            {
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodesendback.CreateBackwardActivityTaskTransitionInstance.error"));
            }

            var toActivityInstance = this.ActivityInstanceManager.CreateBackwardActivityInstanceObject(
               processInstance.AppName,
               processInstance.AppInstanceID,
               processInstance.AppInstanceCode,
               processInstance.ID,
               processInstance.ProcessGUID,
               backwardToTaskActivity,
               backwardType,
               fromActivityInstance.ID,
               previousActivityInstance.ID,
               activityResource.AppRunner);

            toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            //人员来自步骤列表的用户数据
            //User data of personnel from the step list
            toActivityInstance.AssignedToUserIDs = SendBackOperation.BackwardToTaskPerformer.UserID;
            toActivityInstance.AssignedToUserNames = SendBackOperation.BackwardToTaskPerformer.UserName;

            this.ActivityInstanceManager.Insert(toActivityInstance,
                session);

            this.TaskManager.Insert(toActivityInstance,
                SendBackOperation.BackwardToTaskPerformer,
                activityResource.AppRunner,
                session);

            InsertTransitionInstance(processInstance,
                fromActivityInstance,
                toActivityInstance,
                TransitionTypeEnum.Backward,
                TransitionFlyingTypeEnum.NotFlying,
                this.SendBackOperation.ActivityResource.AppRunner,
                session);
        }
    }
}
