using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Compare by priority of connections
    /// 按连线的优先级比较
    /// </summary>
    public class TransitionPriorityCompare : IComparer<Transition>
    {
        #region IComparer<TransitionEntity> Member
        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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
