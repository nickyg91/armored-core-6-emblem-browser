using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace ArmoredCoreSixEmblemBrowser.Core;

public static class AppConfigServiceExtensions
{
    public static void AddAzureAppConfig(this ConfigurationManager configuration, bool isDev)
    {
        var env = isDev ? "Dev" : "Prod";
        // DO NOT SHOW VALUES ON STREAM.
        var managedIdentityClientId = "";
        var appConfigConnectionString = "";
        var tenantId = "";

        if (isDev)
        {
            appConfigConnectionString = configuration["ConnectionStrings:AppConfig"];
            tenantId = configuration["Hyperion:ManagedIdentity:TenantId"];
            managedIdentityClientId = configuration["Hyperion:ManagedIdentity:ClientId"];
        }
        else
        {
            appConfigConnectionString = Environment.GetEnvironmentVariable("APPCONFIG_CONNECTIONSTRING");
            tenantId = Environment.GetEnvironmentVariable("AZURE_TENANTID");
            managedIdentityClientId = Environment.GetEnvironmentVariable("MANAGED_USER_CLIENTID");
        }

        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = managedIdentityClientId,
            TenantId = tenantId,
        });

        // AGAIN DONT SHOW THESE WHEN DEBUGGING!
        configuration.AddAzureAppConfiguration(options =>
        {
            options
                .Connect(appConfigConnectionString)
                .ConfigureKeyVault(kv => { kv.SetCredential(credential); })
                .Select(KeyFilter.Any)
                .Select(KeyFilter.Any, env);
        });
    }
}