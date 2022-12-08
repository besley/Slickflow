using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 邮件发送状态类型
    /// </summary>
    public enum TaskEMailSentStatusEnum
    {
        /// <summary>
        /// 未发送
        /// </summary>
        UnSent = 0,

        /// <summary>
        /// 发送(smtp不保证一定发送成功）
        /// </summary>
        Sent = 1
    }
}
