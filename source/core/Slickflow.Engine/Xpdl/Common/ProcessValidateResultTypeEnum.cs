using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Process Validate Result Type
    /// 流程校验错误类型
    /// </summary>
    public enum ProcessValidateResultTypeEnum
    {
        /// <summary>
        /// No Error
        /// 默认没有校验错误
        /// </summary>
        None = 0,

        /// <summary>
        /// Validated successed
        /// 校验成功
        /// </summary>
        Successed = 1,

        /// <summary>
        /// Without start event
        /// 没有开始节点
        /// </summary>
        WithoutStartEvent = 2,

        /// <summary>
        /// Without end event
        /// 没有结束节点
        /// </summary>
        WithoutEndEvent = 3,

        /// <summary>
        /// Without start end path
        /// 存在孤立活动
        /// </summary>
        WithoutStartEndPath = 4
    }
}
