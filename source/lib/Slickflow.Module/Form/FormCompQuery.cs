using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Query the form based on process information
    /// 根据流程信息查询表单
    /// </summary>
    public class FormCompQuery
    {
        public string ProcessGUID { get; set; }
        public string Version { get; set; }
    }
}
