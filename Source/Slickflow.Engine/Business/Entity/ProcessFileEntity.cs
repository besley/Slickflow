using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程文件实体对象
    /// </summary>
    public class ProcessFileEntity
    {
        public String ProcessGUID { get; set; }
        public String ProcessName { get; set; }
        public String Description { get; set; }
        public String XmlContent { get; set; }
    }
}
