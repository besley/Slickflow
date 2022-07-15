using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程文件实体对象
    /// </summary>
    public class ProcessFileEntity
    {
        public String ProcessGUID { get; set; }
        public String ProcessName { get; set; }
        public String ProcessCode { get; set; }
        public String Version { get; set; }
        public byte IsUsing { get; set; }
        public Byte StartType { get; set; }
        public String StartExpression { get; set; }
        public Byte EndType { get; set; }
        public String EndExpression { get; set; }
        public String Description { get; set; }
        public String XmlContent { get; set; }
        public ProcessTemplateType TemplateType { get; set; }
    }

    /// <summary>
    /// 泳道流程实体对象
    /// </summary>
    public class ProcessFilePool
    {
        public List<ProcessEntity> ProcessEntityList { get; set; }
        public string XmlContent { get; set; }
    }

}
