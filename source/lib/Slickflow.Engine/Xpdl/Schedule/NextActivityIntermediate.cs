using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 网关类型的下一步节点（其下包含子节点)
    /// </summary>
    public class NextActivityIntermediate : NextActivityComponent
    {
        private IList<NextActivityComponent> nextActivityList = new List<NextActivityComponent>();

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="transition">转移</param>
        /// <param name="activity">活动</param>
        public NextActivityIntermediate(string name,
            Transition transition,
            Activity activity)
        {
            base.Name = name;
            base.Transition = transition;
            base.Activity = activity;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="nextActivity">活动节点</param>
        public override void Add(NextActivityComponent nextActivity)
        {
            nextActivityList.Add(nextActivity);
            if (hasChildren == false)
                hasChildren = true;
        }

        /// <summary>
        /// 移除子节点
        /// </summary>
        /// <param name="nextActivity">活动节点</param>
        /// <returns>删除状态</returns>
        public override bool Remove(NextActivityComponent nextActivity)
        {
            bool isRemoved = nextActivityList.Remove(nextActivity);
            if (nextActivityList.Count == 0)
                hasChildren = false;
            return isRemoved;
        }

        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        public override IEnumerator<NextActivityComponent> GetEnumerator()
        {
            foreach (NextActivityComponent c in nextActivityList)
            {
                yield return c;
            }
        }

        /// <summary>
        /// 下一步步骤节点列表
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
