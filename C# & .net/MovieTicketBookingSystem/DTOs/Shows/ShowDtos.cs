using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystem.DTOs.Shows;

public class CreateShowRequest
{
    [Range(1, int.MaxValue)] public int MovieId { get; set; }
    [Range(1, int.MaxValue)] public int ScreenId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [Range(0, double.MaxValue)] public decimal TicketPrice { get; set; }
}
public class UpdateShowRequest : CreateShowRequest;
public record ShowResponse(int ShowId, int MovieId, string MovieTitle, int ScreenId, string ScreenName, int TheatreId, DateTime StartTime, DateTime EndTime, decimal TicketPrice);
public record ShowSeatResponse(int SeatId, string RowName, int SeatNumber, string SeatLabel, string SeatType, decimal Price, string Status);
