using MediatR;
namespace CCC.Application.Features.Contacts.UpdateContact;

public sealed record UpdateContactCommand(
    Guid ContactId,
    string Name,
    string? Phone,
    string? Address,
    string? AvatarUrl,
    short? BirthMonth,
    short? BirthDay) : IRequest<ContactDto>;