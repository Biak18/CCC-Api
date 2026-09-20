using CCC.Application.Abstractions;
using CCC.Application.Common;
using CCC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.Contacts.GetContact;

public sealed class GetContactHandler(IApplicationDbContext context)
    : IRequestHandler<GetContactQuery, ContactDto>
{
    public async Task<ContactDto> Handle(
        GetContactQuery query,
        CancellationToken cancellationToken)
    {
        var contact = await context.Contacts
            .AsNoTracking()
            .Where(x => x.Id == query.ContactId)
            .Select(x => new ContactDto(
                x.Id,
                x.Name,
                x.Phone,
                x.Address,
                x.AvatarUrl,
                x.BirthMonth,
                x.BirthDay,
                x.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        return contact
            ?? throw NotFoundException.For<Contact>(query.ContactId);
    }
}