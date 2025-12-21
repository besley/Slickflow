namespace Slickflow.Engine.Business.Entity
{
    /// <summary>
    /// Task Rplaced Entity
    /// 任务取代实体
    /// </summary>
    public class TaskReplacedEntity
    {
        /// <summary>
        /// The task replaced with a new user
        /// 任务被取代替换为新用户
        /// </summary>
        public string ReplacedByUserId { get; set; }
        public string ReplacedByUserName { get; set; }
    }
}
