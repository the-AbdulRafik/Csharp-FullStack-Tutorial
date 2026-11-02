using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Theatres;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers;

[ApiController, Route("api/theatres")]
public class TheatresController(ITheatreService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20) => Ok(await service.GetTheatresAsync(pageNumber, pageSize));
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) => (await service.GetTheatreAsync(id)) is { } result ? Ok(result) : NotFound();
    [HttpPost] public async Task<IActionResult> Create(CreateTheatreRequest request) { var result = await service.CreateTheatreAsync(request); return CreatedAtAction(nameof(Get), new { id = result.TheatreId }, result); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, UpdateTheatreRequest request) => (await service.UpdateTheatreAsync(id, request)) is { } result ? Ok(result) : NotFound();
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => await service.DeleteTheatreAsync(id) ? NoContent() : NotFound();
}
