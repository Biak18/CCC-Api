using CCC.Application.Abstractions;
using CCC.Application.Common;
using CCC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.Contacts.UpdateContact;

public sealed class UpdateContactHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateContactCommand, ContactDto>
{
    public async Task<ContactDto> Handle(
        UpdateContactCommand command,
        CancellationToken cancellationToken)
    {
        var contact = await context.Contacts
            .FirstOrDefaultAsync(
                x => x.Id == command.ContactId,
                cancellationToken)
            ?? throw NotFoundException.For<Contact>(command.ContactId);

        contact.Update(
            command.Name,
            command.Phone,
            command.Address,
            command.AvatarUrl);

        contact.SetBirthday(
            command.BirthMonth,
            command.BirthDay);

        await context.SaveChangesAsync(cancellationToken);

        return contact.ToDto();
    }
}