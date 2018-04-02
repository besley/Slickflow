using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Extensions;

namespace Slickflow.Data
{
    /// <summary>
    /// 数据实体操作类
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        #region 属性及构造函数
        protected readonly DbContext DbContext;

        public DbSet<T> GetDbSet()
        {
            return DbContext.Set<T>();
        }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }
        #endregion

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>实体</returns>
        public T GetByID(dynamic id)
        {
            return GetDbSet().Find(id);
        }

        /// <summary>
        /// 根据查询条件获取实体
        /// </summary>
        /// <param name="predicate">条件语句</param>
        /// <returns>实体</returns>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return GetDbSet().First<T>(predicate);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns>列表</returns>
        public IQueryable<T> GetAll()
        {
            return GetDbSet().AsQueryable<T>();
        }

        /// <summary>
        /// 记录数目获取
        /// </summary>
        /// <param name="predicate">条件语句</param>
        /// <returns>记录数目</returns>
        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return GetDbSet().Count();
            }
            else
            {
                return GetDbSet().Count(predicate);
            }
        }

        /// <summary>
        /// 插入方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>新实体</returns>
        public T Insert(T entity)
        {
            var entry = GetDbSet().Add(entity);
            return entry.Entity; 
        }

        /// <summary>
        /// 批量插入实体列表
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void Insert(params T[] entities)
        {
            GetDbSet().AddRange(entities);
        }

        /// <summary>
        /// 批量插入实体列表
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void Insert(IEnumerable<T> entities)
        {
            GetDbSet().AddRange(entities);
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据列表</returns>
        public IQueryable<T> Query(string sql, params object[] parameters)
        {
            return GetDbSet().FromSql(sql, parameters);
        }

        /// <summary>
        /// LINQ条件查询
        /// </summary>
        /// <param name="predicate">条件语句</param>
        /// <returns>数据列表</returns>
        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return GetDbSet().Where<T>(predicate);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void Update(T entity)
        {
            GetDbSet().Update(entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void Update(params T[] entities)
        {
            GetDbSet().UpdateRange(entities);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void Update(IEnumerable<T> entities)
        {
            GetDbSet().UpdateRange(entities);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键ID</param>
        public void Delete(dynamic id)
        {
            T entity = GetByID(id);
            if (entity != null) GetDbSet().Remove(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void Delete(params T[] entities)
        {
            GetDbSet().RemoveRange(entities);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void Delete(IEnumerable<T> entities)
        {
            GetDbSet().RemoveRange(entities);
        }
    }
}
