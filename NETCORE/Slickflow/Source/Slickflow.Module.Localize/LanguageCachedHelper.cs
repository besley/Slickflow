using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// 内存中的缓存帮助类
    /// </summary>
    internal class LanguageCachedHelper
    {
        /// <summary>
        /// 缓存实例类
        /// </summary>
        private static MemoryCache _lanJsonResourceCache = null;

        static LanguageCachedHelper()
        {
            var cacheOptions = new MemoryCacheOptions();
            //cacheOptions.ExpirationScanFrequency = TimeSpan.FromSeconds(300);
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromDays(1);
            _lanJsonResourceCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// 判断内存项是否存在
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        internal static bool ContainsKey(ProjectTypeEnum project)
        {
            Dictionary<LangTypeEnum, Dictionary<string, string>> resources = null;
            var isExist = _lanJsonResourceCache.TryGetValue(project, out resources);
            return isExist;
        }

        /// <summary>
        /// 设置语言资源缓存
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="jsonResource">语言资源</param>
        internal static void SetJsonResource(ProjectTypeEnum project, 
            Dictionary<LangTypeEnum, Dictionary<string, string>> jsonResource)
        {
            _lanJsonResourceCache.Set(project, jsonResource);
        }

        /// <summary>
        /// 获取语言资源
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>语言资源</returns>
        internal static Dictionary<LangTypeEnum, Dictionary<string, string>> GetJsonResource(ProjectTypeEnum project)
        {
            Dictionary<LangTypeEnum, Dictionary<string, string>> resources = null;
            _lanJsonResourceCache.TryGetValue(project, out resources);
            return resources;
        }

        /// <summary>
        /// 更新语言资源
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="jsonResource">语言资源</param>
        /// <returns></returns>
        internal static bool TryUpdate(ProjectTypeEnum project, 
            Dictionary<LangTypeEnum, Dictionary<string, string>> jsonResource)
        {
            var originalResource = GetJsonResource(project);
            if (originalResource != null)
            {
                _lanJsonResourceCache.Set(project, jsonResource);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="lang">语言</param>
        internal static void SetLang(ProjectTypeEnum project, LangTypeEnum lang)
        {
            var key = string.Format("{0}-{1}", project.ToString().ToUpper(), "LANG");
            _lanJsonResourceCache.Set<LangTypeEnum>(key, lang);
        }

        /// <summary>
        /// 获取当前语言
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>语言</returns>
        internal static LangTypeEnum GetLang(ProjectTypeEnum project)
        {
            var key = string.Format("{0}-{1}", project.ToString().ToUpper(), "LANG");
            return _lanJsonResourceCache.Get<LangTypeEnum>(key);
        }
    }
}
