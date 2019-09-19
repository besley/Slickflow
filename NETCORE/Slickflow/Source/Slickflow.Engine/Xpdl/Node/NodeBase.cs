using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// 节点的基类
    /// </summary>
    public abstract class NodeBase
    {
        #region 属性和构造函数
        /// <summary>
        /// 节点定义属性
        /// </summary>
        private ActivityEntity _activity;
        /// <summary>
        /// 节点实例
        /// </summary>
        public ActivityInstanceEntity ActivityInstance
        {
            get;
            set;
        }
        

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentActivity"></param>
        public NodeBase(ActivityEntity currentActivity)
        {
            _activity = currentActivity;
        }
        #endregion
    }
}
