using Scalar.AspNetCore;
using WebApiPractice.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddSingleton<StudentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
{
    options
        .WithTitle("My Practice API")
        .WithTheme(ScalarTheme.Mars); // Themes: Moon, Mars, DeepSpace, Purple, Solarized, etc.
});
}



app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();


