using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Module.Logging
{
    /// <summary>
    /// Job Log Entity
    /// </summary>
    [Table("WhJobLog")]
    public class JobLogEntity
    {
        public int ID { get; set; }
        public string JobType { get; set; }
        public string JobName { get; set; }
        public string JobKey { get; set; }
        public string RefClass { get; set; }
        public string RefIDs { get; set; }
        public short Status { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
    }
}
