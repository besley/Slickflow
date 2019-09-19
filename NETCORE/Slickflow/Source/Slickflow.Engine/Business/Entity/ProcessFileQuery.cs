using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程xml文件查询实体
    /// </summary>
    public class ProcessFileQuery
    {
        public int ID { get; set; }
        public string ProcessGUID { get; set; }
        public string Version { get; set; }
    }
}
