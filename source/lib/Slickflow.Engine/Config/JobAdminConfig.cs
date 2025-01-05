using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Config
{
    /// <summary>
    /// JOB Administration Variables Setting
    /// 作业参数定义
    /// </summary>
    internal class JobAdminConfig
    {
        /// <summary>
        /// email sending flag
        /// 是否需要发送邮件的标志
        /// </summary>
        internal static readonly Boolean EMailSendUtility_SendEMailFlag = false;

        /// <summary>
        /// Local Web App
        /// 流程关联的应用地址
        /// </summary>
        internal static readonly string EMailSendUtility_LocalHostWebApp = "http://localhost/sfmvc/";

        /// <summary>
        /// EMail Account
        /// 邮件发送账户
        /// </summary>
        internal static readonly string EMailSendUtility_SendEMailAccount = "test@abc.com";

        /// <summary>
        /// EMail Accoutn Password
        /// 邮件发送账户的密码
        /// </summary>
        internal static readonly string EMailSendUtility_SendEMailPassword = "123456789";

        /// <summary>
        /// EMail send host
        /// 邮件发送的主机
        /// </summary>
        internal static readonly string EMailSendUtility_SendEMailHost = "smtp.abc.com";

        /// <summary>
        /// EMail send host port
        /// 哟经发送端口
        /// </summary>
        internal static readonly int EMailSendUtility_SendEMailHostPort = 25;
    }
}
