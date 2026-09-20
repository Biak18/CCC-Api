using CCC.Application.Abstractions;
using CCC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCC.Application.Features.PushTokens.RegisterPushToken;

public sealed class RegisterPushTokenHandler(IApplicationDbContext context)
    : IRequestHandler<RegisterPushTokenCommand, RegisterPushTokenResult>
{
    public async Task<RegisterPushTokenResult> Handle(
        RegisterPushTokenCommand command,
        CancellationToken cancellationToken)
    {
        var token = command.Token.Trim();

        var existing = await context.PushTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Token == token,
                cancellationToken);

        if (existing is not null)
        {
            return new RegisterPushTokenResult(
                new PushTokenDto(
                    existing.Id,
                    existing.Token,
                    existing.CreatedAt),
                Created: false);
        }

        var pushToken = PushToken.Create(token);

        context.PushTokens.Add(pushToken);

        await context.SaveChangesAsync(cancellationToken);

        return new RegisterPushTokenResult(
            new PushTokenDto(
                pushToken.Id,
                pushToken.Token,
                pushToken.CreatedAt),
            Created: true);
    }
}