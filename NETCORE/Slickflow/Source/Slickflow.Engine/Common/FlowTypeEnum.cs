using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 转移类型
    /// </summary>
    public enum FlowTypeEnum
    {
        /// <summary>
        /// 前行
        /// </summary>
        Forward = 1,

        /// <summary>
        /// 后退
        /// </summary>
        Backward = 2, //(包括撤销、退回和返签类型)
    }
}
