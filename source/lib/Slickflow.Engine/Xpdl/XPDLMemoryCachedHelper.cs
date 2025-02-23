using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Slickflow.Engine.Config;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// XPDL memroy cachaed helper
    /// 流程文件缓存
    /// </summary>
    internal class XPDLMemoryCachedHelper
    {
        private static MemoryCache _xpdlCache;
        static XPDLMemoryCachedHelper()
        {
            var cacheOptions = new MemoryCacheOptions();
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromDays(WfConfig.EXPIRED_DAYS);        //default days is 1
            _xpdlCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// Set xpdl cache
        /// 设置流程缓存
        /// </summary>
        internal static void SetXpdlCache(string processID, string version, Process process)
        {
            var str = string.Format("{0}-{1}", processID, version);
            var strMD5 = MD5Helper.GetMD5(str);

            _xpdlCache.Set<Process>(strMD5, process);
        }

        /// <summary>
        /// Get xpdl cache
        /// 获取流程缓存
        /// </summary>
        internal static Process GetXpdlCache(string processID, string version)
        {
            var str = string.Format("{0}-{1}", processID, version);
            var strMD5 = MD5Helper.GetMD5(str);
            Process process = null;

            _xpdlCache.TryGetValue<Process>(strMD5, out process);
            return process;
        }

        /// <summary>
        /// Update xpdl cache
        /// 更新流程缓存
        /// </summary>
        internal static Boolean TryUpdate(string processID, string version, Process process)
        {
            var str = string.Format("{0}-{1}", processID, version);
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
