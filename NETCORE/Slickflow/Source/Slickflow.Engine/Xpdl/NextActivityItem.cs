using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 单一下一步节点（不包含子节点）
    /// </summary>
    public class NextActivityItem : NextActivityComponent
    {
        public NextActivityItem(string name, 
            TransitionEntity transition,
            ActivityEntity activity)
        {
            base.Name = name;
            base.Transition = transition;
            base.Activity = activity;
        }

        public override IEnumerator<NextActivityComponent> GetEnumerator()
        {
            return null;
        }
    }
}
