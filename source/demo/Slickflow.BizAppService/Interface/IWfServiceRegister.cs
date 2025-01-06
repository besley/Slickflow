﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Common;

namespace Slickflow.BizAppService.Service
{
    /// <summary>
    /// Workflow Service Register Interface
    /// 工作流注册接口
    /// </summary>
    public interface IWfServiceRegister
    {
        WfAppRunner WfAppRunner { get; set; }
        void RegisterWfAppRunner(WfAppRunner runner);
    }
}
