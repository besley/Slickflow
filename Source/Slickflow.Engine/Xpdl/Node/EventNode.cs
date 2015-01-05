using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    internal class EventNode : NodeBase, IDynamicRunable
    {
        internal EventNode(ActivityEntity activity)
            : base(activity)
        {

        }

        #region IDynamicRunable Members

        public object InvokeMethod(TaskImplementDetail implementation, object[] userParameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
