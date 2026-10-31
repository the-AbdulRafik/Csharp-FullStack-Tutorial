using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers;

[ApiController, Route("api/bookings")]
public class BookingsController(BookingService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.BookingId }, result);
    }
    [HttpGet] public async Task<IActionResult> GetAll([FromQuery] int? showId = null, [FromQuery] BookingStatus? status = null) => Ok(await service.GetAsync(showId, status));
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) => (await service.GetByIdAsync(id)) is { } result ? Ok(result) : NotFound();
    [HttpGet("show/{showId:int}")] public async Task<IActionResult> GetByShow(int showId) => Ok(await service.GetByShowAsync(showId));
    [HttpPut("{id:int}/cancel")] public async Task<IActionResult> Cancel(int id) => (await service.CancelAsync(id)) is { } result ? Ok(result) : NotFound();
}
