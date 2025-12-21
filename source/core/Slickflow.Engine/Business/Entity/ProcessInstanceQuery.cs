using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process Instance Query
    /// 流程实例查询实体
    /// </summary>
    public class ProcessInstanceQuery : QueryBase
    {
        public string ProcessId { get; set; }
        public string ApplicationInstanceId { get; set; }
        public string AppName { get; set; }
    }
}
