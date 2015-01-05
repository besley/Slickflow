using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Service
{
    public interface IXPDLReader
    {
        bool IsReadable();
        XmlDocument Read(ProcessEntity entity);
    }
}
