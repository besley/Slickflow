using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Slickflow.Module.Logging;

namespace Slickflow.Module.Essential.Message
{
    /// <summary>
    /// 消息处理器
    /// </summary>
    public class MessageMediator
    {
        /// <summary>
        /// 激活消息服务
        /// </summary>
        /// <param name="consumeMessageFunction">消息函数</param>
        /// <param name="topic">主题</param>
        /// <param name="line">内容</param>
        public void InvokeFromMessage(Func<MessageEntity, MessageConsumedResult> consumeMessageFunction, 
            string topic, 
            string line)
        {
            try
            {
                var result = consumeMessageFunction?.Invoke(new MessageEntity(topic, line));
                if (result.Status == MessageConsumedStatus.Success)
                {
                    LogService.RecordLog(JobTypeEnum.Message,
                        LogNameEnum.TriggerMessagingStartupProcess.ToString(),
                        topic,
                        string.Empty,
                        string.Empty,
                        1,
                        LogAdminDefine.ADMIN_MESSAGE_SUCCESS,
                        line);
                }
                else
                {
                    LogService.RecordLog(JobTypeEnum.Message,
                        LogNameEnum.TriggerMessagingStartupProcess.ToString(),
                        topic,
                        string.Empty,
                        string.Empty,
                        -1,
                        result.Message,
                        line);
                }
            }
            catch (System.Exception ex)
            {
                LogService.RecordLog(JobTypeEnum.Message,
                    LogNameEnum.TriggerMessagingStartupProcess.ToString(),
                    topic,
                    string.Empty,
                    string.Empty,
                    -1,
                    ex.Message,
                    line);
                throw;
            }
        }
    }
}
