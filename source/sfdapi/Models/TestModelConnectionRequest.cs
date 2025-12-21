namespace sfdapi.Models
{
    /// <summary>
    /// Test model connection request
    /// 测试模型连接请求
    /// </summary>
    public class TestModelConnectionRequest
    {
        public string BaseUrl { get; set; }
        public string ApiUUID { get; set; }
        public string ApiKey { get; set; }
        public string ModelProvider { get; set; }
    }
}
