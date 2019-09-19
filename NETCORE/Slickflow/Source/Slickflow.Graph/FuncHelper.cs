using System;

namespace Slickflow.Graph
{
    /// <summary>
    /// 函数执行封装方法
    /// </summary>
    public class FuncHelper
    {
        /// <summary>
        /// 数组函数执行
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="funcToRun">数组</param>
        /// <returns>结果</returns>
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
        /// 单个函数执行
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="funcToRun">要执行的函数</param>
        /// <returns>结果</returns>
        public static T Runner<T>(Func<T> funcToRun)
        {
            T t = funcToRun();
            return t;
        }
    }
}
