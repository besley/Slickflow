using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Service Method Type
    /// </summary>
    public enum ServiceMethodEnum
    {
        None = 0,

        WebApi = 1,

        LocalService = 2,

        CSharpLibrary = 3,

        StoreProcedure = 4
    }
}
