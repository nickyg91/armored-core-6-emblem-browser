namespace ArmoredCoreSixEmblemBrowser.Domain;

public interface IEmblemBlobStorageService
{
    Task<string> UploadBlob(Guid id, string name, byte[] file);
    Task<(byte[] Image, DateTimeOffset DateCreated)> DownloadBlob(string url);
}