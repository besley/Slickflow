using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 流程校验结果
    /// </summary>
    public class ProcessValidateResult
    {
        /// <summary>
        /// 流程校验结果类型
        /// </summary>
        public ProcessValidateResultTypeEnum ProcessValidatedResultType { get; set; }
        public ProcessValidateResult()
        {
            ProcessValidatedResultType = ProcessValidateResultTypeEnum.None;
        }
    }
}
