using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Logging
{
    /// <summary>
    /// 作业事件类型
    /// </summary>
    public enum JobTypeEnum
    {
        Normal = 0,

        Timer = 1,

        Message = 2,

        EMail = 3
    }
}
