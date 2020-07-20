using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 活动实例的实体对象
    /// </summary>
    [Table("WfActivityInstance")]
    public class ActivityInstanceEntity
    {
        public int ID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public string ActivityGUID { get; set; }
        public string ActivityName { get; set; }
        public short ActivityType { get; set; }
        public short ActivityState { get; set; }
        public short WorkItemType { get; set; }
        public string AssignedToUserIDs { get; set; }
        public string AssignedToUserNames { get; set; }
        public Nullable<short> GatewayDirectionTypeID { get; set; }
        public byte CanNotRenewInstance { get; set; }
        public int TokensRequired { get; set; }
        public int TokensHad { get; set; }
        public Nullable<short> ComplexType { get; set; }
        public Nullable<short> MergeType { get; set; }
        public Nullable<int> MIHostActivityInstanceID { get; set; }
        public Nullable<float> CompleteOrder { get; set; }
        public Nullable<short> SignForwardType { get; set; }
        public short BackwardType { get; set; }
        public Nullable<int> BackSrcActivityInstanceID { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public string LastUpdatedByUserID { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public Nullable<System.DateTime> EndedDateTime { get; set; }
        public string EndedByUserID { get; set; }
        public string EndedByUserName { get; set; }
        public byte RecordStatusInvalid { get; set; }
        
    }
}
