using StackExchange.Redis;

namespace ArmoredCoreSixEmblemBrowser.Data.Cache;

public interface ICacheService
{
    ConnectionMultiplexer Connect(byte numberOfRetries);
    IDatabase Database { get; }
    Task WriteImage(string key, Stream imageData, TimeSpan ttl);
    Task<Stream?> GetImage(string key);
}