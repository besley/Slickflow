using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Core
{
    /// <summary>
    /// 回退返回的参数类
    /// </summary>
    public class ReturnDataContext
    {
        public int ActivityInstanceID { get; set; }
        public int ProcessInstanceID { get; set; }
    }
}
