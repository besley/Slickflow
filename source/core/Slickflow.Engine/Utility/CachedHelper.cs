using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Xml;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// Process definition file cache
    /// 流程定义文件Cache
    /// </summary>
    internal class CachedHelper
    {
        private static readonly ConcurrentDictionary<string, XmlDocument> _xpdlCache = new ConcurrentDictionary<string, XmlDocument>();
        private static readonly ConcurrentDictionary<int, object> _fullEntityMapCache = new ConcurrentDictionary<int, object>();

        /// <summary>
        /// Set Xpdl Cache
        /// </summary>
        internal static XmlDocument SetXpdlCache(string processId, string version, XmlDocument xmlDoc)
        {
            var str = processId + version;
            var strMD5 = MD5Helper.GetMD5(str);

            return _xpdlCache.GetOrAdd(strMD5, xmlDoc);
        }

        /// <summary>
        /// Read xpdl cache
        /// </summary>
        internal static XmlDocument GetXpdlCache(string processId, string version)
        {
            XmlDocument xmlDoc = null;
            var str = processId + version;
            var strMD5 = MD5Helper.GetMD5(str);      

            if (_xpdlCache.ContainsKey(strMD5))
            {
                xmlDoc = _xpdlCache[strMD5];
            }
            return xmlDoc;
        }

        /// <summary>
        /// update cache
        /// </summary>
        internal static bool TryUpdate(string processId, string version, XmlDocument xmlDoc)
        {
            var str = processId + version;
            var strMD5 = MD5Helper.GetMD5(str);

            if (_xpdlCache.ContainsKey(strMD5))
            {
                XmlDocument outXmlDoc;
                if (_xpdlCache.TryRemove(strMD5, out outXmlDoc))
                {
                    _xpdlCache.GetOrAdd(strMD5, xmlDoc);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Cache the dynamic mapping method for entity Reader conversion
        /// 设置实体Reader转换的动态映射方法缓存
        /// </summary>
        internal static object SetEntityCache(int id, object obj)
        {
            return _fullEntityMapCache.GetOrAdd(id, obj);
        }

        /// <summary>
        /// Cache the dynamic mapping method for reading entity Reader conversion
        /// 读取实体Reader转换的动态映射方法缓存
        /// </summary>
        internal static object GetEntityCache(int id)
        {
            object obj = null;
            if (_fullEntityMapCache.ContainsKey(id))
            {
                obj = _fullEntityMapCache[id];
            }
            return obj;
        }
    }
}