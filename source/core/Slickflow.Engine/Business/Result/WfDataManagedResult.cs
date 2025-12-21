using Slickflow.Engine.Core.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Result
{
    public class WfDataManagedResult
    {
        public string Message { get; set; }
        public WfDataManagedStatusEnum Status { get; set; }

        public WfDataManagedResult()
        {
            Status = WfDataManagedStatusEnum.Default;
            Message = string.Empty;
        }

        /// <summary>
        /// Default
        /// 缺省方法
        /// </summary>
        /// <returns></returns>
        public static WfDataManagedResult Default()
        {
            return new WfDataManagedResult();
        }

        public static WfDataManagedResult Success()
        {
            var result = new WfDataManagedResult();
            result.Status = WfDataManagedStatusEnum.Success;
            return result;
        }

        public static WfDataManagedResult Failed(string errorMessage)
        {
            var result = new WfDataManagedResult();
            result.Status = WfDataManagedStatusEnum.Failed;
            result.Message = errorMessage;
            return result;
        }

        public static WfDataManagedResult Exception(string exceptionMessage)
        {
            var result = new WfDataManagedResult();
            result.Status = WfDataManagedStatusEnum.Exception;
            result.Message = exceptionMessage;
            return result;
        }
    }
}
