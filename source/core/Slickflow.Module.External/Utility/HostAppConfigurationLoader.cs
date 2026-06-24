using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Slickflow.Module.External.Utility
{
    /// <summary>
    /// Loads the host application's appsettings.json (e.g. sfdapi) so the plugin can read config.
    /// When the plugin runs inside sfdapi, the app base directory contains appsettings.json with Supabase keys.
    /// </summary>
    public static class HostAppConfigurationLoader
    {
        private const string AiModelProviderSection = "AiModelProvider";
        private const string SupabaseProjectUrlKey = "SupabaseProjectUrl";
        private const string SupabaseServiceRoleKeyKey = "SupabaseServiceRoleKey";

        private static IConfiguration _configuration;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets configuration built from host's appsettings.json (and appsettings.Development.json if present).
        /// Tries Directory.GetCurrentDirectory() first (so test project folder works when run via dotnet run),
        /// then AppContext.BaseDirectory, and uses the first base path that yields non-empty Supabase keys.
        /// </summary>
        public static IConfiguration GetConfiguration()
        {
            if (_configuration != null)
                return _configuration;
            lock (_lock)
            {
                if (_configuration != null)
                    return _configuration;
                var urlKey = $"{AiModelProviderSection}:{SupabaseProjectUrlKey}";
                var keyKey = $"{AiModelProviderSection}:{SupabaseServiceRoleKeyKey}";
                var baseDir = AppContext.BaseDirectory ?? "";
                var pathsToTry = new[]
                {
                    Directory.GetCurrentDirectory(),
                    baseDir,
                    Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."))
                };
                foreach (var basePath in pathsToTry)
                {
                    if (string.IsNullOrWhiteSpace(basePath) || !Directory.Exists(basePath))
                        continue;
                    var config = BuildConfigFromBasePath(basePath);
                    var url = config[urlKey]?.Trim();
                    var keyVal = config[keyKey]?.Trim();
                    if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(keyVal))
                    {
                        _configuration = config;
                        return _configuration;
                    }
                }
                var fallbackPath = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
                _configuration = BuildConfigFromBasePath(fallbackPath);
                return _configuration;
            }
        }

        /// <summary>
        /// Reads AiModelProvider:SupabaseProjectUrl from host appsettings.
        /// Tries AppContext.BaseDirectory first, then Directory.GetCurrentDirectory() if value is empty (e.g. when running test from project folder).
        /// </summary>
        public static string GetSupabaseProjectUrl()
        {
            var key = $"{AiModelProviderSection}:{SupabaseProjectUrlKey}";
            var v = (GetConfiguration()[key]?.Trim()) ?? "";
            if (!string.IsNullOrWhiteSpace(v)) return v;
            var fallback = BuildConfigFromBasePath(Directory.GetCurrentDirectory());
            return fallback[key]?.Trim() ?? "";
        }

        /// <summary>
        /// Reads AiModelProvider:SupabaseServiceRoleKey from host appsettings.
        /// Tries AppContext.BaseDirectory first, then Directory.GetCurrentDirectory() if value is empty.
        /// </summary>
        public static string GetSupabaseServiceRoleKey()
        {
            var key = $"{AiModelProviderSection}:{SupabaseServiceRoleKeyKey}";
            var v = (GetConfiguration()[key]?.Trim()) ?? "";
            if (!string.IsNullOrWhiteSpace(v)) return v;
            var fallback = BuildConfigFromBasePath(Directory.GetCurrentDirectory());
            return fallback[key]?.Trim() ?? "";
        }

        /// <summary>
        /// Builds configuration from a base path (no caching). Used as fallback when primary config has no Supabase keys.
        /// </summary>
        private static IConfiguration BuildConfigFromBasePath(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath) || !Directory.Exists(basePath))
                return new ConfigurationBuilder().Build();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false);
            return builder.Build();
        }
    }
}
