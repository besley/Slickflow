using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SlickOne.WebUtility
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
        private const string WebApiRequestHeaderAuthorization = "Authorization";
        private const string WebApiRequestHeaderNamePrefix = "BASIC ";
        private const string WebApiRequestHeaderNameHashed = "BASIC-HASHED";

        private static readonly HttpClient HttpClient;

        /// <summary>
        /// URL 属性
        /// </summary>
        private string URL
        {
            get;
            set;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        static HttpClientHelper()
        {
            HttpClient = new System.Net.Http.HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// 创建基本HttpClientHelper类
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>帮助类</returns>
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
        /// 返回请求结果
        /// </summary>
        /// <returns>字符串</returns>
        public string Get()
        {
            var response = HttpClient.GetAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;

            return message;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T1">类型</typeparam>
        /// <returns>对象</returns>
        public T1 Get<T1>()
            where T1 : class
        {
            var response = HttpClient.GetAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T1>(message);

            return result;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public T2 Post<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T2>(message);

            return result;
        }

        /// <summary>
        /// Post方法
        /// </summary>
        /// <param name="t">动态实体</param>
        /// <returns>响应结果</returns>
        public ResponseResult Post(dynamic t)
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<ResponseResult>(message);

            return result;
        }

        /// <summary>
        /// Post获取分页数据
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public List<T2> Query<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var resp = HttpClient.PostAsync(URL, content);
            var response = resp.Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<ResponseResult<List<T2>>>(message);

            return result.Entity;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public T2 Insert<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T2>(message);

            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public T2 Update<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = HttpClient.PutAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T2>(message);

            return result;
        }
    }
}
