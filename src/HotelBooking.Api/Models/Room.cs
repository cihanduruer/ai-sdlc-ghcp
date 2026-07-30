namespace HotelBooking.Api.Models;

public sealed class Room
{
    public int Id { get; set; }

    public required string Number { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public int Capacity { get; set; }

    public decimal PricePerNight { get; set; }

    public ICollection<Booking> Bookings { get; set; } = [];
}
