using HotelBooking.Api.Contracts;
using HotelBooking.Api.Data;
using HotelBooking.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotelBookingContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("HotelBooking")));
builder.Services.AddSingleton<BookingWriteLock>();
builder.Services.AddCors(options =>
    options.AddPolicy("WebClient", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()));

var app = builder.Build();

app.UseCors("WebClient");

using (var scope = app.Services.CreateScope())
{
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
}

var api = app.MapGroup("/api");

api.MapGet("/rooms", async (
    DateOnly? checkIn,
    DateOnly? checkOut,
    HotelBookingContext db,
    CancellationToken cancellationToken) =>
{
    if (checkIn.HasValue != checkOut.HasValue)
    {
        return Results.BadRequest(new { message = "Provide both check-in and check-out dates." });
    }

    if (checkIn.HasValue && checkOut <= checkIn)
    {
        return Results.BadRequest(new { message = "Check-out must be after check-in." });
    }

    var rooms = await db.Rooms
        .AsNoTracking()
        .OrderBy(room => (double)room.PricePerNight)
        .Select(room => new RoomResponse(
            room.Id,
            room.Number,
            room.Name,
            room.Description,
            room.Capacity,
            room.PricePerNight,
            !checkIn.HasValue || !room.Bookings.Any(booking =>
                checkIn.Value < booking.CheckOut && checkOut!.Value > booking.CheckIn)))
        .ToListAsync(cancellationToken);

    return Results.Ok(rooms);
});

api.MapGet("/bookings", async (HotelBookingContext db, CancellationToken cancellationToken) =>
{
    var bookings = await db.Bookings
        .AsNoTracking()
        .OrderBy(booking => booking.CheckIn)
        .Select(booking => new BookingResponse(
            booking.Id,
            booking.RoomId,
            booking.Room.Number,
            booking.Room.Name,
            booking.GuestName,
            booking.GuestEmail,
            booking.CheckIn,
            booking.CheckOut,
            booking.Room.PricePerNight * (booking.CheckOut.DayNumber - booking.CheckIn.DayNumber)))
        .ToListAsync(cancellationToken);

    return Results.Ok(bookings);
});

api.MapPost("/bookings", async (
    CreateBookingRequest request,
    HotelBookingContext db,
    BookingWriteLock bookingWriteLock,
    CancellationToken cancellationToken) =>
{
    var errors = BookingValidator.Validate(request);
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    var room = await db.Rooms.FindAsync([request.RoomId], cancellationToken);
    if (room is null)
    {
        return Results.NotFound(new { message = "Room not found." });
    }

    await bookingWriteLock.WaitAsync(cancellationToken);
    try
    {
        var hasConflict = await db.Bookings.AnyAsync(
            booking => booking.RoomId == request.RoomId
                && request.CheckIn < booking.CheckOut
                && request.CheckOut > booking.CheckIn,
            cancellationToken);

        if (hasConflict)
        {
            return Results.Conflict(new { message = "This room is no longer available for those dates." });
        }

        var booking = request.ToEntity();
        db.Bookings.Add(booking);
        await db.SaveChangesAsync(cancellationToken);

        var response = new BookingResponse(
            booking.Id,
            room.Id,
            room.Number,
            room.Name,
            booking.GuestName,
            booking.GuestEmail,
            booking.CheckIn,
            booking.CheckOut,
            room.PricePerNight * (booking.CheckOut.DayNumber - booking.CheckIn.DayNumber));

        return Results.Created($"/api/bookings/{booking.Id}", response);
    }
    finally
    {
        bookingWriteLock.Release();
    }
});

api.MapDelete("/bookings/{id:int}", async (
    int id,
    HotelBookingContext db,
    CancellationToken cancellationToken) =>
{
    var booking = await db.Bookings.FindAsync([id], cancellationToken);
    if (booking is null)
    {
        return Results.NotFound();
    }

    db.Bookings.Remove(booking);
    await db.SaveChangesAsync(cancellationToken);
    return Results.NoContent();
});

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();

public partial class Program;
