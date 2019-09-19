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
using Slickflow.Scheduler.Config;

namespace Slickflow.Scheduler.Utility
{
    /// <summary>
    /// 发送邮件工具类
    /// </summary>
    /// <summary>
    /// 发送邮件工具类
    /// </summary>
    public class EMailSendUtility
    {
        public event EventHandler OnEMailSendCompleted;

        private int TaskID = 0;
        public EMailSendUtility(int taskID)
        {
            TaskID = taskID;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">邮件主题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="receiveEmail">收件人</param>
        public async Task SendEMailAsync(string receiveEmail, string pageUrl)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(JobAdminDefine.EMailSendUtility_SendEMailAccount,
                JobAdminDefine.EMailSendUtility_SendEMailPassword);
            smtp.Host = JobAdminDefine.EMailSendUtility_SendEMailHost;
            smtp.Port = JobAdminDefine.EMailSendUtility_SendEMailHostPort;
            smtp.EnableSsl = false;     //是否使用安全连接传输

            var body = JobAdminDefine.EMailSendUtility_SendEMailContent.Replace("{0}", pageUrl);

            //邮件信息
            MailMessage mail = new MailMessage();
            mail.Subject = JobAdminDefine.EMailSendUtility_SendEMailTitle;
            mail.Body = body;
            mail.From = new MailAddress(JobAdminDefine.EMailSendUtility_SendEMailAccount);      //发件人地址
            mail.SubjectEncoding = UTF8Encoding.UTF8;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.Priority = MailPriority.Normal;
            mail.IsBodyHtml = true;
            mail.To.Add(receiveEmail);
            smtp.SendCompleted += SendCompletedCallback;
            await smtp.SendMailAsync(mail);//发送邮件
        }

        /// <summary>
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

            if (OnEMailSendCompleted != null) OnEMailSendCompleted(TaskID, null);
        }
    }
}