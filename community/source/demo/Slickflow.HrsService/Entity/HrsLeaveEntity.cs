using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.HrsService.Entity
{
    [Table("HrsLeave")]
    public class HrsLeaveEntity
    {
        public int ID { get; set; }
        public string LeaveType { get; set; }
        public decimal Days { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CurrentActivityText { get; set; }
        /// <summary>
        ///    NotStart = 0, 未启动，流程记录为空 (Not start)
        ///    Ready = 1, 准备状态 (Ready)
        ///    Running = 2, 运行状态 (Running)
        ///    Completed = 4, 完成 (Completed)
        ///    Suspended = 5,挂起 (Suspended)
        ///    Canceled = 6,取消 (Canceled)
        ///    Discarded = 7终止 (Terminated)
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }
        public string Opinions { get; set; }
        public string CreatedUserID { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        
    }
}