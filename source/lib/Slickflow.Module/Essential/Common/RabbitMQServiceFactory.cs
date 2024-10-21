using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Module.Essential.Entity;

namespace Slickflow.Module.Essential.Common
{
    /// <summary>
    /// 消息队列服务创建类
    /// </summary>
    public class RabbitMQServiceFactory
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="func">消息使用函数</param>
        /// <returns消息队列服务</returns>
        public static RabbitMQService Create()
        {
            var mqService = new RabbitMQService();
            return mqService;
        }

        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="func">消息使用函数</param>
        /// <returns消息队列服务</returns>
        public static RabbitMQService CreateMessage(Func<MessageEntity, MessageConsumedResult> func = null)
        {
            var mqService = new RabbitMQService();
            mqService.ConsumeMessageFunction = func;

            return mqService;
        }

        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="func">消息使用函数</param>
        /// <returns消息队列服务</returns>
        public static RabbitMQService CreateSignal(Func<SignalEntity, List<SignalConsumedResult>> func = null)
        {
            var mqService = new RabbitMQService();
            mqService.ConsumeSignalFunction = func;

            return mqService;
        }
    }
}
