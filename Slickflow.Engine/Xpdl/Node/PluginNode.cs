using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Xpdl.Node
{
    internal class PluginNode : NodeBase, IDynamicRunable
    {
        internal PluginNode(ActivityEntity activity)
            : base(activity)
        {
        }

        #region IDynamicRunable Members
        /// <summary>
        /// 执行插件方法
        /// </summary>
        /// <param name="implementation"></param>
        /// <param name="userParameters"></param>
        /// <returns></returns>
        public object InvokeMethod(TaskImplementDetail implementation, object[] userParameters)
        {
            object result = null;
            try
            {
                Assembly assembly = Assembly.Load(implementation.Assembly);
                Type myInterfaceType = assembly.GetType(implementation.Interface);
                Type myClassType = null;
                foreach (Type t in assembly.GetTypes())
                {
                    //遍历实现接口的类类型
                    if (myInterfaceType.IsAssignableFrom(t) && t.IsClass)
                    {
                        myClassType = t;
                        break;
                    }
                }

                if (myClassType != null)
                {
                    //创建类实例，并运行所调用的方法
                    MethodInfo methodInfo = myClassType.GetMethod(implementation.Method);
                    object instance = Activator.CreateInstance(myClassType);

                    if (methodInfo != null)
                    {
                        result = methodInfo.Invoke(instance, userParameters);
                    }
                }
                else
                {
                    string errorMessage = string.Format("调用外部插件或脚本发生错误，Assembly名称:{0}, Interface名称:{1},详细信息：{2}",
                        assembly.FullName,
                        implementation.Interface,
                        "未实现接口定义的方法，不正确的Assembly!");
                    throw new DLRuntimeInteroperationException(errorMessage);
                }
                return result;
            }
            catch (System.Exception ex)
            {
                string errorMessage = string.Format("调用外部插件或脚本发生错误，Assembly名称:{0}, Interface名称:{1},详细信息：{2}",
                    implementation.Assembly,
                    implementation.Interface,
                    ex.Message);
                throw new DLRuntimeInteroperationException(errorMessage);
            }
        }

        #endregion
    }
}
