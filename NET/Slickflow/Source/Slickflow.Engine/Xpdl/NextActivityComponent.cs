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
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public TransitionEntity Transition
        {
            get;
            set;
        }

        public ActivityEntity Activity
        {
            get;
            set;
        }

        protected bool hasChildren = false;
        public bool HasChildren
        {
            get
            {
                return hasChildren;
            }
        }
        #endregion

        protected abstract bool GetChildren();
        
        
        public abstract IEnumerator<NextActivityComponent> GetEnumerator();
        
        public virtual void Add(NextActivityComponent nextActivity)
        {
            throw new InvalidOperationException("非组合节点，不能添加子项！");
        }

        public virtual bool Remove(NextActivityComponent nextActivity)
        {
            throw new InvalidOperationException("非组合节点，无子项可以删除！");
        }
    }
}
