using System;
using System.Collections.Generic;
using Slickflow.Module.Localize;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next Activity Component
    /// </summary>
    public abstract class NextActivityComponent
    {
        #region Property
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
        public Transition Transition
        {
            get;
            set;
        }
        public Activity Activity
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
        
        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<NextActivityComponent> GetEnumerator();
        
        /// <summary>
        /// Add Children
        /// 添加子节点
        /// </summary>
        /// <param name="nextActivity"></param>
        public virtual void Add(NextActivityComponent nextActivity)
        {
            throw new InvalidOperationException(LocalizeHelper.GetEngineMessage("nextactivitycomponent.Add.error"));
        }

        /// <summary>
        /// Remove Children
        /// 删除子节点
        /// </summary>
        /// <param name="nextActivity"></param>
        /// <returns></returns>
        public virtual bool Remove(NextActivityComponent nextActivity)
        {
            throw new InvalidOperationException(LocalizeHelper.GetEngineMessage("nextactivitycomponent.Remove.error"));
        }
    }
}
