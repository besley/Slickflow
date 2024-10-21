﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Module.Essential;
using Slickflow.Module.Essential.Common;
using Slickflow.Module.Essential.Entity;

namespace Slickflow.Engine.Essential
{
    /// <summary>
    /// 消息队列中的消息委托函数
    /// </summary>
    public class MessageDelegateService
    {
        #region 消息发布
        /// <summary>
        /// 构建要发布的消息
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="throwActivity">消息活动</param>
        /// <param name="throwActivityInstance">消息活动实例</param>
        public void PublishMessage(ProcessInstanceEntity processInstance,
            Activity throwActivity,
            ActivityInstanceEntity throwActivityInstance)
        {
            var topic = throwActivity.TriggerDetail.Expression;
            ProcessEntity catchProcess = null;
            var catchActivity = XPDLHelper.GetMessageCatchActivity(processInstance, throwActivity, throwActivityInstance, out catchProcess);

            var appRunner = new WfAppRunner();
            appRunner.MessageTopic = topic;
            appRunner.AppName = throwActivityInstance.AppName;
            appRunner.AppInstanceID = throwActivityInstance.AppInstanceID;
            appRunner.AppInstanceCode = throwActivityInstance.AppInstanceCode;
            appRunner.UserID = throwActivityInstance.CreatedByUserID;
            appRunner.UserName = throwActivityInstance.CreatedByUserName;

            var msgRunnerView = new MessageRunnerView();
            msgRunnerView.ProcessEntity = catchProcess;
            msgRunnerView.ActivityEntity = catchActivity;
            msgRunnerView.WfAppRunner = appRunner;

            var jsonRunner = JsonConvert.SerializeObject(msgRunnerView);

            var messageService = RabbitMQServiceFactory.CreateMessage();
            messageService.Publish(topic, jsonRunner);
        }
        #endregion

        #region 消息消费函数
        /// <summary>
        /// 消息消费函数
        /// </summary>
        /// <param name="entity">消息实体对象</param>
        /// <returns>消费结果</returns>
        public MessageConsumedResult ConsumeMessage(MessageEntity entity)
        {
            var msgResult = MessageConsumedResult.Default();
            var topic = entity.Topic;
            var msgRunnerView = JsonConvert.DeserializeObject<MessageRunnerView>(entity.Line);

            if (!string.IsNullOrEmpty(topic))
            {
                try
                {
                    IWorkflowService wfService = new WorkflowService();
                    WfExecutedResult wfResult = WfExecutedResult.Default();
                    var catchActivity = msgRunnerView.ActivityEntity;
                    if (catchActivity.ActivityType == ActivityTypeEnum.StartNode)
                    {
                        if (catchActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message
                            && catchActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                        {
                            var startRunner = GetRunnerFromMessagExchange(msgRunnerView);
                            wfResult = wfService.StartProcess(startRunner);
                            if (wfResult.Status == WfExecutedStatus.Success)
                            {
                                msgResult.Status = MessageConsumedStatus.Success;
                            }
                            else
                            {
                                msgResult.Status = MessageConsumedStatus.Exception;
                                msgResult.Message = wfResult.Message;
                            }
                        }
                        else
                        {
                            msgResult.Status = MessageConsumedStatus.Exception;
                            msgResult.Message = string.Format("Unknown activity type detail:{0}, message direction:{1}",
                                catchActivity.TriggerDetail.TriggerType.ToString(),
                                catchActivity.TriggerDetail.MessageDirection.ToString());
                        }
                    }
                    else
                    {
                        if (catchActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Message
                            && catchActivity.TriggerDetail.MessageDirection == MessageDirectionEnum.Catch)
                        {
                            var runner = GetRunnerFromMessagExchange(msgRunnerView);
                            wfResult = wfService.RunProcessAuto(runner);
                            if (wfResult.Status == WfExecutedStatus.Success)
                            {
                                msgResult.Status = MessageConsumedStatus.Success;
                            }
                            else
                            {
                                msgResult.Status = MessageConsumedStatus.Exception;
                                msgResult.Message = wfResult.Message;
                            }
                        }
                        else
                        {
                            msgResult.Status = MessageConsumedStatus.Exception;
                            msgResult.Message = string.Format("Unknown activity type detail:{0}, message direction:{1}",
                                catchActivity.TriggerDetail.TriggerType.ToString(),
                                catchActivity.TriggerDetail.MessageDirection.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgResult.Status = MessageConsumedStatus.Failed;
                    msgResult.Message = ex.Message;
                    throw;
                }
            }

            if (msgResult.Status == MessageConsumedStatus.Exception
                || msgResult.Status == MessageConsumedStatus.Failed)
            {
                throw new WfMessageDelegationException(msgResult.Message);
            }
            return msgResult;
        }

        /// <summary>
        /// 从消息视图中封装运行者对象
        /// </summary>
        /// <param name="msgRunnerView">消息视图</param>
        /// <returns>运行者对象</returns>
        private WfAppRunner GetRunnerFromMessagExchange(MessageRunnerView msgRunnerView)
        {
            var runner = new WfAppRunner();
            runner.ProcessGUID = msgRunnerView.ProcessEntity.ProcessGUID;
            runner.Version = msgRunnerView.ProcessEntity.Version;
            runner.ProcessCode = msgRunnerView.ProcessEntity.ProcessCode;
            runner.AppName = msgRunnerView.WfAppRunner.AppName;
            runner.AppInstanceID = msgRunnerView.WfAppRunner.AppInstanceID;
            runner.AppInstanceCode = msgRunnerView.WfAppRunner.AppInstanceCode;
            runner.UserID = msgRunnerView.WfAppRunner.UserID;
            runner.UserName = msgRunnerView.WfAppRunner.UserName;

            //封装下一步的步骤信息
            var aim = new ActivityInstanceManager();
            var runningActivityInstance = aim.GetActivityInstanceLatest(runner.AppInstanceID, runner.ProcessGUID, msgRunnerView.ActivityEntity.ActivityGUID);
            if (runningActivityInstance != null)
            {
                runner.ActivityInstanceID = runningActivityInstance.ID;
                if (!string.IsNullOrEmpty(runningActivityInstance.NextStepPerformers))
                {
                    runner.NextActivityPerformers = NextStepUtility.DeserializeNextStepPerformers(runningActivityInstance.NextStepPerformers);
                }
            }

            return runner;
        }
        #endregion
    }
}
