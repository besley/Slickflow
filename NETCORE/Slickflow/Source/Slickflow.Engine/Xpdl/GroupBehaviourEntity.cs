using System;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 连线上的行为属性实体
    /// </summary>
    public class GroupBehaviourEntity
    {
        /// <summary>
        /// 优先级(用于Split分支类型）
        /// </summary>
        public short Priority
        {
            get;set;
        }

        /// <summary>
        /// 强制必需选项（用于Join合并类型）
        /// </summary>
        public Boolean Forced
        {
            get;set;
        }
    }
}
