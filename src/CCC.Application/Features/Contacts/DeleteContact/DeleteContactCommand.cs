using MediatR;

namespace CCC.Application.Features.Contacts.DeleteContact;

public sealed record DeleteContactCommand(
    Guid ContactId) : IRequest<bool>;