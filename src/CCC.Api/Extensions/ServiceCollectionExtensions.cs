namespace CCC.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiVersionedCors(
        this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddDefaultPolicy(policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()));

        return services;
    }
}
