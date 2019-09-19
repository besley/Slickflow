using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Entity
{
    /// <summary>
    /// Job Log Entity
    /// </summary>
    [Table("WhJobSchedule")]
    public class JobScheduleEntity
    {
        public int ID { get; set; }
        public byte ScheduleType { get; set; }
        public string ScheduleGUID { get; set; }
        public string ScheduleName { get; set; }
        public string Title { get; set; }
        public short Status { get; set; }
        public string CronExpression { get; set; }
        public Nullable<DateTime> LastUpdatedDateTime { get; set; }
        public string LastUpdatedByUserID { get; set; }
        public string LastUpdatedByUserName { get; set; }
    }
}
