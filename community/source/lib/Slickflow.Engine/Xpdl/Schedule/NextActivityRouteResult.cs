using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Business;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 下一步的活动结果类：
    /// 1. 如果RouteChoiceType为单一，则对NextActivity属性赋值；
    /// 2. 如果RouteChoiceType为或多选、必全选，则对NestedNextActivityList 属性赋值。
    /// </summary>
    public class NextActivityRouteResult
    {
         /// <summary>
        /// 路由选择类型的枚举
        /// </summary>
        public NextActivityRouteChoiceEnum RouteChoiceType
        {
            get;
            set;
        }

        /// <summary>
        /// 下一步节点
        /// </summary>
        public Activity NormalActivity
        {
            get;
            set;
        }

        /// <summary>
        /// 下一个路由节点
        /// </summary>
        public Activity GatewayActivity
        {
            get;
            set;
        }

        /// <summary>
        /// 下一步的节点列表
        /// </summary>
        public IList<NextActivityRouteResult> Children
        {
            get;
            set;
        }

        /// <summary>
        /// 下一步活动结果
        /// </summary>
        /// <param name="routeChoiceType">路由选择类型</param>
        public NextActivityRouteResult(NextActivityRouteChoiceEnum routeChoiceType)
        {
            RouteChoiceType = routeChoiceType;
        }
    }


}
