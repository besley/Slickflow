using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using ServiceStack.Text;


namespace Slickflow.Winform.Utility
{
/// <summary>
    /// Mime内容格式
    /// </summary>
    public enum MimeFormat
    {
        XML = 0,
        JSON = 1
    }

    /// <summary>
    /// HttpClient 帮助类
    /// </summary>
    public class HttpClientHelper
    {
        #region 创建和构造
        private const string WebApiRequestHeaderName = "BASIC-HASHED";

        public HttpClient HttpClient
        {
            get;
            set;
        }

        private HttpClient Create(MimeFormat format, string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            switch (format)
            {
                case MimeFormat.XML:
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/xml"));
                    break;
                case MimeFormat.JSON:
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    break;
            }

            if (url != string.Empty)
            {
                client.BaseAddress = new Uri(url);
            }
            return client;
        }

        public static HttpClientHelper CreateHelper(string url)
        {
            var helper = new HttpClientHelper();
            var client = helper.Create(MimeFormat.JSON, url);
            helper.HttpClient = client;
            return helper;
        }
        #endregion

        /// <summary>
        /// HttpPost方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T2 Post<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<T1>(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<T2>(message);
                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// HttpPost方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseResult Post(dynamic t)
        {
            string jsonValue = JsonSerializer.SerializeToString(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<ResponseResult>(message);
                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// HttpGet方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public ResponseResult<T1> Get<T1>()
            where T1 : class
        {
            ResponseResult<T1> result = null;
            var resp = HttpClient.GetAsync("");
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(message))
                {
                    result = JsonSerializer.DeserializeFromString<ResponseResult<T1>>(message);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Post获取分页数据
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseResult<List<T2>> GetPaged<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<T1>(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<ResponseResult<List<T2>>>(message);
                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Post获取分页数据
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseResult<List<dynamic>> GetDynamicPaged<T1>(T1 t)
            where T1 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<T1>(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseResult<List<dynamic>>>(message);

                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 根据ID获取动态类型数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult<dynamic> GetDynamicEntity()
        {
            ResponseResult<dynamic> result = null;
            var resp = HttpClient.GetAsync("");
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(message))
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseResult<dynamic>>(message);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Post获取分页数据
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseResult<List<T2>> Query<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<T1>(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<ResponseResult<List<T2>>>(message);

                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取全部数据（仅限于数据量规模不大的操作）
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public ResponseResult<List<T1>> GetAll<T1>()
            where T1 : class
        {
            ResponseResult<List<T1>> result = null;
            var resp = HttpClient.GetAsync("");
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(message))
                {
                    result = JsonSerializer.DeserializeFromString<ResponseResult<List<T1>>>(message);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseResult Insert<T1>(T1 t)
            where T1 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<T1>(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8,
                "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var resposne = resp.Result;
                var message = resposne.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<ResponseResult>(message);

                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///  带新ID的插入方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseResult<T1> Insert2<T1>(T1 t)
            where T1 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<T1>(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8,
                "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<ResponseResult<T1>>(message);

                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// HttpPut更新
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseResult Update<T1>(T1 t)
            where T1 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<T1>(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8,
                "application/json");
            var resp = HttpClient.PutAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<ResponseResult>(message);

                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 批处理保存
        /// </summary>
        /// <typeparam name="T1">实体</typeparam>
        /// <param name="t">字典:{["insert", "update", "delete"], List<T1>}</param>
        /// <returns></returns>
        public ResponseResult SaveBatch<T1>(Dictionary<string, List<T1>> t)
            where T1 : class
        {
            string jsonValue = JsonSerializer.SerializeToString<Dictionary<string, List<T1>>>(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8,
                "application/json");
            var resp = HttpClient.PostAsync("", content);
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.DeserializeFromString<ResponseResult>(message);

                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public ResponseResult Delete<T1>()
            where T1 : class
        {
            ResponseResult result = null;
            var resp = HttpClient.DeleteAsync("");
            try
            {
                var response = resp.Result;
                var message = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(message))
                {
                    result = JsonSerializer.DeserializeFromString<ResponseResult>(message);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (resp.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {
                    throw new Exception("网络连接异常!");
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
