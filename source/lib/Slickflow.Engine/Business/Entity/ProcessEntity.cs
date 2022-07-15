using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// 流程实体类
    /// </summary>
    [Table("WfProcess")]
    public class ProcessEntity
    {
        public int ID { get; set; }
        public string ProcessGUID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessCode { get; set; }
        public string Version { get; set; }
        public byte IsUsing { get; set; }
        public string AppType { get; set; }
        public Nullable<byte> PackageType { get; set; }
        public Nullable<int> PackageProcessID { get; set; }
        public string PageUrl { get; set; }
        public string XmlFileName { get; set; }
        public string XmlFilePath { get; set; }
        public string XmlContent { get; set; }
        public byte StartType { get; set; }
        public string StartExpression { get; set; }
        public byte EndType { get; set; }
        public string EndExpression { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Nullable<DateTime> LastUpdatedDateTime { get; set; }
    }
}
