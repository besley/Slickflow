using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Activity Instance Entity
    /// 活动实例的实体对象
    /// </summary>
    [Table("wf_activity_instance")]
    public class ActivityInstanceEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("process_instance_id")]
        public int ProcessInstanceId { get; set; }
        [Column("app_name")]
        public string AppName { get; set; }
        [Column("app_instance_id")]
        public string AppInstanceId { get; set; }
        [Column("app_instance_code")]
        public string AppInstanceCode { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("activity_id")]
        public string ActivityId { get; set; }
        [Column("activity_name")]
        public string ActivityName { get; set; }
        [Column("activity_code")]
        public string ActivityCode { get; set; }
        [Column("activity_type")]
        public short ActivityType { get; set; }
        [Column("activity_state")]
        public short ActivityState { get; set; }
        [Column("work_item_type")]
        public short WorkItemType { get; set; }
        [Column("assigned_user_ids")]
        public string AssignedUserIds { get; set; }
        [Column("assigned_user_names")]
        public string AssignedUserNames { get; set; }
        [Column("gateway_direction_type_id")]
        public Nullable<short> GatewayDirectionTypeId { get; set; }
        [Column("cannot_renew_instance")]
        public byte CanNotRenewInstance { get; set; }
        [Column("approval_status")]
        public short ApprovalStatus { get; set; }
        [Column("tokens_required")]
        public int TokensRequired { get; set; }
        [Column("tokens_had")]
        public int TokensHad { get; set; }
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
        /// <summary>
        /// Sign Together /Sign Forward Type
        /// 会签或加签类型       
        /// </summary>
        [Column("complex_type")]
        public Nullable<short> ComplexType { get; set; }
        /// <summary>
        /// Sign Together Execution Type: Sequecne / Parallel
        /// 会签执行类型：串行或并行
        /// </summary>
        [Column("merge_type")]
        public Nullable<short> MergeType { get; set; }
        [Column("main_activity_instance_id")]
        public Nullable<int> MainActivityInstanceId { get; set; }
        /// <summary>
        /// Sign Together Passover Type: Count / Percentage
        /// 会签通过率类型：个数或百分比
        /// </summary>
        [Column("compare_type")]
        public Nullable<short> CompareType { get; set; }
        /// <summary>
        /// Sign Together Order
        /// Sequence:[1, 2, 3...]
        /// Prallele:[-1]
        /// 会签执行顺序：串行为1，2，3 并行为-1
        /// </summary>
        [Column("complete_order")]
        public Nullable<float> CompleteOrder { get; set; }
        [Column("sign_forward_type")]
        public Nullable<short> SignForwardType { get; set; }
        [Column("backward_type")]
        public short BackwardType { get; set; }
        [Column("back_src_activity_instance_id")]
        public Nullable<int> BackSrcActivityInstanceId { get; set; }
        [Column("back_org_activity_instance_id")]
        public Nullable<int> BackOrgActivityInstanceId { get; set; }
        [Column("next_step_performers")]
        public string NextStepPerformers { get; set; }
        [Column("created_user_id")]
        public string CreatedUserId { get; set; }
        [Column("created_user_name")]
        public string CreatedUserName { get; set; }
        [Column("created_datetime")]
        public System.DateTime CreatedDateTime { get; set; }
        [Column("updated_user_id")]
        public string UpdatedUserId { get; set; }
        [Column("updated_user_name")]
        public string UpdatedUserName { get; set; }
        [Column("updated_datetime")]
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }
        [Column("ended_datetime")]
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        [Column("ended_user_id")]
        public string EndedUserId { get; set; }
        [Column("ended_user_name")]
        public string EndedUserName { get; set; }
        [Column("record_status_invalid")]
        public byte RecordStatusInvalid { get; set; }
    }
}
