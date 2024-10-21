using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Utility;

namespace Slickflow.Designer.Controllers.WebApi
{
    /// <summary>
    /// 流程定义XML操作控制器
    /// </summary>
    public class LanguageController : Controller
    {
        #region 语言设置
        /// <summary>
        /// 设置本地化语言
        /// </summary>
        /// <param name="id">语言类型</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult SetLang(string id)
        {
            var result = ResponseResult.Default();
            try
            {
                LangTypeEnum lang = EnumHelper.ParseEnum<LangTypeEnum>(id);
                LocalizeHelper.SetLang(lang);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("languagecontroller.setlang.error", ex.Message));
            }
            return result;
        }
        #endregion
    }
}