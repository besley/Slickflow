using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Slickflow.Data
{
    /// <summary>
    /// 数据操作类接口
    /// </summary>
    /// <typeparam name="T">数据实体类型</typeparam>
    public interface IRepository<T> where T : class
    {
        DbSet<T> GetDbSet();

        T GetByID(dynamic id);
        T Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();

        IQueryable<T> Query(string sql, params object[] parameters);
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate = null);


        //insert, update, delete
        T Insert(T entity);
        void Insert(params T[] entities);
        void Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
        void Delete(dynamic id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
    }
}
