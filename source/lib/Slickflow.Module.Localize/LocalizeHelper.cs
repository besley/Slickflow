using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// 本地化语言帮助
    /// </summary>
    public class LocalizeHelper
    {
        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <returns>本地化信息</returns>
        public static string GetEngineMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Engine, key);
            return localMessage;
        }

        /// <summary>
        /// 获取本地语言显示信息=
        /// </summary>
        /// <param name="key">显示项</param>
        /// <param name="message">附属消息</param>
        /// <returns>本地化信息</returns>
        public static string GetEngineMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Engine, key, message);
            return localMessage;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <returns>本地化信息</returns>
        public static string GetDesignerMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Designer, key);
            return localMessage;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <param name="message">附属消息</param>
        /// <returns>本地化信息</returns>
        public static string GetDesignerMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Designer, key, message);
            return localMessage;
        }

        
        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <returns>本地化信息</returns>
        public static string GetWebMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Web, key);
            return localMessage;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <param name="message">附属消息</param>
        /// <returns>本地化信息</returns>
        public static string GetWebMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Web, key, message);
            return localMessage;
        }

        /// <summary>
        /// 设置当前项目的语言类型
        /// </summary>
        /// <param name="lang">语言</param>
        public static void SetLang(LangTypeEnum lang)
        {
            LanguageKeyCachedHelper.SetLang(lang);
        }

        /// <summary>
        /// 获取当前项目的语言类型
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>语言</returns>
        public static LangTypeEnum GetLang()
        {
            var lang = LanguageKeyCachedHelper.GetLang();
            return lang;
        }

        /// <summary>
        /// 设置项目默认语言==中文
        /// </summary>
        /// <param name="project">项目类型</param>
        public static void SetDefault()
        {
            var lang = LanguageKeyCachedHelper.GetLang();
            if (lang == LangTypeEnum.none)
            {
                LanguageKeyCachedHelper.SetLang(LangTypeEnum.zh);
            }
        }
    }
}
