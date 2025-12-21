using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Convertor
{
    /// <summary>
    /// Node Convert Interface
    /// 节点转变实体
    /// </summary>
    public interface IConvert
    {
       public Activity Convert();
    }
}
