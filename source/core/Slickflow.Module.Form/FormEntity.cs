using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Form Entity
    /// 表单实体对象
    /// </summary>
    [Table("FbForm")]
    public class FormEntity
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string FormCode { get; set; }
        public string Version { get; set; }
        public string FieldSummary { get; set; }
        public string TemplateContent { get; set; }
        public string HTMLContent { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> LastUpdatedDate { get; set; }
    }
}
