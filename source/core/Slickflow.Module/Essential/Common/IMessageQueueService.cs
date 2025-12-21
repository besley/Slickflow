using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Module.Essential.Entity;

namespace Slickflow.Module.Essential.Common
{
    /// <summary>
    /// Message Queue Service Interface
    /// 消息队列服务接口
    /// </summary>
    public interface IMessageQueueService
    {
        void Publish(string topic, string line);
        void Publish(MessageEntity message);
        void Subscribe(string topic, string jobName, string triggerType);
        void Unsubscribe(string topic);
    }
}
