using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Message
{
    public interface IMessageQueueService
    {
        void Publish(string topic, string line);
        void Publish(MessageEntity message);
        void Subscribe(string topic);
        void Unsubscribe(string topic);
    }
}
