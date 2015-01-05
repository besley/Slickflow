using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 任务委托实体
    /// </summary>
    public class TaskEntrustedEntity
    {
        public int ID { get; set; }
        public string RunnerID { get; set; }
        public string RunnerName { get; set; }
        public string EntrustToUserID { get; set; }
        public string EntrustToUserName { get; set; }
    }
}
