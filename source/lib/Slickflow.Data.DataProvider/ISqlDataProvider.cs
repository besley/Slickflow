namespace Slickflow.Data.DataProvider
{
    /// <summary>
    /// 流程数据SQL接口
    /// Process data SQL interface
    /// </summary>
    public interface ISqlDataProvider
    {
        /// <summary>
        /// 获取任务分页数据
        /// Retrieve task pagination data
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        string GetSqlTaskPaged(string sql);

        /// <summary>
        /// 获取我的任务数据
        /// Get my task data
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        string GetSqlTaskOfMineByAtcitivityInstance(string sql);

        /// <summary>
        /// 获取我的任务数据
        /// Get my task data
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        string GetSqlTaskOfMineByAppInstance(string sql);
    }
}
