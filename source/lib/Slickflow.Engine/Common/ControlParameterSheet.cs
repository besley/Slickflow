using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// List of dynamic variable control sheet
    /// 动态变量控制列表清单
    /// </summary>
    public class ControlParameterSheet
    {
        /// <summary>
        /// Conditional parameter variables, used for ConditionalStart/ConditionalTask
        /// 条件参数变量，用于ConditionalStart/ConditionalTask
        /// </summary>
        public IDictionary<string, string> ConditionalVariables { get; set; }

        /// <summary>
        /// 加签类型
        /// Run time (dynamic) signing of flow variables
        /// </summary>
        public string SignForwardType { get; set; }

        /// <summary>
        /// Approval rate type: count/percentage
        /// 加签通过率类型 count/percentage
        /// </summary>
        public string SignForwardCompareType { get; set; }

        /// <summary>
        /// Order of signing completion
        /// 加签完成次序
        /// </summary>
        public float SignForwardCompleteOrder { get; set; }

        /// <summary>
        /// Handling the situation of returning as a countersignature node and running it again
        /// </summary>
        public byte RecreatedMultipleInstanceWhenResending { get; set; }

        /// <summary>
        /// Enhance merged control variables
        /// 增强合并控制变量
        /// </summary>
        public int EOrJoinTokenPassCount { get; set; }

        /// <summary>
        /// Wether to cancel adjacent nodes
        /// 是否取消相邻节点
        /// </summary>
        public byte IsCancellingBrothersNode { get; set; }
    }
}
