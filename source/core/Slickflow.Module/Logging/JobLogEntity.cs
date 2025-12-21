using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Module.Logging
{
    /// <summary>
    /// Job Log Entity
    /// 作业日志实体
    /// </summary>
    [Table("wf_job_log")]
    public class JobLogEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("job_type")]
        public string JobType { get; set; }
        [Column("job_name")]
        public string JobName { get; set; }
        [Column("job_key")]
        public string JobKey { get; set; }
        [Column("ref_class")]
        public string RefClass { get; set; }
        [Column("ref_ids")]
        public string RefIDs { get; set; }
        [Column("status")]
        public short Status { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("stack_trace")]
        public string StackTrace { get; set; }
        [Column("inner_stack_trace")]
        public string InnerStackTrace { get; set; }
        [Column("request_data")]
        public string RequestData { get; set; }
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }
        [Column("created_user_id")]
        public string CreatedUserId { get; set; }
        [Column("created_user_name")]
        public string CreatedUserName { get; set; }
    }
}
