using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Entity attribute process node binding information
    /// 实体属性流程节点绑定信息
    /// </summary>
    [Table("FbFormFieldActivityEdit")]
    public class FieldActivityEditEntity
    {
        public int Id { get; set; }
        public int ProcessDefId { get; set; }
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessVersion { get; set; }
        public string ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string FormVersion { get; set; }
        public string FieldsPermission { get; set; }
    }
}
