using System;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Function execution encapsulation method
    /// 函数执行封装方法
    /// </summary>
    public class FuncHelper
    {
        /// <summary>
        /// Array function execution
        /// 数组函数执行
        /// </summary>
        public static T Runner<T>(params Func<T>[] funcToRun)
        {
            Object obj = null;
            foreach (var func in funcToRun)
            {
                T t = func();
                obj = t;
            }
            return (T)obj;
        }

        /// <summary>
        /// Single function execution
        /// 单个函数执行
        /// </summary>
        public static T Runner<T>(Func<T> funcToRun)
        {
            T t = funcToRun();
            return t;
        }
    }
}
