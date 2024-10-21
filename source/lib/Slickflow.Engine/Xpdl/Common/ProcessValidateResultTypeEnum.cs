using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 流程校验错误类型
    /// </summary>
    public enum ProcessValidateResultTypeEnum
    {
        /// <summary>
        /// 默认没有校验错误
        /// </summary>
        None = 0,

        /// <summary>
        /// 校验成功
        /// </summary>
        Successed = 1,

        /// <summary>
        /// 没有开始节点
        /// </summary>
        WithoutStartEvent = 2,

        /// <summary>
        /// 没有结束节点
        /// </summary>
        WithoutEndEvent = 3,

        /// <summary>
        /// 存在孤立活动
        /// </summary>
        WithoutStartEndPath = 4
    }
}
