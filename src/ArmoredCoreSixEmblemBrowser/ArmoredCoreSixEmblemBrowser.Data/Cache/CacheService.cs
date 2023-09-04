using System.Collections;
using System.Text;
using System.Text.Json;
using ArmoredCoreSixEmblemBrowser.Core.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace ArmoredCoreSixEmblemBrowser.Data.Cache;

public class CacheService : ICacheService
{
    private readonly Lazy<ConnectionMultiplexer> _redisConnection;
    private readonly string _connectionString;
    private readonly byte _maxRetries;
    public IDatabase Database => _redisConnection.Value.GetDatabase();

    public CacheService(IOptions<AppConfigSettings> settings)
    {
        _connectionString = settings.Value.RedisSettingsOptions.ConnectionString;
        _maxRetries = settings.Value.RedisSettingsOptions.MaxRetries;
        _redisConnection = new Lazy<ConnectionMultiplexer>(() => Connect(0));
    }

    public ConnectionMultiplexer Connect(byte numberOfRetries = 0)
    {
        ConnectionMultiplexer muxer;
        try
        {
            muxer = ConnectionMultiplexer.Connect(_connectionString);
        }
        catch (RedisConnectionException e)
        {
            Console.WriteLine(e);
            if (numberOfRetries < _maxRetries)
            {
                return Connect(numberOfRetries++);
            }
            throw;
        }
        return muxer;
    }
    
    public async Task WriteImage(string key, Stream imageData, TimeSpan ttl)
    {
        imageData.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(imageData);
        var bytes = Encoding.UTF8.GetBytes(await reader.ReadToEndAsync());
        await Database.StringSetAsync(key, bytes, ttl);
    }

    public async Task<Stream?> GetImage(string key)
    {
        byte[]? bytes = await Database.StringGetAsync(key);
        if (bytes == null || bytes.Length == 0)
        {
            return null;
        }
        var stream = new MemoryStream();
        await stream.WriteAsync(bytes);
        if (stream.Position != 0 && stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);    
        }
        
        return stream;
    }
    
}