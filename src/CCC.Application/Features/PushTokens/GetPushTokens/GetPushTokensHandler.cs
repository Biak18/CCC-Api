using CCC.Application.Abstractions;
using CCC.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.PushTokens.GetPushTokens;

public sealed class GetPushTokensHandler(IApplicationDbContext context)
    : IRequestHandler<GetPushTokensQuery, PagedResult<PushTokenDto>>
{
    public async Task<PagedResult<PushTokenDto>> Handle(
        GetPushTokensQuery query,
        CancellationToken cancellationToken)
    {
        var tokens = context.PushTokens
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt);

        var totalCount = await tokens
            .CountAsync(cancellationToken);

        var items = await tokens
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new PushTokenDto(
                x.Id,
                x.Token,
                x.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<PushTokenDto>(
            items,
            query.Page,
            query.PageSize,
            totalCount);
    }
}