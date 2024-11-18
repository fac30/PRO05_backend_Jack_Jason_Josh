using J3.Data;
using J3.Models;
using Microsoft.EntityFrameworkCore;

namespace J3.Routes;

public static class DevRoutes
{
    public static void MapDevRoutes(this WebApplication app)
    {
        app.MapPost("/dev/seed",
            async (ColourContext context) => {
                /* Create/Select Test User */
                var user = await context.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    user = new User
                    {
                        Name = "Test User",
                        Email = "test@example.com",
                        Hash = "notarealpasswordhash"
                    };
                    
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }

                // Create Test Collections & Assign Them to Test User
                var collections = new List<Collection>
                {
                    new Collection
                    {
                        Name = "Public Sunset Palette",
                        Description = "Warm evening colors",
                        Type = "palette",
                        IsPublic = true,
                        UserId = user.Id
                    },
                    new Collection
                    {
                        Name = "Private Ocean Palette",
                        Description = "Cool ocean tones",
                        Type = "palette",
                        IsPublic = false,
                        UserId = user.Id
                    },
                    new Collection
                    {
                        Name = $"{user.Name}'s Faves",
                        Description = $"Colours that {user.Name} has favourited",
                        Type = "favourite",
                        IsPublic = true,
                        UserId = user.Id
                    }
                };

                context.Collections.AddRange(collections);
                await context.SaveChangesAsync();

                return Results.Ok("Test collections seeded successfully");
            }
        );        
    }
}