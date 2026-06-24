using System;
using System.Collections.Generic;
using System.IO;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Engine.Utility;

namespace Slickflow.Module.External.Document
{
    /// <summary>
    /// LocalService: generates a PDF document from process variables and saves it to disk.
    /// 本地服务：从流程变量生成 PDF 文档并保存到指定目录，供 LocalService 节点调用。
    ///
    /// Process variables read (via EventService):
    ///   document_title   — 文档标题（可选，默认 "流程文档"）
    ///   output_dir       — 输出目录（可选，默认系统临时目录下 slickflow-docs）
    ///   applicant        — 申请人
    ///   department       — 部门
    ///   amount           — 金额
    ///   description      — 描述/说明
    ///   approver         — 审批人
    ///   approval_date    — 审批日期
    ///   approval_result  — 审批结果
    ///   remarks          — 备注
    ///   process_code     — 流程编号
    ///   process_name     — 流程名称
    ///   start_date       — 开始日期
    ///   end_date         — 结束日期
    ///
    /// Output variable saved back:
    ///   pdf_output_path  — 生成的 PDF 文件完整路径
    /// </summary>
    public class PdfEmailService : ExternalServiceBase, IExternalService
    {
        private static readonly string[] KnownFieldKeys =
        {
            "applicant", "department", "amount", "description",
            "approver", "approval_date", "approval_result", "remarks",
            "process_code", "process_name", "start_date", "end_date"
        };

        public override void Execute()
        {
            var _title = GetVar("document_title");
            var documentTitle = string.IsNullOrWhiteSpace(_title) ? "流程文档" : _title;
            var _dir = GetVar("output_dir");
            var outputDir = string.IsNullOrWhiteSpace(_dir)
                ? Path.Combine(Path.GetTempPath(), "slickflow-docs") : _dir;

            // Collect display fields from process variables
            var fields = new Dictionary<string, string>();
            foreach (var key in KnownFieldKeys)
            {
                var val = GetVar(key);
                if (!string.IsNullOrEmpty(val))
                    fields[TranslateKey(key)] = val;
            }

            // Generate PDF file
            Directory.CreateDirectory(outputDir);
            var fileName  = $"{documentTitle}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var outputPath = Path.Combine(outputDir, fileName);
            PdfGenerateUtility.GenerateProcessDocument(documentTitle, fields, outputPath);

            // Save output path back into process variable for downstream nodes
            try
            {
                EventService?.SaveVariable(ProcessVariableScopeEnum.Process, "pdf_output_path", outputPath);
            }
            catch { /* non-critical */ }
        }

        private string GetVar(string key)
        {
            try { return EventService?.GetVariable(ProcessVariableScopeEnum.Process, key); }
            catch { return null; }
        }

        private static string TranslateKey(string key) => key switch
        {
            "applicant"       => "申请人",
            "department"      => "部门",
            "amount"          => "金额",
            "description"     => "描述",
            "approver"        => "审批人",
            "approval_date"   => "审批日期",
            "approval_result" => "审批结果",
            "remarks"         => "备注",
            "process_code"    => "流程编号",
            "process_name"    => "流程名称",
            "start_date"      => "开始日期",
            "end_date"        => "结束日期",
            _                 => key,
        };
    }
}
