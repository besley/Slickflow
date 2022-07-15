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
    /// 节点退回器
    /// </summary>
    internal abstract class NodeSendBack
    {
        #region 属性及构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sendbackOperation">退回选项</param>
        /// <param name="session">数据会话</param>
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

        #region 抽象方法
        internal abstract void Execute();
        #endregion

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
                this.SendBackOperation.BackwardToTaskActivity,
                backwardType,
                backSrcActivityInstanceID,
                backOrgActivityInstanceID,
                runner);

            return entity;
        }

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
                throw new WorkflowException(LocalizeHelper.GetEngineMessage("nodesendback.ExecuteInstanceImp.error"));
            }

            TaskManager.Insert(toActivityInstance,
                activityResource.NextActivityPerformers[toActivityInstance.ActivityGUID],
                activityResource.AppRunner,
                session);
        }

        /// <summary>
        /// 插入连线实例的方法
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="toActivityInstance">到达活动实例</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="runner">执行者</param>
        /// <param name="session">Session</param>
        /// <returns>新转移实例ID</returns>
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
        /// 创建退回时的流转节点对象、任务和转移数据
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">运行节点实例</param>
        /// <param name="backwardToTaskActivity">退回到的节点</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
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

            //实例化Activity
            var toActivityInstance = this.ActivityInstanceManager.CreateBackwardActivityInstanceObject(
               processInstance.AppName,
               processInstance.AppInstanceID,
               processInstance.AppInstanceCode,
               processInstance.ID,
               backwardToTaskActivity,
               backwardType,
               fromActivityInstance.ID,
               previousActivityInstance.ID,
               activityResource.AppRunner);

            //进入准备运行状态
            toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            //人员来自步骤列表的用户数据
            toActivityInstance.AssignedToUserIDs = SendBackOperation.BackwardToTaskPerformer.UserID;
            toActivityInstance.AssignedToUserNames = SendBackOperation.BackwardToTaskPerformer.UserName;

            //插入活动实例数据
            this.ActivityInstanceManager.Insert(toActivityInstance,
                session);

            //插入任务数据
            this.TaskManager.Insert(toActivityInstance,
                SendBackOperation.BackwardToTaskPerformer,
                activityResource.AppRunner,
                session);

            //插入转移数据
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
