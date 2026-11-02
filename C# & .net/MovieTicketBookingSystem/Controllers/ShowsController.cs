using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.Constants;
using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers;

[ApiController, Route("api/shows")]
public class ShowsController(IShowService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll([FromQuery] DateTime? date = null, [FromQuery] int? movieId = null, [FromQuery] int? theatreId = null) => Ok(await service.GetAsync(date, movieId, theatreId));
    [HttpGet("search")] public async Task<IActionResult> Search([FromQuery] DateTime date) => Ok(await service.GetAsync(date, null, null));
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) => (await service.GetByIdAsync(id)) is { } result ? Ok(result) : NotFound();
    [HttpGet("movie/{movieId:int}")] public async Task<IActionResult> GetByMovie(int movieId) => Ok(await service.GetAsync(null, movieId, null));
    [HttpGet("theatre/{theatreId:int}")] public async Task<IActionResult> GetByTheatre(int theatreId) => Ok(await service.GetAsync(null, null, theatreId));
    [HttpPost, Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")] public async Task<IActionResult> Create(CreateShowRequest request) { var result = await service.CreateAsync(request); return CreatedAtAction(nameof(Get), new { id = result.ShowId }, result); }
    [HttpPut("{id:int}"), Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")] public async Task<IActionResult> Update(int id, UpdateShowRequest request) => (await service.UpdateAsync(id, request)) is { } result ? Ok(result) : NotFound();
    [HttpDelete("{id:int}"), Authorize(Roles = UserRoles.Admin)] public async Task<IActionResult> Delete(int id) => await service.DeleteAsync(id) ? NoContent() : NotFound();
    [HttpGet("{showId:int}/seats")] public async Task<IActionResult> Seats(int showId) => (await service.GetSeatsAsync(showId)) is { } result ? Ok(result) : NotFound();
    [HttpGet("{showId:int}/seats/available")] public async Task<IActionResult> AvailableSeats(int showId) => (await service.GetSeatsAsync(showId, SeatStatus.Available)) is { } result ? Ok(result) : NotFound();
    [HttpGet("{showId:int}/seats/booked")] public async Task<IActionResult> BookedSeats(int showId) => (await service.GetSeatsAsync(showId, SeatStatus.Booked)) is { } result ? Ok(result) : NotFound();
}
