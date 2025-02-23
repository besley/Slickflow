using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Process Variable Query
    /// 流程变量查询实体
    /// </summary>
    public class ProcessVariableQuery
    {
        public ProcessVariableTypeEnum VariableType { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ActivityID { get; set; }
        public string Name { get; set; }
    }
}
