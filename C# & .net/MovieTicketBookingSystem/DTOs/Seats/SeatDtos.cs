using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystem.DTOs.Seats;

public class CreateSeatRequest
{
    [Range(1, int.MaxValue)] public int ScreenId { get; set; }
    [Required, StringLength(5)] public string RowName { get; set; } = string.Empty;
    [Range(1, int.MaxValue)] public int SeatNumber { get; set; }
    [Required, StringLength(30)] public string SeatType { get; set; } = "Regular";
}
public class UpdateSeatRequest : CreateSeatRequest;
public class BulkCreateSeatsRequest
{
    [Range(1, int.MaxValue)] public int ScreenId { get; set; }
    [Required] public List<SeatDefinition> Seats { get; set; } = [];
}
public class SeatDefinition
{
    [Required, StringLength(5)] public string RowName { get; set; } = string.Empty;
    [Range(1, int.MaxValue)] public int SeatNumber { get; set; }
    [Required, StringLength(30)] public string SeatType { get; set; } = "Regular";
}
public record SeatResponse(int SeatId, int ScreenId, string RowName, int SeatNumber, string SeatType, string SeatLabel);
