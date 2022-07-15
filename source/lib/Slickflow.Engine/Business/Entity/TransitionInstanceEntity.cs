using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 节点转移类
    /// </summary>
    [Table("WfTransitionInstance")]
    public class TransitionInstanceEntity
    {
        public int ID { get; set; }
        public string TransitionGUID { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public byte TransitionType { get; set; }
        public byte FlyingType { get; set; }
        public int FromActivityInstanceID { get; set; }
        public string FromActivityGUID { get; set; }
        public short FromActivityType { get; set; }
        public string FromActivityName { get; set; }
        public int ToActivityInstanceID { get; set; }
        public string ToActivityGUID { get; set; }
        public short ToActivityType { get; set; }
        public string ToActivityName { get; set; }
        public byte ConditionParseResult { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public byte RecordStatusInvalid { get; set; }
    }
}
