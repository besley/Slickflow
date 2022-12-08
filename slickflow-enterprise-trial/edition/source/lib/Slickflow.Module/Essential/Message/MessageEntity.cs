using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Module.Essential.Message
{
    /// <summary>
    /// Message Entity
    /// </summary>
    public class MessageEntity
    {
        public string Topic { get; set; }
        public string Line { get; set; }

        public MessageEntity(string topic, string line)
        {
            Topic = topic;
            Line = line;
        }
    }
}
