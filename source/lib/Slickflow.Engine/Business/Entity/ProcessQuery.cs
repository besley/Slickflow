using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process Query
    /// 流程查询类
    /// </summary>
    public class ProcessQuery : QueryBase
    {
        public string ProcessID { get; set; }
        public string Version { get; set; }
        public string ProcessName { get; set; }
        public ProcessStartTypeEnum StartType { get; set; }
    }
}
