using System;
using System.Collections.Generic;
using System.Linq;


namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 网关类型的下一步节点（其下包含子节点)
    /// </summary>
    public class NextActivityGateway : NextActivityComponent
    {
        private IList<NextActivityComponent> nextActivityList = new List<NextActivityComponent>();

        public NextActivityGateway(string name, 
            TransitionEntity transition,
            ActivityEntity activity)
        {
            base.Name = name;
            base.Transition = transition;
            base.Activity = activity;
        }

        public override void Add(NextActivityComponent nextActivity)
        {
            nextActivityList.Add(nextActivity);
            if (hasChildren == false)
                hasChildren = true;
        }

        public override bool Remove(NextActivityComponent nextActivity)
        {
            bool isRemoved = nextActivityList.Remove(nextActivity);
            if (nextActivityList.Count == 0)
                hasChildren = false;
            return isRemoved;
        }

        public override IEnumerator<NextActivityComponent> GetEnumerator()
        {
            foreach (NextActivityComponent c in nextActivityList)
            {
                yield return c;
            }
        }

        public List<NextActivityComponent> NextActivityList
        {
            get
            {
                return nextActivityList.ToList();
            }
        }
    }
}
