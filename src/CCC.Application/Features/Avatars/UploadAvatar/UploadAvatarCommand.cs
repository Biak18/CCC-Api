using MediatR;

namespace CCC.Application.Features.Avatars.UploadAvatar;

public sealed record UploadAvatarCommand(
    Stream Content,
    string FileName,
    string ContentType,
    long Length) : IRequest<UploadAvatarResponse>;
