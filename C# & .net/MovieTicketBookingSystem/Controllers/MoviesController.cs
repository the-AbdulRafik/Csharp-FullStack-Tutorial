using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Movies;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController(IMovieService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MovieResponse>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] string? title = null) =>
        Ok(await service.GetAllAsync(pageNumber, pageSize, title));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieResponse>> GetById(int id) =>
        await service.GetByIdAsync(id) is { } movie ? Ok(movie) : NotFound();

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<MovieResponse>>> Search([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("The title query parameter is required.");
        return Ok(await service.GetAllAsync(1, 100, title.Trim()));
    }

    [HttpPost]
    public async Task<ActionResult<MovieResponse>> Create(CreateMovieRequest request)
    {
        var movie = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = movie.MovieId }, movie);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MovieResponse>> Update(int id, UpdateMovieRequest request) =>
        await service.UpdateAsync(id, request) is { } movie ? Ok(movie) : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
