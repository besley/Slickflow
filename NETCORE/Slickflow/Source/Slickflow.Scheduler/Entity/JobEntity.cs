using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slickflow.Scheduler.Entity
{
    /// <summary>
    /// Job Entity
    /// </summary>
    [Table("WfJobs")]
    public class JobEntity
    {
        public int ID { get; set; }
        public string RefClass { get; set; }
        public string RefIDs { get; set; }
        public string ScheduleName { get; set; }
        public short Status { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        
    }
}
