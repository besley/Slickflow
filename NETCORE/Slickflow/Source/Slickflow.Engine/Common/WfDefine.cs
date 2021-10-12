using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 常用定义类
    /// </summary>
    public class WfDefine
    {
        //流程部分
        public const string WF_XPDL_ERROR = "XPDL DEFINITION ERROR";
        public const string WF_PROCESS_START_ERROR = "PROCESS STARTUP ERROR";
        public const string WF_PROCESS_RUN_ERROR = "PROCESS RUN ERROR";
        public const string WF_PROCESS_WITHDRAW_ERROR = "PROCESS WITHDRAW ERROR";
        public const string WF_PROCESS_SENDBACK_ERROR = "PROCESS SENDBACK ERROR";
        public const string WF_PROCESS_RESEND_ERROR = "PROCESS RESEND ERROR";
        public const string WF_PROCESS_REVISE_ERROR = "PROCESS REVISE ERROR";
        public const string WF_PROCESS_JUMP_ERROR = "PROCESS JUMP ERROR";
        public const string WF_PROCESS_REVERSE_ERROR = "PROCESS REVERSE ERROR";
        public const string WF_PROCESS_SIGN_FORWARD_ERROR = "PROCESS SIGNFORWARD ERROR";
        public const string WF_PROCESS_SIGN_TOGETHER_ERROR = "PROCESS SIGNTOGETHER ERROR";
        public const string WF_PROCESS_REJECT_ERROR = "PROCESS REJECT ERROR";
        public const string WF_PROCESS_CLOSE_ERROR = "PROCESS CLOSE ERROR";
        

        //普通常量定义
        public const string WF_XPDL_GATEWAY_BYPASS_GUID = "GATEWAY-BYPASS-GUID";
        public const string WF_XPDL_JUMP_BYPASS_GUID = "JUMP-BYPASS-GUID";
        public const string WF_XPDL_SEND_BACK_UNKNOWN_GUID = "SEND-BACK-UNKNOWN_GUID";
        public const string WF_XPDL_NEXT_ACTIVITY_PERFORMERS_ONLY = "NEXT-ACTIVITY-PERFORMERS-ONLY";

        //公用变量定义
        public const string WF_VARIABLE_DIRECT_NEXTSTEP = "WF_VARIABLE_DIRECT_NEXTSTEP";
        public const string WF_VARIABLE_DIRECT_NEXTSTEP_VALUE = "1";

        //邮件发送
        public const string WF_PROCESS_TASK_EMAIL_ERROR = "PROCESS TASK EMAL SEND ERROR";

        //外部事件注册标志
        public const bool WF_PROCESS_REGISTER_EVENT = true;

        //系统内部用户
        public const string SYSTEM_INTERNAL_USER_ID = "SYSTEM_INTERNAL_USER_ID";
        public const string SYSTEM_INTERNAL_USER_NAME = "SYSTEM_INTERNAL_USER_NAME";

        //系统定时作业用户
        public const string SYSTEM_JOBTIMER_USER_ID = "SYSTEM_JOBTIMER_USER_ID";
        public const string SYSTEM_JOBTIMER_USER_NAME = "SYSTEM_JOBTIMER_USER_NAME";

        //系统自动作业名称
        public const string WF_JOB_QUEUE_TIMER = "WF_JOB_QUEUE_TIMER";
        public const string WF_JOB_QUEUE_CONDITIONAL = "WF_JOB_QUEUE_CONDITIONAL";

    }
}
