using System;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程变量定义实体（对应 BPMN sf:variables 中 Variable 属性）
    /// 表 wf_variable，仅存储变量定义（名称、类型、方向等），运行时值在 wf_process_variable
    /// </summary>
    [Table("wf_variable")]
    public class VariableEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("activity_id")]
        public string ActivityId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("direction")]
        public string Direction { get; set; }
        [Column("default_value")]
        public string DefaultValue { get; set; }
        [Column("is_required")]
        public short IsRequired { get; set; }
        [Column("is_referenced")]
        public short IsReferenced { get; set; }
        [Column("source_ref")]
        public string SourceRef { get; set; }
        [Column("source_variable_name")]
        public string SourceVariableName { get; set; }
        [Column("sort_order")]
        public int? SortOrder { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }
        [Column("updated_datetime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}
