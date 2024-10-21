using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 转移定义
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// SequenceFlow ID
        /// </summary>
        public string ID
        { 
            get;
            set;
        }
        /// <summary>
        /// 转移GUID
        /// </summary>
        public String TransitionGUID
        {
            get;
            set;
        }

        /// <summary>
        /// 转移描述
        /// </summary>
        public String Description
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
        public ConditionDetail Condition
        {
            get;
            set;
        }

        /// <summary>
        /// 群体行为类型
        /// </summary>
        public GroupBehaviour GroupBehaviours
        {
            get;
            set;
        }

        /// <summary>
        /// 起始活动
        /// </summary>
        public Activity FromActivity
        {
            get;
            set;
        }

        /// <summary>
        /// 到达活动
        /// </summary>
        public Activity ToActivity
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 转移列表类
    /// </summary>
    public class TransitonList : List<Transition>
    {

    }
}
