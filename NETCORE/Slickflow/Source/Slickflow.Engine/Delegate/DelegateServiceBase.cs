/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
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
