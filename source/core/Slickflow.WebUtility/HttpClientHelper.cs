using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Slickflow.WebUtility
{
    /// <summary>
    /// Mime format
    /// Mime内容格式
    /// </summary>
    public enum MimeFormat
    {
        XML = 0,
        JSON = 1
    }

    /// <summary>
    /// HttpClient Helper
    /// </summary>
    public class HttpClientHelper
    {
        private const string WebApiRequestHeaderAuthorization = "Authorization";
        private const string WebApiRequestHeaderNamePrefix = "BASIC ";
        private const string WebApiRequestHeaderNameHashed = "BASIC-HASHED";

        private static readonly HttpClient HttpClient;

        /// <summary>
        /// URL 
        /// </summary>
        private string URL
        {
            get;
            set;
        }

        static HttpClientHelper()
        {
            HttpClient = new System.Net.Http.HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Create method
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpClientHelper CreateHelper(string url)
        {
            var helper = new HttpClientHelper();

            if (url != string.Empty)
            {
                helper.URL = url;
            }
            return helper;
        }


        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            using (var response = HttpClient.GetAsync(URL).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                return message;
            }
        }

        /// <summary>
        /// Get by generic
        /// </summary>
        public T1 Get<T1>()
            where T1 : class
        {
            using (var response = HttpClient.GetAsync(URL).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T1>(message);
                return result;
            }
        }

        /// <summary>
        /// Post
        /// </summary>
        public T2 Post<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            using (StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json"))
            using (var response = HttpClient.PostAsync(URL, content).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T2>(message);
                return result;
            }
        }

        /// <summary>
        /// Post
        /// </summary>
        public ResponseResult Post(dynamic t)
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            using (StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json"))
            using (var response = HttpClient.PostAsync(URL, content).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ResponseResult>(message);
                return result;
            }
        }

        /// <summary>
        /// Query
        /// </summary>
        public List<T2> Query<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            using (StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json"))
            using (var response = HttpClient.PostAsync(URL, content).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ResponseResult<List<T2>>>(message);
                return result.Entity;
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        public T2 Insert<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            using (StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json"))
            using (var response = HttpClient.PostAsync(URL, content).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T2>(message);
                return result;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        public T2 Update<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            using (StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json"))
            using (var response = HttpClient.PutAsync(URL, content).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T2>(message);
                return result;
            }
        }

        /// <summary>
        /// Put
        /// </summary>
        public T2 Put<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            using (StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json"))
            using (var response = HttpClient.PutAsync(URL, content).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T2>(message);
                return result;
            }
        }

        /// <summary>
        /// Put (返回ResponseResult)
        /// </summary>
        public ResponseResult Put(dynamic t)
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            using (StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json"))
            using (var response = HttpClient.PutAsync(URL, content).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ResponseResult>(message);
                return result;
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        public string Delete()
        {
            using (var response = HttpClient.DeleteAsync(URL).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                return message;
            }
        }

        /// <summary>
        /// Delete by generic
        /// </summary>
        public T1 Delete<T1>()
            where T1 : class
        {
            using (var response = HttpClient.DeleteAsync(URL).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T1>(message);
                return result;
            }
        }

        /// <summary>
        /// Delete with request body
        /// </summary>
        public T2 Delete<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            // 注意：HttpClient的DeleteAsync方法不支持请求体
            // 这里使用自定义的HttpRequestMessage来实现带请求体的DELETE请求
            var request = new HttpRequestMessage(HttpMethod.Delete, URL)
            {
                Content = new StringContent(JsonConvert.SerializeObject(t), Encoding.UTF8, "application/json")
            };

            using (request)
            using (var response = HttpClient.SendAsync(request).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T2>(message);
                return result;
            }
        }

        /// <summary>
        /// Delete with request body (返回ResponseResult)
        /// </summary>
        public ResponseResult Delete(dynamic t)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, URL)
            {
                Content = new StringContent(JsonConvert.SerializeObject(t), Encoding.UTF8, "application/json")
            };

            using (request)
            using (var response = HttpClient.SendAsync(request).Result)
            {
                var message = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ResponseResult>(message);
                return result;
            }
        }
    }
}