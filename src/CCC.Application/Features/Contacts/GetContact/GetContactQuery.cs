using MediatR;

namespace CCC.Application.Features.Contacts.GetContact;


public sealed record GetContactQuery(
    Guid ContactId) : IRequest<ContactDto>;