using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 活动视图
    /// </summary>
    public class ActivityView
    {
        public string ActivityGUID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityType { get; set; }
        public string TriggerType { get; set; }
        public string MessageDirection { get; set; }
        public string Expression { get; set; }
    }
}
