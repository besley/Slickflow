using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 任务查询实体对象
    /// </summary>
    public class TaskQuery : QueryBase
    {
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public string Version { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }

    }
}
