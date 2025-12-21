using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Utility
{
    public enum ChatModelType
    {
        Chat = 1,
        Reasoner = 2
    }
    public class ChatRequest
    {
        public string Model { get; set; }
        public ChatMessage[] Messages { get; set; }
        public double temperature { get; set; }
        public int max_tokens { get; set; }
    }

    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
