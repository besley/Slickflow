using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.WebDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.WebDemo.Ashx
{
    /// <summary>
    /// Wf 的摘要说明
    /// </summary>
    public class Wf : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string result = string.Empty;
            string Action = string.Empty;
            if (context.Request.QueryString["Action"] != null && !string.IsNullOrWhiteSpace(context.Request.QueryString["Action"].ToString()))
            {
                string ProcessGUID = GetRequestString(context, "ProcessGUID");
                string Version = GetRequestString(context, "Version");
                Action = GetRequestString(context, "Action");
                switch (Action)
                {
                    case "QueryProcessFile":
                        ProcessFileQuery query = new ProcessFileQuery();
                        query.ProcessGUID = ProcessGUID;
                        query.Version = Version;
                        ResponseResult<ProcessFileEntity> _result = QueryProcessFile(query);
                        result = JsonHelper.ObjectToJson(_result);
                        break;


                    case "QueryReadyActivityInstance":
                        string AppInstanceID = GetRequestString(context, "AppInstanceID");
                        TaskQueryEntity taskQuery = new TaskQueryEntity();
                        taskQuery.ProcessGUID = ProcessGUID;
                        taskQuery.AppInstanceID = AppInstanceID;

                        ResponseResult<List<ActivityInstanceEntity>> activityInstanceResult = QueryReadyActivityInstance(taskQuery);
                        result = JsonHelper.ObjectToJson(activityInstanceResult);
                        break;
                }
            }
            context.Response.Write(result);
            context.Response.End();
        }




        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult<ProcessFileEntity> QueryProcessFile(ProcessFileQuery query)
        {
            var result = ResponseResult<ProcessFileEntity>.Default();
            try
            {
                var wfService = new WorkflowService();
                var entity = wfService.GetProcessFile(query.ProcessGUID, query.Version);
                result = ResponseResult<ProcessFileEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessFileEntity>.Error(string.Format("获取流程XML文件失败！{0}", ex.Message));
            }
            return result;
        }


        public ResponseResult<List<ActivityInstanceEntity>> QueryReadyActivityInstance(TaskQueryEntity query)
        {
            var result = ResponseResult<List<ActivityInstanceEntity>>.Default();
            try
            {
                var wfService = new WorkflowService();
                var itemList = wfService.GetRunningActivityInstance(query).ToList();


                result = ResponseResult<List<ActivityInstanceEntity>>.Success(itemList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ActivityInstanceEntity>>.Error(string.Format(
                    "获取待办任务数据失败, 异常信息:{0}",
                    ex.Message));
            }
            return result;
        }

        private string GetRequestString(HttpContext context, string Key)
        {
            string val = string.Empty;
            if (context != null && !string.IsNullOrWhiteSpace(Key))
            {
                if (context.Request.QueryString[Key] != null && !string.IsNullOrWhiteSpace(context.Request.QueryString[Key].ToString()))
                {
                    val = context.Request.QueryString[Key].ToString();
                }
                else
                {
                    if (context.Request.Form[Key] != null && !string.IsNullOrWhiteSpace(context.Request.Form[Key].ToString()))
                    {
                        val = context.Request.Form[Key].ToString();
                    }
                }
            }
            return val;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}