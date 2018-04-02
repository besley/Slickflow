using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程模型工厂类
    /// </summary>
    internal class ProcessModelFactory
    {
        /// <summary>
        /// 创建流程模型实例
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程模型</returns>
        internal static IProcessModel Create(string processGUID, string version)
        {
            IProcessModel processModel = new ProcessModel(processGUID, version);
            return processModel;
        }
    }
}
