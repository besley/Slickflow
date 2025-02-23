using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Activity Instance Entity
    /// 活动实例的实体对象
    /// </summary>
    [Table("WfActivityInstance")]
    public class ActivityInstanceEntity
    {
        public int ID { get; set; }
        public int ProcessInstanceID { get; set; }
        public string AppName { get; set; }
        public string AppInstanceID { get; set; }
        public string AppInstanceCode { get; set; }
        public string ProcessID { get; set; }
        public string ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public short ActivityType { get; set; }
        public short ActivityState { get; set; }
		public short WorkItemType { get; set; }
        public string AssignedToUserIDs { get; set; }
        public string AssignedToUserNames { get; set; }
        public Nullable<short> GatewayDirectionTypeID { get; set; }
        public byte CanNotRenewInstance { get; set; }
        public short ApprovalStatus { get; set; }
        public int TokensRequired { get; set; }
        public int TokensHad { get; set; }
        public Nullable<short> JobTimerType { get; set; }
        public Nullable<short> JobTimerStatus { get; set; }
        public string TriggerExpression { get; set; }
        public Nullable<DateTime> OverdueDateTime { get; set; }
        public Nullable<DateTime> JobTimerTreatedDateTime { get; set; }
        /// <summary>
        /// Sign Together /Sign Forward Type
        /// 会签或加签类型       
        /// </summary>
        public Nullable<short> ComplexType { get; set; }
        /// <summary>
        /// Sign Together Execution Type: Sequecne / Parallel
        /// 会签执行类型：串行或并行
        /// </summary>
        public Nullable<short> MergeType { get; set; }              
        public Nullable<int> MIHostActivityInstanceID { get; set; }
        /// <summary>
        /// Sign Together Passover Type: Count / Percentage
        /// 会签通过率类型：个数或百分比
        /// </summary>
        public Nullable<short> CompareType { get; set; }
        /// <summary>
        /// Sign Together Order
        /// Sequence:[1, 2, 3...]
        /// Prallele:[-1]
        /// 会签执行顺序：串行为1，2，3 并行为-1
        /// </summary>
        public Nullable<float> CompleteOrder { get; set; }          
        public Nullable<short> SignForwardType { get; set; }
        public short BackwardType { get; set; }
        public Nullable<int> BackSrcActivityInstanceID { get; set; }
        public Nullable<int> BackOrgActivityInstanceID { get; set; }
        public string NextStepPerformers { get; set; }
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
