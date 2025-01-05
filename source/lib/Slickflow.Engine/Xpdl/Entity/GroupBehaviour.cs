using System;
using Slickflow.Engine.Common;


namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Group Behaviour on Transiton
    /// </summary>
    public class GroupBehaviour
    {
        /// <summary>
        /// Default branch (OrSplit, if other branches do not meet the criteria, select the default branch)
        /// 默认分支(OrSplit，其它分支不满足条件，选择默认分支)
        /// </summary>
        public Boolean DefaultBranch
        {
            get;set;
        }

        /// <summary>
        /// Priority (for XOrSplit branch type)
        /// 优先级(用于XOrSplit分支类型）
        /// </summary>
        public short Priority
        {
            get;set;
        }

        /// <summary>
        /// Mandatory required option (for EORJoin merge type)
        /// 强制必需选项(用于EOrJoin合并类型)
        /// </summary>
        public Boolean Forced
        {
            get;set;
        }

        /// <summary>
        /// Approval Options (for ApprovalorSplit, 1-Agree; 1-Reject)
        /// 审批选项(用于ApprovalOrSplit, 1-同意;-1-拒绝)
        /// </summary>
        public ApprovalStatusEnum Approval
        {
            get;set;
        }
    }
}
