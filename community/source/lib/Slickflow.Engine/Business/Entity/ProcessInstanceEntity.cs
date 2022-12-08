using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程实例类
    /// </summary>
    [Table("WfProcessInstance")]
    public class ProcessInstanceEntity
    {
        public int ID { get; set; }
        public string ProcessGUID { get; set; }
        public string ProcessName { get; set; }
        public string Version { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string AppInstanceCode { get; set; }
        public short ProcessState { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string SubProcessGUID { get; set; }
        public int InvokedActivityInstanceID { get; set; }
        public string InvokedActivityGUID { get; set; }
        public Nullable<short> JobTimerType { get; set; }
        public Nullable<short> JobTimerStatus { get; set; }
        public string TriggerExpression { get; set; }
        public Nullable<DateTime> OverdueDateTime { get; set; }
        public Nullable<DateTime> JobTimerTreatedDateTime { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public Nullable<DateTime> LastUpdatedDateTime { get; set; }
        public string LastUpdatedByUserID { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public Nullable<DateTime> EndedDateTime { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }
        public byte RecordStatusInvalid { get; set; }
    }
}
