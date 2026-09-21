namespace CCC.Application.Features.Avatars.GetAvatar;

public sealed record GetAvatarResponse(
    byte[] Content,
    string ContentType,
    string FileName);
