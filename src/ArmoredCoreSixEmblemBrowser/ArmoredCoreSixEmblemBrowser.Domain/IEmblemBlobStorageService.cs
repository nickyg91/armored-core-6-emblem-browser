namespace ArmoredCoreSixEmblemBrowser.Domain;

public interface IEmblemBlobStorageService
{
    Task<string> UploadBlob(Guid id, string name, byte[] file);
    Task<byte[]> DownloadBlob(string url);
}