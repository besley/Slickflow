using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process Variable Entity
    /// 流程实体类
    /// </summary>
    [Table("wf_process_variable")]
    public class ProcessVariableEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("variable_scope")]
        public string VariableScope { get; set; }
        [Column("app_instance_id")]
        public string AppInstanceId { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("process_instance_id")]
        public int ProcessInstanceId { get; set; }
        [Column("activity_instance_id")]
        public int ActivityInstanceId { get; set; }
        [Column("activity_id")]
        public string ActivityId { get; set; }
        [Column("activity_name")]
        public string ActivityName { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("value")]
        public string Value { get; set; }
        [Column("media_type")]
        public string MediaType { get; set;  }
        [Column("updated_datetime")]
        public DateTime UpdatedDateTime { get; set; }
    }
}
