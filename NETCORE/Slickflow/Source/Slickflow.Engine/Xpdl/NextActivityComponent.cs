using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 下一步节点的抽象类
    /// </summary>
    public abstract class NextActivityComponent
    {
        #region 属性列表
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 转移
        /// </summary>
        public TransitionEntity Transition
        {
            get;
            set;
        }

        /// <summary>
        /// 活动节点
        /// </summary>
        public ActivityEntity Activity
        {
            get;
            set;
        }

        protected bool hasChildren = false;
        /// <summary>
        /// 是否有子节点
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return hasChildren;
            }
        }
        #endregion
        
        /// <summary>
        /// 枚举
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<NextActivityComponent> GetEnumerator();
        
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="nextActivity"></param>
        public virtual void Add(NextActivityComponent nextActivity)
        {
            throw new InvalidOperationException("非组合节点，不能添加子项！");
        }

        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="nextActivity"></param>
        /// <returns></returns>
        public virtual bool Remove(NextActivityComponent nextActivity)
        {
            throw new InvalidOperationException("非组合节点，无子项可以删除！");
        }
    }
}
