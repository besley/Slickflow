/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
*  
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

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
                LocalizeHelper.SetLang(ProjectTypeEnum.Designer, lang);
                LocalizeHelper.SetLang(ProjectTypeEnum.Engine, lang);
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