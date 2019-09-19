using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Entity
{
    /// <summary>
    /// 用户邮件地址
    /// </summary>
    [Table("SysUser")]
    public class UserEMailEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string EMail { get; set; }
    }
}
