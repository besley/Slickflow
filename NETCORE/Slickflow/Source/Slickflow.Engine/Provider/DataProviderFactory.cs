using Slickflow.Data;

namespace Slickflow.Engine.Provider
{
    /// <summary>
    ///  数据提供类创建
    /// </summary>
    internal class DataProviderFactory
    {
        internal static IWfDataProvider _wfDataProvider = null;
        internal static IWfDataProvider WfDataProvider { get { return _wfDataProvider; } }

        /// <summary>
        /// 获取不同类型数据库的提供类
        /// 数据库：Oracle、MySQL、KingBase等
        /// </summary>
        /// <returns>数据提供类</returns>
        internal static IWfDataProvider GetProvider()
        {
            if (DBTypeExtenstions.GetDbType() == DBTypeEnum.SQLSERVER)
            {
                return null;
            }
            else if (DBTypeExtenstions.GetDbType() == DBTypeEnum.ORACLE)
            {
                ;// _wfDataProvider = new OracleWfDataProvider();
            }
            else if (DBTypeExtenstions.GetDbType() == DBTypeEnum.MYSQL)
            {
                ;//_wfDatatProvider = new MySqlWfDataProvider();
            }
            else
            {
                throw new System.Exception("请指定数据库提供者类型！");
            }

            return _wfDataProvider;
        }
    }
}
