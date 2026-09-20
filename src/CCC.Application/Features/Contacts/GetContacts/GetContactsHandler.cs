using CCC.Application.Abstractions;
using CCC.Application.Common;
using CCC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.Contacts.GetContacts;

public sealed class GetContactsHandler(IApplicationDbContext context)
    : IRequestHandler<GetContactsQuery, PagedResult<ContactDto>>
{
    public async Task<PagedResult<ContactDto>> Handle(
        GetContactsQuery query,
        CancellationToken cancellationToken)
    {
        var contacts = context.Contacts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";

            contacts = contacts.Where(x =>
                EF.Functions.Like(x.Name, pattern) ||
                (x.Phone != null &&
                 EF.Functions.Like(x.Phone, pattern)));
        }

        if (query.BirthMonth.HasValue)
        {
            contacts = contacts.Where(
                x => x.BirthMonth == query.BirthMonth);
        }

        contacts = Sort(contacts, query);

        var totalCount = await contacts
            .CountAsync(cancellationToken);

        var items = await contacts
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new ContactDto(
                x.Id,
                x.Name,
                x.Phone,
                x.Address,
                x.AvatarUrl,
                x.BirthMonth,
                x.BirthDay,
                x.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<ContactDto>(
            items,
            query.Page,
            query.PageSize,
            totalCount);
    }

    private static IQueryable<Contact> Sort(
        IQueryable<Contact> contacts,
        GetContactsQuery query)
    {
        var descending = query.SortDirection
            .Equals("desc", StringComparison.OrdinalIgnoreCase);

        return query.SortBy.ToLowerInvariant() switch
        {
            "createdat" => descending
                ? contacts.OrderByDescending(x => x.CreatedAt)
                : contacts.OrderBy(x => x.CreatedAt),
            "birthday" => descending
                ? contacts
                    .OrderByDescending(x => x.BirthMonth)
                    .ThenByDescending(x => x.BirthDay)
                : contacts
                    .OrderBy(x => x.BirthMonth)
                    .ThenBy(x => x.BirthDay),
            _ => descending
                ? contacts.OrderByDescending(x => x.Name)
                : contacts.OrderBy(x => x.Name)
        };
    }
}