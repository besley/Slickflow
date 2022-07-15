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
using System.Text;
using System.Data;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// 流程委托服务类
    /// </summary>
    public class ProcessDelegateService : DelegateServiceBase, IDelegateService
    {
        #region 属性及构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="context">上下文</param>
        public ProcessDelegateService(IDbSession session, DelegateContext context) 
            : base(session, context)
        {
            
        }
        #endregion

        #region 获取及设置方法
        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体主键ID</param>
        /// <returns>实体</returns>
        public override T GetInstance<T>(int id)
        {
            var pim = new ProcessInstanceManager();
            var entity = pim.GetById(Session.Connection, id, Session.Transaction);
            return entity as T;
        }
        #endregion
    }
}
