﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.BizAppService.Entity
{
    /// <summary>
    /// Woprkflow Application Result
    /// 流程和应用交互结果对象封装
    /// </summary>
    public class WfAppResult
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public WfAppResult()
        {
            Status = 0;
        }

        /// <summary>
        /// Default
        /// </summary>
        /// <returns></returns>
        public static WfAppResult Default()
        {
            return new WfAppResult { Status = 0, Message = string.Empty };
        }

        /// <summary>
        /// Success
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static WfAppResult Success(string message = null)
        {
            return new WfAppResult { Status = 1, Message = message };
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static WfAppResult Error(string message)
        {
            return new WfAppResult { Status = -1, Message = message };
        }
    }
}
