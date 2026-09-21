using FluentValidation;
using System.Text.RegularExpressions;

namespace CCC.Application.Features.Avatars.GetAvatar;

public sealed class GetAvatarValidator
    : AbstractValidator<GetAvatarQuery>
{
    // Only serves files this API generated: {32-hex-guid}.{ext}.
    // Blocks path traversal (no slashes or dots beyond the extension).
    private static readonly Regex SafeFileNameRegex =
        new("^[0-9a-f]{32}\\.(jpg|jpeg|png|webp)$", RegexOptions.Compiled);

    public GetAvatarValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(f => SafeFileNameRegex.IsMatch(f.ToLowerInvariant()))
            .WithMessage("Avatar was not found.");
    }
}
