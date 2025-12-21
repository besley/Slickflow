
using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Slickflow.Data
{
    /// <summary>
    /// Session interface for data connection transactions
    /// 数据连接事务的Session接口
    /// </summary>
    public interface IDbSession : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        IDbTransaction BeginTrans(IsolationLevel isolation = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
    }

    /// <summary>
    /// Session object for database connection transaction
    /// 数据库连接事务的Session对象
    /// </summary>
    public class DbSession : IDbSession
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        /// <summary>
        /// Database Connection
        /// </summary>
        public IDbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Database Transaction
        /// </summary>
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conn"></param>
        internal DbSession(IDbConnection conn)
        {
            _connection = conn;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        internal DbSession(IDbConnection conn, IDbTransaction trans)
        {
            _connection = conn;
            _transaction = trans;
        }

        /// <summary>
        /// Begin Transaction
        /// 开启事务
        /// </summary>
        /// <param name="isolation"></param>
        /// <returns></returns>
        public IDbTransaction BeginTrans(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            _transaction = _connection.BeginTransaction(isolation);
            return _transaction;
        }

        /// <summary>
        /// Commit Transaction
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// Rollback Transaction
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        /// <summary>
        /// resource release
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                    
                }
                _connection.Close();
                _connection = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
