using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Slickflow.WebDemoV2._0.Common
{
    public enum ActivityStateEnum
    {
        /// <summary>
        /// 准备状态
        /// </summary>
        [Description("准备状态")]
        Ready = 1,

        /// <summary>
        /// 运行状态
        /// </summary>
        [Description("运行状态")]
        Running = 2,

        /// <summary>
        /// 完成状态
        /// </summary>
        [Description("完成状态")]
        Completed = 4,

        /// <summary>
        /// 挂起
        /// </summary>
        [Description("挂起")]
        Suspended = 5,

        /// <summary>
        /// 撤销状态
        /// </summary>
        [Description("撤销状态")]
        Withdrawed = 6,

        /// <summary>
        /// 退回状态
        /// </summary>
        [Description("退回状态")]
        Sendbacked = 7
    }
}