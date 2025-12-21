using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Job Info Entity
    /// 作业信息
    /// </summary>
    [Table("wf_job_info")]
    public class JobInfoEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("job_name")]
        public string JobName { get; set; }
        [Column("topic")]
        public string Topic { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("process_name")]
        public string ProcessName { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("activity_id")]
        public string ActivityId { get; set; }
        [Column("activity_name")]
        public string ActivityName { get; set; }
        [Column("activity_type")]
        public string ActivityType { get; set; }
        [Column("trigger_type")]
        public string TriggerType { get; set; }
        [Column("message_direction")]
        public string MessageDirection { get; set; }
        [Column("job_status")]
        public string JobStatus { get; set; }
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }
        [Column("created_user_id")]
        public string CreatedUserId { get; set; }
        [Column("created_user_name")]
        public string CreatedUserName { get; set; }
        [Column("updated_datetime")]
        public Nullable<DateTime> UpdatedDateTime { get; set; }
        [Column("updated_user_id")]
        public string UpdatedUserId { get; set; }
        [Column("updated_user_name")]
        public string UpdatedUserName { get; set; }
    }
}
