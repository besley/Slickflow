using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Field Query
    /// 字段查询
    /// </summary>
    public class FieldQuery
    {
        public string ProcessID { get; set; }
        public string ProcessVersion { get; set; }
        public string ProcessName { get; set; }
        public string ProcessCode { get; set; }
        public string ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public IList<Form> FormList { get; set; }

    }
}
