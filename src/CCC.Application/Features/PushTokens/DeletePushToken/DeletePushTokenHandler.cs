using CCC.Application.Abstractions;
using CCC.Application.Common;
using CCC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.PushTokens.DeletePushToken;

public sealed class DeletePushTokenHandler(IApplicationDbContext context)
    : IRequestHandler<DeletePushTokenCommand, bool>
{
    public async Task<bool> Handle(
        DeletePushTokenCommand command,
        CancellationToken cancellationToken)
    {
        var pushToken = await context.PushTokens
            .FirstOrDefaultAsync(
                x => x.Id == command.PushTokenId,
                cancellationToken)
            ?? throw NotFoundException.For<PushToken>(command.PushTokenId);

        context.PushTokens.Remove(pushToken);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public sealed class DeletePushTokenByValueHandler(IApplicationDbContext context)
    : IRequestHandler<DeletePushTokenByValueCommand, bool>
{
    public async Task<bool> Handle(
        DeletePushTokenByValueCommand command,
        CancellationToken cancellationToken)
    {
        var token = command.Token.Trim();

        var pushToken = await context.PushTokens
            .FirstOrDefaultAsync(
                x => x.Token == token,
                cancellationToken)
            ?? throw new NotFoundException(
                "The push token was not found.");

        context.PushTokens.Remove(pushToken);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}