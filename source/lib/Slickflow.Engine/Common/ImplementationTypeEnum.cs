using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Enumeration types of task implementation types
    /// 任务实现类型的枚举类型
    /// </summary>
    public enum ImplementationTypeEnum
    {
        /// <summary>
        /// Manual
        /// 人工参与
        /// </summary>
        Manual = 1,

        /// <summary>
        /// Plugin
        /// 插件方式执行
        /// </summary>
        Plugin = 2,

        /// <summary>
        /// Script
        /// 脚本方式
        /// </summary>
        Script = 4,

        /// <summary>
        /// Automantic
        /// 自动类型
        /// </summary>
        Automantic = 6
    }
}
