using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Essential.Message
{
    /// <summary>
    /// 消息执行结果
    /// </summary>
    public class MessageConsumedResult
    {
        public MessageConsumedStatus Status { get; set; }
        public String Message { get; set; }
       
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


    /// <summary>
    /// 状态执行枚举类型
    /// </summary>
    public enum MessageConsumedStatus
    {
        /// <summary>
        /// 缺省状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// 成功状态
        /// </summary>
        Success = 1,

        /// <summary>
        /// 执行失败状态
        /// </summary>
        Failed = 2,

        /// <summary>
        /// 异常状态
        /// </summary>
        Exception = 3
    }
}
