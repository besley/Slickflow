using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 任务取代实体
    /// </summary>
    public class TaskReplacedEntity
    {
        /// <summary>
        /// 任务被取代替换为新用户
        /// </summary>
        public string ReplacedByUserID { get; set; }
        public string ReplacedByUserName { get; set; }
    }
}
