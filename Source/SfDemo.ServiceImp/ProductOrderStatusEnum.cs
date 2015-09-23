using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfDemo.ServiceImp
{
    /// <summary>
    /// 生产订单状态
    /// </summary>
    public enum ProductOrderStatusEnum
    {
        /// <summary>
        /// 完成同步
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 完成派单
        /// </summary>
        Dispatched = 2,

        /// <summary>
        /// 完成打样
        /// </summary>
        Sampled = 3,

        /// <summary>
        /// 完成生产
        /// </summary>
        Manufactured = 4,

        /// <summary>
        /// 完成质检
        /// </summary>
        QCChecked = 5,

        /// <summary>
        /// 完成称重
        /// </summary>
        Weighted = 6,

        /// <summary>
        /// 完成发货
        /// </summary>
        Deliveried = 7,

        /// <summary>
        /// 最终完成
        /// </summary>
        Complted = 8
    }
}
