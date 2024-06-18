using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程实体类
    /// </summary>
    [Table("WfProcessVariable")]
    public class ProcessVariableEntity
    {
        public int ID { get; set; }
        public string VariableType { get; set; }
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ActivityGUID { get; set; }
        public string ActivityName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
    }
}
