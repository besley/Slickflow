using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Entity
{
    /// <summary>
    /// job hangfire entity
    /// </summary>
    public class JobHangFireEntity
    {
        public string ID { get; set; }
        public string Cron { get; set; }
        public string LastJobID { get; set; }
        public string LastJobState { get; set; }
        public Nullable<DateTime> CreateAt { get; set; }
    }
}
