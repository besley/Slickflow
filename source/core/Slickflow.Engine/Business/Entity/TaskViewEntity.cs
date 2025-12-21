using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Task View
    /// 任务视图类
    /// </summary>
    [Table("vw_wf_task_details")]
    public class TaskViewEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("app_name")]
        public string AppName { get; set; }

        [Column("app_instance_id")]
        public string AppInstanceId { get; set; }

        [Column("app_instance_code")]
        public string AppInstanceCode { get; set; }

        [Column("process_instance_id")]
        public int ProcessInstanceId { get; set; }

        [Column("process_id")]
        public string ProcessId { get; set; }

        [Column("version")]
        public string Version { get; set; }

        [Column("activity_id")]
        public string ActivityId { get; set; }

        [Column("activity_instance_id")]
        public int ActivityInstanceId { get; set; }

        [Column("activity_name")]
        public string ActivityName { get; set; }

        [Column("activity_code")]
        public string ActivityCode { get; set; }

        [Column("activity_type")]
        public short ActivityType { get; set; }

        [Column("work_item_type")]
        public short WorkItemType { get; set; }

        [Column("task_type")]
        public short TaskType { get; set; }

        [Column("sub_process_type")]
        public Nullable<short> SubProcessType { get; set; }

        [Column("sub_process_def_id")]
        public int SubProcessDefId { get; set; }

        [Column("sub_process_id")]
        public string SubProcessId { get; set; }

        [Column("entrusted_task_id")]
        public Nullable<int> EntrustedTaskId { get; set; }        //被委托任务Id

        [Column("main_activity_instance_id")]
        public Nullable<int> MainActivityInstanceId { get; set; }

        [Column("complete_order")]
        public Nullable<float> CompleteOrder { get; set; }

        [Column("assigned_user_id")]
        public string AssignedUserId { get; set; }

        [Column("assigned_user_name")]
        public string AssignedUserName { get; set; }

        [Column("is_email_sent")]
        public byte IsEMailSent { get; set; }

        [Column("approval_status")]
        public short ApprovalStatus { get; set; }

        [Column("created_datetime")]
        public System.DateTime CreatedDateTime { get; set; }

        [Column("updated_datetime")]
        public Nullable<DateTime> UpdatedDateTime { get; set; }

        [Column("ended_datetime")]
        public Nullable<System.DateTime> EndedDateTime { get; set; }

        [Column("ended_user_id")]
        public string EndedUserId { get; set; }

        [Column("ended_user_name")]
        public string EndedUserName { get; set; }

        [Column("task_state")]
        public short TaskState { get; set; }

        [Column("activity_state")]
        public short ActivityState { get; set; }

        [Column("main_activity_state")]
        public short MainActivityState { get; set; }

        [Column("record_status_invalid")]
        public byte RecordStatusInvalid { get; set; }

        [Column("process_state")]
        public short ProcessState { get; set; }
    }
}
