using System;
using System.IO;
using System.Reflection;

namespace Slickflow.Engine.Config
{
    /// <summary>
    /// Workflow Paramters Configuration
    /// 引擎环境变量配置
    /// </summary>
    internal class WfConfig
    {
        /// <summary>
        /// BPMN file cache expired days
        /// 缓存过期天数设置
        /// </summary>
        internal static readonly int EXPIRED_DAYS = 1;

        /// <summary>
        /// BPMN file cache enabled true/false
        /// 是否设置缓存选项
        /// </summary>
        internal static readonly bool EXPIRED_DAYS_ENABLED = false;

        /// <summary>
        /// Local Service File Path, When there is a localservice delegate event, 
        /// will fire to the external method
        /// 本地服务文件路径设置，用于调用外部服务dll
        /// </summary>
        internal static readonly string EXTERNAL_SERVICE_FILE_PATH = "Plugin\\Slickflow.Module.External.dll";
    }
}
