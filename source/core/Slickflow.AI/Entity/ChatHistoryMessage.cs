using System;

namespace Slickflow.AI.Entity
{
    /// <summary>
    /// Single chat history message (role + content) for multi-turn RAG context.
    /// </summary>
    public class ChatHistoryMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
