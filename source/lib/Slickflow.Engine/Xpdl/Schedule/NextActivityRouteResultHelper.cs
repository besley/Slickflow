﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;


namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Help class for the next activity routing list
    /// 下一步活动路由列表的帮助类
    /// </summary>
    internal class NextActivityRouteResultHelper
    {
        internal static NextActivityRouteResult CreateNextActivityRouteResult(Activity activity)
        {
            NextActivityRouteResult result;
            if (activity.ActivityType == ActivityTypeEnum.GatewayNode)
            {
                if (activity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplit)
                {
                    result = new NextActivityRouteResult(NextActivityRouteChoiceEnum.MustAll);
                }
                else
                {
                    result = new NextActivityRouteResult(NextActivityRouteChoiceEnum.OrMultiple);
                }
                result.GatewayActivity = activity;
            }
            else
            {
                result = new NextActivityRouteResult(NextActivityRouteChoiceEnum.Single);
                result.NormalActivity = activity;
            }
            return result;
        }

        /// <summary>
        /// Add child nodes to the next step list
        /// 给下一步列表中继续添加子节点
        /// </summary>
        /// <param name="nextActivityRoutedResult"></param>
        /// <param name="child"></param>
        internal static void AddNewNextActivityToRoutedList(ref NextActivityRouteResult nextActivityRoutedResult,
            NextActivityRouteResult child)
        {
            if (nextActivityRoutedResult.Children == null)
            {
                nextActivityRoutedResult.Children = new List<NextActivityRouteResult>();
            }

            if (child != null)
            {
                nextActivityRoutedResult.Children.Add(child);
            }
        }
    }
}
