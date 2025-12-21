using System;
using Slickflow.Module.Essential.Entity;
using Slickflow.Module.Logging;

namespace Slickflow.Module.Essential
{
    /// <summary>
    /// Message Mediator
    /// 消息处理器
    /// </summary>
    public class MessageMediator
    {
        /// <summary>
        /// Invoke from message
        /// 激活消息服务
        /// </summary>
        /// <param name="consumeMessageFunction">message function</param>
        /// <param name="topic">topic</param>
        /// <param name="line">content</param>
        /// <param name="jobName">job name</param>
        public void InvokeFromMessage(Func<MessageEntity, MessageConsumedResult> consumeMessageFunction,
            string topic,
            string line,
            string jobName)
        {
            try
            {
                var result = consumeMessageFunction?.Invoke(new MessageEntity(topic, line, jobName));
                if (result.Status == MessageConsumedStatus.Success)
                {
                    LogService.RecordInfoLog(JobTypeEnum.Message,
                        jobName,
                        topic,
                        string.Empty,
                        string.Empty,
                        1,
                        LogAdminDefine.ADMIN_MESSAGE_SUCCESS,
                        line);
                }
                else
                {
                    LogService.RecordInfoLog(JobTypeEnum.Message,
                        jobName,
                        topic,
                        string.Empty,
                        string.Empty,
                        -1,
                        result.Message,
                        line);
                }
            }
            catch (Exception ex)
            {
                var stackTrace = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                LogService.RecordErrorLog(JobTypeEnum.Message,
                    jobName,
                    topic,
                    string.Empty,
                    string.Empty,
                    -1,
                    string.Format("Exception Message:{0}", ex.Message),
                    ex.StackTrace,
                    ex.InnerException != null ? ex.InnerException.StackTrace : null,
                    line);
                throw;
            }
        }
    }
}
