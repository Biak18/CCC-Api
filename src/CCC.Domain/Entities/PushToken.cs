using CCC.Domain.Exceptions;

namespace CCC.Domain.Entities;

public sealed class PushToken
{
    public Guid Id { get; private set; }

    public string Token { get; private set; } = string.Empty;

    public DateTimeOffset? CreatedAt { get; private set; }

    private PushToken()
    {
    }

    private PushToken(
        Guid id,
        string token)
    {
        Id = id;
        Token = token;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static PushToken Create(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new DomainException(
                "Push token is required.");
        }

        return new PushToken(
            Guid.NewGuid(),
            token.Trim());
    }
}