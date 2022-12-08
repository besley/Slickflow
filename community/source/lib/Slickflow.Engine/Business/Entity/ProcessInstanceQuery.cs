using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程实例查询实体
    /// </summary>
    public class ProcessInstanceQuery : QueryBase
    {
        public int ProcessID { get; set; }
        public string ProcessGUID { get; set; }
        public string ApplicationInstanceID { get; set; }
        public string AppName { get; set; }
    }
}
