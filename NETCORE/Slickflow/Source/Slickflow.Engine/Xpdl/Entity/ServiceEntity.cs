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
    public class ServiceEntity
    {
        #region 属性列表
        /// <summary>
        /// 方法类型
        /// </summary>
        public ServiceMethodEnum Method { get; set; }

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

}
