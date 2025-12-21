using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// Localization Help Tool
    /// 本地化帮助工具
    /// </summary>
    internal class LocalizeUtility
    {
        static LocalizeUtility()
        {
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Engine);
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Designer);
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Web);
        }

        /// <summary>
        /// Read json resource file
        /// 读取JSON资源文件
        /// </summary>
        /// <param name="project"></param>
        /// <param name="jsonFile"></param>
        static void ReadProjectJSONResourceFromFile(ProjectTypeEnum project)
        {
            var jsonFile = GetJSONFileNameByProject(project);
            try
            {
                var langJsonResource = new Dictionary<LangTypeEnum, Dictionary<string, string>>();
                foreach (var item in jsonFile)
                {
                    var jsonResource = ReadProjectJSONResourceFromFile(item.Value);
                    langJsonResource.Add(item.Key, jsonResource);
                }
                LanguageJsonResourceCachedHelper.SetJsonResource(project, langJsonResource);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred when reading json resource file, detai:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Get json data from resource file
        /// </summary>
        /// <param name="prjType"></param>
        /// <returns></returns>
        private static Dictionary<LangTypeEnum, string> GetJSONFileNameByProject(ProjectTypeEnum prjType)
        {
            var dictLang = new Dictionary<LangTypeEnum, string>();
            dictLang[LangTypeEnum.en] = string.Format("Slickflow.Module.Localize.{0}.en.json", prjType.ToString());
            dictLang[LangTypeEnum.zh] = string.Format("Slickflow.Module.Localize.{0}.zh.json", prjType.ToString());

            return dictLang;
        }

        /// <summary>
        /// Get json data from resource file
        /// 从资源文件读取JSON数据
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        private static Dictionary<string, string> ReadProjectJSONResourceFromFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonFile = reader.ReadToEnd(); //Make string equal to full file
                var JsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
                return JsonDictionary;
            }
        }

        /// <summary>
        /// Get language item
        /// 获取语言单条项目
        /// </summary>
        /// <param name="project"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetItem(ProjectTypeEnum project, string key)
        {
            var lang = LanguageKeyCachedHelper.GetLang();
            var lanJsonResource = LanguageJsonResourceCachedHelper.GetJsonResource(project);
            var value = lanJsonResource[lang][key];
            return value;
        }

        /// <summary>
        /// Get local language display information
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="project"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string GetMessage(ProjectTypeEnum project, string key)
        {
            var message = GetItem(project, key);
            return message;
        }

        /// <summary>
        /// Get local language display information
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="project"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static string GetMessage(ProjectTypeEnum project, string key, string message)
        {
            var fullMessage = string.Format("{0}:{1}", GetItem(project, key), message);
            return fullMessage;
        }
    }
}
