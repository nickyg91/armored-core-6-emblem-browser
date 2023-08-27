using Microsoft.EntityFrameworkCore.Storage;

namespace ArmoredCoreSixEmblemBrowser.Data.Contexts;

public interface IUnitOfWork<T> where T : class
{
    Task<IDbContextTransaction> BeginTransaction();
    Task RollbackTransaction(IDbContextTransaction transaction);
    Task<bool> SaveChanges();
    Task CommitTransaction(IDbContextTransaction transaction);
}