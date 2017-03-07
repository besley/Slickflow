using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Slickflow.WebDemoV2._0.Common
{
    public enum TaskStateEnum
    {
        /// <summary>
        /// 等待办理
        /// </summary>
        [Description("等待办理")]
        Waiting = 1,

        /// <summary>
        /// 办理状态
        /// </summary>
        [Description("办理状态")]
        Handling = 2,

        /// <summary>
        /// 正常完成
        /// </summary>
        [Description("正常完成")]
        Completed = 4,

        /// <summary>
        /// 撤销
        /// </summary>
        [Description("撤销")]
        Withdrawed = 8,

        /// <summary>
        /// 退回
        /// </summary>
        [Description("退回")]
        Rejected = 16,

        /// <summary>
        /// 多人可以办理，当别人办理完后，置关闭状态
        /// </summary>
        [Description("多人可以办理，当别人办理完后，置关闭状态")]
        Closed = 32,
    }
}