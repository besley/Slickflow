using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Slickflow.Module.Localize
{
    public class LanguageKeyCachedHelper
    {
        /// <summary>
        /// 语言Key缓存
        /// </summary>
        private static MemoryCache _languageCache = null;

        static LanguageKeyCachedHelper()
        {
            var cacheOptions = new MemoryCacheOptions();
            //cacheOptions.ExpirationScanFrequency = TimeSpan.FromSeconds(300);
            cacheOptions.ExpirationScanFrequency = TimeSpan.MaxValue;
            _languageCache = new MemoryCache(cacheOptions);
        }

        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="lang">语言</param>
        internal static void SetLang(LangTypeEnum lang)
        {
            _languageCache.Set<LangTypeEnum>("LANG", lang);
        }

        /// <summary>
        /// 获取当前语言
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>语言</returns>
        internal static LangTypeEnum GetLang()
        {
            var lang = _languageCache.Get<LangTypeEnum>("LANG");
            return lang;
        }
    }
}
