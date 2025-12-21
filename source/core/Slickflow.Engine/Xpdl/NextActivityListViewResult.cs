using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Next Activity Tree Result
    /// </summary>
    public class NextActivityListViewResult
    {
        public string Message { get; set; }
        public IList<NodeView> StepList { get; set; }
    }
}
