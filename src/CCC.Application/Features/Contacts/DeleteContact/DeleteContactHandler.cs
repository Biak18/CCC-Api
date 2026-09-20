using CCC.Application.Abstractions;
using CCC.Application.Common;
using CCC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.Contacts.DeleteContact;

public sealed class DeleteContactHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteContactCommand, bool>
{
    public async Task<bool> Handle(
        DeleteContactCommand command,
        CancellationToken cancellationToken)
    {
        var contact = await context.Contacts
            .FirstOrDefaultAsync(
                x => x.Id == command.ContactId,
                cancellationToken)
            ?? throw NotFoundException.For<Contact>(command.ContactId);

        context.Contacts.Remove(contact);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}