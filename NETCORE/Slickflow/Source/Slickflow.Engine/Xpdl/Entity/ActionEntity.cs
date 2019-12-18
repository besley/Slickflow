using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Slickflow.Engine.Xpdl.Entity
{
    /// <summary>
    /// 操作实体
    /// </summary>
    public class ActionEntity
    {
        #region 属性列表
        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionTypeEnum ActionType { get; set; }

        /// <summary>
        /// 事件触发类型
        /// </summary>
        public FireTypeEnum FireType { get; set; }

        /// <summary>
        /// 方法类型
        /// </summary>
        public ActionMethodEnum ActionMethod { get; set; }

        /// <summary>
        /// 子方法类型
        /// </summary>
        public SubMethodEnum SubMethod { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }
        #endregion
    }

    /// <summary>
    /// 操作基本类型
    /// </summary>
    public enum ActionTypeEnum
    {
        /// <summary>
        /// 空类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 事件
        /// </summary>
        Event = 1
    }

    /// <summary>
    /// 操作方法类型
    /// </summary>
    public enum ActionMethodEnum
    {
        /// <summary>
        /// 空类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 本地程序
        /// </summary>
        LocalMethod = 1,

        /// <summary>
        /// 外部插件方法
        /// </summary>
        WebApi = 3,

        /// <summary>
        /// SQL 语句
        /// </summary>
        SQL = 5,

        /// <summary>
        /// 存储过程
        /// </summary>
        StoreProcedure = 7,

        /// <summary>
        /// 执行脚本
        /// </summary>
        Script = 9,

        /// <summary>
        /// Python 脚本
        /// </summary>
        Python = 11,

        /// <summary>
        /// WebAPI 服务
        /// </summary>
        PlugIn = 13
    }

    /// <summary>
    /// 子方法類型
    /// </summary>
    public enum SubMethodEnum
    {
        /// <summary>
        /// 空白
        /// </summary>
        None = 0,

        /// <summary>
        /// Get方法
        /// </summary>
        HttpGet = 1,

        /// <summary>
        /// Post方法
        /// </summary>
        HttpPost = 2,

        /// <summary>
        /// Put方法
        /// </summary>
        HttpPut = 3,

        /// <summary>
        /// Delete方法
        /// </summary>
        HttpDelete = 4
    }

    /// <summary>
    /// 事件发生类型
    /// </summary>
    public enum FireTypeEnum
    {
        /// <summary>
        /// 空类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 执行前
        /// </summary>
        Before = 1,

        /// <summary>
        /// 执行后
        /// </summary>
        After = 2
    }        


}
