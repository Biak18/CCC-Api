using CCC.Api.Extensions;
using CCC.Api.Middleware;
using CCC.Application;
using CCC.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddApiVersionedCors();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi(options => options.AddBearerSecurityScheme());

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

/// <summary>
/// Exposed so integration tests can use WebApplicationFactory&lt;Program&gt;.
/// </summary>
public partial class Program;
