namespace CCC.Api.Contracts;

public sealed record CreateContactRequest(
    string Name,
    string? Phone,
    string? Address,
    string? AvatarUrl,
    short? BirthMonth,
    short? BirthDay);

public sealed record UpdateContactRequest(
    string Name,
    string? Phone,
    string? Address,
    string? AvatarUrl,
    short? BirthMonth,
    short? BirthDay);