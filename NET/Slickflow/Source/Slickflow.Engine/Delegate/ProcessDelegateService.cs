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
        public ProcessDelegateService(IDbSession session, int id) : base(session, id)
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
            var entity = pim.GetById(id);
            return entity as T;
        }
        #endregion
    }
}
