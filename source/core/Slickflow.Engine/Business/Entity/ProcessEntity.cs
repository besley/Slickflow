using System;
using System.Collections.Generic;


namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process Entity
    /// 流程实体类
    /// </summary>
    [Table("wf_process")]
    public class ProcessEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("process_name")]
        public string ProcessName { get; set; }
        [Column("process_code")]
        public string ProcessCode { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("status")]
        public byte Status { get; set; } 
        [Column("app_type")]
        public string AppType { get; set; }
        [Column("package_type")]
        public Nullable<byte> PackageType { get; set; }
        [Column("package_id")]
        public Nullable<int> PackageId { get; set; }
        [Column("participant_guid")]
        public string ParticipantGUID { get; set; }
        [Column("page_url")]
        public string PageUrl { get; set; }
        [Column("xml_file_name")]
        public string XmlFileName { get; set; }
        [Column("xml_file_path")]
        public string XmlFilePath { get; set; }
        [Column("xml_content")]
        public string XmlContent { get; set; }
        [Column("start_type")]
        public byte StartType { get; set; }
        [Column("start_expression")]
        public string StartExpression { get; set; }
        [Column("end_type")]
        public byte EndType { get; set; }
        [Column("end_expression")]
        public string EndExpression { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }
        [Column("updated_datetime")]
        public Nullable<DateTime> UpdatedDateTime { get; set; }
    }
}
