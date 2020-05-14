using System;
using System.Collections.Generic;
using Slickflow.Engine.Xpdl.Entity;


namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 单一下一步节点(不包含子节点）
    /// </summary>
    public class NextActivityItem : NextActivityComponent
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="transition">转移</param>
        /// <param name="activity">活动</param>
        public NextActivityItem(string name, 
            TransitionEntity transition,
            ActivityEntity activity)
        {
            base.Name = name;
            base.Transition = transition;
            base.Activity = activity;
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public override IEnumerator<NextActivityComponent> GetEnumerator()
        {
            return null;
        }
    }
}
