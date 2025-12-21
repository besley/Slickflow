using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// Localize Hlper
    /// 本地化语言帮助
    /// </summary>
    public class LocalizeHelper
    {
        /// <summary>
        /// Get Engine message
        /// 获取引擎项目显示内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEngineMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Engine, key);
            return localMessage;
        }

        /// <summary>
        /// Get Engine message
        /// 获取引擎项目显示内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetEngineMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Engine, key, message);
            return localMessage;
        }

        /// <summary>
        /// Get Designer Message
        /// 获取设计器项目信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDesignerMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Designer, key);
            return localMessage;
        }

        /// <summary>
        /// Get Designer Message
        /// 获取设计器项目信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetDesignerMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Designer, key, message);
            return localMessage;
        }

        
        /// <summary>
        /// Get Web Message
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetWebMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Web, key);
            return localMessage;
        }

        /// <summary>
        /// Get web message
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetWebMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Web, key, message);
            return localMessage;
        }

        /// <summary>
        /// Get Job Message
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJobMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Job, key);
            return localMessage;
        }

        /// <summary>
        /// Get Job Message
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetJobMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Job, key, message);
            return localMessage;
        }

        /// <summary>
        /// Set the language type for the current project
        /// 设置当前项目的语言类型
        /// </summary>
        /// <param name="lang">语言</param>
        public static void SetLang(LangTypeEnum lang)
        {
            LanguageKeyCachedHelper.SetLang(lang);
        }

        /// <summary>
        /// Get the language type for the current project
        /// 获取当前项目的语言类型
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static LangTypeEnum GetLang()
        {
            var lang = LanguageKeyCachedHelper.GetLang();
            return lang;
        }

        /// <summary>
        /// Set default language=zh
        /// 设置项目默认语言==中文
        /// </summary>
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
