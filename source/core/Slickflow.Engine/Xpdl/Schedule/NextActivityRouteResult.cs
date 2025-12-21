using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Business;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next activity result category:
    /// 1.  If RouteChoiceType is single, assign a value to the NextActivity property;
    /// 2.  If RouteChoiceType is or multiple-choice, all must be selected, then assign a value to the NestedNextActiveList property.
    /// 下一步的活动结果类：
    /// 1. 如果RouteChoiceType为单一，则对NextActivity属性赋值；
    /// 2. 如果RouteChoiceType为或多选、必全选，则对NestedNextActivityList 属性赋值。
    /// </summary>
    public class NextActivityRouteResult
    {
        /// <summary>
        /// Route Choice Type
        /// 路由选择类型的枚举
        /// </summary>
        public NextActivityRouteChoiceEnum RouteChoiceType
        {
            get;
            set;
        }

        /// <summary>
        /// Normal Activity
        /// 下一步节点
        /// </summary>
        public Activity NormalActivity
        {
            get;
            set;
        }

        /// <summary>
        /// Gateway Activity
        /// 下一个路由节点
        /// </summary>
        public Activity GatewayActivity
        {
            get;
            set;
        }

        /// <summary>
        /// Children
        /// 下一步的节点列表
        /// </summary>
        public IList<NextActivityRouteResult> Children
        {
            get;
            set;
        }

        public NextActivityRouteResult(NextActivityRouteChoiceEnum routeChoiceType)
        {
            RouteChoiceType = routeChoiceType;
        }
    }
}
