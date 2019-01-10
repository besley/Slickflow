/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;

namespace Slickflow.Data
{
    /// <summary>
    /// Data Repository
    /// Implement Select, Insert, Update, Delete
    /// </summary>
    public interface IRepository
    {
        //select
        T GetById<T>(dynamic primaryId) where T : class;
        T GetById<T>(IDbConnection conn, dynamic primaryId, IDbTransaction trans) where T : class;
        T GetDefaultByName<T>(string colName, string value) where T : class;
        T GetFirst<T>(string sql, dynamic param = null, bool buffered = true) where T : class;
        T GetFirst<T>(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true) where T : class;
        IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        IEnumerable<T> Query<T>(string sql, dynamic param = null, bool buffered = true) where T : class;
        IEnumerable<T> Query<T>(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true) where T : class;
        IEnumerable<dynamic> Query(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true);
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(IDbConnection conn, string sql, Func<TFirst, TSecond, TReturn> map,
             dynamic param = null, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null);
        //SqlMapper.GridReader GetMultiple(string sql, IDbConnection conn, dynamic param = null, 
        //    IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        //count
        int Count<T>(IDbConnection conn, IPredicate predicate, bool buffered = false) where T : class;
        int Count<T>(IDbConnection conn, string sql, bool buffered = false) where T : class;
        int Count(string sql, DynamicParameters parameters = null);

        //lsit
        IEnumerable<T> GetList<T>(IDbConnection conn, IPredicate predicate = null,
            IList<ISort> sort = null, bool buffered = false) where T : class;


        //paged select
        IEnumerable<T> GetPaged<T>(IDbConnection conn, int pageIndex, int pageSize, object predictate,
            IList<ISort> sort = null, bool buffered = false) where T : class;

        //paged query with store procedure
        IEnumerable<T> GetPaged<T>(IDbConnection conn, Pager pager, bool buffered = false) where T : class;

        //execute
        Int32 Execute(IDbConnection conn, string sql, dynamic param = null, IDbTransaction transaction = null);
        Int32 ExecuteCommand(IDbCommand cmd);
        Int32 ExecuteProc(string procName, DynamicParameters param = null);
        Int32 ExecuteProc(IDbConnection conn, string procName, DynamicParameters param = null);
        IList<T> ExecProcQuery<T>(IDbConnection conn, string procName, DynamicParameters param) where T : class;
        IList<T> ExecProcQuery<T>(string procName, DynamicParameters param) where T : class;

        //insert, update, delete
        dynamic Insert<T>(T entity) where T : class;
        dynamic Insert<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class;
        void InsertBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class;
        bool Update<T>(T entity) where T : class;
        bool Update<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class;
        bool UpdateBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class;
        bool Delete<T>(dynamic primaryId) where T : class;
        bool Delete<T>(IDbConnection conn, dynamic primaryId, IDbTransaction transaction = null) where T : class;
        bool Delete<T>(IDbConnection conn, IPredicate predicate, IDbTransaction transaction = null) where T : class;
        int DeleteBatch<T>(IDbConnection conn, IEnumerable<dynamic> ids, IDbTransaction transaction = null) where T : class;
    }
}
