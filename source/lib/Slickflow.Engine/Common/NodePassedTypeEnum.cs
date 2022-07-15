using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 节点通过类型
    /// </summary>
    public enum NodePassedTypeEnum
    {
        /// <summary>
        /// 缺省状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// 允许通过
        /// </summary>
        Passed = 1,

        /// <summary>
        /// 拒绝状态
        /// </summary>
        NotPassed = 2,

        /// <summary>
        /// 需要更多审批同意
        /// </summary>
        NeedToBeMoreApproved = 3
    }
}
