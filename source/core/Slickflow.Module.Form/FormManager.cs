using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Slickflow.Data;

namespace Slickflow.Module.Form
{
    /// <summary>
    /// Form Manager
    /// 表单管理类
    /// </summary>
    internal class FormManager : ManagerBase
    {
        /// <summary>
        /// Get all forms
        /// 获取所有表单数据
        /// </summary>
        /// <returns></returns>
        internal List<FormEntity> GetAll()
        {
            var strSQL = @"SELECT 
                                F.*
                           FROM FbForm F
                           ORDER BY F.FormName";
            List<FormEntity> list = Repository.Query<FormEntity>(strSQL, null).ToList();
            return list;
        }

        /// <summary>
        /// Get form by id
        /// 根据表单ID获取表单实体
        /// </summary>
        internal FormEntity GetById(int formId)
        {
            var entity = Repository.GetById<FormEntity>(formId);
            return entity;
        }

        /// <summary>
        /// Get field summary list by formid
        /// 获取字段摘要列表
        /// </summary>
        internal IList<FieldEntity> GetFieldSummaryList(int formId)
        {
            var list = new List<FieldEntity>();
            var entity = GetById(formId);
            var fieldSummary = entity.FieldSummary;

            if (!string.IsNullOrEmpty(fieldSummary))
            {
                var fieldSummaryList = JArray.Parse(fieldSummary).Values<string>().ToList();

                foreach (var field in fieldSummaryList)
                {
                    if (field != null)
                    {
                        var item = new FieldEntity
                        {
                            FieldName = field
                        };
                        list.Add(item);
                    }
                }
            }
            return list;
        }
    }
}