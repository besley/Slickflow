using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.WebDemoV2._0.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Slickflow.WebDemoV2._0.Ashx
{
    /// <summary>
    /// Wf2Xml 的摘要说明
    /// </summary>
    public class Wf2Xml : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string result = string.Empty;
            string Action = string.Empty;
            if (context.Request.QueryString["Action"] != null && !string.IsNullOrWhiteSpace(context.Request.QueryString["Action"].ToString()))
            {
                Action = context.Request.QueryString["Action"].ToString();
                switch (Action)
                {
                    case "QueryProcessFile":
                        string ProcessGUID = context.Request.QueryString["ProcessGUID"] == null ? string.Empty : context.Request.QueryString["ProcessGUID"].ToString();
                        string Version = context.Request.QueryString["Version"] == null ? string.Empty : context.Request.QueryString["Version"].ToString();
                        ProcessFileQuery query = new ProcessFileQuery();
                        query.ProcessGUID = ProcessGUID;
                        query.Version = Version;
                        ResponseResult<ProcessFileEntity> _result = QueryProcessFile(query);
                        result = JsonHelper.ObjectToJson(_result);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}