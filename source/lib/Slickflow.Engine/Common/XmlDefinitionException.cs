using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// XML 文件定义异常类
    /// </summary>
    public class XmlDefinitionException : System.ApplicationException
    {
         public XmlDefinitionException(string message)
            : base(message)
        {

        }

         public XmlDefinitionException(string message, System.Exception ex)
            : base(message, ex)
        {

        }
    }
}
