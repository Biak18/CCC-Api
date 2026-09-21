using FluentValidation;

namespace CCC.Application.Features.Avatars.UploadAvatar;

public sealed class UploadAvatarValidator
    : AbstractValidator<UploadAvatarCommand>
{
    private const long MaxBytes = 5 * 1024 * 1024;

    private static readonly string[] AllowedExtensions =
        [".jpg", ".jpeg", ".png", ".webp"];

    public UploadAvatarValidator()
    {
        RuleFor(x => x.Length)
            .GreaterThan(0)
            .WithMessage("File must not be empty.")
            .LessThanOrEqualTo(MaxBytes)
            .WithMessage("File must be 5 MB or smaller.");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(HaveAllowedExtension)
            .WithMessage("Only .jpg, .jpeg, .png and .webp files are allowed.");
    }

    private static bool HaveAllowedExtension(string fileName) =>
        AllowedExtensions.Contains(
            Path.GetExtension(fileName).ToLowerInvariant());
}
