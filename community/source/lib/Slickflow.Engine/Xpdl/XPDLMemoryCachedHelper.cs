using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程文件缓存
    /// </summary>
    internal class XPDLMemoryCachedHelper
    {
        private static MemoryCache _xpdlCache;
        static XPDLMemoryCachedHelper()
        {
            var cacheOptions = new MemoryCacheOptions();
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromDays(1);
            _xpdlCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// 设置流程缓存
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="process">流程</param>
        internal static void SetXpdlCache(string processGUID, string version, Process process)
        {
            var str = string.Format("{0}-{1}", processGUID, version);
            var strMD5 = MD5Helper.GetMD5(str);

            _xpdlCache.Set<Process>(strMD5, process);
        }

        /// <summary>
        /// 获取流程缓存
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <returns>流程</returns>
        internal static Process GetXpdlCache(string processGUID, string version)
        {
            var str = string.Format("{0}-{1}", processGUID, version);
            var strMD5 = MD5Helper.GetMD5(str);
            Process process = null;

            _xpdlCache.TryGetValue<Process>(strMD5, out process);
            return process;
        }

        /// <summary>
        /// 更新流程缓存
        /// </summary>
        /// <param name="processGUID">流程GUID</param>
        /// <param name="version">版本</param>
        /// <param name="process">流程</param>
        /// <returns></returns>
        internal static Boolean TryUpdate(string processGUID, string version, Process process)
        {
            var str = string.Format("{0}-{1}", processGUID, version);
            var strMD5 = MD5Helper.GetMD5(str);
            Process processCached = null;

            _xpdlCache.TryGetValue(strMD5, out processCached);
            if (processCached != null)
            {
                _xpdlCache.Set<Process>(strMD5, process);
                return true;
            }
            return false;
        }
    }
}
