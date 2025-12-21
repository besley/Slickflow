using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Essential
{
    /// <summary>
    /// Message Runner View
    /// 消息运行者对象视图
    /// </summary>
    public class MessageRunnerView
    {
        public ProcessEntity ProcessEntity { get; set; }
        public Activity ActivityEntity { get; set; }
        public WfAppRunner WfAppRunner { get; set; }
    }
}
