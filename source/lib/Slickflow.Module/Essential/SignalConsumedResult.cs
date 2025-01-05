using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Essential
{
    /// <summary>
    /// Signal consumed result
    /// 信号消费结果
    /// </summary>
    public class SignalConsumedResult
    {
        public SignalConsumedStatus Status { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerStackTrace { get; set; }

        public SignalConsumedResult()
        {
            Status = SignalConsumedStatus.Default;
            Message = string.Empty;
        }

        /// <summary>
        /// Default
        /// </summary>
        /// <returns></returns>
        public static SignalConsumedResult Default()
        {
            return new SignalConsumedResult();
        }
    }
}
