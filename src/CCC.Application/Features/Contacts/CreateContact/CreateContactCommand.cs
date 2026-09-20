using MediatR;

namespace CCC.Application.Features.Contacts.CreateContact;

public sealed record CreateContactCommand(
    string Name,
    string? Phone,
    string? Address,
    string? AvatarUrl,
    short? BirthMonth,
    short? BirthDay) : IRequest<ContactDto>;