using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace Slickflow.WebApi.Utility
{
    /// <summary>
    /// 响应结果类
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:1-成功；0-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Default(string message = null)
        {
            var result = new ResponseResult();
            result.Status = 0;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:1-成功；0-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Success(string message = null)
        {
            var result = new ResponseResult();
            result.Status = 1;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:1-成功；0-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Error(string errorMessage)
        {
            var result = new ResponseResult();
            result.Status = -1;
            result.Message = errorMessage;
            return result;
        }

        /// <summary>
        /// 解析返回结果中的状态标志位
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Parse(string message)
        {
            var result = JsonSerializer.DeserializeFromString<ResponseResult>(message);
            var success = result.Status == 1;

            return success;
        }

        public static ResponseResult Deserialize(string message)
        {
            var result = JsonSerializer.DeserializeFromString<ResponseResult>(message);
            return result;
        }
    }

    /// <summary>
    /// 响应消息类
    /// </summary>
    public class ResponseResult<T> : ResponseResult
        where T : class
    {
        /// <summary>
        /// 业务实体
        /// </summary>
        public T Entity
        {
            get;
            set;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:1-成功; 0-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Default()
        {
            var result = new ResponseResult<T>();
            result.Entity = null;
            result.Status = 0;
            result.Message = string.Empty;

            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:1-成功; 0-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Success(T t, string message = null)
        {
            var result = new ResponseResult<T>();
            result.Entity = t;
            result.Status = 1;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// Http 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:1-成功; 0-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Error(string message = null)
        {
            var result = new ResponseResult<T>();
            result.Entity = null;
            result.Status = -1;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// Http 响应消息反序列化类
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public new static ResponseResult<T> Deserialize(string message)
        {
            var response = JsonSerializer.DeserializeFromString<ResponseResult<T>>(message);
            return response;
        }
    }
}