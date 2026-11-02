using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.Constants;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers;

[ApiController, Route("api/bookings")]
public class BookingsController(IBookingService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> Create(CreateBookingRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.BookingId }, result);
    }
    [HttpGet] 
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")]
    public async Task<IActionResult> GetAll([FromQuery] int? showId = null, [FromQuery] BookingStatus? status = null) => Ok(await service.GetAsync(showId, status));
    [HttpGet("{id:int}")] 
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")]
    public async Task<IActionResult> Get(int id) => (await service.GetByIdAsync(id)) is { } result ? Ok(result) : NotFound();
    [HttpGet("show/{showId:int}")] 
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")]
    public async Task<IActionResult> GetByShow(int showId) => Ok(await service.GetByShowAsync(showId));
    [HttpPut("{id:int}/cancel")] 
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")]
    public async Task<IActionResult> Cancel(int id) => (await service.CancelAsync(id)) is { } result ? Ok(result) : NotFound();
}
