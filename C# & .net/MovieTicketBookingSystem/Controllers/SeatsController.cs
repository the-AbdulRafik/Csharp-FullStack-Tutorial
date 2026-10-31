using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Seats;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers;

[ApiController, Route("api/seats")]
public class SeatsController(CatalogueService service) : ControllerBase
{
    [HttpGet("screen/{screenId:int}")] public async Task<IActionResult> GetByScreen(int screenId) => Ok(await service.GetSeatsAsync(screenId));
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) => (await service.GetSeatAsync(id)) is { } result ? Ok(result) : NotFound();
    [HttpPost] public async Task<IActionResult> Create(CreateSeatRequest request) { var result = await service.CreateSeatAsync(request); return CreatedAtAction(nameof(Get), new { id = result.SeatId }, result); }
    [HttpPost("bulk")] public async Task<IActionResult> CreateBulk(BulkCreateSeatsRequest request) => StatusCode(StatusCodes.Status201Created, await service.CreateSeatsAsync(request));
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, UpdateSeatRequest request) => (await service.UpdateSeatAsync(id, request)) is { } result ? Ok(result) : NotFound();
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => await service.DeleteSeatAsync(id) ? NoContent() : NotFound();
}
