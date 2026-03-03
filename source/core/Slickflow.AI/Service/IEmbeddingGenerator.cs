namespace Slickflow.AI.Service
{
    /// <summary>
    /// 文本向量嵌入生成器接口
    /// </summary>
    public interface IEmbeddingGenerator
    {
        /// <summary>
        /// 生成文本的向量嵌入
        /// </summary>
        Task<float[]> GenerateEmbeddingAsync(string text);
    }
}
