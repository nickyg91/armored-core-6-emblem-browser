using ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.Repositories;
using ArmoredCoreSixEmblemBrowser.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser;

public class EmblemUnitOfWork : IUnitOfWork<Emblem>
{
    private readonly EmblemBrowserContext _context;
    private readonly IEmblemRepository _emblemRepository;

    public EmblemUnitOfWork(
        EmblemBrowserContext context,
        IEmblemRepository emblemRepository
        )
    {
        _context = context;
        _emblemRepository = emblemRepository;
    }

    public IEmblemRepository EmblemRepository => _emblemRepository;
    
    public async Task RollbackTransaction(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }

    public async Task<IDbContextTransaction> BeginTransaction()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task CommitTransaction(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }
}