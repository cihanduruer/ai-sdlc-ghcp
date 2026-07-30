using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.Data;

public sealed class HotelBookingContext(DbContextOptions<HotelBookingContext> options)
    : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(room => room.Number).HasMaxLength(10);
            entity.Property(room => room.Name).HasMaxLength(80);
            entity.Property(room => room.Description).HasMaxLength(300);
            entity.Property(room => room.PricePerNight).HasPrecision(10, 2);
            entity.HasIndex(room => room.Number).IsUnique();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(booking => booking.GuestName).HasMaxLength(120);
            entity.Property(booking => booking.GuestEmail).HasMaxLength(254);
            entity.HasIndex(booking => new { booking.RoomId, booking.CheckIn, booking.CheckOut });
        });
    }
}
