using CCC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCC.Infrastructure.Persistence.Configurations;

public sealed class PushTokenConfiguration
    : IEntityTypeConfiguration<PushToken>
{
    public void Configure(EntityTypeBuilder<PushToken> builder)
    {
        builder.ToTable("push_tokens", "public");

        builder.HasKey(x => x.Id)
            .HasName("push_tokens_pkey");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Token)
            .HasColumnName("token")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()");

        builder.HasIndex(x => x.Token)
            .IsUnique()
            .HasDatabaseName("push_tokens_token_key");
    }
}