using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Slickflow.Module.Localize;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;

namespace sfdapi.Controllers
{
    /// <summary>
    /// File Uploader Controller
    /// 文件上传控制器
    /// </summary>
    public class FineUploadController : Controller
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        /// <summary>
        /// File Import
        /// 文件导入过程
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Import()
        {
            string message = string.Empty;

            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return Ok(new { success = false, Message = "Unsupported media type!" });
            }

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);
                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        try
                        {
                            using (var streamReader = new StreamReader(section.Body))
                            {
                                var xmlContent = await streamReader.ReadToEndAsync();
                                var isOk = CreateNewProcess(xmlContent, out message);
                                return Ok(new { success = isOk, Message =  message});
                            }
                        }
                        catch (System.Exception ex)
                        {
                            return Ok(new { success = false, Message = ex.Message });
                        }
                    }
                }
                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }
            return Ok(new { success = false, Message = "Unknown other reasons！" });
        }

        /// <summary>
        /// Create new process
        /// 创建新流程
        /// </summary>
        private bool CreateNewProcess(string xmlContent, out string message)
        {
            bool isUploaded = false;
            message = string.Empty;

            //xml package
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);
            if (!string.IsNullOrEmpty(xmlContent) 
                && xmlDocument.DocumentElement != null)
            {
                var wfService = new WorkflowService();
                wfService.ImportProcess(xmlContent);
                isUploaded = true;
                message = LocalizeHelper.GetDesignerMessage("fineuploadcontroller.importprocess.success");
            }
            else
            {
                message = LocalizeHelper.GetDesignerMessage("fineuploadcontroller.createnewprocess.norootelement.warning");
            }
            return isUploaded;
        }
    }
}