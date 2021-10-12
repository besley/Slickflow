using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 节点类型详细信息
    /// </summary>
    public class ActivityTypeDetail
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        public ActivityTypeEnum ActivityType { get; set; }

        /// <summary>
        /// 触发器类型
        /// </summary>
        public TriggerTypeEnum TriggerType { get; set; }

        /// <summary>
        /// 消息捕获抛出类型
        /// </summary>
        public MessageDirectionEnum MessageDirection { get; set; }

        /// <summary>
        /// 会签加签类型
        /// </summary>
        public ComplexTypeEnum ComplexType { get; set; }

        /// <summary>
        /// 串行并行类型(会签加签)
        /// </summary>
        public MergeTypeEnum MergeType { get; set; }

        /// <summary>
        /// 会签加签节点的通过率设置类型
        /// </summary>
        public CompareTypeEnum CompareType { get; set; }

        /// <summary>
        /// 会签主节点记录的通过率
        /// </summary>
        public Nullable<float> CompleteOrder { get; set; }

        /// <summary>
        /// 加签类型
        /// </summary>
        public SignForwardTypeEnum SignForwardType { get; set; }

        /// <summary>
        /// 跳转信息
        /// </summary>
        public SkipInfo SkipInfo { get; set; }


        /// <summary>
        /// 子流程调用类型
        /// </summary>
        public SubProcessTypeEnum SubProcessType { get;set;}
        /// <summary>
        /// 子流程信息
        /// </summary>
        public String SubProcessGUID { get; set; }

        /// <summary>
        /// 子流程动态指定查询
        /// </summary>
        public string SubVariableName { get; set; }

        /// <summary>
        /// 事件类型表达式
        /// </summary>
        public String Expression { get; set; }
    }

    /// <summary>
    /// 触发器类型
    /// </summary>
    public enum TriggerTypeEnum
    {
        /// <summary>
        /// 默认是没有定时器
        /// </summary>
        None = 0,

        /// <summary>
        /// 定时器
        /// </summary>
        Timer = 1,

        /// <summary>
        /// 消息
        /// </summary>
        Message = 2,

        /// <summary>
        /// 条件
        /// </summary>
        Conditional = 3
    }

    /// <summary>
    /// 节点的其它附属类型
    /// </summary>
    public enum ComplexTypeEnum
    {
        /// <summary>
        /// 多实例-会签节点
        /// </summary>
        SignTogether = 1,

        /// <summary>
        /// 多实例-加签节点
        /// </summary>
        SignForward = 2
    }

    /// <summary>
    /// 会签节点合并类型
    /// </summary>
    public enum MergeTypeEnum
    {
        /// <summary>
        /// 串行
        /// </summary>
        Sequence = 1,

        /// <summary>
        /// 并行
        /// </summary>
        Parallel = 2
    }

    /// <summary>
    /// 会签节点的通过率设置类型
    /// </summary>
    public enum CompareTypeEnum
    {
        /// <summary>
        /// 个数
        /// </summary>
        Count = 1,

        /// <summary>
        /// 百分比
        /// </summary>
        Percentage = 2
    }

    /// <summary>
    /// 加签类型
    /// </summary>
    public enum SignForwardTypeEnum
    {
        /// <summary>
        /// 不加签
        /// </summary>
        None = 0,

        /// <summary>
        /// 前加签
        /// </summary>
        SignForwardBefore = 1,

        /// <summary>
        /// 后加签
        /// </summary>
        SignForwardBehind = 2,

        /// <summary>
        /// 并行加签
        /// </summary>
        SignForwardParallel = 3
    }

    /// <summary>
    /// 子流程调用类型
    /// </summary>
    public enum SubProcessTypeEnum
    {
        /// <summary>
        /// 固定
        /// </summary>

        Fixed = 0,

        /// <summary>
        /// 动态
        /// </summary>
        Dynamic = 1
    }

    /// <summary>
    /// 节点类型上描述的跳转信息
    /// </summary>
    public class SkipInfo
    {
        /// <summary>
        /// 是否跳转
        /// </summary>
        public Boolean IsSkip { get; set; }

        /// <summary>
        /// 跳转到的节点
        /// </summary>
        public string Skipto { get; set; }
    }
}
