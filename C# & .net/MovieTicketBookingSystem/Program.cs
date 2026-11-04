using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Repositories;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;
using MovieTicketBookingSystem.Middleware;
using MovieTicketBookingSystem.Models;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using MovieTicketBookingSystem.Authorization;


var builder = WebApplication.CreateBuilder(args);
var jwtConfiguration = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtConfiguration["Key"];
var jwtIssuer = jwtConfiguration["Issuer"];
var jwtAudience = jwtConfiguration["Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
    throw new InvalidOperationException("Jwt:Key must be configured with at least 32 characters.");
if (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
    throw new InvalidOperationException("Jwt:Issuer and Jwt:Audience must be configured.");

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDevelopment", policy =>
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieTicketBooking")));
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<CatalogueService>();
builder.Services.AddScoped<ITheatreService>(sp => sp.GetRequiredService<CatalogueService>());
builder.Services.AddScoped<IScreenService>(sp => sp.GetRequiredService<CatalogueService>());
builder.Services.AddScoped<ISeatService>(sp => sp.GetRequiredService<CatalogueService>());
builder.Services.AddScoped<ShowService>();
builder.Services.AddScoped<IShowService>(sp => sp.GetRequiredService<ShowService>());
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<IBookingService>(sp => sp.GetRequiredService<BookingService>());


// Authentication and Authorization
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthorizationHandler, BookingCancellationHandler>();


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BookingCancellation", policy =>
        policy.RequireAuthenticatedUser()
            .AddRequirements(new BookingCancellationRequirement()));
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    database.Database.Migrate();
}

// 1. HTTPS Redirection
app.UseHttpsRedirection();

// 2. Routing
//app.UseRouting();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseCors("AngularDevelopment");
if (app.Environment.IsDevelopment())
{
    await RoleSeeder.SeedAsync(app.Services, app.Configuration);
    app.MapOpenApi();
    app.MapScalarApiReference();
}


// 3. Endpoint Metadata Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}

// 4. Authentication
// Validates JWT and populates HttpContext.User
app.UseAuthentication();



// 5. Authentication and Authorization Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();
    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}

// 6. Authorization
// Checks endpoint access permissions
app.UseAuthorization();


// ==========================================
// 12. MAP CONTROLLERS
// ==========================================

app.MapControllers();


// ==========================================
// 13. RUN APPLICATION
// ==========================================

app.Run();

// SEEDING COMMANDS

// DOTNET USER-SECRETS SET "SEEDUSERS:ADMINPASSWORD" "Admin@123"
// DOTNET USER-SECRETS SET "SEEDUSERS:MANAGERPASSWORD" "MANAGER@123"