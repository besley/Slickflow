using System;
using System.IO;
using System.Reflection;

namespace Slickflow.Engine.Config
{
    /// <summary>
    /// 流程参数配置列表
    /// </summary>
    internal class WfConfig
    {
        private static readonly int _expiredDays = 1;
        private static readonly string _externalServiceFile = "Plugin\\Slickflow.Module.External.dll";

        /// <summary>
        /// 获取过期时间设置参数
        /// </summary>
        /// <returns></returns>
        internal static int GetDefaultExpiredDays()
        {
            return _expiredDays;
        }

        /// <summary>
        /// 获取外部服务文件，加载
        /// </summary>
        /// <returns></returns>
        internal static Assembly LoadExternalServiceFile()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var serviceFile = Path.Combine(directory, _externalServiceFile);
            var assembly = Assembly.LoadFrom(serviceFile);

            return assembly;
        }
    }
}
