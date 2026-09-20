using CCC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Contact> Contacts { get; }

    DbSet<PushToken> PushTokens { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
