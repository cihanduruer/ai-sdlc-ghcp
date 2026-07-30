namespace HotelBooking.Api.Models;

public sealed class Booking
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public required string GuestName { get; set; }

    public required string GuestEmail { get; set; }

    public DateOnly CheckIn { get; set; }

    public DateOnly CheckOut { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public Room Room { get; set; } = null!;
}
