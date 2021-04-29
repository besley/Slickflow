using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl.Common;


namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 操作实体
    /// </summary>
    public class ActionEntity
    {
        #region 属性列表
        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionTypeEnum ActionType { get; set; }

        /// <summary>
        /// 事件触发类型
        /// </summary>
        public FireTypeEnum FireType { get; set; }

        /// <summary>
        /// 方法类型
        /// </summary>
        public ActionMethodEnum ActionMethod { get; set; }

        /// <summary>
        /// 子方法类型
        /// </summary>
        public SubMethodEnum SubMethod { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 反射方法配置信息
        /// </summary>
        public MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// 文本脚本代码
        /// </summary>
        public CodeInfo CodeInfo { get; set; }
        #endregion
    }

    /// <summary>
    /// 反射组件方法
    /// </summary>
    public class MethodInfo
    {
        /// <summary>
        /// Assembly Full Name
        /// </summary>
        public string AssemblyFullName { get; set; }

        /// <summary>
        /// Class Full Name
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        /// Class Constructor Parameter
        /// </summary>
        public object[] ConstructorParameters { get; set; }

        /// <summary>
        /// Method Name
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Method Parameters
        /// </summary>
        public object[] MethodParameters { get; set; }
    }

    /// <summary>
    /// Code Info
    /// </summary>
    public class CodeInfo
    {
        /// <summary>
        /// Code Script Text
        /// </summary>
        public string CodeText { get; set; }
    }
}
