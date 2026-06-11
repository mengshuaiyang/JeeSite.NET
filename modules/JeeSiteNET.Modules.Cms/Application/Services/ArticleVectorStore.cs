namespace JeeSiteNET.Modules.Cms.Application.Services;

public interface IArticleVectorStore
{
    Task StoreVectorAsync(string articleCode, ReadOnlyMemory<float> vector);
    Task<ReadOnlyMemory<float>?> GetVectorAsync(string articleCode);
}

public class DefaultArticleVectorStore : IArticleVectorStore
{
    public Task StoreVectorAsync(string articleCode, ReadOnlyMemory<float> vector) => Task.CompletedTask;
    public Task<ReadOnlyMemory<float>?> GetVectorAsync(string articleCode) => Task.FromResult<ReadOnlyMemory<float>?>(null);
}
