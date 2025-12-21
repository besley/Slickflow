using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Field Manager
    /// 字段管理类
    /// </summary>
    internal class FieldManager : ManagerBase
    {
        /// <summary>
        /// Get field activity edit by form id
        /// 跟进表单获取字段列表
        /// </summary>
        internal FieldActivityEditEntity GetFieldActivityEditListByForm(string formId, 
            string processId,
            string processVersion,
            string activityId)
        {
            var strSQL = @"SELECT 
                                F.*
                           FROM FbFormFieldActivityEdit F
                           WHERE ProcessId=@processId
                                AND ProcessVersion=@processVersion
                                AND ActivityId=@activityId
                                AND FormId=@formId";
            var list = Repository.Query<FieldActivityEditEntity>(strSQL, 
                new { 
                    processId = processId,
                    processVersion = processVersion,
                    activityId = activityId,
                    formId = int.Parse(formId)
                }).ToList();

            if (list != null && list.Count() == 1)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Insert process node field editing permission data
        /// 插入流程节点字段编辑权限数据
        /// </summary>
        /// <param name="entity"></param>
        internal void InsertFormFieldEdit(FieldActivityEditEntity entity)
        {
            Repository.Insert<FieldActivityEditEntity>(entity);
        }

        /// <summary>
        /// Update process node field editing permission data
        /// 更新流程节点字段编辑权限
        /// </summary>
        /// <param name="entity"></param>
        internal void UpdateFormFieldEdit(FieldActivityEditEntity entity)
        {
            Repository.Update<FieldActivityEditEntity>(entity);
        }
    }
}
