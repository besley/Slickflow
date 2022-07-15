using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Logging
{
    /// <summary>
    /// 日志服务接口
    /// </summary>
    public interface ILogService
    {
        void Record(object logEntity);
    }
}
