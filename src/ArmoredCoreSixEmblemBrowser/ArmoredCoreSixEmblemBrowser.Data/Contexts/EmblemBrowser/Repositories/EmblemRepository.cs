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

    public IEnumerable<Emblem> SearchEmblems(string name, string shareId)
    {
        var emblems = new List<Emblem>();
        if (!string.IsNullOrEmpty(name))
        {
            var emblemsByName = _context.Emblems.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            emblems.AddRange(emblemsByName);
        }

        if (string.IsNullOrEmpty(shareId))
        {
            return emblems.DistinctBy(x => x.Id);
        }
        
        var emblemsByShareId = _context.Emblems.Where(x => x.ShareId.ToLower().Contains(shareId.ToLower()));
        emblems.AddRange(emblemsByShareId);

        return emblems.DistinctBy(x => x.Id);
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