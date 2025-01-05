﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// XML File Definition Exception
    /// XML 文件定义异常类
    /// </summary>
    public class XmlDefinitionException : System.ApplicationException
    {
        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="message"></param>
        public XmlDefinitionException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public XmlDefinitionException(string message, System.Exception ex)
            : base(message, ex)
        {

        }
    }
}
