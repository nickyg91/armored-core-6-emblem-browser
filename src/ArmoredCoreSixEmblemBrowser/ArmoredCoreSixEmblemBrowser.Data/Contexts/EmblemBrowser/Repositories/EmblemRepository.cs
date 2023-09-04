using ArmoredCoreSixEmblemBrowser.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.Repositories;

public class EmblemRepository : IEmblemRepository
{
    private readonly EmblemBrowserContext _context;

    public EmblemRepository(EmblemBrowserContext context)
    {
        _context = context;
    }
    
    public async Task<(IEnumerable<Emblem> Emblems, int TotalEmblems)> GetPaginatedEmblems(int pageNumber, int totalPerPage)
    {
        var totalEmblems = await _context.Emblems.CountAsync();

        var emblems = _context.Emblems
            .Skip(totalPerPage * pageNumber)
            .Take(totalPerPage)
            .OrderByDescending(x => x.CreatedAtUtc);

        return (emblems, totalEmblems);
    }

    public async Task<(IEnumerable<Emblem> Emblems, int TotalEmblems)> SearchEmblems(string nameOrShareId, List<PlatformType> platforms, int pageNumber, int totalPerPage)
    {
        var emblems = new List<Emblem>();
        if (!string.IsNullOrEmpty(nameOrShareId))
        {
            var emblemsByName = _context.Emblems
                .Where(
                    x => x.Name.ToLower().Contains(nameOrShareId.ToLower()) 
                         || x.ShareId.ToLower().Contains(nameOrShareId.ToLower()));
            emblems.AddRange(emblemsByName);
        }

        if (platforms.Count <= 0)
        {
            return (emblems.DistinctBy(x => x.Id).Skip(totalPerPage * pageNumber)
                .Take(totalPerPage)
                .OrderByDescending(x => x.CreatedAtUtc), emblems.Count);
        }

        var emblemsByPlatform = await _context.Emblems.Where(x => platforms.Contains(x.Platform)).ToListAsync();
        emblems.AddRange(emblemsByPlatform);

        return (emblems
            .DistinctBy(x => x.Id)
            .Skip(totalPerPage * pageNumber)
            .Take(totalPerPage)
            .OrderByDescending(x => x.CreatedAtUtc), emblems.Count);
    }

    public async Task AddEmblem(Emblem emblem)
    {
        await _context.Emblems.AddAsync(emblem);
    }

    public async Task RemoveEmblem(int id)
    {
        var emblem = await _context.Emblems.FindAsync(id);
        if (emblem == null)
        {
            return;
        }
        _context.Emblems.Remove(emblem);
    }

    public async Task<Emblem?> GetById(int id)
    {
        return await _context.Emblems.FindAsync(id);
    }
}