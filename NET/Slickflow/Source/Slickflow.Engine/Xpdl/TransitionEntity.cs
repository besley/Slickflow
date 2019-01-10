/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 转移定义
    /// </summary>
    public class TransitionEntity
    {
        /// <summary>
        /// 转移GUID
        /// </summary>
        public String TransitionGUID
        {
            get;
            set;
        }

        /// <summary>
        /// 起始活动GUID
        /// </summary>
        public String FromActivityGUID
        {
            get;
            set;
        }

        /// <summary>
        /// 到达活动GUID
        /// </summary>
        public String ToActivityGUID
        {
            get;
            set;
        }

        /// <summary>
        /// 方向类型
        /// </summary>
        public TransitionDirectionTypeEnum DirectionType
        {
            get;
            set;
        }

        /// <summary>
        /// 接收者类型
        /// </summary>
        public Receiver Receiver
        {
            get;
            set;
        }

        /// <summary>
        /// 条件
        /// </summary>
        public ConditionEntity Condition
        {
            get;
            set;
        }

        /// <summary>
        /// 群体行为类型
        /// </summary>
        public GroupBehaviourEntity GroupBehaviour
        {
            get;
            set;
        }

        /// <summary>
        /// 起始活动
        /// </summary>
        public ActivityEntity FromActivity
        {
            get;
            set;
        }

        /// <summary>
        /// 到达活动
        /// </summary>
        public ActivityEntity ToActivity
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 转移列表类
    /// </summary>
    public class TransitonList : List<TransitionEntity>
    {

    }
}
