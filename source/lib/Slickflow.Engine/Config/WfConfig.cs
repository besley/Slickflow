using System;
using System.IO;
using System.Reflection;

namespace Slickflow.Engine.Config
{
    /// <summary>
    /// Workflow Configuration
    /// </summary>
    internal class WfConfig
    {
        /// <summary>
        /// BPMN file cache expired days
        /// </summary>
        internal static readonly int EXPIRED_DAYS = 1;

        /// <summary>
        /// BPMN file cache enabled true/false
        /// </summary>
        internal static readonly bool EXPIRED_DAYS_ENABLED = false;

        /// <summary>
        /// Local Service File Path
        /// </summary>
        internal static readonly string EXTERNAL_SERVICE_FILE_PATH = "Plugin\\Slickflow.Module.External.dll";
    }
}
