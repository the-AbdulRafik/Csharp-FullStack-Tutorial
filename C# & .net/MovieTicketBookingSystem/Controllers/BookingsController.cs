using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MovieTicketBookingSystem.Constants;
using MovieTicketBookingSystem.Authorization;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers;

[ApiController, Route("api/bookings")]
public class BookingsController(
    IBookingService service,
    IAuthorizationService authorizationService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> Create(CreateBookingRequest request)
    {
        var customerEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(customerEmail))
            return Unauthorized();

        request.CustomerEmail = customerEmail;
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
    [Authorize]
    public async Task<IActionResult> Cancel(int id)
    {
        var authorization = await authorizationService
            .AuthorizeAsync(User, id.ToString(), "BookingCancellation");

        if (!authorization.Succeeded)
            return Forbid();

        return (await service.CancelAsync(id)) is { } result ? Ok(result) : NotFound();
    }
}
