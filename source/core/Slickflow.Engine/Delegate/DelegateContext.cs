using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// Delegate execution context
    /// 代理执行上下文
    /// </summary>
    public class DelegateContext
    {
        public string AppInstanceId { get; set; }
        public string ProcessId { get; set; }
        public int ProcessInstanceId { get; set; }
        public int ActivityInstanceId { get; set; }
        public string ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public ActivityResource ActivityResource { get; set; }
    }
}
