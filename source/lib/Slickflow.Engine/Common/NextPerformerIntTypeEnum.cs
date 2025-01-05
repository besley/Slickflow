﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// The next step is to handle the user specified type
    /// Types of user operations used internally for execution
    /// 下一步办理用户指定类型
    /// 内部使用的执行用户操作类型
    /// </summary>
    public enum NextPerformerIntTypeEnum
    {
        /// <summary>
        /// Clearly specify the steps and role list
        /// 明确指定步骤和角色列表
        /// </summary>
        Specific = 0,

        /// <summary>
        /// Read node definition source
        /// Including system defined roles and user lists
        /// 读取节点定义来源
        /// 包括系统定义的角色及用户列表
        /// </summary>
        Definition = 1,

        /// <summary>
        /// Simulate testing using a single user
        /// 模拟使用单一用户进行的测试
        /// </summary>
        Single = 3,

        /// <summary>
        /// It has already passed the previous step and is intended for return
        /// 已经流转过得上一步，用于退回
        /// </summary>
        Traced = 5
    }
}
