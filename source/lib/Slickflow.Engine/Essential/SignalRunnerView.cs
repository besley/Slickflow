using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Essential
{
    /// <summary>
    /// Signal Runner View
    /// 信号运行者对象视图
    /// </summary>
    public class SignalRunnerView
    {
        public ProcessEntity ProcessEntity { get; set; }
        public Activity ActivityEntity { get; set; }
        public WfAppRunner WfAppRunner { get; set; }
    }
}
