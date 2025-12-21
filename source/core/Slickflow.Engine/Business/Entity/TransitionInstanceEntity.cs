using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Transition Instance Entity
    /// 节点转移类
    /// </summary>
    [Table("wf_transition_instance")]
    public class TransitionInstanceEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("transition_id")]
        public string TransitionId { get; set; }
        [Column("app_name")]
        public string AppName { get; set; }
        [Column("app_instance_id")]
        public string AppInstanceId { get; set; }
        [Column("process_instance_id")]
        public int ProcessInstanceId { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("transition_type")]
        public byte TransitionType { get; set; }
        [Column("flying_type")]
        public byte FlyingType { get; set; }
        [Column("from_activity_instance_id")]
        public int FromActivityInstanceId { get; set; }
        [Column("from_activity_id")]
        public string FromActivityId { get; set; }
        [Column("from_activity_type")]
        public short FromActivityType { get; set; }
        [Column("from_activity_name")]
        public string FromActivityName { get; set; }
        [Column("to_activity_instance_id")]
        public int ToActivityInstanceId { get; set; }
        [Column("to_activity_id")]
        public string ToActivityId { get; set; }
        [Column("to_activity_type")]
        public short ToActivityType { get; set; }
        [Column("to_activity_name")]
        public string ToActivityName { get; set; }
        [Column("condition_parsed_result")]
        public byte ConditionParsedResult { get; set; }
        [Column("created_user_id")]
        public string CreatedUserId { get; set; }
        [Column("created_user_name")]
        public string CreatedUserName { get; set; }
        [Column("created_datetime")]
        public System.DateTime CreatedDateTime { get; set; }
        [Column("record_status_invalid")]
        public byte RecordStatusInvalid { get; set; }
    }
}
