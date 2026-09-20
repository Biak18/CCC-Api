using FluentValidation;

namespace CCC.Application.Features.Contacts.GetContacts;

public sealed class GetContactsValidator
    : AbstractValidator<GetContactsQuery>
{
    private static readonly string[] AllowedSortFields =
        ["name", "createdat", "birthday"];

    public GetContactsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.BirthMonth)
            .InclusiveBetween((short)1, (short)12)
            .When(x => x.BirthMonth.HasValue);

        RuleFor(x => x.SortBy)
            .Must(x => AllowedSortFields.Contains(x.ToLowerInvariant()))
            .WithMessage(
                "Allowed sort fields: name, createdAt, birthday.");

        RuleFor(x => x.SortDirection)
            .Must(x => x.Equals("asc", StringComparison.OrdinalIgnoreCase)
                || x.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Sort direction must be asc or desc.");
    }
}