using System;
using Slickflow.Engine.Common;


namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 连线上的行为属性实体
    /// </summary>
    public class GroupBehaviour
    {
        /// <summary>
        /// 默认分支(OrSplit，其它分支不满足条件，选择默认分支)
        /// </summary>
        public Boolean DefaultBranch
        {
            get;set;
        }

        /// <summary>
        /// 优先级(用于XOrSplit分支类型）
        /// </summary>
        public short Priority
        {
            get;set;
        }

        /// <summary>
        /// 强制必需选项(用于EOrJoin合并类型)
        /// </summary>
        public Boolean Forced
        {
            get;set;
        }

        /// <summary>
        /// 审批选项(用于ApprovalOrSplit, 1-同意;-1-拒绝)
        /// </summary>
        public ApprovalStatusEnum Approval
        {
            get;set;
        }
    }
}
