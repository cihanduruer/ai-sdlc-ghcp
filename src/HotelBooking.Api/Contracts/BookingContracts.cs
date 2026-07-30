using System.ComponentModel.DataAnnotations;
using HotelBooking.Api.Models;

namespace HotelBooking.Api.Contracts;

public sealed record RoomResponse(
    int Id,
    string Number,
    string Name,
    string Description,
    int Capacity,
    decimal PricePerNight,
    bool IsAvailable);

public sealed record BookingResponse(
    int Id,
    int RoomId,
    string RoomNumber,
    string RoomName,
    string GuestName,
    string GuestEmail,
    DateOnly CheckIn,
    DateOnly CheckOut,
    decimal TotalPrice);

public sealed record CreateBookingRequest(
    int RoomId,
    string GuestName,
    string GuestEmail,
    DateOnly CheckIn,
    DateOnly CheckOut)
{
    public Booking ToEntity() => new()
    {
        RoomId = RoomId,
        GuestName = GuestName.Trim(),
        GuestEmail = GuestEmail.Trim(),
        CheckIn = CheckIn,
        CheckOut = CheckOut,
        CreatedAtUtc = DateTime.UtcNow
    };
}

public static class BookingValidator
{
    public static Dictionary<string, string[]> Validate(CreateBookingRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.RoomId <= 0)
        {
            errors[nameof(request.RoomId)] = ["Select a room."];
        }

        if (string.IsNullOrWhiteSpace(request.GuestName))
        {
            errors[nameof(request.GuestName)] = ["Guest name is required."];
        }
        else if (request.GuestName.Trim().Length > 120)
        {
            errors[nameof(request.GuestName)] = ["Guest name cannot exceed 120 characters."];
        }

        if (string.IsNullOrWhiteSpace(request.GuestEmail)
            || !new EmailAddressAttribute().IsValid(request.GuestEmail))
        {
            errors[nameof(request.GuestEmail)] = ["Enter a valid email address."];
        }

        if (request.CheckIn < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            errors[nameof(request.CheckIn)] = ["Check-in cannot be in the past."];
        }

        if (request.CheckOut <= request.CheckIn)
        {
            errors[nameof(request.CheckOut)] = ["Check-out must be after check-in."];
        }

        return errors;
    }
}
