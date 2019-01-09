/*
* Slickflow 开源项目遵循LGPL协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
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
using System.Threading.Tasks;
using Slickflow.Engine.Common;
using Slickflow.Data;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// 委托服务基类
    /// </summary>
    public abstract class DelegateServiceBase
    {
        #region 属性、抽象方法及构造
        private ActivityResource _activityResource = null;
        public int ID { get; set; }
        public IDbSession Session { get; set; }
        public abstract T GetInstance<T>(int id) where T : class;

        public DelegateServiceBase(IDbSession session, int id)
        {
            ID = id;
            Session = session;
        }
        #endregion

        /// <summary>
        /// 读取实例主键ID
        /// </summary>
        /// <returns>主键ID</returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// 设置活动资源
        /// </summary>
        /// <param name="activityResource">资源</param>
        internal void SetActivityResource(ActivityResource activityResource)
        {
            _activityResource = activityResource;
        }
    }
}
