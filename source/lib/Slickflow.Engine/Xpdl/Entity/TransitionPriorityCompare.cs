using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 按连线的优先级比较
    /// </summary>
    public class TransitionPriorityCompare : IComparer<Transition>
    {
        #region IComparer<TransitionEntity> 成员
        /// <summary>
        /// 比较实现
        /// </summary>
        /// <param name="x">实体X</param>
        /// <param name="y">实体Y</param>
        /// <returns>比较结果</returns>
        public int Compare(Transition x, Transition y)
        {
            if (x.GroupBehaviours != null 
                && y.GroupBehaviours != null 
                && x.GroupBehaviours.Priority < y.GroupBehaviours.Priority)
                return -1;

            if (x.GroupBehaviours != null
                && y.GroupBehaviours != null 
                && x.GroupBehaviours.Priority > y.GroupBehaviours.Priority)
                return 1;
            
            if (x.GroupBehaviours != null
                && y.GroupBehaviours != null 
                && x.GroupBehaviours.Priority == y.GroupBehaviours.Priority)
                return 0;

            return 0;
        }

        #endregion
    }
}
