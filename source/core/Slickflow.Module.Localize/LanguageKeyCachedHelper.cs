using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// Language Key Cache Helper
    /// 语言缓存帮助类
    /// </summary>
    public class LanguageKeyCachedHelper
    {
        private static MemoryCache _languageCache = null;
        static LanguageKeyCachedHelper()
        {
            var cacheOptions = new MemoryCacheOptions();
            //cacheOptions.ExpirationScanFrequency = TimeSpan.FromSeconds(300);
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromDays(1);
            _languageCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// Set current language
        /// 设置当前语言
        /// </summary>
        /// <param name="lang"></param>
        internal static void SetLang(LangTypeEnum lang)
        {
            _languageCache.Set<LangTypeEnum>("LANG", lang);
        }

        /// <summary>
        /// Get current language
        /// 获取当前语言
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        internal static LangTypeEnum GetLang()
        {
            var lang = _languageCache.Get<LangTypeEnum>("LANG");
            return lang;
        }
    }
}
