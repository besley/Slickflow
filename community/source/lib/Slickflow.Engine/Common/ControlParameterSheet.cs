using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 动态变量控制列表清单
    /// </summary>
    public class ControlParameterSheet
    {
        //条件参数变量，用于ConditionalStart/ConditionalTask
        public IDictionary<string, string> ConditionalVariables { get; set; }

        //运行时(动态)加签流转变量
        public string SignForwardType { get; set; }
        public string SignForwardCompareType { get; set; }
        public float SignForwardCompleteOrder { get; set; }

        //增强合并控制变量
        public int EOrJoinTokenPassCount { get; set; }

        //退回控制变量
        public byte IsCancellingBrothersNode { get; set; }
    }
}
