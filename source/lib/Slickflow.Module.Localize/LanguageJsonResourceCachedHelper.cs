using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// 语言JSON缓存
    /// </summary>
    public class LanguageJsonResourceCachedHelper
    {
        /// <summary>
        /// 缓存实例类
        /// </summary>
        private static MemoryCache _languageJsonResouceCache = null;
        static LanguageJsonResourceCachedHelper()
        {
            var cacheOptions = new MemoryCacheOptions();
            //cacheOptions.ExpirationScanFrequency = TimeSpan.FromSeconds(300);
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromDays(1);
            _languageJsonResouceCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// 设置语言资源缓存
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="jsonResource">语言资源</param>
        internal static void SetJsonResource(ProjectTypeEnum project,
            Dictionary<LangTypeEnum, Dictionary<string, string>> jsonResource)
        {
            _languageJsonResouceCache.Set(project, jsonResource);
        }

        /// <summary>
        /// 获取语言资源
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>语言资源</returns>
        internal static Dictionary<LangTypeEnum, Dictionary<string, string>> GetJsonResource(ProjectTypeEnum project)
        {
            Dictionary<LangTypeEnum, Dictionary<string, string>> resources = null;
            _languageJsonResouceCache.TryGetValue(project, out resources);
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
                _languageJsonResouceCache.Set(project, jsonResource);
                return true;
            }
            return false;
        }

    }
}
