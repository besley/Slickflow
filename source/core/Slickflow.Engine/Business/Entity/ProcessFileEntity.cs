using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process File Entity
    /// 流程文件实体对象
    /// </summary>
    [Table("wf_process")]
    public class ProcessFileEntity
    {
        [Column("process_id")]
        public String ProcessId { get; set; }
        [Column("process_name")]
        public String ProcessName { get; set; }
        [Column("process_code")]
        public String ProcessCode { get; set; }
        [Column("version")]
        public String Version { get; set; }
        [Column("status")]
        public byte Status { get; set; }
        [Column("start_type")]
        public Byte StartType { get; set; }
        [Column("start_expression")]
        public String StartExpression { get; set; }
        [Column("end_type")]
        public Byte EndType { get; set; }
        [Column("end_expression")]
        public String EndExpression { get; set; }
        [Column("description")]
        public String Description { get; set; }
        [Column("xml_content")]
        public String XmlContent { get; set; }
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
