using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Config
{
    /// <summary>
    /// JOB Administration Variables Setting
    /// </summary>
    public class JobAdminDefine
    {
        //administration user account variables
        internal static readonly string ADMIN_USER_ID = "ADMIN_1001";
        internal static readonly string ADMIN_USER_NAME = "ADMINISTRATOR_JOB";
        internal static readonly string ADMIN_APP_NAME = "APP_ADMIN_JOB";
        internal static readonly string ADMIN_APP_INSTANCE_ID = "ADMIN_JOB_Z9000";
        internal static readonly string ADMIN_MESSAGE_SUCCESS = "SUCCESS";

        //email sending flag
        internal static readonly byte EMailSendUtility_SendEMailFlag = 0;
        //email template can be modified by customers
        //the customers can use their own email html template, this is only a demo
        internal static readonly string EMailSendUtility_SendEMailTitle = "新待办事项提醒邮件！";
        internal static readonly string EMailSendUtility_SendEMailContent = @" < h1 >新待办任务提醒</h1>" +
                "<p>您有一条新的待办任务： " +
                "<a href='http://localhost/sfmvc/order/{0}'>请在此处填写具体的业务单据名称和单据序号</a>  " +
                "请您及时登录业务系统处理！谢谢！</p>";

        internal static readonly string EMailSendUtility_SendEMailAccount = "test@abc.com";
        internal static readonly string EMailSendUtility_SendEMailPassword = "123456789";
        internal static readonly string EMailSendUtility_SendEMailHost = "smtp.abc.com";
        internal static readonly int EMailSendUtility_SendEMailHostPort = 25;
    }
}
