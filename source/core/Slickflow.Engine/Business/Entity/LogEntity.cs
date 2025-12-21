using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Log Entity
    /// 日志记录实体
    /// </summary>
    [Table("wf_log")]
    public class LogEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("event_type_id")]
        public int EventTypeId { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("severity")]
        public string Severity { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("stack_trace")]
        public string StackTrace { get; set; }
        [Column("inner_stack_trace")]
        public string InnerStackTrace { get; set; }
        [Column("request_data")]
        public string RequestData { get; set; }
        [Column("time_stamp")]
        public DateTime Timestamp { get; set; }
    }
}
