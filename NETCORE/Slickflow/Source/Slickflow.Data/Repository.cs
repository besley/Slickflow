/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using System.Data.SqlClient;
using DapperExtensions.Sql;

namespace Slickflow.Data
{
    /// <summary>
    /// Repository基类
    /// </summary>
    public class Repository : IRepository
    {
        public Repository()
        {

        }

        /// <summary>
        /// 根据主键ID获取记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T GetById<T>(dynamic primaryId) where T : class
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                return conn.Get<T>(primaryId as object);
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T GetById<T>(IDbConnection conn, dynamic primaryId, IDbTransaction trans) where T : class
        {
            return conn.Get<T>(primaryId as object, trans);
        }

        /// <summary>
        /// 根据字段列名称获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T GetDefaultByName<T>(string colName, string value) where T : class
        {
            var dataList = GetByName<T>(colName, value).ToList<T>();

            if (dataList.Count() > 0)
                return dataList.FirstOrDefault<T>();
            else
                return null;
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName<T>() where T : class
        {
            System.Attribute attr = System.Attribute.GetCustomAttributes(typeof(T))[0];
            var tableName = (attr as dynamic).TableName;
            return tableName;
        }

        /// <summary>
        /// 根据字段列名称获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IList<T> GetByName<T>(string colName, string value) where T : class
        {
            var tblName = GetTableName<T>();
            var sql = string.Format("SELECT * FROM {0} WHERE {1}=@colValue", tblName, colName);

            try
            {
                using (IDbConnection conn = SessionFactory.CreateConnection())
                {
                    IList<T> dataList = SqlMapper.Query<T>(conn, sql, new { colValue = value }).ToList();
                    return dataList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="ids">ID列表</param>
        /// <returns>实体列表</returns>
        public IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                var dataList = GetByIds<T>(conn, ids);
                return dataList;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="conn">链接</param>
        /// <param name="ids">ID列表</param>
        /// <param name="trans">事务</param>
        /// <param name="buffered">缓存</param>
        /// <returns>实体列表</returns>
        public IEnumerable<T> GetByIds<T>(IDbConnection conn, IList<dynamic> ids, IDbTransaction trans = null, bool buffered = true) where T : class
        {
            var tblName = GetTableName<T>();
            var idsin = ids.ToArray<dynamic>();
            var sql = string.Format("SELECT * FROM dbo.{0} WHERE Id in @ids", tblName);

            IEnumerable<T> dataList = SqlMapper.Query<T>(conn, sql, new { ids = idsin }, trans);
            return dataList;
        }

        /// <summary>
        /// 获取全部数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                IEnumerable<T> dataList = conn.GetList<T>();
                return dataList;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 获取全部数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(IDbConnection conn, IDbTransaction trans) where T : class
        {
            var dataList = conn.GetList<T>(null, null, trans);
            return dataList;
        }

        /// <summary>
        /// 查询匹配的一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public T GetFirst<T>(string sql, dynamic param = null, bool buffered = true) where T : class
        {
            T entity = null;
            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                var list = SqlMapper.Query<T>(conn, sql, param as object, null, buffered).ToList();
                if (list != null && list.Count() > 0)
                {
                    entity = list[0];
                }
                return entity;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public T GetFirst<T>(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null,
            bool buffered = true) where T : class
        {
            T entity = null;
            var list = SqlMapper.Query<T>(conn, sql, param as object, trans, buffered).ToList();
            if (list != null && list.Count() > 0)
            {
                entity = list[0];
            }
            return entity;

        }

        /// <summary>
        /// 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, dynamic param = null, bool buffered = true) where T : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return SqlMapper.Query<T>(conn, sql, param as object, null, buffered);
            }
        }


        /// <summary>
        /// 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true) where T : class
        {
            return SqlMapper.Query<T>(conn, sql, param as object, trans, buffered);
        }

        /// <summary>
        /// 根据条件筛选数据集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true)
        {
            return SqlMapper.Query(conn, sql, param as object, trans, buffered);
        }

        /// <summary>
        /// 根据表达式筛选
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(IDbConnection conn, string sql, Func<TFirst, TSecond, TReturn> map,
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null)
        {
            return SqlMapper.Query(conn, sql, map, param as object, transaction, buffered, splitOn);
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(IDbConnection conn, IPredicate predicate = null, IList<ISort> sort = null,
            bool buffered = false) where T : class
        {
            return conn.GetList<T>(predicate, sort, null, null, buffered);
        }

        /// <summary>
        /// 分页方法调用示例：
        /// 1. 单一条件
        //  using (SqlConnection cn = new SqlConnection(_connectionString))
        //  {
        //    cn.Open();
        //
        //    //排序字段
        //    var sortList = new List<DapperExtensions.ISort>();
        //    sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });
        //
        //    var predicate = Predicates.Field<Person>(f => f.Active, Operator.Eq, true);
        //    List<Person> list = cn.GetPaged<Person>(cn, query.PageIndex, query.PageSize, 
        //            predicate, sortList, false).ToList();
        //
        //    cn.Close();
        //  }
        //
        //  2. 组合条件
        //  using (SqlConnection cn = new SqlConnection(_connectionString))
        //  {
        //    cn.Open();
        //
        //    //排序字段
        //    var sortList = new List<DapperExtensions.ISort>();
        //    sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });
        //
        //    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
        //    pg.Predicates.Add(Predicates.Field<Person>(f => f.Active, Operator.Eq, true));
        //    pg.Predicates.Add(Predicates.Field<Person>(f => f.LastName, Operator.Like, "Br%"));
        //
        //    List<Person> list = cn.GetPaged<Person>(cn, query.PageIndex, query.PageSize, 
        //            pg, sortList, false).ToList();
        //
        //    cn.Close();
        //  }
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="allRowsCount">总记录数</param>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged<T>(IDbConnection conn, int pageIndex, int pageSize, object predicate,
            IList<ISort> sort = null, bool buffered = false) where T : class
        {
            return conn.GetPage<T>(predicate, sort, pageIndex, pageSize, null, null, buffered);
        }

        /// <summary>
        /// 分页查询（存储过程）
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="conn">连接</param>
        /// <param name="pager">分页对象</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged<T>(IDbConnection conn, Pager pager, bool buffered = false) where T : class
        {
            var tblName = string.IsNullOrEmpty(pager.TableName) ? GetTableName<T>() : pager.TableName;
            var keyFieldName = string.IsNullOrEmpty(pager.KeyFieldName) ? "ID" : pager.KeyFieldName;

            var p = new DynamicParameters();
            p.Add("@pageIndex", pager.PageIndex);
            p.Add("@pageSize", pager.PageSize);
            p.Add("@tblName", tblName);
            p.Add("@fldName", keyFieldName);
            p.Add("@isDesc", pager.IsDesc);
            p.Add("@strWhere", pager.StrWhere);
            p.Add("@fldOrder", pager.FieldOrder);

            return conn.Query<T>("pr_sys_QueryPaged", p, null, buffered, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public int Count<T>(IDbConnection conn, IPredicate predicate, bool buffered = false) where T : class
        {
            return conn.Count<T>(predicate);
        }

        /// <summary>
        /// 统计查询语句记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public int Count<T>(IDbConnection conn, string sql, bool buffered = false) where T : class
        {
            var cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                int count = (int)(ExecuteScalar(conn, cmd) ?? 0);
                return count;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 带参数的SQL的Count求和
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Count(string sql, DynamicParameters parameters = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return conn.Query<int>(sql, parameters).Single<int>();
            }
        }

        /// <summary>
        /// 获取多实体集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public SqlMapper.GridReader GetMultiple(string sql, dynamic param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(IDbConnection conn, string sql, dynamic param = null, IDbTransaction transaction = null)
        {
            return conn.Execute(sql, param as object, transaction);
        }

        /// <summary>
        /// 执行command操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteCommand(IDbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(string procName, DynamicParameters param = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return conn.Execute(procName, param, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(IDbConnection conn, string procName, DynamicParameters param = null)
        {
            return conn.Execute(procName, param, null, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int ExecuteProc(IDbConnection conn, string procName, DynamicParameters param = null, IDbTransaction transaction = null)
        {
            return conn.Execute(procName, param, transaction, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 存储过程执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> ExecProcQuery<T>(IDbConnection conn, string procName, DynamicParameters param)
            where T : class
        {
            IList<T> list = conn.Query<T>(procName, param, null, false, null, CommandType.StoredProcedure).ToList<T>();
            return list;
        }

        /// <summary>
        /// 存储过程执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> ExecProcQuery<T>(string procName, DynamicParameters param)
            where T : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                IList<T> list = conn.Query<T>(procName, param, null, false, null, CommandType.StoredProcedure).ToList<T>();
                return list;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回查询结果
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbConnection conn, string sql, bool buffered = false)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            return ExecuteScalar(conn, cmd);
        }

        /// <summary>
        /// 执行SQL语句，并返回数值
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbConnection conn, IDbCommand cmd)
        {
            try
            {
                bool wasClosed = conn.State == ConnectionState.Closed;
                if (wasClosed) conn.Open();

                return cmd.ExecuteScalar();
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>主键自增值</returns>
        public dynamic Insert<T>(T entity) where T : class
        {
            var newId = 0;
            var session = SessionFactory.CreateSession();

            session.BeginTrans();
            try
            {
                newId = Insert<T>(session.Connection, entity, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
            return newId;
        }


        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public dynamic Insert<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class
        {
            dynamic result = conn.Insert<T>(entity, transaction);
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>是否成功</returns>
        public bool Update<T>(T entity) where T : class
        {
            var session = SessionFactory.CreateSession();

            session.BeginTrans();
            try
            {
                var isOk = Update<T>(session.Connection, entity, session.Transaction);
                session.Commit();

                return isOk;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>是否成功</returns>
        public bool Update<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class
        {
            bool isOk = conn.Update<T>(entity, transaction);
            return isOk;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(dynamic id) where T : class
        {
            var session = SessionFactory.CreateSession();

            session.BeginTrans();
            try
            {
                var isOk = Delete<T>(session.Connection, id, session.Transaction);
                session.Commit();
                return isOk;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public bool Delete<T>(IDbConnection conn, dynamic primaryId, IDbTransaction transaction = null) where T : class
        {
            var entity = GetById<T>(primaryId);
            var obj = entity as T;
            bool isOk = conn.Delete<T>(obj, transaction);
            return isOk;
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool Delete<T>(IDbConnection conn, IPredicate predicate, IDbTransaction transaction = null) where T : class
        {
            return conn.Delete<T>(predicate, transaction);
        }

        /// <summary>
        /// 批量插入功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        public void InsertBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
			var tblName = string.Format("dbo.{0}", typeof(T).Name);
            var tran = (SqlTransaction)transaction;
            using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.TableLock, tran))
            {
                bulkCopy.BatchSize = entityList.Count();
                bulkCopy.DestinationTableName = tblName;
                var table = new DataTable();
                DapperExtensions.Sql.ISqlGenerator sqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration());
                var classMap = sqlGenerator.Configuration.GetMap<T>();
                var props = classMap.Properties.Where(x=>x.Ignored == false).ToArray();
                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyInfo.PropertyType) ?? propertyInfo.PropertyInfo.PropertyType);
                }
                var values = new object[props.Count()];
                foreach (var itemm in entityList)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].PropertyInfo.GetValue(itemm, null);
                    }
                    table.Rows.Add(values);
                }
                bulkCopy.WriteToServer(table);
            }
        }

        /// <summary>
        /// 批量更新（）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public bool UpdateBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
            bool isOk = false;
            foreach (var item in entityList)
            {
                Update<T>(conn, item, transaction);
            }
            isOk = true;
            return isOk;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteBatch<T>(IDbConnection conn, IEnumerable<dynamic> ids, IDbTransaction transaction = null) where T : class
        {
            var tblName = GetTableName<T>();
            var idsin = string.Join(",", ids.ToArray<dynamic>());
            var sql = string.Format("DELETE FROM dbo.{0} WHERE ID in (@ids)", tblName);
            var result = SqlMapper.Execute(conn, sql, new { ids = idsin });

            return result;
        }
    }
}
