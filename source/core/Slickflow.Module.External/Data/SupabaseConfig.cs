using Slickflow.Module.External.Utility;
using System;

namespace Slickflow.Module.External.Data
{
    /// <summary>
    /// Supabase connection config. Reads from host appsettings.json (sfdapi) first:
    /// AiModelProvider:SupabaseProjectUrl, AiModelProvider:SupabaseServiceRoleKey.
    /// Falls back to environment variables if not set in appsettings.
    /// </summary>
    public static class SupabaseConfig
    {
        /// <summary>Supabase project URL (from appsettings or env SUPABASE_URL / SUPABASE_PROJECT_URL).</summary>
        public static string Url
        {
            get
            {
                var fromApp = HostAppConfigurationLoader.GetSupabaseProjectUrl();
                if (!string.IsNullOrWhiteSpace(fromApp))
                    return fromApp;
                return Environment.GetEnvironmentVariable("SUPABASE_URL")
                    ?? Environment.GetEnvironmentVariable("SUPABASE_PROJECT_URL")
                    ?? "";
            }
        }

        /// <summary>Supabase service_role or anon key (from appsettings or env).</summary>
        public static string ApiKey
        {
            get
            {
                var fromApp = HostAppConfigurationLoader.GetSupabaseServiceRoleKey();
                if (!string.IsNullOrWhiteSpace(fromApp))
                    return fromApp;
                return Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY")
                    ?? Environment.GetEnvironmentVariable("SUPABASE_SERVICE_ROLE_KEY")
                    ?? "";
            }
        }
    }
}
