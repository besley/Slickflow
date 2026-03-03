using System;
using System.Collections.Generic;
using System.Text;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Graph.Roslyn
{
    /// <summary>
    /// Roslyn Executed Result
    /// 执行结果
    /// </summary>
    public class RoslynExecuteResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public ProcessEntity Process { get; set; }

        /// <summary>
        /// Default
        /// </summary>
        public static RoslynExecuteResult Default()
        {
            var result = new RoslynExecuteResult();
            result.Status = 0;
            return result;
        }

        /// <summary>
        /// Success
        /// </summary>
        public static RoslynExecuteResult Success(string message = null)
        {
            var result = new RoslynExecuteResult();
            result.Status = 1;
            result.Message = message;
            return result;
        }

        /// <summary>
        /// Error
        /// </summary>
        public static RoslynExecuteResult Error(string message = null)
        {
            var result = new RoslynExecuteResult();
            result.Status = -1;
            result.Message = message;
            return result;
        }
    }
}
