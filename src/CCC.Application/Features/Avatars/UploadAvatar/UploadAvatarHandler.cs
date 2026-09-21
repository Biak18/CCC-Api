using CCC.Application.Abstractions;
using MediatR;

namespace CCC.Application.Features.Avatars.UploadAvatar;

public sealed class UploadAvatarHandler(IAvatarStorage storage)
    : IRequestHandler<UploadAvatarCommand, UploadAvatarResponse>
{
    public async Task<UploadAvatarResponse> Handle(
        UploadAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(command.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{extension}";

        var url = await storage.UploadAsync(
            command.Content,
            fileName,
            command.ContentType,
            cancellationToken);

        return new UploadAvatarResponse(fileName, url);
    }
}
