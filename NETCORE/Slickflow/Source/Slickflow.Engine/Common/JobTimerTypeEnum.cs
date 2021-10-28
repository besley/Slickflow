using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 作业定时器类型
    /// </summary>
    public enum JobTimerTypeEnum
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,
        /// <summary>
        /// 定时
        /// </summary>
        Timer = 1,

        /// <summary>
        /// 条件
        /// </summary>
        Conditional = 2,

        /// <summary>
        /// 邮件
        /// </summary>
        EMail = 3,

        /// <summary>
        /// 消息
        /// </summary>
        Message = 4
    }
}
