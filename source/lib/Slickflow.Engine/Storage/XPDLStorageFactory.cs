using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Manager;


namespace Slickflow.Engine.Storage
{
    /// <summary>
    /// 外部存储的工厂方法
    /// </summary>
    internal class XPDLStorageFactory
    {
        /// <summary>
        /// 创建外部存储方法实例
        /// </summary>
        /// <returns></returns>
        internal static IXPDLStorage CreateXPDLStorage()
        {
            IXPDLStorage localStorage = null;
            //读取本地文件XML存储方式
            var storageType = ConfigHelper.GetAppSettingString("WorkflowFileStorageType");
            if (!string.IsNullOrEmpty(storageType) && storageType == "localfile")
            {
                localStorage = new XPDLLocalStorage();
            }
            return localStorage;
        }
    }
}
