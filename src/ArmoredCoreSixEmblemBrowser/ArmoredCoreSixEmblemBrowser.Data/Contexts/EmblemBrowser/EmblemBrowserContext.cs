using ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.EntityConfigurations;
using ArmoredCoreSixEmblemBrowser.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser;

public class EmblemBrowserContext : DbContext
{
    public EmblemBrowserContext(DbContextOptions options) : base(options)
    {
    }
 
    public DbSet<Emblem> Emblems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder) 
    {
        builder.HasDefaultSchema("ac6");
        var emblemConfig = new EmblemEntityConfiguration();
        emblemConfig.Configure(builder.Entity<Emblem>());
    }
}