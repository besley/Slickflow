using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// 本地化帮助工具
    /// </summary>
    internal class LocalizeUtility
    {
        /// <summary>
        /// 静态构造方法
        /// </summary>
        static LocalizeUtility()
        {
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Engine);
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Designer);
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Web);
        }

        /// <summary>
        /// 读取JSON资源文件
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="jsonFile">json文件</param>
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
                LocalizeCachedHelper.SetJsonResource(project, langJsonResource);
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException(string.Format("An error occurred when reading json resource file, detai:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 返回JSON文件资源
        /// </summary>
        /// <param name="prjType">项目类型</param>
        /// <returns>Key-Value字典对象</returns>
        private static Dictionary<LangTypeEnum, string> GetJSONFileNameByProject(ProjectTypeEnum prjType)
        {
            var dictLang = new Dictionary<LangTypeEnum, string>();
            dictLang[LangTypeEnum.en] = string.Format("Slickflow.Module.Localize.{0}.en.json", prjType.ToString());
            dictLang[LangTypeEnum.zh] = string.Format("Slickflow.Module.Localize.{0}.zh.json", prjType.ToString());

            return dictLang;
        }

        /// <summary>
        /// 从资源文件读取JSON数据
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <returns>Key-Value字典</returns>
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
        /// 获取应用配置的节点信息
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="key">键值</param>
        /// <returns>本地显示</returns>
        private static string GetItem(ProjectTypeEnum project, string key)
        {
            var lang = LanguageCachedHelper.GetLang(project);
            var lanJsonResource = LocalizeCachedHelper.GetJsonResource(project);
            var value = lanJsonResource[lang][key];
            return value;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="key">显示项</param>
        /// <returns></returns>
        internal static string GetMessage(ProjectTypeEnum project, string key)
        {
            var message = GetItem(project, key);
            return message;
        }

        /// <summary>
        /// 获取本地语言显示信息
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="key">显示项</param>
        /// <param name="message">附属消息</param>
        /// <returns></returns>
        internal static string GetMessage(ProjectTypeEnum project, string key, string message)
        {
            var fullMessage = string.Format("{0}:{1}", GetItem(project, key), message);
            return fullMessage;
        }
    }
}
