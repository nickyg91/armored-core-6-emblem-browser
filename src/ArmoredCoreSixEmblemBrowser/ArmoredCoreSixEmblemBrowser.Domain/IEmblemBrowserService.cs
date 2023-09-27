using ArmoredCoreSixEmblemBrowser.Domain.Objects;
using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Domain;

public interface IEmblemBrowserService
{
    public Task<EmblemSearchResult> GetEmblems(int pageNumber, int totalPerPage);

    public Task<EmblemSearchResult> GetFilteredEmblems(int pageNumber, int totalPerPage, string nameOrShareId,
        List<PlatformType> platforms);
    public int ReportEmblem(int id);
    public Task<bool> DeleteEmblem(int id);
    public Task<Emblem> AddEmblem(Emblem emblem);
    public Task<(byte[] ImageData, string Extension)> GetEmblemImage(int id);
    Task<List<string>> GetAllTags();
}