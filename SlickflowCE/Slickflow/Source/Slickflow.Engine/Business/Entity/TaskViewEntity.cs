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
    /// 任务视图类
    /// </summary>
    [Table("vwWfActivityInstanceTasks")]
    public class TaskViewEntity
    {
        public int TaskID { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public string Version { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ActivityGUID { get; set; }
        public int ActivityInstanceID { get; set; }
        public string ActivityName { get; set; }
        public short ActivityType { get; set; }
        public short WorkItemType { get; set; }
        public string PreviousUserID { get; set; }          //上一步审核人ID
        public string PreviousUserName { get; set; }
        public string PreviousDateTime { get; set; }
        public short TaskType { get; set; }
        public Nullable<int> EntrustedTaskID { get; set; }        //被委托任务ID
        public string AssignedToUserID { get; set; }
        public string AssignedToUserName { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }
        public short TaskState { get; set; }
        public short ActivityState { get; set; }
        public byte RecordStatusInvalid { get; set; }
        public short ProcessState { get; set; }
    }
}
