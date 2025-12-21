using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Xml;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Slickflow.Module.Localize;

//using Slickflow.Migration;

namespace sfdapi.Controllers
{
    /// <summary>
    /// File Uplaod
    /// 文件上传控制器
    /// </summary>
    public class FineUploadMxGrpController : Controller
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        /// <summary>
        /// Import file
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
        /// It was used by mxGraph project users
        /// We make comment with this mehtod, if there is any mxGraph project users, it can reference it again
        /// 2025-01-05
        /// Besley
        /// 创建新流程
        /// </summary>
        /// <param name="xmlContent"></param>
        private bool CreateNewProcess(string xmlContent, out string message)
        {
            bool isUploaded = false;
            message = string.Empty;

            //xml package
            if (!string.IsNullOrEmpty(xmlContent))
            {
                //migrate xml process to BPMN2 process
                //We make comment with this mehtod, if there is any mxGraph project users, it can reference it again
                //2025-01-05
                //MigrateHelper.MigrateXmlProcess(xmlContent);

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