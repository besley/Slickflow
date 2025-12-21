using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Config;
using Slickflow.Module.Localize;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// EMail send utility
    /// </summary>
    public class EMailSendUtility
    {
        public event EventHandler OnEMailSendCompleted;

        private int TaskId = 0;
        public EMailSendUtility(int taskId)
        {
            TaskId = taskId;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        public async Task SendEMailAsync(string title, string body, string receiveEmail)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(JobAdminConfig.EMailSendUtility_SendEMailAccount,
                JobAdminConfig.EMailSendUtility_SendEMailPassword);
            smtp.Host = JobAdminConfig.EMailSendUtility_SendEMailHost;
            smtp.Port = JobAdminConfig.EMailSendUtility_SendEMailHostPort;

            //是否使用安全连接传输
            //Whether to use a secure connection for transmission
            smtp.EnableSsl = false;    

            //邮件信息
            //EMail info
            MailMessage mail = new MailMessage();
            mail.Subject = title;  
            mail.Body = body;
            mail.From = new MailAddress(JobAdminConfig.EMailSendUtility_SendEMailAccount);      
            mail.SubjectEncoding = UTF8Encoding.UTF8;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.Priority = MailPriority.Normal;
            mail.IsBodyHtml = true;
            mail.To.Add(receiveEmail);
            smtp.SendCompleted += SendCompletedCallback;

            await smtp.SendMailAsync(mail);
        }

        /// <summary>
        /// The callback method for sending emails after completion
        /// 邮件发送完毕的回调方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            SmtpClient callbackClient = sender as SmtpClient;
            MailMessage callbackMailMessage = e.UserState as MailMessage;
            if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
            {
                LogManager.RecordLog(WfDefine.WF_PROCESS_TASK_EMAIL_ERROR, LogEventType.Error,
                    LogPriority.Normal, callbackMailMessage, e.Error);
            }

            if (callbackClient != null) callbackClient.Dispose();
            if (callbackMailMessage != null) callbackMailMessage.Dispose();

            if (OnEMailSendCompleted != null) OnEMailSendCompleted(TaskId, null);
        }
    }
}