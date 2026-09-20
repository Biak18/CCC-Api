# CCC-Api

ASP.NET Core Web API on **.NET 10**, built with Clean Architecture +
Vertical Slice, as described in `ARCHITECTURE.md`.

## Projects

| Project | Purpose | References |
|---|---|---|
| `src/CCC.Domain` | Entities, value objects, enums, domain exceptions | none |
| `src/CCC.Application` | Features (commands/queries/handlers/validators), DTOs, abstractions | Domain |
| `src/CCC.Infrastructure` | EF Core `AppDbContext`, configurations, external services | Application, Domain |
| `src/CCC.Api` | Controllers, middleware, OpenAPI, composition root | Application, Infrastructure (composition only) |
| `tests/CCC.UnitTests` | Domain behavior tests (xUnit) | Domain, Application |
| `tests/CCC.IntegrationTests` | Full HTTP pipeline tests over SQLite in-memory | Api |

## Getting started

1. Open `CCC-Api.sln` in Visual Studio 2026 (or run `dotnet build`).
2. Set the connection string in `src/CCC.Api/appsettings.json`
   (defaults to LocalDB) or use user secrets.
3. Create the database:

   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate \
     --project src/CCC.Infrastructure \
     --startup-project src/CCC.Api \
     --output-dir Persistence/Migrations
   dotnet ef database update \
     --project src/CCC.Infrastructure \
     --startup-project src/CCC.Api
   ```

4. Run `CCC.Api`. The OpenAPI document is at `/openapi/v1.json`
   in Development.

## What is included

- CQRS with **MediatR 12.4.1** (`ICommand<T>` / `IQuery<T>` derive from
  `IRequest<T>`; handlers are discovered by `RegisterServicesFromAssembly`).
- MediatR pipeline behaviors: `LoggingBehavior` then `ValidationBehavior`
  (FluentValidation runs automatically before every handler).
- Centralized error handling via `IExceptionHandler` + `ProblemDetails`
  (400 / 401 / 403 / 404 / 409 / 422 / 500).
- Pagination, search, and whitelisted sorting on `GET /api/products`.
- `AsNoTracking()` + projection to DTOs for read queries.
- Sample domain rules (`Order.Confirm`, `Product.Create`) with unit tests.

## Endpoints

```text
GET    /api/products?page=1&pageSize=20&search=&sortBy=name&sortDirection=asc
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

## Adding a feature

```text
Application/Features/<Area>/<UseCase>/
├── <UseCase>Command.cs   (or Query)
├── <UseCase>Handler.cs
├── <UseCase>Validator.cs
└── <UseCase>Response.cs  (or shared DTO)
```

Handlers are discovered and registered automatically by MediatR in
`AddApplication()`; no manual DI wiring is needed.

> **MediatR licensing:** v12.x is MIT. v13+ is commercially licensed for
> organizations above the free-use revenue threshold, so this solution
> pins `MediatR 12.4.1`.
