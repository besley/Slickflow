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
        public String TransitionGUID
        {
            get;
            set;
        }

        public String FromActivityGUID
        {
            get;
            set;
        }

        public String ToActivityGUID
        {
            get;
            set;
        }

        public TransitionDirectionTypeEnum DirectionType
        {
            get;
            set;
        }

        public ConditionEntity Condition
        {
            get;
            set;
        }

        public GroupBehaviourEntity GroupBehaviour
        {
            get;
            set;
        }

        public ActivityEntity FromActivity
        {
            get;
            set;
        }

        public ActivityEntity ToActivity
        {
            get;
            set;
        }
    }

    public class TransitonList : List<TransitionEntity>
    {

    }
}
