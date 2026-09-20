using CCC.Application.Abstractions;
using CCC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CCC.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Contact> Contacts => Set<Contact>();

    public DbSet<PushToken> PushTokens => Set<PushToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
