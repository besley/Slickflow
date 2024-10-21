using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Essential
{
    /// <summary>
    /// 消息执行结果
    /// </summary>
    public class MessageConsumedResult
    {
        public MessageConsumedStatus Status { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerStackTrace { get; set; }

        public MessageConsumedResult()
        {
            Status = MessageConsumedStatus.Default;
            Message = string.Empty;
        }

        /// <summary>
        /// 缺省方法
        /// </summary>
        /// <returns></returns>
        public static MessageConsumedResult Default()
        {
            return new MessageConsumedResult();
        }
    }
}
