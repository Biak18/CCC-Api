using CCC.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CCC.IntegrationTests;

public sealed class ApiFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private SqliteConnection _connection = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            services.Remove(descriptor);

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_connection));

            using var scope = services
                .BuildServiceProvider()
                .CreateScope();

            scope.ServiceProvider
                .GetRequiredService<AppDbContext>()
                .Database
                .EnsureCreated();
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public new async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        await base.DisposeAsync();
    }
}
