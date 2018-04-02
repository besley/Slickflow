using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Slickflow.Data
{
    /// <summary>
    /// 数据会话接口
    /// </summary>
    public interface IDbSession : IDisposable
    {
        DbContext DbContext { get; }
        DbConnection DbConnection { get; }
        IRepository<T> GetRepository<T>() where T : class;
        int SaveChanges();
        int ExecuteSqlCommand(string sql, params object[] paramters);
        IDbContextTransaction BeginTrans();
        void Commit();
        void Rollback();
    }
}
