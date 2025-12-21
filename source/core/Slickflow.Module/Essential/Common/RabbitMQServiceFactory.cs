using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Module.Essential.Entity;

namespace Slickflow.Module.Essential.Common
{
    /// <summary>
    /// RabbitMQ Service Factory
    /// 消息队列服务创建类
    /// </summary>
    public class RabbitMQServiceFactory
    {
        /// <summary>
        /// Create
        /// </summary>
        public static RabbitMQService Create()
        {
            var mqService = new RabbitMQService();
            return mqService;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="func">message consume function</param>
        public static RabbitMQService CreateMessage(Func<MessageEntity, MessageConsumedResult> func = null)
        {
            var mqService = new RabbitMQService();
            mqService.ConsumeMessageFunction = func;

            return mqService;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="func">message consume function</param>
        public static RabbitMQService CreateSignal(Func<SignalEntity, List<SignalConsumedResult>> func = null)
        {
            var mqService = new RabbitMQService();
            mqService.ConsumeSignalFunction = func;

            return mqService;
        }
    }
}
