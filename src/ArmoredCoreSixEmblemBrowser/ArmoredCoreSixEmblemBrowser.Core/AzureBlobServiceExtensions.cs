using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;

namespace ArmoredCoreSixEmblemBrowser.Core;

public static class AzureBlobServiceExtensions
{
    public static void AddBlobService(
        this IServiceCollection services, string connectionString)
    {
        services.AddScoped<BlobServiceClient>(_ => new BlobServiceClient(connectionString));
    }

}