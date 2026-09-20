using FluentValidation;

namespace CCC.Application.Features.PushTokens.RegisterPushToken;

public sealed class RegisterPushTokenValidator
    : AbstractValidator<RegisterPushTokenCommand>
{
    public RegisterPushTokenValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .MaximumLength(4000);
    }
}