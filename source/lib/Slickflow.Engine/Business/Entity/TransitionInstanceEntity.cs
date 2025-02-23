using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Transition Instance Entity
    /// 节点转移类
    /// </summary>
    [Table("WfTransitionInstance")]
    public class TransitionInstanceEntity
    {
        public int ID { get; set; }
        public string TransitionID { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ProcessID { get; set; }
        public byte TransitionType { get; set; }
        public byte FlyingType { get; set; }
        public int FromActivityInstanceID { get; set; }
        public string FromActivityID { get; set; }
        public short FromActivityType { get; set; }
        public string FromActivityName { get; set; }
        public int ToActivityInstanceID { get; set; }
        public string ToActivityID { get; set; }
        public short ToActivityType { get; set; }
        public string ToActivityName { get; set; }
        public byte ConditionParseResult { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public byte RecordStatusInvalid { get; set; }
    }
}
