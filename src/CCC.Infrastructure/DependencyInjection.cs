
using CCC.Application.Abstractions;
using CCC.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CCC.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found in configuration.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name)));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<AppDbContext>());

        var supabaseUrl = configuration["Supabase:Url"]?.TrimEnd('/')
      ?? throw new InvalidOperationException("Supabase:Url is required.");

        var supabaseAnonKey = configuration["Supabase:AnonKey"]
     ?? throw new InvalidOperationException("Supabase:AnonKey is required.");


        // don't need auth for this project
        return services;
    }
}
