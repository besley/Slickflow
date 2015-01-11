using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Slickflow.Engine.Business.Entity;

namespace Slickflow.MvcDemo.Data.Entity
{
    [Table("BizAppFlow")]
    public class BizAppFlowEntity
    {
        public string AppName { get; set; }
        public string ActivityName { get; set; }
        public string AppInstanceID { get; set; }
        public string Remark { get; set; }
        public DateTime ChangedTime { get; set; }
        public string ChangedUserID { get; set; }
        public string ChangedUserName { get; set; }
    }
}