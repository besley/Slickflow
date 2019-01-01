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
        public const string WF_XPDL_ERROR = "XPDL定义错误";
        public const string WF_PROCESS_START_ERROR = "流程启动异常";
        public const string WF_PROCESS_RUN_ERROR = "流程流转异常";
        public const string WF_PROCESS_WITHDRAW_ERROR = "流程撤销异常";
        public const string WF_PROCESS_SENDBACK_ERROR = "流程退回异常";
        public const string WF_PROCESS_RESEND_ERROR = "流程返送异常";
        public const string WF_PROCESS_JUMP_ERROR = "流程跳转异常";
        public const string WF_PROCESS_REVERSE_ERROR = "流程返签异常";
        public const string WF_PROCESS_SIGN_FORWARD_ERROR = "流程加签异常";
        public const string WF_PROCESS_SIGN_TOGETHER_ERROR = "流程会签异常";

        //普通常量定义
        public const string WF_XPDL_GATEWAY_BYPASS_GUID = "GATEWAY-BYPASS-GUID";
        public const string WF_XPDL_JUMP_BYPASS_GUID = "JUMP-BYPASS-GUID";

        //公用变量定义
        public const string WF_VARIABLE_DIRECT_NEXTSTEP = "WF_VARIABLE_DIRECT_NEXTSTEP";
        public const string WF_VARIABLE_DIRECT_NEXTSTEP_VALUE = "1";

        //邮件发送
        public const string WF_PROCESS_TASK_EMAIL_ERROR = "邮件发送异常";

        //外部事件注册标志
        public const bool WF_PROCESS_REGISTER_EVENT = true;
    }
}
