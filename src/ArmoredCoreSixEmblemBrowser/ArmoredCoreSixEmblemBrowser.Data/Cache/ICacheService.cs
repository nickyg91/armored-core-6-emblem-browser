using StackExchange.Redis;

namespace ArmoredCoreSixEmblemBrowser.Data.Cache;

public interface ICacheService
{
    ConnectionMultiplexer Connect(byte numberOfRetries);
    IDatabase Database { get; }
    Task WriteImage(string key, byte[] imageData, TimeSpan? ttl = null);
    Task<byte[]?> GetImage(string key);
    Task<List<string>> GetTagsForEmblem(string key);
    Task SetTagsForEmblem(string key, List<string> tags);
    Task AddTagsToSet(List<string> tags);
    Task<List<string>> GetAllTags();
    Task<List<int>> GetFilteredEmblemsByTags(List<string> tags);
}