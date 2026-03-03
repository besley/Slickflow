using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Process Template Define
    /// 流程示例变量定义
    /// </summary>
    internal class ProcessTemplateDefine
    {
        //标准流程模板变量名称
        //Standard Process Template Variable Name
        public const string WF_TEMPLATE_STANDARD_BLANK = "Blank";
        public const string WF_TEMPLATE_STANDARD_DEFAULT = "Default";
        public const string WF_TEMPLATE_STANDARD_SIMPLE = "Simple";
        public const string WF_TEMPLATE_STANDARD_SEQUENCE = "Sequence";
        public const string WF_TEMPLATE_STANDARD_GATEWAY = "Gateway";
        public const string WF_TEMPLATE_STANDARD_MULTIPLEINSTANCE = "MultipleInstance";
        public const string WF_TEMPLATE_STANDARD_ANDSPLITMI = "AndSplitMI";
        public const string WF_TEMPLATE_STANDARD_SUBPROCESS = "SubProcess";
        public const string WF_TEMPLATE_STANDARD_CONDITIONAL = "Conditional";
        public const string WF_TEMPLATE_STANDARD_COMPLEX = "Complex";
        public const string WF_TEMPLATE_STANDARD_PARALLEL = "Parallel";
        public const string WF_TEMPLATE_STANDARD_RUNPROCESS = "RunProcess";

        //业务流程模板变量名称
        //Business Process Template Variable Name
        public const string WF_TEMPLATE_BUSINESS_ASKFORLEAVE = "AskforLeave";
        public const string WF_TEMPLATE_BUSINESS_EORDER = "EOrder";
        public const string WF_TEMPLATE_BUSINESS_REIMBURSEMENT = "Reimbursement";
        public const string WF_TEMPLATE_BUSINESS_WAREHOUSING = "Warehousing";
        public const string WF_TEMPLATE_BUSINESS_OFFICEIN = "OfficeIn";
        public const string WF_TEMPLATE_BUSINESS_CONTRACT = "Contract";
    }
}
