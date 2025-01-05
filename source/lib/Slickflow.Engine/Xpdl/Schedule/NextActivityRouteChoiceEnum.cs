using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next Activity Route Choice Type
    /// 下一步分支选择的类型
    /// </summary>
    public enum NextActivityRouteChoiceEnum
    {
        /// <summary>
        /// Single Choice
        /// 单一选择
        /// </summary>
        Single = 1,

        /// <summary>
        /// Or Multiple
        /// 或多选
        /// </summary>
        OrMultiple = 2,

        /// <summary>
        /// Must All
        /// 必全选
        /// </summary>
        MustAll = 3
    }
}
