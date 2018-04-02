using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Data;
using Microsoft.EntityFrameworkCore;

namespace Slickflow.Engine.Business.Data
{
    /// <summary>
    /// 上下文创建工厂类
    /// </summary>
    public class DbFactory
    {
        /// <summary>
        /// 创建数据会话
        /// </summary>
        /// <returns>会话</returns>
        public static IDbSession CreateSession()
        {
            var session = SessionFactory.CreateSession(new WfDbContext());
            return session;
        }
    }
}
