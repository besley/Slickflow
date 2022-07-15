using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// 工作流模块-用户实体
    /// 用以适配客户方组织机构或权限对象模型
    /// 字段类型：字符串
    /// </summary>
    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }
}
