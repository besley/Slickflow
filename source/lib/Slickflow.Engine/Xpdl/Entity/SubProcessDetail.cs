using Slickflow.Engine.Xpdl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 子流程属性配置详细
    /// </summary>
    public class SubProcessDetail
    {
        /// <summary>
        /// 子流程调用类型
        /// </summary>
        public SubProcessTypeEnum SubProcessType { get; set; }
        /// <summary>
        /// 子流程信息
        /// </summary>
        public string SubProcessGUID { get; set; }

        /// <summary>
        /// 子流程动态指定查询
        /// </summary>
        public string SubVariableName { get; set; }
    }
}
