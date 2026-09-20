using FluentValidation;

namespace CCC.Application.Features.PushTokens.GetPushTokens;

public sealed class GetPushTokensValidator
    : AbstractValidator<GetPushTokensQuery>
{
    public GetPushTokensValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 200);
    }
}