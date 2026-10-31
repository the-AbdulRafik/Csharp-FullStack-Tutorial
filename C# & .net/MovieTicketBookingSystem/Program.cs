using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Repositories;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;
using MovieTicketBookingSystem.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieTicketBooking")));
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<CatalogueService>();
builder.Services.AddScoped<ShowService>();
builder.Services.AddScoped<BookingService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    database.Database.Migrate();
}

app.UseMiddleware<GlobalExceptionHandler>();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
