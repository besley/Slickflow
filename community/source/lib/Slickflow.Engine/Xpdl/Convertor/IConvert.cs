using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// 节点转变实体
    /// </summary>
    public interface IConvert
    {
        /// <summary>
        /// 转变方法
        /// </summary>
        /// <param name="entity">活动实体</param>
        /// <returns></returns>
       public Activity Convert();
    }
}
