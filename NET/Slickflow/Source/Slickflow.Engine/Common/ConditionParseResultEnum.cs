using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 条件解析结果的枚举类型
    /// </summary>
    public enum ConditionParseResultEnum
    {
        /// <summary>
        /// 未通过
        /// </summary>
        NotPassed = 0,

        /// <summary>
        /// 满足条件
        /// </summary>
        Passed = 1
    }
}
