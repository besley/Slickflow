using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Config;
using Slickflow.Module.Localize;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// EMail send utility — SMTP configuration is read from JobAdminConfig,
    /// which can be initialised via JobAdminConfig.Configure(IConfiguration).
    /// 邮件发送工具类，SMTP 配置通过 JobAdminConfig 读取（支持 appsettings.json）
    /// </summary>
    public class EMailSendUtility
    {
        public event EventHandler OnEMailSendCompleted;

        private readonly int TaskId;

        public EMailSendUtility(int taskId)
        {
            TaskId = taskId;
        }

        /// <summary>
        /// Send an email asynchronously.
        /// 异步发送邮件
        /// </summary>
        public async Task SendEMailAsync(string title, string body, string receiveEmail)
        {
            var smtp = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential(
                    JobAdminConfig.SmtpAccount,
                    JobAdminConfig.SmtpPassword),
                Host      = JobAdminConfig.SmtpHost,
                Port      = JobAdminConfig.SmtpPort,
                EnableSsl = JobAdminConfig.SmtpSsl,
            };

            var mail = new MailMessage
            {
                Subject         = title,
                Body            = body,
                From            = new MailAddress(JobAdminConfig.SmtpAccount),
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding    = Encoding.UTF8,
                Priority        = MailPriority.Normal,
                IsBodyHtml      = true,
            };
            mail.To.Add(receiveEmail);
            smtp.SendCompleted += SendCompletedCallback;

            await smtp.SendMailAsync(mail);
        }

        /// <summary>
        /// Send email with attachment (PDF bytes).
        /// 发送带附件的邮件（PDF 字节流）
        /// </summary>
        public async Task SendEMailWithAttachmentAsync(string title, string body, string receiveEmail,
            byte[] attachmentBytes, string attachmentFileName)
        {
            var smtp = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential(
                    JobAdminConfig.SmtpAccount,
                    JobAdminConfig.SmtpPassword),
                Host      = JobAdminConfig.SmtpHost,
                Port      = JobAdminConfig.SmtpPort,
                EnableSsl = JobAdminConfig.SmtpSsl,
            };

            var mail = new MailMessage
            {
                Subject         = title,
                Body            = body,
                From            = new MailAddress(JobAdminConfig.SmtpAccount),
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding    = Encoding.UTF8,
                Priority        = MailPriority.Normal,
                IsBodyHtml      = true,
            };
            mail.To.Add(receiveEmail);

            if (attachmentBytes != null && attachmentBytes.Length > 0)
            {
                var ms = new System.IO.MemoryStream(attachmentBytes);
                mail.Attachments.Add(new Attachment(ms, attachmentFileName, "application/pdf"));
            }

            smtp.SendCompleted += SendCompletedCallback;
            await smtp.SendMailAsync(mail);
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (sender is SmtpClient callbackClient)
            {
                if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
                {
                    var callbackMailMessage = e.UserState as MailMessage;
                    LogManager.RecordLog(WfDefine.WF_PROCESS_TASK_EMAIL_ERROR, LogEventType.Error,
                        LogPriority.Normal, callbackMailMessage, e.Error);
                }
                callbackClient.Dispose();
            }

            if (e.UserState is MailMessage msg) msg.Dispose();
            OnEMailSendCompleted?.Invoke(TaskId, null);
        }
    }
}
