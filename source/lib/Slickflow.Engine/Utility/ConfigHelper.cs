using System;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Reflection;
using Slickflow.Module.Localize;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取应用配置的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingString(string key)
        {
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                return value;
            }
            catch (System.NullReferenceException ex)
            {
                throw new Exception(LocalizeHelper.GetEngineMessage("confighelper.GetAppSettingString.error", key));
            }
        }

        /// <summary>
        /// 获取连接串的节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
                return connectionString;
            }
            catch (System.NullReferenceException ex)
            {
                throw new Exception(LocalizeHelper.GetEngineMessage("confighelper.GetConnectionString.error", key));
            }
        }

        /// <summary>
        /// 获取当前应用程序的执行路径
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string directory = Path.GetDirectoryName(path);

            return directory;
        }
    }
}
