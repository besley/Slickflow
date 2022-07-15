using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 触发器类型信息
    /// </summary>
    public class TriggerDetail
    {
        /// <summary>
        /// 触发器类型
        /// </summary>
        public TriggerTypeEnum TriggerType { get; set; }

        /// <summary>
        /// 消息捕获抛出类型
        /// </summary>
        public MessageDirectionEnum MessageDirection { get; set; }

        /// <summary>
        /// 事件类型表达式
        /// </summary>
        public string Expression { get; set; }
    }
}
