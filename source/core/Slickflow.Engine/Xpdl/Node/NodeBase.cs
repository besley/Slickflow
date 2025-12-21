using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Utility;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    /// <summary>
    /// Node base
    /// </summary>
    public abstract class NodeBase
    {
        #region Property and Constructor
        internal Activity Activity
        {
            get;
            set;
        }

        public ActivityInstanceEntity ActivityInstance
        {
            get;
            set;
        }
        
        public NodeBase(Activity currentActivity)
        {
            Activity = currentActivity;
        }
        #endregion
    }
}
