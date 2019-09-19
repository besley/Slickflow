using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// 代理执行上下文
    /// </summary>
    public class DelegateContext
    {
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ActivityGUID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public ActivityResource ActivityResource { get; set; }
    }
}
