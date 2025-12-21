using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Process node field editing permission entity
    /// 流程节点字段编辑权限实体
    /// </summary>
    public class FieldActivityEditInfo
    {
        public int Id { get; set; }
        public int ProcessDefId { get; set; }
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessVersion { get; set; }
        public string ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int FormId { get; set; }
        public string FormVersion { get; set; }
        public string FormName { get; set; }
        public IList<FieldActivityEdit> FieldActivityEditList { get; set; } 
    }
}
