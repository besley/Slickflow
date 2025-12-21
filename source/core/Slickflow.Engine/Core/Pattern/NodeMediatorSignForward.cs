using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Mediator Sign Forward
    /// 加签主节点执行器
    /// </summary>
    internal class NodeMediatorSignForward : NodeMediator
    {
        internal NodeMediatorSignForward(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                //读取加签控制参数
                //记录加签通过类型和通过率
                //Read signature control parameters
                //Record the type and rate of approval for additional signatures
                var controlParamSheet = base.ActivityForwardContext.ActivityResource.AppRunner.ControlParameterSheet;
                if (controlParamSheet != null
                    && controlParamSheet.SignForwardCompareType.ToUpper() == "COUNT")
                {
                    base.ActivityForwardContext.FromActivityInstance.CompareType = (short)CompareTypeEnum.Count;
                    base.ActivityForwardContext.FromActivityInstance.CompleteOrder = controlParamSheet.SignForwardCompleteOrder;
                }
                else
                {
                    base.ActivityForwardContext.FromActivityInstance.CompareType = (short)CompareTypeEnum.Percentage;
                    if (controlParamSheet.SignForwardCompleteOrder != 0)
                        base.ActivityForwardContext.FromActivityInstance.CompleteOrder = controlParamSheet.SignForwardCompleteOrder;
                    else
                        base.ActivityForwardContext.FromActivityInstance.CompleteOrder = 1;
                }

                //更新当前实例节点为主节点，并且置当前节点为挂起状态
                //Update the current instance node as the primary node and put the current node in a suspended state
                var signForwardType = EnumHelper.ParseEnum<SignForwardTypeEnum>(controlParamSheet.SignForwardType);
                UpgradeToMainSignForwardNode(base.ActivityForwardContext.FromActivityInstance, signForwardType);

                //产生加签记录，即要发送给加签人的活动实例记录
                //Generate endorsement records, which are activity instance records to be sent to the endorser
                CreateSignForwardTasks(ActivityForwardContext.ActivityResource);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Upgrade the current node to become the signing master node
        /// 升级当前节点为加签主节点
        /// </summary>
        /// <param name="currentActivityInstance"></param>
        /// <param name="signForwardType"></param>
        private void UpgradeToMainSignForwardNode(ActivityInstanceEntity currentActivityInstance,
            SignForwardTypeEnum signForwardType)
        {
            currentActivityInstance.ComplexType = (short)ComplexTypeEnum.SignForward;
            if (signForwardType == SignForwardTypeEnum.SignForwardBefore 
                || signForwardType == SignForwardTypeEnum.SignForwardBehind)
            {
                currentActivityInstance.MergeType = (short)MergeTypeEnum.Sequence;
            }
            else
            {
                currentActivityInstance.MergeType = (short)MergeTypeEnum.Parallel;
            }
            currentActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
            
            base.ActivityInstanceManager.Update(currentActivityInstance, base.Session);
        }

        /// <summary>
        /// Create a signature node record
        /// 创建加签节点记录
        /// </summary>
        /// <param name="activityResource"></param>
        private void CreateSignForwardTasks(ActivityResource activityResource)
        {
            SignForwardTypeEnum signForwardType = (SignForwardTypeEnum)Enum.Parse(typeof(SignForwardTypeEnum), 
                base.ActivityForwardContext.ActivityResource.AppRunner.ControlParameterSheet.SignForwardType);

            //根据当前活动实例的记录为加签发起人创建一条新的记录，并修改CompleteOrder
            //Create a new record for adding the issuer based on the current activity instance record,
            //and modify the CompleteOrder accordingly
            var newActivityInstance = base.ActivityInstanceManager.CreateActivityInstanceObject(base.ActivityForwardContext.FromActivityInstance);
            newActivityInstance.AssignedUserIds = activityResource.AppRunner.UserId;
            newActivityInstance.AssignedUserNames = activityResource.AppRunner.UserName;
            newActivityInstance.MainActivityInstanceId = base.ActivityForwardContext.FromActivityInstance.Id;
            newActivityInstance.SignForwardType = (short)signForwardType;

            if (signForwardType == SignForwardTypeEnum.SignForwardParallel)
            {
                var controlParamSheet = base.ActivityForwardContext.ActivityResource.AppRunner.ControlParameterSheet;
                if (controlParamSheet.SignForwardCompareType.ToUpper() == "COUNT")
                {
                    newActivityInstance.CompleteOrder = controlParamSheet.SignForwardCompleteOrder;
                }
                else
                {
                    newActivityInstance.CompleteOrder = 1;
                }
            }
            newActivityInstance.ComplexType = (short)ComplexTypeEnum.SignForward;

            //获取加签人集合
            //Obtain the collection of signatories
            var plist = activityResource.NextActivityPerformers[base.ActivityForwardContext.CurrentActivity.ActivityId];

            //前加签是别人先审核，然后自己再审核
            //Sign forward before is someone else's first review, and then self review
            if (signForwardType == SignForwardTypeEnum.SignForwardBefore)
            {
                newActivityInstance.CompleteOrder = plist.Count + 1;
                newActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
            }
            else if (signForwardType == SignForwardTypeEnum.SignForwardBehind)
            {
                //后加签是自己审批后，其他接收人再加签
                //Sign forward behind refers to the process of obtaining approval from oneself and then having other recipients sign again
                newActivityInstance.CompleteOrder = 1;
            }
			else if (signForwardType == SignForwardTypeEnum.SignForwardParallel)
            {
                //并行加签子节点都为-1，与并行会签保持一致
                newActivityInstance.CompleteOrder = -1;
            }

            //主节点挂起后，插入当前人的加签记录信息, 并插入任务记录
            //After the master node is suspended,
            //insert the signature record information of the current person and insert the task record
            base.ActivityInstanceManager.Insert(newActivityInstance, base.Session);
            var signer = new Performer(base.ActivityForwardContext.ActivityResource.AppRunner.UserId,
                base.ActivityForwardContext.ActivityResource.AppRunner.UserName);
            base.TaskManager.Insert(newActivityInstance, signer, base.ActivityForwardContext.ActivityResource.AppRunner, base.Session);

            //创建新加签节点记录
            //Create a new signing node record
            var signforwardActivityInstance = new ActivityInstanceEntity();
            for (var i = 0; i < plist.Count; i++)
            {
                signforwardActivityInstance = base.ActivityInstanceManager.CreateActivityInstanceObject(base.ActivityForwardContext.FromActivityInstance);
                signforwardActivityInstance.ComplexType = (short)ComplexTypeEnum.SignForward;
                signforwardActivityInstance.AssignedUserIds = plist[i].UserId;
                signforwardActivityInstance.AssignedUserNames = plist[i].UserName;
                signforwardActivityInstance.MainActivityInstanceId = base.ActivityForwardContext.FromActivityInstance.Id;
                
                if (signForwardType == SignForwardTypeEnum.SignForwardBefore)
                {
                    signforwardActivityInstance.CompleteOrder = (short)(i + 1);
                    if (i > 0)
                    {
                        //加签是串行加签，逐次完成
                        //Sign forward is a serial signing process that is completed step by step
                        signforwardActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
                    }
                }
                else if (signForwardType == SignForwardTypeEnum.SignForwardBehind)
                {
                    signforwardActivityInstance.CompleteOrder = (short)(i + 2);
                    signforwardActivityInstance.ActivityState = (short)ActivityStateEnum.Suspended;
                }
				else if (signForwardType == SignForwardTypeEnum.SignForwardParallel)
                {
                    signforwardActivityInstance.CompleteOrder = -1;
                }
                
                signforwardActivityInstance.SignForwardType = (short)signForwardType;

                base.ActivityInstanceManager.Insert(signforwardActivityInstance, base.Session);
                base.TaskManager.Insert(signforwardActivityInstance, plist[i], activityResource.AppRunner, base.Session);
            }
        }
    }
}
