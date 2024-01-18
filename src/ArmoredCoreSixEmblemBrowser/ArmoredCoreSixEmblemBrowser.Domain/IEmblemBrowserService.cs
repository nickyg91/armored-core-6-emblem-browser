using ArmoredCoreSixEmblemBrowser.Domain.Objects;
using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Domain;

public interface IEmblemBrowserService
{
    public Task<EmblemSearchResult> GetEmblems(int pageNumber, int totalPerPage);

    public Task<EmblemSearchResult> GetFilteredEmblems(int pageNumber, int totalPerPage, string nameOrShareId,
        List<PlatformType> platforms, List<string> tags);
    public int ReportEmblem(int id);
    public Task<bool> DeleteEmblem(int id);
    public Task<Emblem> AddEmblem(Emblem emblem);
    public Task<(byte[] ImageData, string Extension, DateTimeOffset CacheTtl)> GetEmblemImage(int id);
    Task<List<string>> GetAllTags();
    Task<Emblem> GetById(int id);
}