using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 下一步分支选择的类型
    /// </summary>
    public enum NextActivityRouteChoiceEnum
    {
        /// <summary>
        /// 单一选择
        /// </summary>
        Single = 1,

        /// <summary>
        /// 或多选
        /// </summary>
        OrMultiple = 2,

        /// <summary>
        /// 必全选
        /// </summary>
        MustAll = 3
    }
}
