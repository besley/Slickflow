using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Slickflow.Data
{
    /// <summary>
    /// 数据交易会话
    /// </summary>
    public class DbSession : IDbSession
    {
        #region 属性及构造函数
        private DbContext _dbContext;
        private DbConnection _dbConnection;
        private IDbContextTransaction _transaction;
        private bool disposed = false;
        private Dictionary<Type, object> repositories;

        /// <summary>
        /// 数据上下文
        /// </summary>
        public DbContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        public DbConnection DbConnection
        {
            get
            {
                return _dbConnection;
            }
        }


        /// <summary>
        /// 数据库事务对象
        /// </summary>
        private IDbContextTransaction Transaction
        {
            get { return _transaction; }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="context"></param>
        public DbSession(DbContext context)
        {
            _dbContext = context;
            _dbConnection = context.Database.GetDbConnection();
        }
        #endregion

        /// <summary>
        /// 获取数据操作类
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <returns>数据操作类接口</returns>
        public IRepository<T> GetRepository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<T>(_dbContext);
            }
            return (IRepository<T>)repositories[type];
        }

        /// <summary>
        /// SQL语句执行
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paramters">参数</param>
        /// <returns>执行状态</returns>
        public int ExecuteSqlCommand(string sql, params object[] paramters)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, paramters);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns>成功状态</returns>
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 开启会话
        /// </summary>
        /// <param name="isolation"></param>
        /// <returns></returns>
        public IDbContextTransaction BeginTrans()
        {
            _transaction = _dbContext.Database.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing">清除标识</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}