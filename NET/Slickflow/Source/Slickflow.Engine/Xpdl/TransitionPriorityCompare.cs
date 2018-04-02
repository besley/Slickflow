using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 按连线的优先级比较
    /// </summary>
    public class TransitionPriorityCompare : IComparer<TransitionEntity>
    {
        #region IComparer<TransitionEntity> 成员

        public int Compare(TransitionEntity x, TransitionEntity y)
        {
            if (x.GroupBehaviour.Priority > y.GroupBehaviour.Priority)
                return 1;

            if (x.GroupBehaviour.Priority < y.GroupBehaviour.Priority)
                return -1;
            else
                return 0;
            
        }

        #endregion
    }
}
