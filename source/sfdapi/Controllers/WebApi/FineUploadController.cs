/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
*  
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

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

namespace Slickflow.Designer.Controllers.WebApi
{
    /// <summary>
    /// 文件上传控制器
    /// </summary>
    public class FineUploadController : Controller
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        /// <summary>
        /// 文件导入过程
        /// </summary>
        /// <returns>导入结果对象</returns>
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
        /// 解析XML文件
        /// </summary>
        /// <param name="xmlContent"></param>
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
                var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
                nsmgr.AddNamespace("bpmn2", "http://www.omg.org/spec/BPMN/20100524/MODEL");

                var processNode = xmlDocument.DocumentElement.SelectSingleNode("bpmn2:process", nsmgr);
                if (processNode != null)
                {
                    var processName = XMLHelper.GetXmlAttribute(processNode, "id");
                    var processGUID = Guid.NewGuid().ToString();
                    var processCode = XMLHelper.GetXmlAttribute(processNode, "id");
                    //获取8位随机字符串和数字序列作为ProcessCode，保证唯一性
                    if (string.IsNullOrEmpty(processCode)) processCode = (new RandomSequenceGenerator()).GetRandomSequece();

                    if (!string.IsNullOrEmpty(processName) && !string.IsNullOrEmpty(processGUID))
                    {
                        var processEntity = new ProcessEntity
                        {
                            ProcessGUID = processGUID,
                            ProcessName = processName,
                            ProcessCode = processCode,
                            Version = "1",
                            IsUsing = 1,
                            XmlContent = xmlContent,
                            CreatedDateTime = System.DateTime.Now
                        };

                        var wfService = new WorkflowService();
                        wfService.ImportProcess(processEntity);
                        isUploaded = true;
                        message = LocalizeHelper.GetDesignerMessage("fineuploadcontroller.importprocess.success");
                    }
                    else
                    {
                        message = LocalizeHelper.GetDesignerMessage("fineuploadcontroller.createnewprocess.warning");
                    }
                }
                else
                {
                    message = LocalizeHelper.GetDesignerMessage("fineuploadcontroller.createnewprocess.noxmlnode.warning");
                }
            }
            else
            {
                message = LocalizeHelper.GetDesignerMessage("fineuploadcontroller.createnewprocess.norootelement.warning");
            }
            return isUploaded;
        }
    }
}