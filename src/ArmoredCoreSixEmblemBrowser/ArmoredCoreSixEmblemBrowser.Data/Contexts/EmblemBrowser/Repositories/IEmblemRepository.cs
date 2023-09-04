using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.Repositories;

public interface IEmblemRepository
{
    Task<(IEnumerable<Emblem> Emblems, int TotalEmblems)> GetPaginatedEmblems(int pageNumber, int totalPerPage);
    Task<(IEnumerable<Emblem> Emblems, int TotalEmblems)> SearchEmblems(string nameOrShareId, List<PlatformType> platforms, int pageNumber, int totalPerPage);
    Task AddEmblem(Emblem emblem);
    Task RemoveEmblem(int id);
    Task<Emblem?> GetById(int id);
    Task<(string? ImageIdentifier, string ImageExtension)> GetEmblemImageIdentifier(int id);
}