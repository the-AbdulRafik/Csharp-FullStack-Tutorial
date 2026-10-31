using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;

namespace MovieTicketBookingSystem.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var databaseHealthy = await db.Database.CanConnectAsync();
        return databaseHealthy
            ? Ok(new { status = "Healthy", service = "MovieTicketBookingSystem", database = "Healthy" })
            : StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = "Unhealthy", service = "MovieTicketBookingSystem", database = "Unavailable" });
    }
}
