# Project Architecture & Conventions

## Clean Architecture Structure for "Trusted"

The solution follows Clean Architecture with 5 primary projects under `Trusted.slnx`:

```text
Trusted.slnx
├── Trusted.API/                   (Presentation Layer - Web API)
│   ├── Controllers/
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Dockerfile
│   ├── Program.cs
│   ├── Trusted.API.csproj
│   ├── Trusted.API.http
│   ├── appsettings.json
│   └── appsettings.Development.json
│
├── Trusted.Application/           (Business Logic Layer)
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Services/
│   ├── DependencyInjection.cs
│   └── Trusted.Application.csproj
│
├── Trusted.Core/                  (Domain / Core Layer)
│   ├── Common/
│   ├── Entities/
│   │   └── User.cs                (IdentityUser implementation)
│   ├── Enums/
│   ├── IRepositories/
│   ├── IServices/
│   ├── DependencyInjection.cs
│   ├── GenericResponse.cs
│   └── Trusted.Core.csproj
│
├── Trusted.Infrastructure/        (Infrastructure / Data Access Layer)
│   ├── DataSeeding/
│   ├── EF Configuration/
│   ├── Migrations/
│   ├── Repositories/
│   ├── ApplicationDbContext.cs
│   ├── DependencyInjection.cs
│   └── Trusted.Infrastructure.csproj
│
└── Trusted.ViewModel/             (Shared ViewModels & DTOs Layer)
    └── Trusted.ViewModel.csproj
```

### Layer Details & Dependencies

1. **`Trusted.API`** (Presentation Layer)
   - SDK: `Microsoft.NET.Sdk.Web`
   - Role: HTTP API endpoints, Middleware, Swagger/OpenAPI.
   - References: `Trusted.Application`, `Trusted.ViewModel`.

2. **`Trusted.Application`** (Business Logic Layer)
   - SDK: `Microsoft.NET.Sdk` (Class Library)
   - Role: Application use cases, business services (`Services/`), DI configuration (`AddApplication`).
   - References: `Trusted.Core`, `Trusted.Infrastructure`, `Trusted.ViewModel`.

3. **`Trusted.Core`** (Domain / Core Layer)
   - SDK: `Microsoft.NET.Sdk` (Class Library + `Microsoft.AspNetCore.App` framework ref)
   - Role: Domain entities (`Entities/`), interfaces (`IRepositories/`, `IServices/`), constants (`Common/`, `Enums/`), generic responses (`GenericResponse<T>`).
   - References: `Trusted.ViewModel`.

4. **`Trusted.Infrastructure`** (Infrastructure Layer)
   - SDK: `Microsoft.NET.Sdk` (Class Library)
   - Role: DbContext (`ApplicationDbContext`), EF Core configurations, repository implementations, migrations, seeding (`DataSeeding/`).
   - References: `Trusted.Core`.

5. **`Trusted.ViewModel`** (ViewModels / DTOs)
   - SDK: `Microsoft.NET.Sdk` (Class Library)
   - Role: Request/Response DTOs and shared models used across Presentation, Application, and Core layers.

---

## Authentication & JWT Token Architecture

### 1. User & Identity Entity (`Trusted.Core/Entities/User.cs`)
- Inherits from `Microsoft.AspNetCore.Identity.EntityFrameworkCore` / `IdentityUser`.
- Custom domain properties added directly to `User`.

### 2. JWT Configuration Options (`Trusted.Core` / `Trusted.ViewModel`)
- `JwtOptions` bound to `"Jwt"` appsettings section:
  - `Key` (Secret key for SymmetricSecurityKey signature)
  - `Issuer` (Token issuer)
  - `Audience` (Token target audience)
  - `ExpiryMinutes` (Expiration window)

### 3. JWT Provider Abstraction (`Trusted.Core/IServices/IJwtProvider.cs`)
- Interface `IJwtProvider`:
  ```csharp
  public interface IJwtProvider
  {
      string GenerateToken(User user, IEnumerable<string> roles);
  }
  ```

### 4. Authentication & DI Setup Pattern (`Trusted.Core/DependencyInjection.cs`)
- Registers `AddIdentity<User, IdentityRole>()` bound to `ApplicationDbContext`.
- Configures `JwtBearer` authentication scheme:
  ```csharp
  services.AddAuthentication(options =>
  {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(options =>
  {
      options.SaveToken = true;
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience
      };
  });
  ```
- Configures Identity options (e.g. `RequireUniqueEmail = true`).
