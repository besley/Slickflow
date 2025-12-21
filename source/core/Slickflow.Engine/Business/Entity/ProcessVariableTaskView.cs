using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process Variable Task View
    /// 流程变量的任务视图
    /// </summary>
    public class ProcessVariableTaskView
    {
        public int TaskId { get; set; }
        public string VariableScope { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string MediaType { get; set;  }
    }
}
