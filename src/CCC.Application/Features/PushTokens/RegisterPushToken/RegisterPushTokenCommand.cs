using MediatR;

namespace CCC.Application.Features.PushTokens.RegisterPushToken;

/// <summary>
/// Idempotent: registering an existing token returns the stored row.
/// </summary>
public sealed record RegisterPushTokenCommand(
    string Token) : IRequest<RegisterPushTokenResult>;

public sealed record RegisterPushTokenResult(
    PushTokenDto PushToken,
    bool Created);