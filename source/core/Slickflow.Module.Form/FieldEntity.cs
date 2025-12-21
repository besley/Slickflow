using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Field Entity
    /// 字段实体
    /// </summary>
    public class FieldEntity
    {
        public string FieldId { get; set; }
        public string FieldName { get; set; }
        public string FieldCode { get; set; }
        public bool IsNotVisible { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
