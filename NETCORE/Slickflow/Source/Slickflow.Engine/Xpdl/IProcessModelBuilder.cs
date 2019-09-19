using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 流程图服务接口，后端创建流程图
    /// </summary>
    public interface IProcessModelBuilder
    {
        IProcessModelBuilder CreateProcess(string processGUID, string version);
        IProcessModelBuilder Start(string name, string code);
        IProcessModelBuilder End(string name, string code);
        IProcessModelBuilder Task(string name, string code);
        IProcessModelBuilder Split(string name, string code);
        IProcessModelBuilder Join(string name, string code);
        
        ProcessEntity Sequence();
    }
}
