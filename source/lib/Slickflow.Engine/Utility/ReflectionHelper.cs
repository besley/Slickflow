using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Slickflow.Module.Localize;
using Slickflow.Engine.Config;
using System.IO;

namespace Slickflow.Engine.Utility
{
    /// <summary>
    /// Reflection Helper
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Get Special Instance
        /// </summary>
        public static T GetSpecialInstance<T>(string fullName) where T : class
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var serviceFile = Path.Combine(directory, WfConfig.EXTERNAL_SERVICE_FILE_PATH);
            var assembly = Assembly.LoadFrom(serviceFile);

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
        /// Get Property
        /// </summary>
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