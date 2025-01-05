using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Merge through type settings
    /// 合并通过类型设置
    /// </summary>
    public enum GatewayJoinPassEnum : int
    {
        /// <summary>
        /// Node
        /// 空白
        /// </summary>
        None = 0,

        /// <summary>
        /// Branches count for merging pass
        /// 满足分支数目，才可以通过
        /// </summary>
        Count = 1,

        /// <summary>
        /// The forced branch to merge passing
        /// 满足强制必须合并分支，才可以通过
        /// </summary>
        Forced = 2
    }
}
