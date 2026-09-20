using FluentValidation;
using MediatR;

namespace CCC.Application.Common.Behaviors;

/// <summary>
/// Runs all registered FluentValidation validators for a request before the
/// handler executes. This is what lets controllers stay thin (ARCHITECTURE.md
/// section 6) - validation happens centrally, not in the handler or controller.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var failures = (await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
