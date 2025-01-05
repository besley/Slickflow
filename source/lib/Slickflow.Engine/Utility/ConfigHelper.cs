using System;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Reflection;
using Slickflow.Module.Localize;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// Config Helper
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// Obtain node information for application configuration
        /// 获取应用配置的节点信息
        /// </summary>
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
        /// Obtain node information of the connection string
        /// 获取连接串的节点信息
        /// </summary>
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
        /// Get the execution path of the current application
        /// 获取当前应用程序的执行路径
        /// </summary>
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
