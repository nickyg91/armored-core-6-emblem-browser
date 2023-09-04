using ArmoredCoreSixEmblemBrowser.Data.Cache;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

namespace ArmoredCoreSixEmblemBrowser.Domain.Services;

public class EmblemBlobStorageService : IEmblemBlobStorageService
{
    private readonly ICacheService _cache;
    private readonly BlobContainerClient _blobContainerClient;

    public EmblemBlobStorageService(BlobServiceClient blobServiceClient, ICacheService cache)
    {
        _cache = cache;
        _blobContainerClient = blobServiceClient.GetBlobContainerClient("ac6-emblems");
    }
    
    public async Task<string> UploadBlob(Guid id, string name, byte[] file)
    {
        var stream = new MemoryStream();
        await stream.WriteAsync(file);
        stream.Seek(0, SeekOrigin.Begin);
        var blobIdentifier = $"ac6-emblem-{id}-{name}";
        var blockBlobClient = _blobContainerClient.GetBlockBlobClient(blobIdentifier);
        await blockBlobClient.UploadAsync(stream);
        await _cache.WriteImage(blobIdentifier, stream, TimeSpan.FromDays(7));
        return blobIdentifier;
    }

    public async Task<Stream> DownloadBlob(string identifier)
    {
        var imageFromCache = await _cache.GetImage(identifier);
        if (imageFromCache != null)
        {
            return imageFromCache;
        }
        var blobClient = _blobContainerClient.GetBlobClient(identifier);
        var downloadedBlob = await blobClient.DownloadAsync();
        var stream = new MemoryStream();
        await downloadedBlob.Value.Content.CopyToAsync(stream);
        await _cache.WriteImage(identifier, stream, TimeSpan.FromDays(7));
        
        return stream;
    }
}