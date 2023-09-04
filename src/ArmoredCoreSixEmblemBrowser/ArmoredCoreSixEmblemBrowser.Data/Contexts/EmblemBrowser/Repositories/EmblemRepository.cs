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
        if (!string.IsNullOrEmpty(nameOrShareId) && platforms.Count == 0)
        {
            var emblemsByName = await _context.Emblems
                .Where(
                    x => x.Name.ToLower().Contains(nameOrShareId.ToLower()) 
                         || x.ShareId.ToLower().Contains(nameOrShareId.ToLower())).ToListAsync();
            return (emblemsByName, emblemsByName.Count);
        }

        if (string.IsNullOrEmpty(nameOrShareId) && platforms.Count > 0)
        {
            var emblemsByPlatform = await _context.Emblems
                .Where(x => platforms.Contains(x.Platform))
                .Skip(totalPerPage * pageNumber)
                .Take(totalPerPage)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync();
            return (emblemsByPlatform, emblemsByPlatform.Count);
        }

        var emblemsByPlatformAndNameOrShareId = await _context.Emblems
            .Where(x => platforms.Contains(x.Platform) &&
                        x.Name.ToLower().Contains(nameOrShareId.ToLower()) 
                        || x.ShareId.ToLower().Contains(nameOrShareId.ToLower()))
            .Distinct()
            .Skip(totalPerPage * pageNumber)
            .Take(totalPerPage)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
        
        return (emblemsByPlatformAndNameOrShareId, emblemsByPlatformAndNameOrShareId.Count);
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

    public async Task<(string? ImageIdentifier, string ImageExtension)> GetEmblemImageIdentifier(int id)
    {
        var resp = await _context.Emblems.Where(x => x.Id == id).Select(x => new { x.ImageUrl, x.ImageExtension })
            .SingleAsync();
        return (resp.ImageUrl, resp.ImageExtension);
    }
}