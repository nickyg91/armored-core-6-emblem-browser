namespace ArmoredCoreSixEmblemBrowser.Core.Configuration;

public class RedisSettings
{
    public string ConnectionString { get; set; }
    public byte MaxRetries { get; set; }
}