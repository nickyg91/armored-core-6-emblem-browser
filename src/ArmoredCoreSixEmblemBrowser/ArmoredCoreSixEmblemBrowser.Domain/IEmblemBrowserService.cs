using ArmoredCoreSixEmblemBrowser.Domain.Objects;
using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Domain;

public interface IEmblemBrowserService
{
    public Task<EmblemSearchResult> GetEmblems(int pageNumber, int totalPerPage);
    public int ReportEmblem(int id);
    public Task<bool> DeleteEmblem(int id);
    public Task<Emblem> AddEmblem(Emblem emblem);
}