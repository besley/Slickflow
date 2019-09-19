using System;
using System.Xml;
using Microsoft.Extensions.Caching.Memory;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// 内存中的缓存帮助类
    /// </summary>
    internal class MemoryCachedHelper
    {
        /// <summary>
        /// 缓存实例类
        /// </summary>
        private static MemoryCache _xpdlCache = null;

        static MemoryCachedHelper()
        {
            //"PROCESS_XPDL_CACHE"
            var cacheOptions = new MemoryCacheOptions();
            //cacheOptions.ExpirationScanFrequency = TimeSpan.FromSeconds(300);
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromDays(1);
            _xpdlCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// 设置流程文件缓存
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="xmlDoc">XML文档</param>
        internal static void SetXpdlCache(string processGUID, string version, XmlDocument xmlDoc)
        {
            var str = processGUID + version;
            var strMD5 = MD5Helper.GetMD5(str);

            _xpdlCache.Set<XmlDocument>(strMD5, xmlDoc);
        }

        /// <summary>
        /// 读取流程文件的缓存
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>XML对象</returns>
        internal static XmlDocument GetXpdlCache(string processGUID, string version)
        {
            XmlDocument xmlDoc = null;
            var str = processGUID + version;
            var strMD5 = MD5Helper.GetMD5(str);
            _xpdlCache.TryGetValue<XmlDocument>(strMD5, out xmlDoc);
            return xmlDoc;
        }

        /// <summary>
        /// 更新流程缓存
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="xmlDoc">xml文本</param>
        /// <returns>更新状态</returns>
        internal static bool TryUpdate(string processGUID, string version, XmlDocument xmlDoc)
        {
            var str = processGUID + version;
            var strMD5 = MD5Helper.GetMD5(str);

            XmlDocument xmlCached = null;
            _xpdlCache.TryGetValue(strMD5, out xmlCached);
            if (xmlCached != null)
            {
                _xpdlCache.Set<XmlDocument>(strMD5, xmlDoc);
                return true;
            }
            return false;
        }
    }
}
