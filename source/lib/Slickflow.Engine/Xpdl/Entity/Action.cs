using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl.Common;


namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// Action
    /// 操作实体
    /// </summary>
    public class Action
    {
        #region Property
        public ActionTypeEnum ActionType { get; set; }
        public FireTypeEnum FireType { get; set; }
        public ActionMethodEnum ActionMethod { get; set; }
        public SubMethodEnum SubMethod { get; set; }
        public string Arguments { get; set; }
        public string Expression { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public CodeInfo CodeInfo { get; set; }
        #endregion
    }

    /// <summary>
    /// Reflection component method
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
