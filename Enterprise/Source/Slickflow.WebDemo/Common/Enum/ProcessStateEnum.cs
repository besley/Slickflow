using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Slickflow.WebDemoV2._0.Common
{
    public enum ProcessStateEnum
    {
        /// <summary>
        /// 未启动，流程记录为空
        /// </summary>
        [Description("未启动，流程记录为空")]
        NotStart = 0,

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
        /// 完成
        /// </summary>
        [Description("完成")]
        Completed = 4,

        /// <summary>
        /// 挂起
        /// </summary>
        [Description("挂起")]
        Suspended = 5,

        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Canceled = 6,

        /// <summary>
        /// 终止
        /// </summary>
        [Description("终止")]
        Discarded = 7
    }
}