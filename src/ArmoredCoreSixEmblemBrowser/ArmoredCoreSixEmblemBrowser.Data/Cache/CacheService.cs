using ArmoredCoreSixEmblemBrowser.Core.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

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
    
    public async Task WriteImage(string key, byte[] imageData, TimeSpan? ttl = null)
    {
        await Database.StringSetAsync(key, imageData, ttl);
    }

    public async Task<(byte[]?, TimeSpan?)> GetImage(string key)
    {
        byte[]? bytes = await Database.StringGetAsync(key);
        TimeSpan? remainingTtl = await Database.KeyTimeToLiveAsync(key);
        if (remainingTtl == null && bytes != null)
        {
            await Database.StringSetAsync(key, bytes, TimeSpan.FromDays(7));
        }
        if (bytes == null || bytes.Length == 0)
        {
            return (null, null);
        }
        return (bytes, remainingTtl);
    }

    public async Task<List<string>> GetTagsForEmblem(string key)
    {
        var tags = await Database.SetMembersAsync($"{key}:tags");
        return tags.Select(x => x.ToString()).ToList();
    }

    public async Task SetTagsForEmblem(string key, List<string> tags)
    {
        var tasks = tags.Select(tag => Database.SetAddAsync("tags", tag.ToLower())).Cast<Task>().ToList();
        tasks.AddRange(tags.Select(tag => Database.SetAddAsync(tag.ToLower(), key)).Cast<Task>().ToList());
        tasks.AddRange(tags.Select(tag => Database.SetAddAsync($"{key}:tags", tag)).Cast<Task>().ToList());
        await Task.WhenAll(tasks);
    }

    public async Task AddTagsToSet(List<string> tags)
    {
        var tasks = tags.Select(tag => Database.SetAddAsync("tags", tag.ToLower())).Cast<Task>().ToList();
        await Task.WhenAll(tasks);
    }

    public async Task<List<string>> GetAllTags()
    {
        var tags = await Database.SetMembersAsync("tags");
        return tags.Select(x => x.ToString()).ToList();
    }

    public async Task<List<int>> GetFilteredEmblemsByTags(List<string> tags)
    {
        var tasks = tags.Select(tag => Database.SetMembersAsync(tag)).ToList();
        var result = (await Task.WhenAll(tasks)).ToList();
        if (!result.Any())
        {
            return new List<int>();
        }
        var ids = result
            .SelectMany(x => x)
            .Select(x => (int)x)
            .Distinct()
            .ToList();
        return ids;
    }
}