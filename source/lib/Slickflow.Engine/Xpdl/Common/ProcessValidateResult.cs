using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Process Validate Result
    /// </summary>
    public class ProcessValidateResult
    {
        public ProcessValidateResultTypeEnum ProcessValidatedResultType { get; set; }
        public ProcessValidateResult()
        {
            ProcessValidatedResultType = ProcessValidateResultTypeEnum.None;
        }
    }
}
