using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Xml;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// 流程定义文件Cache
    /// </summary>
    internal class CachedHelper
    {
        private static readonly ConcurrentDictionary<string, XmlDocument> _xpdlCache = new ConcurrentDictionary<string, XmlDocument>();
        private static readonly ConcurrentDictionary<int, object> _fullEntityMapCache = new ConcurrentDictionary<int, object>();

        /// <summary>
        /// 设置流程文件缓存
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        internal static XmlDocument SetXpdlCache(string processGUID, string version, XmlDocument xmlDoc)
        {
            var str = processGUID + version;
            var strMD5 = MD5Helper.GetMD5(str);

            return _xpdlCache.GetOrAdd(strMD5, xmlDoc);
        }

        /// <summary>
        /// 读取流程文件的缓存
        /// </summary>
        /// <param name="processGUID"></param>
        /// <returns></returns>
        internal static XmlDocument GetXpdlCache(string processGUID, string version)
        {
            XmlDocument xmlDoc = null;
            var str = processGUID + version;
            var strMD5 = MD5Helper.GetMD5(str);      

            if (_xpdlCache.ContainsKey(strMD5))
            {
                xmlDoc = _xpdlCache[strMD5];
            }
            return xmlDoc;
        }

        /// <summary>
        /// 设置实体Reader转换的动态映射方法缓存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static object SetEntityCache(int id, object obj)
        {
            return _fullEntityMapCache.GetOrAdd(id, obj);
        }

        /// <summary>
        /// 读取实体Reader转换的动态映射方法缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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