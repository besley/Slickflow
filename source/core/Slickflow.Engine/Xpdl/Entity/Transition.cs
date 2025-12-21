using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Transition
    /// </summary>
    public class Transition
    {
        public String TransitionId
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public String FromActivityId
        {
            get;
            set;
        }

        public String ToActivityId
        {
            get;
            set;
        }

        /// <summary>
        /// Direction Type
        /// 方向类型
        /// </summary>
        public TransitionDirectionTypeEnum DirectionType
        {
            get;
            set;
        }

        /// <summary>
        /// Receiver
        /// 接收者类型
        /// </summary>
        public Receiver Receiver
        {
            get;
            set;
        }

        public ConditionDetail Condition
        {
            get;
            set;
        }

        /// <summary>
        /// Group behavior types
        /// 群体行为类型
        /// </summary>
        public GroupBehaviour GroupBehaviours
        {
            get;
            set;
        }

        public Activity FromActivity
        {
            get;
            set;
        }

        public Activity ToActivity
        {
            get;
            set;
        }
    }

    public class TransitonList : List<Transition>
    {

    }
}
