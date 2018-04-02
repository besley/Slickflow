using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Slickflow.Data
{
    /// <summary>
    /// 会话工厂类
    /// </summary>
    public class SessionFactory
    {
        /// <summary>
        /// 创建数据库事务会话
        /// </summary>
        /// <returns></returns>
        public static IDbSession CreateSession(DbContext dbContext)
        {
            IDbSession session = new DbSession(dbContext);
            return session;
        }
    }
}
