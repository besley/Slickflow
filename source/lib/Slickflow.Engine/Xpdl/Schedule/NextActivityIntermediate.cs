using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next Activity Intermediate
    /// 中间类型节点
    /// </summary>
    public class NextActivityIntermediate : NextActivityComponent
    {
        #region Property and Constructor
        private IList<NextActivityComponent> nextActivityList = new List<NextActivityComponent>();
        public NextActivityIntermediate(string name,
            Transition transition,
            Activity activity)
        {
            base.Name = name;
            base.Transition = transition;
            base.Activity = activity;
        }
        #endregion

        /// <summary>
        /// Add child
        /// 添加子节点
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
        /// 移除子节点
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
