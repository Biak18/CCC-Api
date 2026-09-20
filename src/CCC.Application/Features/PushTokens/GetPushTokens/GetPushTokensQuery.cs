using CCC.Application.Common;
using MediatR;

namespace CCC.Application.Features.PushTokens.GetPushTokens;

public sealed record GetPushTokensQuery(
    int Page = 1,
    int PageSize = 50)
    : IRequest<PagedResult<PushTokenDto>>;