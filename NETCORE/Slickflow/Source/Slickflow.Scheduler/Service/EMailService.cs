using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Scheduler.Common;
using Slickflow.Scheduler.Entity;
using Slickflow.Scheduler.Manager;
using Slickflow.Scheduler.Utility;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// 消息服务实现类
    /// </summary>
    public class EMailService : IEMailService
    {
        private Repository quickRepository = null;
        private Repository QuickRepository
        {
            get
            {
                if (quickRepository == null)
                    quickRepository = new Repository();
                return quickRepository;
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public IList<UserEMailEntity> GetUserList()
        {
            var userList = QuickRepository.GetAll<UserEMailEntity>().ToList();
            return userList;
        }

        /// <summary>
        /// 待办任务发送邮件通知
        /// </summary>
        public void SendTaskEMail(IList<ProcessEntity> processList,
            IList<UserEMailEntity> userList)
        {
            var wfService = new WorkflowService();
            var taskList = wfService.GetTaskListEMailUnSent();
            if (taskList != null && taskList.Count() > 0)
            {
                foreach (var task in taskList)
                {
                    Func<TaskViewEntity, IList<ProcessEntity>, IList<UserEMailEntity>, Task> func = SendEMailAsync;
                    BackgroundTaskRunner.FireAndForgetTaskAsync(func, task, processList, userList);
                }
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="entity">任务实体</param>
        private async Task SendEMailAsync(TaskViewEntity entity, 
            IList<ProcessEntity> processList, 
            IList<UserEMailEntity> userList)
        {
            var pageUrl = processList.Single(t => t.ProcessGUID == entity.ProcessGUID
                && t.Version == entity.Version).PageUrl;
            pageUrl += entity.AppInstanceID;
            var userEMail = userList.Single(t => t.ID.ToString() == entity.AssignedToUserID).EMail;
                       
            if (!string.IsNullOrEmpty(userEMail)
                && IsValid(userEMail))
            {
                var emailSend = new EMailSendUtility(entity.TaskID);
                emailSend.OnEMailSendCompleted += EmailSend_OnEMailSendCompleted;

                try
                {
                    await emailSend.SendEMailAsync(userEMail, pageUrl);
                }
                catch (System.Exception ex)
                {
                    JobLogManager.RecordLog(JobRefClassEnum.Task.ToString(),
                        entity.TaskID.ToString(),
                        JobScheduleNameEnum.SendTaskEMail.ToString(),
                        -1,
                        ex.Message,
                        ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// 回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmailSend_OnEMailSendCompleted(object sender, EventArgs e)
        {
            var taskID = int.Parse(sender.ToString());
            var wfService = new WorkflowService();

            wfService.SetTaskEMailSent(taskID);
        }

        /// <summary>
        /// 检查邮件地址是否有效
        /// </summary>
        /// <param name="emailaddress">邮件地址</param>
        /// <returns></returns>
        private bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
