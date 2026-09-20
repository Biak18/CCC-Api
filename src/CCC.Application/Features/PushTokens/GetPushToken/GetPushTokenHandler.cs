using CCC.Application.Abstractions;
using CCC.Application.Common;
using CCC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.PushTokens.GetPushToken;

public sealed class GetPushTokenHandler(IApplicationDbContext context)
    : IRequestHandler<GetPushTokenQuery, PushTokenDto>
{
    public async Task<PushTokenDto> Handle(
        GetPushTokenQuery query,
        CancellationToken cancellationToken)
    {
        var pushToken = await context.PushTokens
            .AsNoTracking()
            .Where(x => x.Id == query.PushTokenId)
            .Select(x => new PushTokenDto(
                x.Id,
                x.Token,
                x.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        return pushToken
            ?? throw NotFoundException.For<PushToken>(query.PushTokenId);
    }
}