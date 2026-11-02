using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.Constants;
using MovieTicketBookingSystem.DTOs.Screens;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers;

[ApiController, Route("api/screens")]
public class ScreensController(IScreenService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll([FromQuery] int? theatreId = null) => Ok(await service.GetScreensAsync(theatreId));
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) => (await service.GetScreenAsync(id)) is { } result ? Ok(result) : NotFound();
    [HttpGet("theatre/{theatreId:int}")] public async Task<IActionResult> GetByTheatre(int theatreId) => Ok(await service.GetScreensAsync(theatreId));
    [HttpPost, Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")] public async Task<IActionResult> Create(CreateScreenRequest request) { var result = await service.CreateScreenAsync(request); return CreatedAtAction(nameof(Get), new { id = result.ScreenId }, result); }
    [HttpPut("{id:int}"), Authorize(Roles = $"{UserRoles.Admin},{UserRoles.TheatreManager}")] public async Task<IActionResult> Update(int id, UpdateScreenRequest request) => (await service.UpdateScreenAsync(id, request)) is { } result ? Ok(result) : NotFound();
    [HttpDelete("{id:int}"), Authorize(Roles = UserRoles.Admin)] public async Task<IActionResult> Delete(int id) => await service.DeleteScreenAsync(id) ? NoContent() : NotFound();
}
