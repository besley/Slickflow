using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Transition Instance Query
    /// 转移查询实体
    /// </summary>
    public class TransitionInstanceQuery
    {
        public string AppInstanceID { get; set; }
        public string ProcessID { get; set; }
        public string Version { get; set; }
    }
}
