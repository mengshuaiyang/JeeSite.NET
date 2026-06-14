// 定义 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.Services
namespace JeeSiteNET.Modules.Cms.Application.Services;

// 定义接口 IArticleVectorStore
// 定义接口：IArticleVectorStore
public interface IArticleVectorStore
{
    Task StoreVectorAsync(string articleCode, ReadOnlyMemory<float> vector);
    Task<ReadOnlyMemory<float>?> GetVectorAsync(string articleCode);
}

// 定义class DefaultArticleVectorStore
// 定义类：DefaultArticleVectorStore
public class DefaultArticleVectorStore : IArticleVectorStore
{
    // 方法：StoreVectorAsync
    public Task StoreVectorAsync(string articleCode, ReadOnlyMemory<float> vector) => Task.CompletedTask;
    // 方法：GetVectorAsync
    public Task<ReadOnlyMemory<float>?> GetVectorAsync(string articleCode) => Task.FromResult<ReadOnlyMemory<float>?>(null);
}
