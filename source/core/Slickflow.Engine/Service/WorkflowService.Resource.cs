using System.Collections.Generic;
using System.Linq;
using Slickflow.Module.Form;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Workflow Service - Resource and Form
    /// 工作流服务 - 资源和表单查询
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        #region Role Resource Data
        
        /// <summary>
        /// Get role all
        /// 获取所有角色数据
        /// </summary>
        public IList<Role> GetRoleAll()
        {
            return ResourceService.GetRoleAll();
        }

        /// <summary>
        /// Get role by process
        /// 获取流程定义文件中的角色信息
        /// </summary>
        public IList<Role> GetRoleByProcess(string processId, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            var roleList = processModel.GetRoles();

            return roleList;
        }

        /// <summary>
        /// Get role user list by process
        /// 获取流程文件中角色用户的列表数据
        /// </summary>
        public IList<Role> GetRoleUserListByProcess(string processId, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            var roleList = processModel.GetRoles();
            var idsin = roleList.Select(r => r.Id).ToList().ToArray();

            var newRoleList = ResourceService.FillUsersIntoRoles(idsin);

            return newRoleList;
        }

        /// <summary>
        /// Get user all
        /// 获取所有用户数据
        /// </summary>
        public IList<User> GetUserAll()
        {
            return ResourceService.GetUserAll();
        }

        /// <summary>
        /// Get user list by role
        /// 根据角色获取用户列表
        /// </summary>
        public IList<User> GetUserListByRole(string roleId)
        {
            return ResourceService.GetUserListByRole(roleId);
        }

        /// <summary>
        /// Get performer list
        /// 获取节点上的执行者列表
        /// </summary>
        public PerformerList GetPerformerList(NodeView nextNode)
        {
            var performerList = PerformerBuilder.CreatePerformerList(nextNode.Roles);
            return performerList;
        }
        
        #endregion

        #region Form Resource Data
        
        /// <summary>
        /// Get form all
        /// 获取所有表单数据
        /// </summary>
        public IList<Form> GetFormAll()
        {
            return FormService.GetFormAll();
        }

        /// <summary>
        /// Get form list
        /// 获取流程绑定的表单信息
        /// </summary>
        public IList<Form> GetFormList(string processId, string version)
        {
            var processModel = ProcessModelFactory.CreateByProcess(processId, version);
            var list = processModel.GetFormList();
            return list;
        }

        /// <summary>
        /// Get Field Edit Permission List By Process
        /// 根据流程查询表单及字段编辑权限
        /// </summary>
        public IList<Form> GetFormFieldEditComp(FieldQuery query)
        {
            var processModel = ProcessModelFactory.CreateByProcess(query.ProcessId, query.ProcessVersion);
            var formList = processModel.GetFormList();

            foreach (var form in formList)
            {
                var filedEditList = FormService.GetFieldActivityEditByForm(form.FormId, query);
                form.FieldActivityEditList = filedEditList;
            }
            return formList;
        }
        
        #endregion
    }
}
