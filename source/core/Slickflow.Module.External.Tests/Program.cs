using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Slickflow.Engine.External;
using Slickflow.Module.External.Customer;
using Slickflow.Module.External.Customer.Entity;
using Slickflow.Module.External.Data;

namespace Slickflow.Module.External.Tests
{
    /// <summary>
    /// Simulates a customer–AI chat: sets user_message and ai_response,
    /// runs CustomerExtractService (extract → variable "customer"), CustomerSaveService (save → biz_customer), MessageService (save → biz_conversation).
    /// Configure Supabase in appsettings.json (AiModelProvider) or env SUPABASE_URL / SUPABASE_SERVICE_ROLE_KEY.
    /// </summary>
    internal static class Program
    {
        private const int TestProcessInstanceId = 90001;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            EnsureSupabaseConfigFromProjectAppSettings();
            Console.WriteLine("=== CustomerExtract / CustomerSave & MessageService test (Supabase) ===\n");

            var url = SupabaseConfig.Url;
            var key = SupabaseConfig.ApiKey;
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key))
            {
                Console.WriteLine("Supabase is not configured. Set in appsettings.json (AiModelProvider:SupabaseProjectUrl, SupabaseServiceRoleKey)");
                Console.WriteLine("or environment variables: SUPABASE_URL (or SUPABASE_PROJECT_URL), SUPABASE_SERVICE_ROLE_KEY (or SUPABASE_ANON_KEY).");
                Console.WriteLine("Then run again to save customer and conversation to Supabase.");
                Environment.ExitCode = 1;
                return;
            }
            Console.WriteLine("[Config] Supabase URL: {0}\n", url);

            // 1. Simulated customer message (contains name, phone, wechat for contact extraction)
            var userMessage = @"你好，我想咨询一下产品报价。我叫张三，手机号 13812345678，微信号 zhang_san_88，方便的话发下资料到邮箱 zhang@example.com。";
            // 2. Simulated AI reply
            var aiResponse = @"您好张三，感谢您的咨询。已记录您的联系方式（手机 13812345678，微信 zhang_san_88）。我们会将产品资料发送至 zhang@example.com，请注意查收。如有其他问题欢迎随时联系。";

            var eventService = new MockEventService(TestProcessInstanceId);
            eventService.SetVariable("user_message", userMessage);
            eventService.SetVariable("ai_response", aiResponse);

            Console.WriteLine("[Setup] ProcessInstanceId = {0}", TestProcessInstanceId);
            Console.WriteLine("[Setup] user_message = {0}", userMessage);
            Console.WriteLine("[Setup] ai_response  = {0}\n", (aiResponse?.Length > 60 ? aiResponse.Substring(0, 60) + "..." : aiResponse));

            try
            {
                // 3. CustomerExtractService: extract contact from user_message → write entity to process variable "customer"
                Console.WriteLine("--- Running CustomerExtractService ---");
                var extractService = new ContactExtractService();
                extractService.Executable(eventService);
                Console.WriteLine("CustomerExtractService done (customer written to variable 'customer').\n");

                // 4. CustomerSaveService: read "customer" from variable → save/update biz_customer, set "customer_id"
                Console.WriteLine("--- Running CustomerSaveService ---");
                var saveService = new ContactSaveService();
                saveService.Executable(eventService);
                var customerId = eventService.GetVariable(Slickflow.Engine.Common.ProcessVariableScopeEnum.Process, "customer_id");
                Console.WriteLine("CustomerSaveService done. customer_id = {0}\n", customerId ?? "(none)");

                // 5. Run MessageService: save user_message + ai_response → biz_conversation (with customer_id if set)
                Console.WriteLine("--- Running MessageService ---");
                var conversationService = new ConversationService();
                conversationService.Executable(eventService);
                Console.WriteLine("MessageService done.\n");

                // 6. Verify conversation saved: query biz_conversation by customer_id (table may not have process_instance_id)
                Console.WriteLine("--- Verifying conversation in Supabase ---");
                var convRepo = new SupabaseConversationRepository();
                var customerIdForVerify = customerId ?? "";
                var conversations = string.IsNullOrEmpty(customerIdForVerify)
                    ? new List<BizConversationEntity>()
                    : convRepo.GetByCustomerIdAsync(customerIdForVerify, 5).GetAwaiter().GetResult();
                if (conversations == null || conversations.Count == 0)
                {
                    Console.WriteLine("No conversation found for customer_id={0}. (Table may use different columns or RLS.) Check biz_conversation in Supabase dashboard.", customerIdForVerify ?? "(none)");
                }
                else
                {
                    Console.WriteLine("Found {0} conversation(s) for customer_id={1}.", conversations.Count, customerIdForVerify);
                    var first = conversations[0];
                    Console.WriteLine("  conversation_id = {0}", first.ConversationId ?? first.MessageId ?? "(null)");
                    Console.WriteLine("  customer_id     = {0}", first.CustomerId ?? "(null)");
                    var um = first.UserMessage ?? "";
                    Console.WriteLine("  user_message    = {0}", um.Length > 80 ? um.Substring(0, 80) + "..." : um);
                    var ar = first.AiResponse ?? "";
                    Console.WriteLine("  ai_response     = {0}\n", ar.Length > 80 ? ar.Substring(0, 80) + "..." : ar);
                }

                Console.WriteLine("=== Test run completed. Check Supabase tables: biz_customer, biz_conversation. ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
                Environment.ExitCode = 1;
            }
        }

        /// <summary>
        /// Load appsettings.json and set SUPABASE_URL / SUPABASE_SERVICE_ROLE_KEY so the plugin can read config.
        /// Tries: (1) project root (3 levels up from exe), (2) exe directory (where appsettings.json is copied on build).
        /// </summary>
        private static void EnsureSupabaseConfigFromProjectAppSettings()
        {
            var asmDir = GetExeDirectory();
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = !string.IsNullOrEmpty(asmDir)
                ? Path.GetFullPath(Path.Combine(asmDir, "..", "..", ".."))
                : currentDir;
            var pathsToTry = new[] { asmDir, currentDir, projectRoot };
            foreach (var basePath in pathsToTry)
            {
                if (string.IsNullOrEmpty(basePath) || !Directory.Exists(basePath)) continue;
                var appsettingsPath = Path.Combine(basePath, "appsettings.json");
                if (!File.Exists(appsettingsPath)) continue;
                try
                {
                    var json = File.ReadAllText(appsettingsPath);
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    if (!root.TryGetProperty("AiModelProvider", out var ai)) continue;
                    var url = ai.TryGetProperty("SupabaseProjectUrl", out var u) ? u.GetString()?.Trim() : null;
                    var key = ai.TryGetProperty("SupabaseServiceRoleKey", out var k) ? k.GetString()?.Trim() : null;
                    if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key)) continue;
                    Environment.SetEnvironmentVariable("SUPABASE_URL", url, EnvironmentVariableTarget.Process);
                    Environment.SetEnvironmentVariable("SUPABASE_SERVICE_ROLE_KEY", key, EnvironmentVariableTarget.Process);
                    return;
                }
                catch { }
            }
        }

        private static string GetExeDirectory()
        {
            try
            {
                var entryLoc = Assembly.GetEntryAssembly()?.Location;
                if (!string.IsNullOrWhiteSpace(entryLoc))
                {
                    var dir = Path.GetDirectoryName(entryLoc);
                    if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir)) return dir;
                }
                var execLoc = Assembly.GetExecutingAssembly().Location;
                if (!string.IsNullOrWhiteSpace(execLoc))
                {
                    var dir = Path.GetDirectoryName(execLoc);
                    if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir)) return dir;
                }
            }
            catch { }
            var baseDir = AppContext.BaseDirectory;
            if (!string.IsNullOrWhiteSpace(baseDir) && Directory.Exists(baseDir))
                return baseDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return null;
        }
    }
}
