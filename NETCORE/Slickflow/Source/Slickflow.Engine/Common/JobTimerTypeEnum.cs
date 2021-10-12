using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 作业定时器类型
    /// </summary>
    public enum JobTimerTypeEnum
    {
        Timer = 1,

        Conditional = 2,

        EMail = 3,

        Message = 4
    }
}
