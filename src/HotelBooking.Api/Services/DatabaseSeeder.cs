using HotelBooking.Api.Data;
using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.Services;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<HotelBookingContext>();
        await db.Database.EnsureCreatedAsync();

        if (await db.Rooms.AnyAsync())
        {
            return;
        }

        db.Rooms.AddRange(
            new Room
            {
                Number = "101",
                Name = "City Single",
                Description = "A quiet room for solo travellers with a workspace and city view.",
                Capacity = 1,
                PricePerNight = 89m
            },
            new Room
            {
                Number = "102",
                Name = "Garden Double",
                Description = "A bright double room overlooking the courtyard garden.",
                Capacity = 2,
                PricePerNight = 129m
            },
            new Room
            {
                Number = "201",
                Name = "Harbour King",
                Description = "A king room with harbour views and a comfortable lounge chair.",
                Capacity = 2,
                PricePerNight = 169m
            },
            new Room
            {
                Number = "202",
                Name = "Family Studio",
                Description = "A spacious studio with a king bed, sofa bed, and kitchenette.",
                Capacity = 4,
                PricePerNight = 219m
            },
            new Room
            {
                Number = "301",
                Name = "Skyline Suite",
                Description = "A top-floor suite with separate living area and panoramic views.",
                Capacity = 2,
                PricePerNight = 289m
            },
            new Room
            {
                Number = "302",
                Name = "Accessible Twin",
                Description = "An accessible twin room with step-free access and roll-in shower.",
                Capacity = 2,
                PricePerNight = 139m
            });

        await db.SaveChangesAsync();
    }
}
