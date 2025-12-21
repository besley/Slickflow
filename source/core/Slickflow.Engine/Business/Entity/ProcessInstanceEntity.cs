using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process Instance Entity
    /// 流程实例类
    /// </summary>
    [Table("wf_process_instance")]
    public class ProcessInstanceEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("process_name")]
        public string ProcessName { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("app_name")]
        public string AppName { get; set; }
        [Column("app_instance_id")]
        public string AppInstanceId { get; set; }
        [Column("app_instance_code")]
        public string AppInstanceCode { get; set; }
        [Column("process_state")]
        public short ProcessState { get; set; }
        [Column("sub_process_type")]
        public Nullable<short> SubProcessType { get; set; }
        [Column("sub_process_def_id")]
        public int SubProcessDefId { get; set; }
        [Column("sub_process_Id")]
        public string SubProcessId { get; set; }
        [Column("invoked_activity_instance_id")]
        public int InvokedActivityInstanceId { get; set; }
        [Column("invoked_activity_id")]
        public string InvokedActivityId { get; set; }
        [Column("job_timer_type")]
        public Nullable<short> JobTimerType { get; set; }
        [Column("job_timer_status")]
        public Nullable<short> JobTimerStatus { get; set; }
        [Column("trigger_expression")]
        public string TriggerExpression { get; set; }
        [Column("overdue_datetime")]
        public Nullable<DateTime> OverdueDateTime { get; set; }
        [Column("job_timer_treated_datetime")]
        public Nullable<DateTime> JobTimerTreatedDateTime { get; set; }
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
        [Column("ended_datetime")]
        public Nullable<DateTime> EndedDateTime { get; set; }
        [Column("ended_user_id")]
        public string EndedUserId { get; set; }
        [Column("ended_user_name")]
        public string EndedUserName { get; set; }
        [Column("record_status_invalid")]
        public byte RecordStatusInvalid { get; set; }
    }
}
