using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 网关详细
    /// </summary>
    public class GatewayDetail
    {
        /// <summary>
        /// 网关分支合并类型
        /// </summary>
        public GatewaySplitJoinTypeEnum SplitJoinType { get; set; }

        /// <summary>
        /// 网关方向类型
        /// </summary>
        public GatewayDirectionEnum DirectionType { get; set; }

        /// <summary>
        /// 合并节点通过类型
        /// </summary>
        public GatewayJoinPassEnum JoinPassType { get; set; }

    }
}
