using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Essential.Message
{
    /// <summary>
    /// 消息队列服务创建类
    /// </summary>
    public class MessageQueueServiceFactory
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="func">消息使用函数</param>
        /// <returns消息队列服务</returns>
        public static MessageQueueService Create(Func<MessageEntity, MessageConsumedResult> func = null)
        {
            var mqService = new MessageQueueService();
            mqService.ConsumeMessageFunction = func;

            return mqService;
        }
    }
}
