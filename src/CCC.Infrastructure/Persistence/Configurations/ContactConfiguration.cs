using CCC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCC.Infrastructure.Persistence.Configurations;

public sealed class ContactConfiguration
    : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("contacts", "public", table =>
        {
            table.HasCheckConstraint(
                "contacts_birth_month_check",
                "birth_month >= 1 AND birth_month <= 12");

            table.HasCheckConstraint(
                "contacts_birth_day_check",
                "birth_day >= 1 AND birth_day <= 31");
        });

        builder.HasKey(x => x.Id)
            .HasName("contacts_pkey");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasColumnType("text");

        builder.Property(x => x.Address)
            .HasColumnName("address")
            .HasColumnType("text");

        builder.Property(x => x.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasColumnType("text");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()");

        builder.Property(x => x.BirthMonth)
            .HasColumnName("birth_month")
            .HasColumnType("smallint");

        builder.Property(x => x.BirthDay)
            .HasColumnName("birth_day")
            .HasColumnType("smallint");

        builder.HasIndex(x => new { x.BirthMonth, x.BirthDay })
            .HasDatabaseName("ix_contacts_birthday");
    }
}