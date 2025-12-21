using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Task Entrusted Entity
    /// 任务委托实体
    /// </summary>
    public class TaskEntrustedEntity
    {
        /// <summary>
        /// 被委托任务Id
        /// </summary>
        public int TaskId { get; set; }
        public string RunnerId { get; set; }
        public string RunnerName { get; set; }
        public string EntrustToUserId { get; set; }
        public string EntrustToUserName { get; set; }
    }
}
