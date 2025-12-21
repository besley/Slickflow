using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickOne.WebUtility
{
    /// <summary>
    /// Response Result
    /// 响应结果类
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// Status(0-default, 1-success, -1-error)
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// New Id Value
        /// </summary>
        public dynamic NewId
        {
            get;
            set;
        }

        /// <summary>
        /// Extra Data
        /// </summary>
        public object ExtraData
        {
            get;
            set;
        }

        /// <summary>
        /// Default
        /// </summary>
        public static ResponseResult Default(string message = null)
        {
            var result = new ResponseResult();
            result.Status = 0;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// Success
        /// </summary>
        public static ResponseResult Success(string message = null)
        {
            var result = new ResponseResult();
            result.Status = 1;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// Success
        /// </summary>
        public static ResponseResult Success(dynamic newId, string message = null)
        {
            var result = new ResponseResult();
            result.NewId = newId;
            result.Status = 1;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// Error
        /// </summary>
        public static ResponseResult Error(string errorMessage)
        {
            var result = new ResponseResult();
            result.Status = -1;
            result.Message = errorMessage;
            return result;
        }
    }

    /// <summary>
    /// Response Result by Generic
    /// </summary>
    public class ResponseResult<T> where T : class
    {
        /// <summary>
        /// Status
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// New Id Value
        /// </summary>
        public dynamic NewId
        {
            get;
            set;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public T Entity
        {
            get;
            set;
        }

        /// <summary>
        /// Total Page
        /// </summary>
        public int TotalPages
        {
            get;
            set;
        }

        /// <summary>
        /// Total Rows Count
        /// </summary>
        public int TotalRowsCount
        {
            get;
            set;
        }

        /// <summary>
        /// Default
        /// </summary>
        public static ResponseResult<T> Default()
        {
            var result = new ResponseResult<T>();
            result.Entity = null;
            result.Status = 0;
            result.Message = string.Empty;

            return result;
        }

        /// <summary>
        /// Success
        /// </summary>
        public static ResponseResult<T> Success(T t, string message = null)
        {
            var result = new ResponseResult<T>();
            result.Entity = t;
            result.Status = 1;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// Success
        /// </summary>
        public static ResponseResult<T> Success(int newId, string message = null)
        {
            var result = new ResponseResult<T>();
            result.NewId = newId;
            result.Status = 1;
            result.Message = message;
            return result;
        }
        /// <summary>
        /// Error
        /// </summary>
        public static ResponseResult<T> Error(string message = null)
        {
            var result = new ResponseResult<T>();
            result.Entity = null;
            result.Status = -1;
            result.Message = message;

            return result;
        }
    }
}
