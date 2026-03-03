using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Process Variable Type
    /// 变量范围类型
    /// </summary>
    public enum ProcessVariableScopeEnum
    {
        /// <summary>
        /// System
        /// 系统
        /// </summary>
        System = 1,
        /// <summary>
        /// Process
        /// 流程
        /// </summary>
        Process = 4,

        /// <summary>
        /// Activity
        /// 活动
        /// </summary>
        Activity = 6,
    }
}
