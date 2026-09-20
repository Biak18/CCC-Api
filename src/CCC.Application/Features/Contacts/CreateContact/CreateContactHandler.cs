using CCC.Application.Abstractions;
using CCC.Domain.Entities;
using MediatR;

namespace CCC.Application.Features.Contacts.CreateContact;

public sealed class CreateContactHandler(IApplicationDbContext context)
    : IRequestHandler<CreateContactCommand, ContactDto>
{
    public async Task<ContactDto> Handle(
        CreateContactCommand command,
        CancellationToken cancellationToken)
    {
        var contact = Contact.Create(
            command.Name,
            command.Phone,
            command.Address,
            command.AvatarUrl,
            command.BirthMonth,
            command.BirthDay);

        context.Contacts.Add(contact);

        await context.SaveChangesAsync(cancellationToken);

        return contact.ToDto();
    }
}