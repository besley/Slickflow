using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程变量的任务视图
    /// </summary>
    public class ProcessVariableTaskView
    {
        public int TaskID { get; set; }
        public string VariableType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
