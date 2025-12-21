using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Form data service
    /// 表单数据服务
    /// </summary>
    public class FormService : IFormService
    {
        /// <summary>
        /// Read from list
        /// 获取表单列表
        /// </summary>
        /// <returns></returns>
        public IList<Form> GetFormAll()
        {
            var formManager = new FormManager();
            var itemList = formManager.GetAll();
            IList<Form> formList = new List<Form>();
            foreach (var item in itemList)
            {
                var form = new Form
                {
                    FormId = item.Id.ToString(),
                    FormCode = item.FormCode,
                    FormName = item.FormName
                };
                formList.Add(form);
            }
            return formList;
        }


        /// <summary>
        /// Get field activity edit info by formid
        /// 根据表单获取字段权限列表
        /// </summary>
        public IList<FieldActivityEdit> GetFieldActivityEditByForm(string formId, FieldQuery query)
        {
            var fieldList = new List<FieldActivityEdit>();
            //get field summary list 
            var formManager = new FormManager();
            var fieldManager = new FieldManager();

            var summartyFieldList = formManager.GetFieldSummaryList(int.Parse(formId)).ToList();
            var fieldActivityEditItem = fieldManager.GetFieldActivityEditListByForm(formId, 
                query.ProcessId, query.ProcessVersion, query.ActivityId);

            if (fieldActivityEditItem != null)
            {
                //Fill field permission list
                FillFieledPermission(summartyFieldList, fieldActivityEditItem);
            }

            //转换为Model的标准数据格式
            //Convert to the standard data format of the Model
            foreach (var item in summartyFieldList)
            {
                var fieldActivityEdit = new FieldActivityEdit
                {
                    FieldName = item.FieldName,
                    IsNotVisible = item.IsNotVisible,
                    IsReadOnly = item.IsReadOnly
                };
                fieldList.Add(fieldActivityEdit);
            }
            return fieldList;
        }

        /// <summary>
        /// Fill field permission data
        /// 填充字段权限数据
        /// </summary>
        /// <param name="summartyFieldList"></param>
        /// <param name="fieldActivityEditItem"></param>
        private void FillFieledPermission(List<FieldEntity> summartyFieldList, FieldActivityEditEntity fieldActivityEditItem)
        {
            var fieldPermission = fieldActivityEditItem.FieldsPermission;
            if (!string.IsNullOrEmpty(fieldPermission))
            {
                var fieldPermissionList = JsonSerializer.Deserialize<List<FieldEntity>>(fieldPermission);
                if (fieldPermissionList != null)
                {
                    foreach (var field in summartyFieldList)
                    {
                        var fieldName = field.FieldName;
                        var permissionItem = fieldPermissionList.Single<FieldEntity>(f => f.FieldName == fieldName);
                        if (permissionItem != null)
                        {
                            field.IsNotVisible = permissionItem.IsNotVisible;
                            field.IsReadOnly = permissionItem.IsReadOnly;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Save process node field editing permission data
        /// 保存流程节点字段编辑权限数据
        /// </summary>
        /// <param name="form"></param>
        public void SaveFieldActivityEditInfo(FieldActivityEditInfo info)
        {
            var fm = new FieldManager();
            var itemDB = fm.GetFieldActivityEditListByForm(info.FormId.ToString(), info.ProcessId, info.ProcessVersion, info.ActivityId);
            if (itemDB != null)
            {
                itemDB.FieldsPermission = JsonSerializer.Serialize(info.FieldActivityEditList);
                fm.UpdateFormFieldEdit(itemDB);
            }
            else
            {
                var formManager = new FormManager();
                var formDB = formManager.GetById(info.FormId);

                var entity = new FieldActivityEditEntity();
                entity.FormId = info.FormId;
                entity.ProcessDefId = info.ProcessDefId;
                entity.ProcessId = info.ProcessId; 
                entity.ProcessName = info.ProcessName;
                entity.ProcessVersion = info.ProcessVersion;
                entity.ActivityId = info.ActivityId;
                entity.ActivityName = info.ActivityName;
                entity.FormName = info.FormName;
                entity.FormVersion = formDB.Version;
                entity.FieldsPermission = JsonSerializer.Serialize(info.FieldActivityEditList);
                fm.InsertFormFieldEdit(entity);
            }
        }
    }
}
