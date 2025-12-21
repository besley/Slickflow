using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Module.Essential.Common;
using Slickflow.Engine.Utility;
using Slickflow.Module.Essential;
using Slickflow.Module.Essential.Entity;

namespace Slickflow.Engine.Essential
{
    /// <summary>
    /// Signal Delegate Service
    /// 消息队列中的信号委托函数
    /// </summary>
    public class SignalDelegateService
    {
        #region Signal Publish
        /// <summary>
        /// Build Singal Published
        /// 构建要发布的消息
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="throwActivity"></param>
        /// <param name="throwActivityInstance"></param>
        public void PublishSignal(ProcessInstanceEntity processInstance,
            Activity throwActivity,
            ActivityInstanceEntity throwActivityInstance)
        {
            var topic = throwActivity.TriggerDetail.Expression;

            var appRunner = new WfAppRunner();
            appRunner.MessageTopic = topic;
            appRunner.AppName = throwActivityInstance.AppName;
            appRunner.AppInstanceId = throwActivityInstance.AppInstanceId;
            appRunner.AppInstanceCode = throwActivityInstance.AppInstanceCode;
            appRunner.UserId = throwActivityInstance.CreatedUserId;
            appRunner.UserName = throwActivityInstance.CreatedUserName;

            var signalRunnerView = new SignalRunnerView();
            signalRunnerView.WfAppRunner = appRunner;

            var jsonRunner = JsonConvert.SerializeObject(signalRunnerView);

            var signalService = RabbitMQServiceFactory.CreateSignal();
            signalService.Publish(topic, jsonRunner);
        }
        #endregion

        #region Singal Consume
        /// <summary>
        /// Singal Consume function
        /// 消息消费函数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<SignalConsumedResult> ConsumeSignal(SignalEntity entity)
        {
            var resultList = new List<SignalConsumedResult>();
            //get signal runner info
            var signalRunnerView = JsonConvert.DeserializeObject<SignalRunnerView>(entity.Line);
            var appRunner = signalRunnerView.WfAppRunner;
            var topic = appRunner.MessageTopic;

            var jim = new JobInfoManager();
            var jobInfoList = jim.GetJobSubscribedByTopic(topic);
            foreach (var jobInfo in jobInfoList)
            {
                var signalResult = SignalConsumedResult.Default();
                IWorkflowService wfService = new WorkflowService();
                WfExecutedResult wfResult = WfExecutedResult.Default();

                var actvityType = EnumHelper.ParseEnum<ActivityTypeEnum>(jobInfo.ActivityType);
                if (actvityType == ActivityTypeEnum.StartNode)
                {
                    var triggerTpe = EnumHelper.ParseEnum<TriggerTypeEnum>(jobInfo.TriggerType);
                    if (triggerTpe == TriggerTypeEnum.Signal)
                    {
                        var direction = EnumHelper.ParseEnum<MessageDirectionEnum>(jobInfo.MessageDirection);
                        if (direction == MessageDirectionEnum.Catch)
                        {
                            var startRunner = GetStartRunnerFromSignalExchange(signalRunnerView, jobInfo);
                            wfResult = wfService.StartProcess(startRunner);
                            if (wfResult.Status == WfExecutedStatus.Success)
                            {
                                signalResult.Status = SignalConsumedStatus.Success;
                            }
                            else
                            {
                                signalResult.Status = SignalConsumedStatus.Exception;
                                signalResult.Message = wfResult.Message;
                            }
                        }
                    }
                    if (signalResult.Status == SignalConsumedStatus.Exception
                        || signalResult.Status == SignalConsumedStatus.Failed)
                    {
                        throw new WfSignalDelegationException(signalResult.Message);
                    }
                    resultList.Add(signalResult);
                }
                else if (actvityType == ActivityTypeEnum.IntermediateNode)
                {
                    var triggerType = EnumHelper.ParseEnum<TriggerTypeEnum>(jobInfo.TriggerType);
                    if (triggerType == TriggerTypeEnum.Signal)
                    {
                        var direction = EnumHelper.ParseEnum<MessageDirectionEnum>(jobInfo.MessageDirection);
                        if (direction == MessageDirectionEnum.Catch)
                        {
                            resultList = RunProcessSignalCatchEventOneByOne(signalRunnerView, jobInfo);
                        }
                    }
                }
            }
            return resultList;
        }

        /// <summary>
        /// Get start info from signal exchange
        /// 从信号交换获取启动信息
        /// </summary>
        /// <param name="msgRunnerView"></param>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        private WfAppRunner GetStartRunnerFromSignalExchange(SignalRunnerView msgRunnerView, JobInfoEntity jobInfo)
        {
            var runner = new WfAppRunner();
            runner.ProcessId = jobInfo.ProcessId;
            runner.Version = jobInfo.Version;
            runner.AppName = msgRunnerView.WfAppRunner.AppName;
            runner.AppInstanceId = msgRunnerView.WfAppRunner.AppInstanceId;
            runner.AppInstanceCode = msgRunnerView.WfAppRunner.AppInstanceCode;
            runner.UserId = msgRunnerView.WfAppRunner.UserId;
            runner.UserName = msgRunnerView.WfAppRunner.UserName;
            return runner;
        }

        /// <summary>
        /// Run the process belonging to the SignalCatch node
        /// 运行属于SignalCatch节点的流程
        /// </summary>
        /// <param name="signalRunner"></param>
        /// <param name="jobInfo"></param>
        private List<SignalConsumedResult> RunProcessSignalCatchEventOneByOne(SignalRunnerView signalRunner, JobInfoEntity jobInfo)
        {
            var resultList = new List<SignalConsumedResult>();
            //封装下一步的步骤信息
            //Encapsulate the next step information
            var aim = new ActivityInstanceManager();
            var runningActivityInstanceList = aim.GetActivityInstanceList(jobInfo.ProcessId, jobInfo.Version, jobInfo.ActivityId);
            foreach (var activityInstance in runningActivityInstanceList)
            {
                var signalResult = SignalConsumedResult.Default();
                var runner = GetRunnerFromActivityInstance(signalRunner, jobInfo, activityInstance);
                var wfService = new WorkflowService();
                var wfResult = wfService.RunProcessAuto(runner);
                if (wfResult.Status == WfExecutedStatus.Success)
                {
                    signalResult.Status = SignalConsumedStatus.Success;
                }
                else
                {
                    signalResult.Status = SignalConsumedStatus.Exception;
                    signalResult.Message = wfResult.Message;
                }

                if (signalResult.Status == SignalConsumedStatus.Exception
                    || signalResult.Status == SignalConsumedStatus.Failed)
                {
                    throw new WfSignalDelegationException(signalResult.Message);
                }
                resultList.Add(signalResult);
            }
            return resultList;
        }

        /// <summary>
        /// Get runner info from activity instance
        /// 从活动实例获取运行者信息
        /// </summary>
        /// <param name="signalRunner"></param>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        private WfAppRunner GetRunnerFromActivityInstance(SignalRunnerView signalRunner, 
            JobInfoEntity jobInfo, 
            ActivityInstanceEntity activityInstance)
        {
            var runner = new WfAppRunner();
            runner.ProcessId = jobInfo.ProcessId;
            runner.Version = jobInfo.Version;
            runner.AppName = activityInstance.AppName;
            runner.AppInstanceId = activityInstance.AppInstanceId;
            runner.AppInstanceCode = activityInstance.AppInstanceCode;
            //using activityinstanceid to call RunProcessAuto() method
            runner.ActivityInstanceId = activityInstance.Id;
            runner.UserId = signalRunner.WfAppRunner.UserId;
            runner.UserName = signalRunner.WfAppRunner.UserName;

            if (!string.IsNullOrEmpty(activityInstance.NextStepPerformers))
            {
                runner.NextActivityPerformers = NextStepUtility.DeserializeNextStepPerformers(activityInstance.NextStepPerformers);
            }
            return runner;
        }
        #endregion
    }
}
