using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Form field editing permission
    /// 表单字段编辑权限
    /// </summary>
    public class FieldActivityEdit
    {
        public string FieldName { get; set; }
        public bool IsNotVisible { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
