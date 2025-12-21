using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Task Entity
    /// 任务实体对象
    /// </summary>
    [Table("wf_task")]
    public class TaskEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("activity_instance_id")]
        public int ActivityInstanceId { get; set; }
        [Column("process_instance_id")]
        public int ProcessInstanceId { get; set; }
        [Column("app_name")]
        public string AppName { get; set; }
        [Column("app_instance_id")]
        public string AppInstanceId { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("activity_id")]
        public string ActivityId { get; set; }
        [Column("activity_name")]
        public string ActivityName { get; set; }
        [Column("task_type")]
        public short TaskType { get; set; }
        [Column("task_state")]
        public short TaskState { get; set; }
        [Column("entrusted_task_id")]
        public Nullable<int> EntrustedTaskId { get; set; }        //被委托任务Id
        [Column("assigned_user_id")]
        public string AssignedUserId { get; set; }
        [Column("assigned_user_name")]
        public string AssignedUserName { get; set; }
        [Column("is_email_sent")]
        public byte IsEMailSent { get; set; }
        [Column("created_user_id")]
        public string CreatedUserId { get; set; }
        [Column("created_user_name")]
        public string CreatedUserName { get; set; }
        [Column("created_datetime")]
        public System.DateTime CreatedDateTime { get; set; }
        [Column("updated_datetime")]
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }
        [Column("updated_user_id")]
        public string UpdatedUserId { get; set; }
        [Column("updated_user_name")]
        public string UpdatedUserName { get; set; }
        [Column("ended_user_id")]
        public string EndedUserId { get; set; }
        [Column("ended_user_name")]
        public string EndedUserName { get; set; }
        [Column("ended_datetime")]
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        [Column("record_status_invalid")]
        public byte RecordStatusInvalid { get; set; }
    }
}
