using FluentValidation;

namespace CCC.Application.Features.Contacts.CreateContact;

public sealed class CreateContactValidator
    : AbstractValidator<CreateContactCommand>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Phone)
            .MaximumLength(50);

        RuleFor(x => x.Address)
            .MaximumLength(500);

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(2000);

        RuleFor(x => x.BirthMonth)
            .InclusiveBetween((short)1, (short)12)
            .When(x => x.BirthMonth.HasValue);

        RuleFor(x => x.BirthDay)
            .InclusiveBetween((short)1, (short)31)
            .When(x => x.BirthDay.HasValue);

        RuleFor(x => x)
            .Must(x => x.BirthMonth.HasValue == x.BirthDay.HasValue)
            .WithMessage(
                "Birth month and birth day must be provided together.")
            .WithName("Birthday");
    }
}