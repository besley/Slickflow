using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.HrsService.Entity
{
    [Table("HrsLeaveOpinion")]
    public class HrsLeaveOpinionEntity
    {
        public int ID { get; set; }
        public string AppInstanceID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityGUID { get; set; }
        public string Remark { get; set; }
        public DateTime ChangedTime { get; set; }
        public string ChangedUserID { get; set; }
        public string ChangedUserName { get; set; }
    }
}