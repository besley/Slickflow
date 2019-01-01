using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 变量范围类型
    /// </summary>
    public enum ScopeTypeEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,

        /// <summary>
        /// 流程
        /// </summary>
        Process = 1,

        /// <summary>
        /// 活动
        /// </summary>
        Activity = 2,

        /// <summary>
        /// 任务
        /// </summary>
        Task = 3
    }
}
