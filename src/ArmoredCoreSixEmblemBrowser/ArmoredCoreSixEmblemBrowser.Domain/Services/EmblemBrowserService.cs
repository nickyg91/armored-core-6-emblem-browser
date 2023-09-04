using ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser;
using ArmoredCoreSixEmblemBrowser.Domain.Objects;
using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Domain.Services;

public class EmblemBrowserService : IEmblemBrowserService
{
    private readonly EmblemUnitOfWork _emblemUnitOfWork;
    private readonly IEmblemBlobStorageService _emblemBlobStorageService;

    public EmblemBrowserService(EmblemUnitOfWork emblemUnitOfWork, IEmblemBlobStorageService emblemBlobStorageService)
    {
        _emblemUnitOfWork = emblemUnitOfWork;
        _emblemBlobStorageService = emblemBlobStorageService;
    }
    
    public async Task<EmblemSearchResult> GetEmblems(int pageNumber, int totalPerPage)
    {
        var emblems = await _emblemUnitOfWork.EmblemRepository.GetPaginatedEmblems(pageNumber - 1, totalPerPage);
        return new EmblemSearchResult(emblems.TotalEmblems, emblems.Emblems.ToList());
    }

    public async Task<EmblemSearchResult> GetFilteredEmblems(int pageNumber, int totalPerPage, string nameOrShareId, List<PlatformType> platforms)
    {
        var emblems =
            await _emblemUnitOfWork.EmblemRepository.SearchEmblems(nameOrShareId, platforms, pageNumber - 1, totalPerPage);
        return new EmblemSearchResult(emblems.TotalEmblems, emblems.Emblems.ToList());
    }

    public int ReportEmblem(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEmblem(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Emblem> AddEmblem(Emblem emblem)
    {
        var image = emblem.ImageData.Split(',');
        var fileType = image[0].Split('/')[1].Split(";")[0];
        var allowedTypes = new[] { "png", "jpg", "jpeg" };
        if (!allowedTypes.Contains(fileType))
        {
            throw new ArgumentException($"File type {fileType} not allowed.");
        }
        var base64ImageData = image[1];
        byte[] imageData = Convert.FromBase64String(base64ImageData);
        var blobIdentifier = await _emblemBlobStorageService.UploadBlob(Guid.NewGuid(), emblem.Name, imageData);
        emblem.ImageUrl = blobIdentifier;
        await _emblemUnitOfWork.EmblemRepository.AddEmblem(emblem);
        await _emblemUnitOfWork.SaveChanges();
        return emblem;
    }

    public async Task<(byte[] ImageData, string Extension)> GetEmblemImage(int id)
    {
        var emblem = await _emblemUnitOfWork.EmblemRepository.GetById(id);
        if (emblem == null)
        {
            throw new Exception("Emblem not found.");
        }

        var stream = await _emblemBlobStorageService.DownloadBlob(emblem.ImageUrl!);
        return (stream, emblem.ImageExtension);
    }
}