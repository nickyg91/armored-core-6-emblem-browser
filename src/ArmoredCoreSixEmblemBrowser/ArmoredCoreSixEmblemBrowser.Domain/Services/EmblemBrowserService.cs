using ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser;
using ArmoredCoreSixEmblemBrowser.Domain.Objects;
using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Domain.Services;

public class EmblemBrowserService : IEmblemBrowserService
{
    private readonly EmblemUnitOfWork _emblemUnitOfWork;

    public EmblemBrowserService(EmblemUnitOfWork emblemUnitOfWork)
    {
        _emblemUnitOfWork = emblemUnitOfWork;
    }
    
    public async Task<EmblemSearchResult> GetEmblems(int pageNumber, int totalPerPage)
    {
        var emblems = await _emblemUnitOfWork.EmblemRepository.GetPaginatedEmblems(pageNumber - 1, totalPerPage);
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
        await _emblemUnitOfWork.EmblemRepository.AddEmblem(emblem);
        await _emblemUnitOfWork.SaveChanges();
        return emblem;
    }
}