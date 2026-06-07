namespace JeeSiteNET.Core.Storage;

public interface IFileStorageProvider
{
    string Name { get; }
    Task<string> SaveAsync(string fileId, Stream content, string extension);
    Task<Stream?> GetAsync(string filePath);
    Task DeleteAsync(string filePath);
    string GetUrl(string filePath);
}
