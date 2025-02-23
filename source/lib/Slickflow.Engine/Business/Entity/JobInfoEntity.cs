using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Job Info Entity
    /// 作业信息
    /// </summary>
    [Table("WfJobInfo")]
    public class JobInfoEntity
    {
        public int ID { get; set; }
        public string JobName { get; set; }
        public string Topic { get; set; }
        public string ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string Version { get; set; }
        public string ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityType { get; set; }
        public string TriggerType { get; set; }
        public string MessageDirection { get; set; }
        public string JobStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedUserID { get; set; }
        public string CreatedUserName { get; set; }
        public Nullable<DateTime> LastUpdatedDateTime { get; set; }
        public string LastUpdatedUserID { get; set; }
        public string LastUpdatedUserName { get; set; }
    }
}
