namespace MovieTicketBookingSystem.DTOs.Movies;

public record MovieResponse(
    int MovieId,
    string Title,
    string? Description,
    int DurationMinutes,
    DateOnly ReleaseDate,
    string Language,
    string Genre);
