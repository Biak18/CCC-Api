using MediatR;

namespace CCC.Application.Features.Avatars.GetAvatar;

public sealed record GetAvatarQuery(
    string FileName) : IRequest<GetAvatarResponse>;
