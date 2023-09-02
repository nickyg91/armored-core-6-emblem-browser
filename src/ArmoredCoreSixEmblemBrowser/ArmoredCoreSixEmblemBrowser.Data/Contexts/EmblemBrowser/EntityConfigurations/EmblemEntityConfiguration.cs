using ArmoredCoreSixEmblemBrowser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.EntityConfigurations;

public class EmblemEntityConfiguration : IEntityTypeConfiguration<Emblem>
{
    public void Configure(EntityTypeBuilder<Emblem> builder)
    {
        builder.ToTable("emblem");

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder
            .HasKey(x => x.Id)
            .HasName("pk_emblem_id");

        builder
            .Property(x => x.ShareId)
            .HasMaxLength(256)
            .IsRequired()
            .HasColumnName("share_id");

        builder
            .Property(x => x.Platform)
            .IsRequired()
            .HasColumnName("platform");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(64);

        builder
            .Property(x => x.FileName)
            .IsRequired(false)
            .HasColumnName("file_name")
            .HasMaxLength(64);

        builder
            .Property(x => x.CreatedAtUtc)
            .IsRequired()
            .HasColumnName("created_at_utc")
            .HasDefaultValueSql("timezone('utc', now())");
        builder
            .Property(x => x.ImageUrl)
            .HasColumnName("image_url");

        builder
            .HasIndex(x => x.ShareId, "ix_unique_share_id_platform")
            .IncludeProperties(x => x.Platform)
            .IsUnique();

        builder.Ignore(x => x.ImageData);
    }
}