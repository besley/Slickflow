using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Slickflow.Engine.Xpdl
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
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }
        
        /// <summary>
        /// Assembly 文件路径及名称
        /// </summary>
        public string AssemblyFullName { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string InterfaceFullName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
        #endregion
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum ActionTypeEnum
    {
        /// <summary>
        /// 外部方法
        /// </summary>
        ExternalMethod = 1,

        /// <summary>
        /// WebApi 接口
        /// </summary>
        WebApi = 2
    }
}
