using CCC.Application.Abstractions;
using CCC.Application.Common;
using MediatR;

namespace CCC.Application.Features.Avatars.GetAvatar;

public sealed class GetAvatarHandler(IAvatarStorage storage)
    : IRequestHandler<GetAvatarQuery, GetAvatarResponse>
{
    public async Task<GetAvatarResponse> Handle(
        GetAvatarQuery query,
        CancellationToken cancellationToken)
    {
        var file = await storage.DownloadAsync(
            query.FileName,
            cancellationToken)
            ?? throw new NotFoundException(
                $"Avatar '{query.FileName}' was not found.");

        return new GetAvatarResponse(
            file.Content,
            file.ContentType,
            query.FileName);
    }
}
