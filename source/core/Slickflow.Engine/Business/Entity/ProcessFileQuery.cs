using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process File Query
    /// 流程xml文件查询实体
    /// </summary>
    public class ProcessFileQuery
    {
        public int Id { get; set; }
        public string ProcessId { get; set; }
        public string Version { get; set; }
    }
}
