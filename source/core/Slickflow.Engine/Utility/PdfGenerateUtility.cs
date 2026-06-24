using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// PDF document generation utility for workflow process documents
    /// 工作流程文档PDF生成工具类
    /// </summary>
    public static class PdfGenerateUtility
    {
        private static readonly BaseColor HeaderColor = new BaseColor(22, 119, 255);
        private static readonly BaseColor RowAltColor = new BaseColor(245, 248, 255);

        /// <summary>
        /// Generate a workflow process document PDF from key-value fields
        /// 根据字段键值对生成工作流程文档PDF文件
        /// </summary>
        public static void GenerateProcessDocument(string title, IDictionary<string, string> fields, string outputPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");
            using var stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            var document = new Document(PageSize.A4, 50f, 50f, 60f, 60f);
            PdfWriter.GetInstance(document, stream);
            document.Open();

            AddContent(document, title, fields);
            document.Close();
        }

        /// <summary>
        /// Generate PDF bytes from title and key-value fields (no file I/O)
        /// 生成PDF字节流，不写入文件
        /// </summary>
        public static byte[] GenerateProcessDocumentBytes(string title, IDictionary<string, string> fields)
        {
            using var ms = new MemoryStream();
            var document = new Document(PageSize.A4, 50f, 50f, 60f, 60f);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            AddContent(document, title, fields);
            document.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Generate a simple PDF from plain text content
        /// 从纯文本内容生成简单PDF
        /// </summary>
        public static byte[] GenerateFromContent(string title, string content)
        {
            using var ms = new MemoryStream();
            var document = new Document(PageSize.A4, 50f, 50f, 60f, 60f);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f, HeaderColor);
            var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 11f);
            var metaFont = FontFactory.GetFont(FontFactory.HELVETICA, 9f, new BaseColor(120, 120, 120));

            document.Add(new Paragraph(title, titleFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 4f });
            document.Add(new Paragraph($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", metaFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 14f });
            document.Add(new Paragraph(content, bodyFont));

            document.Close();
            return ms.ToArray();
        }

        private static void AddContent(Document document, string title, IDictionary<string, string> fields)
        {
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18f, HeaderColor);
            var metaFont = FontFactory.GetFont(FontFactory.HELVETICA, 9f, new BaseColor(120, 120, 120));
            var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10f);
            var valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 10f);

            document.Add(new Paragraph(title, titleFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 4f });
            document.Add(new Paragraph($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", metaFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 16f });

            if (fields == null || fields.Count == 0)
                return;

            var table = new PdfPTable(2) { WidthPercentage = 100f, SpacingBefore = 6f };
            table.SetWidths(new float[] { 3f, 7f });

            var row = 0;
            foreach (var kvp in fields)
            {
                var bg = row % 2 == 0 ? new BaseColor(255, 255, 255) : RowAltColor;
                table.AddCell(new PdfPCell(new Phrase(kvp.Key, labelFont))
                {
                    Padding = 7f,
                    BackgroundColor = new BaseColor(235, 241, 255),
                    BorderColor = new BaseColor(220, 220, 220)
                });
                table.AddCell(new PdfPCell(new Phrase(kvp.Value ?? string.Empty, valueFont))
                {
                    Padding = 7f,
                    BackgroundColor = bg,
                    BorderColor = new BaseColor(220, 220, 220)
                });
                row++;
            }

            document.Add(table);
        }
    }
}
