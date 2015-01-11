using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.MvcDemo.Data.Entity
{
    public class ProcessInstance
    {
        public int ID { get; set; }
        public string ProcessGUID { get; set; }
        public string ProcessName { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string AppInstanceCode { get; set; }
        public short ProcessState { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Nullable<int> ParentProcessInstanceID { get; set; }
        public string ParentProcessGUID { get; set; }
        public int InvokedActivityInstanceID { get; set; }
        public string InvokedActivityGUID { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public string LastUpdatedByUserID { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }
        public byte RecordStatusInvalid { get; set; }
        #region   一下为扩展
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 下一步审批者姓名
        /// </summary>
        public string AssignedToUserName { get; set; }
        #endregion
    }
}