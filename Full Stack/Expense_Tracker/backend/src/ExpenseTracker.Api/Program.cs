using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Extensions;
using ExpenseTracker.Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<HealthService>();
builder.Services.AddDbContext<ExpenseTrackerDbContext>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDevelopment", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseCors("FrontendDevelopment");
    app.MapOpenApi();
    app.MapScalarApiReference(options => options
        .WithTitle("ExpenseTracker API")
        .WithTheme(ScalarTheme.Default));
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


