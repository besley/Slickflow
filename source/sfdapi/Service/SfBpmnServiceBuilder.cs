using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Template;
using System.Text;
using System.Text.Json;
using Slickflow.WebUtility;
using sfdapi.Models;
using Slickflow.Graph.Model;

namespace sfdapi.Service
{
    public class SfBpmnServiceBuilder
    {
        private readonly HttpClient _httpClient;

        public SfBpmnServiceBuilder()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(120);
        }

        public async Task<ProcessFileEntity> CreateBpmnProcessByTemplate(string templateName)
        {
            try
            {
                var xmlContent = BpmnFileSampleDefine.RebuildBpmnPorcessXmlContentByTemplate(templateName);
                var workflowService = new WorkflowService();
                var processFileEntity = workflowService.CreateProcessByXML(xmlContent);
                return processFileEntity;
            }
            catch (Exception ex)
            {
                LogManager.RecordLog("An error occurred when generating process by template",
                    LogEventType.Exception, LogPriority.Normal, templateName, ex);
                throw new InvalidOperationException($"An error occurred when creating the process by template, detail:{ex.Message}");
            }
        }

        #region Load Process Template Content
        public ProcessTemplate LoadTemplateContent(ProcessTemplateType templateType)
        {
            var template = ProcessTemplateFactory.LoadTemplateContent(templateType);
            return template;
        }
        #endregion
    }
}
