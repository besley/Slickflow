using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// AI Service Type
    /// </summary>
    public enum AIServiceTypeEnum
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Large Language Model
        /// </summary>
        LLM = 1,

        /// <summary>
        /// PlugIn: such as OCR, auto gen...
        /// </summary>
        PlugIn = 2,
    }
}
