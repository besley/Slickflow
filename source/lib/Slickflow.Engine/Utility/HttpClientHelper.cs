using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace Slickflow.Engine.Utility
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
        /// 创建HttpClientHelper类
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static HttpClientHelper CreateHelper(string url, string ticket)
        {
            var helper = new HttpClientHelper();
            helper.URL = url;

            var authStr = WebApiRequestHeaderNamePrefix + ticket;
            
            if (!HttpClient.DefaultRequestHeaders.Contains(WebApiRequestHeaderAuthorization))
            {
                HttpClient.DefaultRequestHeaders.Add(WebApiRequestHeaderAuthorization, authStr);
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
        /// 异步获取
        /// </summary>
        /// <typeparam name="T1">类型</typeparam>
        /// <returns>对象</returns>
        public async Task<T1> GetAsync<T1>()
            where T1 : class
        {
            var response = await HttpClient.GetAsync(URL);
            var message = await response.Content.ReadAsStringAsync();
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
        /// 异步提交
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public async Task<T2> PostAsync<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(URL, content);
            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T2>(message);

            return result;
        }

        /// <summary>
        /// Post提交
        /// </summary>
        /// <param name="json">Json格式对象</param>
        /// <returns>任意结果</returns>
        public Object Post(string json)
        {
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            return message;
        }

        /// <summary>
        /// Post提交
        /// </summary>
        /// <param name="json">Json格式对象</param>
        /// <returns>任意结果</returns>
        public async Task<Object> PostAsync(string json)
        {
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(URL, content);
            var message = await response.Content.ReadAsStringAsync();
            return message;
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
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<ResponseResult<List<T2>>>(message);

            return result.Entity;
        }

        /// <summary>
        /// 异步查询列表
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public async Task<List<T2>> QueryAsync<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(URL, content);
            var message = await response.Content.ReadAsStringAsync();
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
        /// 异步插入
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public async Task<T2> InsertAsync<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(URL, content);
            var message = await response.Content.ReadAsStringAsync();
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
        public T2 Put<T1, T2>(T1 t)
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

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public async Task<T2> PutAsync<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            string jsonValue = JsonConvert.SerializeObject(t);
            StringContent content = new System.Net.Http.StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = await HttpClient.PutAsync(URL, content);
            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T2>(message);

            return result;
        }

        /// <summary>
        /// Post提交
        /// </summary>
        /// <param name="json">Json格式对象</param>
        /// <returns>任意结果</returns>
        public Object Put(string json)
        {
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            return message;
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
            return Put<T1, T2>(t);
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <typeparam name="T1">类型1</typeparam>
        /// <typeparam name="T2">类型2</typeparam>
        /// <param name="t">对象t</param>
        /// <returns>对象</returns>
        public async Task<T2> UpdateAsync<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            return await PutAsync<T1, T2>(t);
        }

        /// <summary>
        /// 返回请求结果
        /// </summary>
        /// <returns>字符串</returns>
        public string Delete()
        {
            var response = HttpClient.DeleteAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;

            return message;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T1">类型</typeparam>
        /// <returns>对象</returns>
        public T1 Delete<T1>()
            where T1 : class
        {
            var response = HttpClient.DeleteAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T1>(message);

            return result;
        }


        /// <summary>
        /// 异步删除
        /// </summary>
        /// <typeparam name="T1">类型</typeparam>
        /// <returns>对象</returns>
        public async Task<T1> DeleteAsync<T1>()
            where T1 : class
        {
            var response = await HttpClient.DeleteAsync(URL);
            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T1>(message);

            return result;
        }
    }
}
