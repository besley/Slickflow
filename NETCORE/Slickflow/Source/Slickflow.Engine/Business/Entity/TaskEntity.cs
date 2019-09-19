using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 任务实体对象
    /// </summary>
    [Table("WfTasks")]
    public class TaskEntity
    {
        public int ID { get; set; }
        public int ActivityInstanceID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public string ActivityGUID { get; set; }
        public string ActivityName { get; set; }
        public short TaskType { get; set; }
        public short TaskState { get; set; }
        public Nullable<int> EntrustedTaskID { get; set; }        //被委托任务ID
        public string AssignedToUserID { get; set; }
        public string AssignedToUserName { get; set; }
        public byte IsEMailSent { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public string LastUpdatedByUserID { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        public byte RecordStatusInvalid { get; set; }
    }
}
