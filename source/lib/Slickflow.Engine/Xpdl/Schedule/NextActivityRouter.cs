using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next node list (including child nodes)
    /// 下一步节点列表（其下包含子节点)
    /// </summary>
    public class NextActivityRouter : NextActivityComponent
    {
        private IList<NextActivityComponent> nextActivityList = new List<NextActivityComponent>();

        public NextActivityRouter(string name, 
            Transition transition,
            Activity activity)
        {
            base.Name = name;
            base.Transition = transition;
            base.Activity = activity;
        }

        /// <summary>
        /// Add child
        /// </summary>
        /// <param name="nextActivity"></param>
        public override void Add(NextActivityComponent nextActivity)
        {
            nextActivityList.Add(nextActivity);
            if (hasChildren == false)
                hasChildren = true;
        }

        /// <summary>
        /// Remove child
        /// </summary>
        /// <param name="nextActivity"></param>
        /// <returns></returns>
        public override bool Remove(NextActivityComponent nextActivity)
        {
            bool isRemoved = nextActivityList.Remove(nextActivity);
            if (nextActivityList.Count == 0)
                hasChildren = false;
            return isRemoved;
        }

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<NextActivityComponent> GetEnumerator()
        {
            foreach (NextActivityComponent c in nextActivityList)
            {
                yield return c;
            }
        }

        /// <summary>
        /// Next activity list
        /// </summary>
        public List<NextActivityComponent> NextActivityList
        {
            get
            {
                return nextActivityList.ToList();
            }
        }
    }
}
