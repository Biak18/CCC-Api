namespace CCC.Application.Features.Contacts;

public sealed record ContactDto(
    Guid Id,
    string Name,
    string? Phone,
    string? Address,
    string? AvatarUrl,
    short? BirthMonth,
    short? BirthDay,
    DateTimeOffset? CreatedAt);