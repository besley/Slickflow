using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Storage
{
    /// <summary>
    /// Factory method for external storage
    /// 外部存储的工厂方法
    /// </summary>
    internal class XPDLStorageFactory
    {
        /// <summary>
        /// Create XPDL storage
        /// 创建外部存储方法实例
        /// </summary>
        /// <returns></returns>
        internal static IXPDLStorage CreateXPDLStorage()
        {
            IXPDLStorage localStorage = null;
            //读取本地文件XML存储方式
            //Read local file XML storage method
            var storageType = ConfigHelper.GetAppSettingString("WorkflowFileStorageType");
            if (!string.IsNullOrEmpty(storageType) && storageType == "localfile")
            {
                localStorage = new XPDLLocalStorage();
            }
            return localStorage;
        }
    }
}
