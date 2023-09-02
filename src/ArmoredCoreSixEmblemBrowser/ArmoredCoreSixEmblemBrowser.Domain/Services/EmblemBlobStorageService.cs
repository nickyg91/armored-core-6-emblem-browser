using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

namespace ArmoredCoreSixEmblemBrowser.Domain.Services;

public class EmblemBlobStorageService : IEmblemBlobStorageService
{
    private readonly BlobContainerClient _blobContainerClient;

    public EmblemBlobStorageService(BlobServiceClient blobServiceClient)
    {
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
        return blobIdentifier;
    }

    public async Task<Stream> DownloadBlob(string identifier)
    {
        var blobClient = _blobContainerClient.GetBlobClient(identifier);
        var downloadedBlob = await blobClient.DownloadAsync();
        return downloadedBlob.Value.Content;
    }
}