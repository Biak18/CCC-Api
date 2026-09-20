namespace CCC.Application.Features.PushTokens;

public sealed record PushTokenDto(
    Guid Id,
    string Token,
    DateTimeOffset? CreatedAt);