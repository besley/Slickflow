using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 下一步节点树对象
    /// </summary>
    public class NextActivityTreeResult
    {
        public string Message { get; set; }
        public IList<NodeView> StepList { get; set; }
    }
}
