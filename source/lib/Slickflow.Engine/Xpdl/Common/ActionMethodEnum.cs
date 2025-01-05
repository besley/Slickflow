using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Action Method Type
    /// </summary>
    public enum ActionMethodEnum
    {
        None = 0,

        LocalService = 1,

        CSharpLibrary = 2,

        WebApi = 3,

        SQL = 5,

        StoreProcedure = 7,

        Script = 9,

        Python = 11,

        PlugIn = 13
    }

    /// <summary>
    /// Sub Method Type
    /// </summary>
    public enum SubMethodEnum
    {
        None = 0,

        HttpGet = 1,

        HttpPost = 2,

        HttpPut = 3,

        HttpDelete = 4
    }
}
