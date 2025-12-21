using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.WebUtility;

namespace Slickflow.MvcDemo.Controllers.WebApi
{
    /// <summary>
    /// Api Controller Base
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
        /// Get by Id
        /// 按主键查询实体
        /// </summary>
        [HttpGet]
        public ResponseResult<T> Get<T>(dynamic id) where T : class
        {
            var result = ResponseResult<T>.Default();
            try
            {
                var entity = QuickRepository.GetById<T>(id);
                result = ResponseResult<T>.Success(entity, "Read data successfully!");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<T>.Error(
                    string.Format("Rea data failed {0}, exception:{1}！", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Insert data
        /// 插入实体数据
        /// </summary>
        [HttpPost]
        public ResponseResult<dynamic> Insert<T>(T entity) where T : class
        {
            var result = ResponseResult<dynamic>.Default();
            try
            {
                var newId = QuickRepository.Insert<T>(entity);
                result = ResponseResult<dynamic>.Success(newId, "Data created successful！");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<dynamic>.Error(
                    string.Format("Create data failed {0}, exception:{1}", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Update Data
        /// 更新实体数据
        /// </summary>
        [HttpPut]
        public ResponseResult Update<T>(T entity) where T : class
        {
            var result = ResponseResult.Default();
            try
            {
                var isOk = QuickRepository.Update<T>(entity);
                if (isOk == true)
                    result = ResponseResult.Success("Data update successful！");
                else
                    result = ResponseResult.Error(
                        string.Format("Update data failed {0}！", typeof(T).ToString())
                    );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Update failed {0}, exception:{1}", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Delete Data
        /// 删除实体数据
        /// </summary>
        [HttpDelete]
        public ResponseResult Delete<T>(int id) where T : class
        {
            var result = ResponseResult.Default();
            try
            {
                var isOk = QuickRepository.Delete<T>(id);
                if (isOk == true)
                    result = ResponseResult.Success("Data deleted successfully！");
                else
                    result = ResponseResult.Error(
                        string.Format("Delete data failed {0}！", typeof(T).ToString())
                    );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Delete data failed {0}, exception:{1}", typeof(T).ToString(), ex.Message)
                );
            }
            return result;
        }
    }
}