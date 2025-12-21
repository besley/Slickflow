using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Core
{
    /// <summary>
    /// The parameter class returned by rollback
    /// 回退返回的参数类
    /// </summary>
    public class ReturnDataContext
    {
        public int ActivityInstanceId { get; set; }
        public int ProcessInstanceId { get; set; }
    }
}
