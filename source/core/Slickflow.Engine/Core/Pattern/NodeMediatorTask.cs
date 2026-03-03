using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Event;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Mediator Task
    /// ����ڵ�ִ����
    /// </summary>
    internal class NodeMediatorTask : NodeMediator
    {
        internal NodeMediatorTask(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorTask(IDbSession session)
            : base(session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                OnBeforeExecuteWorkItem();

                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskId,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                OnAfterExecuteWorkItem();

                //��ȡ��һ���ڵ��б���������ִ��
                //Get the next node list: and continue execution
                if (canContinueForwardCurrentNode)
                {
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, this.Session);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Complete work item
        /// ��ɽڵ�ʵ��
        /// </summary>   
        internal bool CompleteWorkItem(int? taskId,
            ActivityResource activityResource,
            IDbSession session)
        {
            WfAppRunner runner = new WfAppRunner
            {
                UserId = activityResource.AppRunner.UserId,         
                UserName = activityResource.AppRunner.UserName
            };

            if (taskId != null)
            {
                //��ɱ����񣬷��������Ѿ�ת�Ƶ���һ����ǩ���񣬲�����ִ�������ڵ�
                //Complete this task, return that the task has been transferred to the next co signing task,
                //and do not continue to execute other nodes
                base.TaskManager.Complete(taskId.Value, activityResource.AppRunner, session);
            }

            //���û�ڵ��״̬Ϊ���״̬
            //Set the status of the activity node to complete status
            base.ActivityInstanceManager.Complete(base.LinkContext.FromActivityInstance.Id,
                activityResource.AppRunner,
                session);

            base.LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            Boolean canContinueForwardCurrentNode = base.LinkContext.FromActivityInstance.CanNotRenewInstance == 0;

            return canContinueForwardCurrentNode;
        }

        /// <summary>
        /// Create activity task transition instance
        /// ���������ת��ʵ������
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
            Boolean isParallel = false;
            if (fromActivityInstance.ActivityType == (short)ActivityTypeEnum.GatewayNode)
            {
                //������ʵ����֧�ж�(AndSplit Multiple)
                //Concurrent multi instance branch judgment (AndSplit Multiple)
                var processModel = ProcessModelFactory.CreateByProcessInstance(session.Connection, processInstance, session.Transaction);
                var activityNode = processModel.GetActivity(fromActivityInstance.ActivityId);
                isParallel = processModel.IsAndSplitMI(activityNode);
            }

            if (isParallel)
            {
                //���ж�ʵ������
                //Parallel mutiple instance container
                ActivityInstanceEntity entity = null;
                var plist = activityResource.NextActivityPerformers[toActivity.ActivityId];

                //�������ж�ʵ����֧
                //Create parallel multi instance branches
                for (var i = 0; i < plist.Count; i++)
                {
                    entity = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);
                    entity.AssignedUserIds = plist[i].UserId;
                    entity.AssignedUserNames = plist[i].UserName;
                    entity.ActivityState = (short)ActivityStateEnum.Ready;

                    entity.Id = base.ActivityInstanceManager.Insert(entity, session);

                    base.TaskManager.Insert(entity, plist[i], activityResource.AppRunner, session);

                    InsertTransitionInstance(processInstance,
                        transitionId,
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
                //��ͨ����ڵ�
                //Normal Task Node
                var toActivityInstance = base.CreateActivityInstanceObject(toActivity, processInstance, activityResource.AppRunner);

                //��������˻غ�ķ���
                //Handling returns after multiple returns
                WriteBackSrcOrgInformation(toActivityInstance, fromActivityInstance, session);

                toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);
                
                base.ActivityInstanceManager.Insert(toActivityInstance, session);

                base.CreateNewTask(toActivityInstance, activityResource, session);

                InsertTransitionInstance(processInstance,
                    transitionId,
                    fromActivityInstance,
                    toActivityInstance,
                    transitionType,
                    flyingType,
                    activityResource.AppRunner,
                    session);

                //�����ⲿ�¼���ע�᷽��
                //Call the registration method for external events
                var EventContext = new EventContext
                {
                    AppInstanceId = processInstance.AppInstanceId,
                    ProcessId = processInstance.ProcessId,
                    ProcessInstanceId = processInstance.Id,
                    ActivityId = toActivity.ActivityId,
                    ActivityCode = toActivity.ActivityCode,
                    ActivityResource = activityResource
                };
                EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnActivityCreated,
                    activityResource.AppRunner.EventSubscriptionList,
                    EventContext);
            }
        }

        /// <summary>
        /// Maintain source node information for multiple returns
        /// ά������˻�ʱ��Դ�ڵ���Ϣ
        /// </summary>
        /// <param name="toActivityInstance"></param>
        /// <param name="fromActivityInstance"></param>
        /// <param name="session"></param>
        private void WriteBackSrcOrgInformation(ActivityInstanceEntity toActivityInstance, 
            ActivityInstanceEntity fromActivityInstance,
            IDbSession session)
        {
            if (fromActivityInstance.BackSrcActivityInstanceId != null)
            {
                var backSrcActivityInstance = base.ActivityInstanceManager.GetById(session.Connection,
                    fromActivityInstance.BackSrcActivityInstanceId.Value, session.Transaction);
                toActivityInstance.BackSrcActivityInstanceId = backSrcActivityInstance.BackSrcActivityInstanceId;
                toActivityInstance.BackOrgActivityInstanceId = backSrcActivityInstance.BackOrgActivityInstanceId;
            }
        }
    }
}
