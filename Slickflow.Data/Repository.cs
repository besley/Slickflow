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
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class
        {
            var tblName = string.Format("dbo.{0}", GetTableName<T>());
            var idsin = string.Join(",", ids.ToArray<dynamic>());
            var sql = "SELECT * FROM @table WHERE Id in (@ids)";

            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                IEnumerable<T> dataList = SqlMapper.Query<T>(conn, sql, new { table = tblName, ids = idsin });
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

        ///// <summary>
        ///// 分页
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="allRowsCount"></param>
        ///// <param name="predicate"></param>
        ///// <param name="sort"></param>
        ///// <param name="buffered"></param>
        ///// <returns></returns>
        //public IEnumerable<T> GetPage<T>(IDbConnection conn, int pageIndex, int pageSize, out int allRowsCount, 
        //    IPredicate predicate = null, ISort sort = null, bool buffered = true) where T : class
        //{
        //    IList<ISort> orderBy = new List<ISort>();
        //    orderBy.Add(sort);

        //    return GetPage<T>(conn, pageIndex, pageSize, out allRowsCount, predicate, orderBy, buffered);
        //}

        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="buffered"></param>
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
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class
        {
            bool isOk = conn.Update<T>(entity, transaction);
            return isOk;
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
            //var tblName = string.Format("dbo.{0}", typeof(T).Name);
            //var conn = (SqlConnection)_session.Connection;
            //var tran = (SqlTransaction)transaction;
            //using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, tran))
            //{
            //    bulkCopy.BatchSize = entityList.Count();
            //    bulkCopy.DestinationTableName = tblName;
            //    var table = new DataTable();
            //    var props = TypeDescriptor.GetProperties(typeof(T))
            //                                .Cast<PropertyDescriptor>()
            //                                .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
            //                                .ToArray();
            //    foreach (var propertyInfo in props)
            //    {
            //        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
            //        table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            //    }
            //    var values = new object[props.Length];
            //    foreach (var itemm in entityList)
            //    {
            //        for (var i = 0; i < values.Length; i++)
            //        {
            //            values[i] = props[i].GetValue(itemm);
            //        }
            //        table.Rows.Add(values);
            //    }
            //    bulkCopy.WriteToServer(table);
            //}
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
        public bool DeleteBatch<T>(IDbConnection conn, IEnumerable<dynamic> ids, IDbTransaction transaction = null) where T : class
        {
            bool isOk = false;
            foreach (var id in ids)
            {
                Delete<T>(id, conn, transaction);
            }
            isOk = true;
            return isOk;
        }
    }
}
