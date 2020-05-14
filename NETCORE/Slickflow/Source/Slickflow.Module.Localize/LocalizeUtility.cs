using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Slickflow.Module.Localize
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public enum LangTypeEnum
    {
        zh = 0,
        en = 1
    }

    /// <summary>
    /// 项目类型
    /// </summary>
    public enum ProjectTypeEnum
    {
        Engine = 0,
        Designer = 1,
        WebTest = 2,
        Scheduler = 3
    }

    /// <summary>
    /// 本地化帮助工具
    /// </summary>
    public class LocalizeUtility
    {
        private static LangTypeEnum _langType;

        /// <summary>
        /// 静态构造方法
        /// </summary>
        static LocalizeUtility()
        {
            //Engine Localize Language
            var dictLangEngine = new Dictionary<LangTypeEnum, string>();
            dictLangEngine[LangTypeEnum.en] = "Slickflow.Module.Localize.Engine.en.json";
            dictLangEngine[LangTypeEnum.zh] = "Slickflow.Module.Localize.Engine.zh.json";
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Engine, dictLangEngine);

            //Designer Localize Language
            var dictLangDesigner = new Dictionary<LangTypeEnum, string>();
            dictLangDesigner[LangTypeEnum.en] = "Slickflow.Module.Localize.Designer.en.json";
            dictLangDesigner[LangTypeEnum.zh] = "Slickflow.Module.Localize.Designer.zh.json";
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.Designer, dictLangDesigner);

            //WebTest Localize Language
            var dictLangWebTest = new Dictionary<LangTypeEnum, string>();
            dictLangWebTest[LangTypeEnum.en] = "Slickflow.Module.Localize.WebTest.en.json";
            dictLangWebTest[LangTypeEnum.zh] = "Slickflow.Module.Localize.WebTest.zh.json";
            ReadProjectJSONResourceFromFile(ProjectTypeEnum.WebTest, dictLangWebTest);

            //Scheduler Localize Language
            //var dictLangScheduler = new Dictionary<LangTypeEnum, string>();
            //dictLangScheduler[LangTypeEnum.en] = "Slickflow.Module.Localize.Schduler.en.json";
            //dictLangScheduler[LangTypeEnum.zh] = "Slickflow.Module.Localize.Scheduler.zh.json";
            //ReadProjectJSONResourceFromFile(ProjectTypeEnum.Scheduler, dictLangScheduler);
        }

        static void ReadProjectJSONResourceFromFile(ProjectTypeEnum project, Dictionary<LangTypeEnum, string> jsonFile)
        {
            var langJsonResource = new Dictionary<LangTypeEnum, Dictionary<string, string>>();
            foreach (var item in jsonFile)
            {
                var jsonResource = ReadProjectJSONResourceFromFile(project, item.Value);
                langJsonResource.Add(item.Key, jsonResource);
            }
            LanguageCachedHelper.SetJsonResource(project, langJsonResource);
        }

        /// <summary>
        /// 从资源文件读取JSON数据
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="resourceName">资源名称</param>
        /// <returns>Key-Value字典</returns>
        private static Dictionary<string, string> ReadProjectJSONResourceFromFile(ProjectTypeEnum project, string resourceName)
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
            var lang = _langType;
            var lanJsonResource = LanguageCachedHelper.GetJsonResource(project);
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
