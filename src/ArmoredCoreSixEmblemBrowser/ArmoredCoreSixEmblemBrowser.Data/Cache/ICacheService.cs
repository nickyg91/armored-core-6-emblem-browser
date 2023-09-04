using StackExchange.Redis;

namespace ArmoredCoreSixEmblemBrowser.Data.Cache;

public interface ICacheService
{
    ConnectionMultiplexer Connect(byte numberOfRetries);
    IDatabase Database { get; }
    Task WriteImage(string key, byte[] imageData, TimeSpan ttl);
    Task<byte[]?> GetImage(string key);
}