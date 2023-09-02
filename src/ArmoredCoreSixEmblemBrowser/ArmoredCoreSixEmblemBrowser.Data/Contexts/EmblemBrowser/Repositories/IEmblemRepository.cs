using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.Repositories;

public interface IEmblemRepository
{
    Task<(IEnumerable<Emblem> Emblems, int TotalEmblems)> GetPaginatedEmblems(int pageNumber, int totalPerPage);
    IEnumerable<Emblem> SearchEmblems(string name, string shareId);
    Task AddEmblem(Emblem emblem);
    Task RemoveEmblem(int id);
    Task<Emblem?> GetById(int id);
}