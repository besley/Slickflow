using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Business;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 审批转移检验类型
    /// </summary>
    internal enum ApprovedTransitionValidatedTypeEnum
    {
        Default = 0,

        Successed = 1,

        MultipleInstance = 2
    }

    /// <summary>
    /// 审批转移检验结果对象
    /// </summary>
    internal class ApprovedTransitionValidatedResult
    {
        internal List<Transition> TransitionList { get; set; }
        internal ApprovedTransitionValidatedTypeEnum ValidatedType { get; set; }

        internal static ApprovedTransitionValidatedResult Default()
        {
            var result = new ApprovedTransitionValidatedResult();
            result.ValidatedType = ApprovedTransitionValidatedTypeEnum.Default;
            return result;
        }

        internal static ApprovedTransitionValidatedResult Success()
        {
            var result = new ApprovedTransitionValidatedResult();
            result.ValidatedType = ApprovedTransitionValidatedTypeEnum.Successed;
            return result;
        }
    }

   
}
