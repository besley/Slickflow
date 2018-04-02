using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.ModelDemo.Data;

namespace Slickflow.MvcDemo.Controllers.WebApi
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class ApiControllerBase : Controller
    {
        /// <summary>
        /// 按主键查询实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>结果及实体</returns>
        [HttpGet]
        public ResponseResult<T> Get<T>(dynamic id) where T : class
        {
            var result = ResponseResult<T>.Default();
            try
            {
                using (var session = DbFactory.CreateSession())
                {
                    var entity = session.GetRepository<T>().GetByID(id);
                    result = ResponseResult<T>.Success(entity, "读取数据成功!");
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<T>.Error(
                    string.Format("获取数据{0}失败, 异常信息:{1}！", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 插入实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        public ResponseResult<T> Insert<T>([FromBody] T entity) where T : class
        {
            var result = ResponseResult<T>.Default();
            try
            {
                using (var session = DbFactory.CreateSession())
                {
                    var newEntry = session.GetRepository<T>().Insert(entity);
                    session.SaveChanges();
                    result = ResponseResult<T>.Success(newEntry, "插入数据成功！");
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<T>.Error(
                    string.Format("插入数据{0}失败, 异常信息:{1}", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        [HttpPut]
        public ResponseResult Update<T>([FromBody] T entity) where T : class
        {
            var result = ResponseResult.Default();
            try
            {
                using (var session = DbFactory.CreateSession())
                {
                    session.GetRepository<T>().Update(entity);
                    session.SaveChanges();
                    result = ResponseResult.Success("更新数据成功！");
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("更新数据{0}失败, 错误:{1}", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="id">实体主键ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public ResponseResult Delete<T>(int id) where T : class
        {
            var result = ResponseResult.Default();
            try
            {
                using (var session = DbFactory.CreateSession())
                {
                    session.GetRepository<T>().Delete(id);
                    session.SaveChanges();
                    result = ResponseResult.Success("删除数据成功！");
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("删除数据{0}失败, 错误:{1}", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }
    }
}