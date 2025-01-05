using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Form
    /// 表单
    /// </summary>
    public class Form
    {
        public string FormID { get; set; }
        public string FormName { get; set; }
        public string FormCode { get; set; }   
        public IList<FieldActivityEdit> FieldActivityEditList { get; set; }
    }
}
