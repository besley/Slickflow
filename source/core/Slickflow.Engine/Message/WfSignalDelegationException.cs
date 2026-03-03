using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Message
{
    /// <summary>
    /// Singal Delegattion Exception
    /// </summary>
    public class WfSignalDelegationException : ApplicationException
    {
        public WfSignalDelegationException(string message)
            : base(message)
        {

        }

        public WfSignalDelegationException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
