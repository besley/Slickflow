using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Transition上的条件类
    /// </summary>
    public class ConditionDetail
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public ConditionTypeEnum ConditionType
        {
            get;
            set;
        }

        /// <summary>
        /// 条件表达式
        /// </summary>
        public string ConditionText
        {
            get;
            set;
        }
    }
}
