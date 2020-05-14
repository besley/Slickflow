using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Slickflow.Module.Localize;
using Slickflow.Engine.Config;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取特别类型的实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="fullName">名称</param>
        /// <returns>实例</returns>
        public static T GetSpecialInstance<T>(string fullName) where T : class
        {
            var assembly = WfConfig.LoadExternalServiceFile();
            var list = assembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x)
                        && !x.IsInterface
                        && !x.IsAbstract
                        && x.FullName.ToLower() == fullName.Trim().ToLower()
                        ).ToList();

            if (list != null && list.Count() > 0)
            {
                var type = list[0];
                var instance = Activator.CreateInstance(type, true);

                return instance as T;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("reflectionhelper.GetSpecialInstance.error", fullName));
            }
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="lambda">表达式</param>
        /// <returns>成员信息</returns>
        public static MemberInfo GetProperty(LambdaExpression lambda)
        {
            Expression expression = lambda;
            for (; ; )
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Lambda:
                        expression = ((LambdaExpression)expression).Body;
                        break;
                    case ExpressionType.Convert:
                        expression = ((UnaryExpression)expression).Operand;
                        break;
                    case ExpressionType.MemberAccess:
                        MemberExpression memberExpression = (MemberExpression)expression;
                        MemberInfo mi = memberExpression.Member;
                        return mi;
                    default:
                        return null;
                }
            }
        }
    }
}