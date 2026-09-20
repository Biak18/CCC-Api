using MediatR;
namespace CCC.Application.Features.PushTokens.GetPushToken;

public sealed record GetPushTokenQuery(
    Guid PushTokenId) : IRequest<PushTokenDto>;