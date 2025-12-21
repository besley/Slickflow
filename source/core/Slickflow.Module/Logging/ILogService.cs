using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Logging
{
    /// <summary>
    /// Log Service Interface
    /// 日志服务接口
    /// </summary>
    public interface ILogService
    {
        void Record(object logEntity);
    }
}
