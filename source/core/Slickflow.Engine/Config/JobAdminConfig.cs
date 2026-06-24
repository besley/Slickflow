using System;
using Microsoft.Extensions.Configuration;

namespace Slickflow.Engine.Config
{
    /// <summary>
    /// JOB Administration Variables Setting
    /// 作业参数定义 — 可通过 IConfiguration (appsettings.json) 覆盖默认值
    /// </summary>
    public class JobAdminConfig
    {
        // ── Default fallback values ────────────────────────────────────────────
        internal static readonly bool   EMailSendUtility_SendEMailFlag        = false;
        internal static readonly string EMailSendUtility_LocalHostWebApp      = "http://localhost/sfmvc/";
        internal static readonly string EMailSendUtility_SendEMailAccount     = "test@abc.com";
        internal static readonly string EMailSendUtility_SendEMailPassword    = "123456789";
        internal static readonly string EMailSendUtility_SendEMailHost        = "smtp.abc.com";
        internal static readonly int    EMailSendUtility_SendEMailHostPort    = 25;

        // ── Runtime config (loaded once via Configure()) ───────────────────────
        private static string _smtpHost     = EMailSendUtility_SendEMailHost;
        private static int    _smtpPort     = EMailSendUtility_SendEMailHostPort;
        private static string _smtpAccount  = EMailSendUtility_SendEMailAccount;
        private static string _smtpPassword = EMailSendUtility_SendEMailPassword;
        private static bool   _smtpSsl      = false;
        private static bool   _configured   = false;

        public static string SmtpHost     => _smtpHost;
        public static int    SmtpPort     => _smtpPort;
        public static string SmtpAccount  => _smtpAccount;
        public static string SmtpPassword => _smtpPassword;
        public static bool   SmtpSsl      => _smtpSsl;

        /// <summary>
        /// Initialize from IConfiguration (call once at application startup).
        /// 应用启动时调用，从 appsettings.json 加载 SMTP 配置。
        /// Section path: Email:SmtpHost / Email:SmtpPort / Email:Username / Email:Password / Email:EnableSsl
        /// </summary>
        public static void Configure(IConfiguration configuration)
        {
            if (configuration == null || _configured) return;

            _smtpHost     = configuration["Email:SmtpHost"]     ?? _smtpHost;
            _smtpAccount  = configuration["Email:Username"]     ?? _smtpAccount;
            _smtpPassword = configuration["Email:Password"]     ?? _smtpPassword;
            bool.TryParse(configuration["Email:EnableSsl"],     out _smtpSsl);
            int.TryParse(configuration["Email:SmtpPort"],       out var port);
            if (port > 0) _smtpPort = port;

            _configured = true;
        }
    }
}
