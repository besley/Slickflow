
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 跨网关对象封装
    /// </summary>
    internal class CrossOverGatewayDetail
    {
        internal Boolean IsCrossOverGateway { get; set; }
        internal IList<ActivityInstanceEntity> PrallelledChoicesNodes { get; set; }
        internal ActivityInstanceEntity PreviousActivityInstance { get; set; }
    }
}
