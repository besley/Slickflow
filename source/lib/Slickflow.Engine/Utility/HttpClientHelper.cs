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
    /// Mime format
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
        /// Create Helper
        /// </summary>
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
        /// Create Helper
        /// </summary>
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
        /// HttpGet
        /// </summary>
        public string Get()
        {
            var response = HttpClient.GetAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;

            return message;
        }

        /// <summary>
        /// HttpGet
        /// </summary>
        public T1 Get<T1>()
            where T1 : class
        {
            var response = HttpClient.GetAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T1>(message);

            return result;
        }


        /// <summary>
        /// GetAsync
        /// </summary>
        public async Task<T1> GetAsync<T1>()
            where T1 : class
        {
            var response = await HttpClient.GetAsync(URL);
            var message = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T1>(message);

            return result;
        }

        /// <summary>
        /// HttpPost
        /// </summary>
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
        /// HttpPost async
        /// </summary>
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
        /// Post
        /// </summary>
        public Object Post(string json)
        {
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            return message;
        }

        /// <summary>
        /// Post
        /// </summary>
        public async Task<Object> PostAsync(string json)
        {
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(URL, content);
            var message = await response.Content.ReadAsStringAsync();
            return message;
        }

        /// <summary>
        /// Query
        /// </summary>
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
        /// Query Async
        /// </summary>
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
        /// Insert
        /// </summary>
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
        /// Insert Async
        /// </summary>
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
        /// Put
        /// </summary>
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
        /// Put async
        /// </summary>
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
        /// Put
        /// </summary>
        public Object Put(string json)
        {
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync(URL, content).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            return message;
        }

        /// <summary>
        /// Update
        /// </summary>
        public T2 Update<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            return Put<T1, T2>(t);
        }

        /// <summary>
        /// Update Async
        /// </summary>
        public async Task<T2> UpdateAsync<T1, T2>(T1 t)
            where T1 : class
            where T2 : class
        {
            return await PutAsync<T1, T2>(t);
        }

        /// <summary>
        /// Delete
        /// </summary>
        public string Delete()
        {
            var response = HttpClient.DeleteAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;

            return message;
        }

        /// <summary>
        /// Delete
        /// </summary>
        public T1 Delete<T1>()
            where T1 : class
        {
            var response = HttpClient.DeleteAsync(URL).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T1>(message);

            return result;
        }


        /// <summary>
        /// Delete Aysnc
        /// </summary>
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
