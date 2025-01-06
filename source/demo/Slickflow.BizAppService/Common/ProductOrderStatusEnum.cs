using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.BizAppService.Service
{
    /// <summary>
    /// Product Order Status
    /// 生产订单状态
    /// </summary>
    public enum ProductOrderStatusEnum
    {
        /// <summary>
        /// Ready
        /// 完成同步
        /// </summary>
        Ready = 1,

        /// <summary>
        /// Dispatching
        /// 等待派单
        /// </summary>
        Dispatching = 2,

        /// <summary>
        /// Sampling
        /// 等待打样
        /// </summary>
        Sampling = 3,

        /// <summary>
        /// Manufacturing
        /// 等待生产
        /// </summary>
        Manufacturing = 4,

        /// <summary>
        /// QC Checking
        /// 等待质检
        /// </summary>
        QCChecking = 5,

        /// <summary>
        /// Weighting Goods
        /// 等待称重
        /// </summary>
        Weighting = 6,

        /// <summary>
        /// Delivering to customers
        /// 等待发货
        /// </summary>
        Delivering = 7,

        /// <summary>
        /// Completed
        /// 最终完成
        /// </summary>
        Completed = 8
    }
}
