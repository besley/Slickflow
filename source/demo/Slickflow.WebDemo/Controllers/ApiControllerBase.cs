using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using SlickOne.WebUtility;
using Slickflow.Data;

namespace Slickflow.WebDemo.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class ApiControllerBase : Controller
    {
        private Repository _repository;
        protected Repository QuickRepository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new Repository();
                }
                return _repository;
            }
        }

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
                var entity = QuickRepository.GetById<T>(id);
                result = ResponseResult<T>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<T>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 插入实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        public ResponseResult<dynamic> Insert<T>(T entity) where T : class
        {
            var result = ResponseResult<dynamic>.Default();
            try
            {
                var newId = QuickRepository.Insert<T>(entity);
                result = ResponseResult<dynamic>.Success(newId);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<dynamic>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        [HttpPut]
        public ResponseResult Update<T>(T entity) where T : class
        {
            var result = ResponseResult.Default();
            try
            {
                var isOk = QuickRepository.Update<T>(entity);
                if (isOk == true)
                    result = ResponseResult.Success();
                else
                    result = ResponseResult.Error("Error");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
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
                var isOk = QuickRepository.Delete<T>(id);
                if (isOk == true)
                    result = ResponseResult.Success();
                else
                    result = ResponseResult.Error("Error");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }
    }
}