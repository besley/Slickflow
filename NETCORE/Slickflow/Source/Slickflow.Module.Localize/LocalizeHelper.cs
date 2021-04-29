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
        /// 获取本地语言显示信息
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
        public static string GetGraphMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Graph, key);
            return localMessage;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <param name="message">附属消息</param>
        /// <returns>本地化信息</returns>
        public static string GetGraphMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.Graph, key, message);
            return localMessage;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <returns>本地化信息</returns>
        public static string GetWebTestMessage(string key)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.WebTest, key);
            return localMessage;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="key">显示项</param>
        /// <param name="message">附属消息</param>
        /// <returns>本地化信息</returns>
        public static string GetWebTestMessage(string key, string message)
        {
            var localMessage = LocalizeUtility.GetMessage(ProjectTypeEnum.WebTest, key, message);
            return localMessage;
        }

        /// <summary>
        /// 设置当前项目的语言类型
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="lang">语言</param>
        public static void SetLang(ProjectTypeEnum project, LangTypeEnum lang)
        {
            LanguageCachedHelper.SetLang(project, lang);
        }
    }
}
