using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 接受者类型
    /// </summary>
    public enum ReceiverTypeEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 上级
        /// </summary>
        Superior = 1,

        /// <summary>
        /// 同级
        /// </summary>
        Compeer = 2,

        /// <summary>
        /// 下属
        /// </summary>
        Subordinate = 3,

        /// <summary>
        /// 流程发起人
        /// </summary>
        ProcessInitiator = 1000
    }
}
