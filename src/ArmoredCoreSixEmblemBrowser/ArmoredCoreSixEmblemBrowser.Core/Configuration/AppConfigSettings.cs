namespace ArmoredCoreSixEmblemBrowser.Core.Configuration;

public class AppConfigSettings : IEnvironmentSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public RedisSettings RedisSettingsOptions { get; set; }
    public string BlobStorageConnectionString { get; set; }
}