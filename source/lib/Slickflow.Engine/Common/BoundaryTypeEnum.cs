using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Event Boundary Type
    /// </summary>
    public enum BoundaryTypeEnum : int
    {
        None = 0,

        Boundary = 10,

        Throw = 20,

        Catch = 21
    }
}
