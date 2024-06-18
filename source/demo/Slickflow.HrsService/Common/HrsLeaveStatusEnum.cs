using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.HrsService.Common
{
    /// <summary>
    /// 请假单状态
    /// </summary>
    public enum HrsLeaveStatusEnum
    {
        /// <summary>
        /// 完成同步
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 等待派单
        /// </summary>
        Dispatching = 2,

        /// <summary>
        /// 等待打样
        /// </summary>
        Sampling = 3,

        /// <summary>
        /// 等待生产
        /// </summary>
        Manufacturing = 4,

        /// <summary>
        /// 等待质检
        /// </summary>
        QCChecking = 5,

        /// <summary>
        /// 等待称重
        /// </summary>
        Weighting = 6,

        /// <summary>
        /// 等待发货
        /// </summary>
        Delivering = 7,

        /// <summary>
        /// 最终完成
        /// </summary>
        Completed = 8
    }
}
