using MediatR;
namespace CCC.Application.Features.PushTokens.DeletePushToken;

public sealed record DeletePushTokenCommand(
    Guid PushTokenId) : IRequest<bool>;

public sealed record DeletePushTokenByValueCommand(
    string Token) : IRequest<bool>;