using CCC.Domain.Entities;

namespace CCC.Application.Features.Contacts;

internal static class ContactMappings
{
    public static ContactDto ToDto(this Contact contact) =>
        new(
            contact.Id,
            contact.Name,
            contact.Phone,
            contact.Address,
            contact.AvatarUrl,
            contact.BirthMonth,
            contact.BirthDay,
            contact.CreatedAt);
}