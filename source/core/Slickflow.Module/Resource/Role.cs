using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Module.Resource
{
    /// <summary>
    /// Workflow module - Role Entity
    /// Used to adapt to the client's organizational structure or permission object model
    /// Field type: String
    /// 工作流模块-角色实体
    /// 用以适配客户方组织机构或权限对象模型
    /// 字段类型：字符串
    /// </summary>
    public class Role
    {
        public string Id
        {
            get;
            set;
        }

        public string RoleName
        {
            get;
            set;
        }

        public string RoleCode
        {
            get;
            set;
        }

        public List<User> UserList
        {
            get;
            set;
        }
    }
}
