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
    /// 消息队列中的信号委托函数
    /// </summary>
    public class SignalDelegateService
    {
        #region 信号发布
        /// <summary>
        /// 构建要发布的消息
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="throwActivity">消息活动</param>
        /// <param name="throwActivityInstance">消息活动实例</param>
        public void PublishSignal(ProcessInstanceEntity processInstance,
            Activity throwActivity,
            ActivityInstanceEntity throwActivityInstance)
        {
            var topic = throwActivity.TriggerDetail.Expression;

            var appRunner = new WfAppRunner();
            appRunner.MessageTopic = topic;
            appRunner.AppName = throwActivityInstance.AppName;
            appRunner.AppInstanceID = throwActivityInstance.AppInstanceID;
            appRunner.AppInstanceCode = throwActivityInstance.AppInstanceCode;
            appRunner.UserID = throwActivityInstance.CreatedByUserID;
            appRunner.UserName = throwActivityInstance.CreatedByUserName;

            var signalRunnerView = new SignalRunnerView();
            signalRunnerView.WfAppRunner = appRunner;

            var jsonRunner = JsonConvert.SerializeObject(signalRunnerView);

            var signalService = RabbitMQServiceFactory.CreateSignal();
            signalService.Publish(topic, jsonRunner);
        }
        #endregion

        #region 信号消费函数
        /// <summary>
        /// 消息消费函数
        /// </summary>
        /// <param name="entity">消息实体对象</param>
        /// <returns>消费结果</returns>
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
        /// 开始开始节点的运行者信息
        /// </summary>
        /// <param name="msgRunnerView">消息视图</param>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        private WfAppRunner GetStartRunnerFromSignalExchange(SignalRunnerView msgRunnerView, JobInfoEntity jobInfo)
        {
            var runner = new WfAppRunner();
            runner.ProcessGUID = jobInfo.ProcessGUID;
            runner.Version = jobInfo.Version;
            runner.AppName = msgRunnerView.WfAppRunner.AppName;
            runner.AppInstanceID = msgRunnerView.WfAppRunner.AppInstanceID;
            runner.AppInstanceCode = msgRunnerView.WfAppRunner.AppInstanceCode;
            runner.UserID = msgRunnerView.WfAppRunner.UserID;
            runner.UserName = msgRunnerView.WfAppRunner.UserName;
            return runner;
        }

        /// <summary>
        /// 运行属于SignalCatch节点的流程
        /// </summary>
        /// <param name="signalRunner"></param>
        /// <param name="jobInfo"></param>
        private List<SignalConsumedResult> RunProcessSignalCatchEventOneByOne(SignalRunnerView signalRunner, JobInfoEntity jobInfo)
        {
            var resultList = new List<SignalConsumedResult>();
            //封装下一步的步骤信息
            var aim = new ActivityInstanceManager();
            var runningActivityInstanceList = aim.GetActivityInstanceList(jobInfo.ProcessGUID, jobInfo.Version, jobInfo.ActivityGUID);
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
        /// 开始开始节点的运行者信息
        /// </summary>
        /// <param name="signalRunner">消息视图</param>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        private WfAppRunner GetRunnerFromActivityInstance(SignalRunnerView signalRunner, JobInfoEntity jobInfo, ActivityInstanceEntity activityInstance)
        {
            var runner = new WfAppRunner();
            runner.ProcessGUID = jobInfo.ProcessGUID;
            runner.Version = jobInfo.Version;
            runner.AppName = activityInstance.AppName;
            runner.AppInstanceID = activityInstance.AppInstanceID;
            runner.AppInstanceCode = activityInstance.AppInstanceCode;
            //using activityinstanceid to call RunProcessAuto() method
            runner.ActivityInstanceID = activityInstance.ID;
            runner.UserID = signalRunner.WfAppRunner.UserID;
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
